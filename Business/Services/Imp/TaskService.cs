using AutoMapper;
using Etna_Data;
using Etna_Data.Entities;
using Etna_Data.Models;
using gringotts_application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etna_Business.Services.Imp
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly EtnaDbContext _context;
        public TaskService(IMapper mapper, EtnaDbContext context)
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

        public async Task<TaskUpdateModel> GetTaskById(int id)
        {
            try
            {
                TaskEntity taskEntity = await _context.GetTaskById(id);

                TaskUpdateModel taskUpdateModel= _mapper.Map<TaskUpdateModel>(taskEntity);

                return taskUpdateModel;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error en la obtención de la categoría: {ex.Message}");
            }
        }

        public async Task<List<TaskUpdateModel>> GetAllTasks()
        {
            try
            {
                List<TaskEntity> taskEntityList = await _context.GetTaskList();

                List<TaskUpdateModel> taskUpdateModelList = _mapper.Map<List<TaskUpdateModel>>(taskEntityList);

                return taskUpdateModelList;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error en la obtención de la lista de tareas: {ex.Message}");
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
