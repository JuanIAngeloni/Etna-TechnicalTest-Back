using Etna_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etna_Business.Services
{
    public interface ITaskService
    {
            Task<TaskRegisterModel> RegisterTask(TaskRegisterModel taskRegisterModel);
            Task<TaskRegisterModel> UpdateTask(TaskRegisterModel taskRegisterModel);
            Task<TaskUpdateModel> GetTaskById(int id);
            Task<List<TaskUpdateModel>> GetAllTasks();
    }
}
