using Etna_Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;


namespace Etna_Data
{
    public class EtnaDbContext : DbContext
    {
        public EtnaDbContext(DbContextOptions<EtnaDbContext> options) : base(options) { }

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

        public async Task<List<TaskEntity>> GetTaskList()
        {
            return await Tasks.ToListAsync();
        }

        public async Task<TaskEntity> GetTaskById(int taskId)
        {
            TaskEntity entityEntry = await Tasks.FindAsync(taskId);

            if (entityEntry != null)
            {
                entityEntry.category = await GetCategoryById(entityEntry.categoryId);
            }
            return entityEntry;
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

        public async Task<TaskEntity> DeleteUpdateTask(int taskId)
        {
            TaskEntity existingTask = await GetTaskById(taskId);

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
