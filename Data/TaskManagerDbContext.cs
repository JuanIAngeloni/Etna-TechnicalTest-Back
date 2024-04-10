using Task_Manager.Entities;
using Task_Manager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Task_Manager
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) { }


        public DbSet<UserEntity> Users { get; set; }

        public async Task<UserEntity> GetUserById(int userId)
        {
            return await Users.FindAsync(userId);
        }

        public async Task<UserEntity> SaveNewUser(UserEntity userToRegister)
        {
            EntityEntry<UserEntity> userEntity = await Users.AddAsync(userToRegister);
            await SaveChangesAsync();
            return userEntity.Entity;
        }

        public async Task<UserEntity> GetUserLogged(string email)
        {
            UserEntity userLogged = await Users.FirstOrDefaultAsync(x => x.email == email);
            return userLogged;
        }



        public DbSet<TaskEntity> Tasks { get; set; }

        public async Task<List<TaskEntity>> GetTaskList(TaskFilterModel taskFilterModel, int userId)
        {
            var query = Tasks.AsQueryable();

            if (taskFilterModel != null)
            {
                query = query.Where(task => task.userId == userId);
                query = query.Where(task => !task.isDeleted);

                if (taskFilterModel.taskId.HasValue)
                {
                    query = query.Where(task => task.taskId == taskFilterModel.taskId);
                }

                if (!string.IsNullOrEmpty(taskFilterModel.text))
                {
                    string searchText = taskFilterModel.text.ToLower();
                    query = query.Where
                        (task => task.title.ToLower().Contains(searchText)
                        || task.description.ToLower().Contains(searchText)
                        || task.category.name.ToLower().Contains(searchText));
                }

                if (taskFilterModel.isCompleted.HasValue)
                {
                    query = query.Where(task => task.isCompleted == taskFilterModel.isCompleted);
                }
            }
            query = query.OrderByDescending(task => task.createDate);

            return await query.ToListAsync();
        }

        public async Task<bool> SearchEqualEmails(string emailToCompare)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.email == emailToCompare);


            if (user != null)
            { 
                return true; 
            }

            return false;
        }

        public async Task<TaskEntity> SaveNewTask(TaskEntity taskToSave)
        {
            if (taskToSave != null)
            {

                UserEntity user = await GetUserById(taskToSave.userId);
                if (user != null)
                {
                    taskToSave.user = user;
                }

                CategoryEntity category = await GetCategoryById(taskToSave.categoryId);
                if (category != null)
                {
                    taskToSave.category = category;
                }

                EntityEntry<TaskEntity> entityEntry = await Tasks.AddAsync(taskToSave);
                await SaveChangesAsync();

                return entityEntry.Entity;
            }
            else
            {
                return null;
            }
        }


        public async Task<TaskEntity> UpdateTask(TaskEntity taskToUpdate)
        {
            TaskEntity existingTask = await Tasks.FindAsync(taskToUpdate.taskId);

            if (existingTask != null)
            {
                existingTask.title = taskToUpdate.title;
                existingTask.description = taskToUpdate.description;
                existingTask.priority = taskToUpdate.priority;
                existingTask.updateDate = taskToUpdate.updateDate;
                existingTask.categoryId = taskToUpdate.categoryId;

                UserEntity user = await GetUserById(taskToUpdate.userId);
                if (user != null)
                {
                    existingTask.user = user;
                }

                CategoryEntity category = await GetCategoryById(taskToUpdate.categoryId);
                if (category != null)
                {
                    existingTask.category = category;
                }

                await SaveChangesAsync();

                return existingTask;
            }
            else
            {
                return null;
            }
        }
        public async Task<TaskEntity> UpdateIsCompleteTask(int taskId)
        {
            TaskEntity existingTask = await Tasks.FindAsync(taskId);

            if (existingTask != null)
            {
                if (existingTask.isCompleted == true)
                {
                    existingTask.isCompleted = false;
                }
                else
                {
                    existingTask.isCompleted = true;
                }

                await SaveChangesAsync();

                return existingTask;
            }
            else
            {
                return null;
            }
        }


        public async Task<TaskEntity> DeleteUpdateTask(int taskId)
        {
            TaskEntity existingTask = await Tasks.FindAsync(taskId);

            if (existingTask != null)
            {
                if (existingTask.isDeleted == true)
                {
                    existingTask.isDeleted = false;
                }
                else
                {
                    existingTask.isDeleted = true;
                }

                await SaveChangesAsync();

                return existingTask;
            }
            else
            {
                return null;
            }
        }



        public DbSet<CategoryEntity> Categories { get; set; }


        public async Task<CategoryEntity> GetCategoryById(int categoryId)
        {
            return await Categories.FindAsync(categoryId);
        }

        public async Task<List<CategoryEntity>> GetAllCategories()
        {
            return await Categories.ToListAsync();
        }
    }
}
