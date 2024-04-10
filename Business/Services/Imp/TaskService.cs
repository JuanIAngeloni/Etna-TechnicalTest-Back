using AutoMapper;
using Task_Manager.Entities;
using Task_Manager.Models;
using Task_Manager.Exceptions;

namespace Task_Manager.Services.Imp
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly TaskManagerDbContext _context;
        public TaskService(IMapper mapper, TaskManagerDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<TaskRegisterModel> RegisterTask(TaskRegisterModel taskRegisterModel)
        {

            try
            {
                TaskValidator(taskRegisterModel);

                taskRegisterModel.createDate = DateTime.Now;
                taskRegisterModel.updateDate = DateTime.Now;
                
                TaskEntity newTask= _mapper.Map<TaskEntity>(taskRegisterModel);

                TaskEntity registeredEntity= await _context.SaveNewTask(newTask);
                TaskRegisterModel savedTask = _mapper.Map<TaskRegisterModel>(registeredEntity);
                return savedTask;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error en el registro de la tarea {ex.Message}");
            }
        }

        public async Task<TaskRegisterModel> UpdateTask(TaskRegisterModel taskToUpdate)
        {

            try
            {
                TaskValidator(taskToUpdate);

                taskToUpdate.updateDate = DateTime.Now;

                TaskEntity taskUpdaterEntity= _mapper.Map<TaskEntity>(taskToUpdate);

                TaskEntity updatedTaskEntity = await _context.UpdateTask(taskUpdaterEntity);
                TaskRegisterModel updateTask = _mapper.Map<TaskRegisterModel>(updatedTaskEntity);
                return updateTask;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error en la actualizacion de la tarea: {ex.Message}");
            }

        }


        public async Task<List<TaskUpdateModel>> GetAllTasksByUser(TaskFilterModel taskFilterModel, int userId)
        {
            try
            {
                List<TaskEntity> taskEntityList = await _context.GetTaskList(taskFilterModel, userId);

                List<TaskUpdateModel> taskUpdateModelList = new List<TaskUpdateModel>();

                foreach (var taskEntity in taskEntityList)
                {
                    taskEntity.category = await _context.GetCategoryById(taskEntity.categoryId);

                    TaskUpdateModel taskUpdateModel = _mapper.Map<TaskUpdateModel>(taskEntity);

                    taskUpdateModelList.Add(taskUpdateModel);
                }

                return taskUpdateModelList;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error en la obtención de la lista de tareas: {ex.Message}");
            }
        }

        public async Task<TaskUpdateModel> UpdateIsCompletedTask(int id)
        {
            try
            {
                TaskEntity taskEntityUpdated = await _context.UpdateIsCompleteTask(id);

                TaskUpdateModel taskUpdatedModel = _mapper.Map<TaskUpdateModel>(taskEntityUpdated);

                return taskUpdatedModel;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error en el completado de la tarea: {ex.Message}");
            }
        }

        public async Task<TaskUpdateModel> DeleteUpdateTask(int id)
        {
            try
            {
                TaskEntity taskEntityDeleteUpdate = await _context.DeleteUpdateTask(id);

                TaskUpdateModel taskUpdateModel = _mapper.Map<TaskUpdateModel>(taskEntityDeleteUpdate);

                return taskUpdateModel;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error en el borrado de la tarea: {ex.Message}");
            }
        }




        public void TaskValidator(TaskRegisterModel taskToValidate)
        {
            if (taskToValidate.title.Length > 50)
            {
                var msg = "El título no puede exceder los 50 caracteres";
                throw new ApiException(msg);
            }
            if (taskToValidate.description.Length > 500)
            {
                var msg = "La descripción no puede exceder los 50 caracteres";
                throw new ApiException(msg);
            }
            if (taskToValidate.categoryId == null)
            {
                var msg = "La categoria es invalida";
                throw new ApiException(msg);
            }

            if (taskToValidate.priority < 1 || taskToValidate.priority > 5)
            {
                var msg = "La prioridad debe ser un numero de 1 al 5";
                throw new ApiException(msg);
            }
        }


    }
}
