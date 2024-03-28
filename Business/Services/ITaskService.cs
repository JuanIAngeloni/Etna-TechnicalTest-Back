using Etna_Data.Models;

namespace Etna_Business.Services
{
    public interface ITaskService
    {
        Task<TaskRegisterModel> RegisterTask(TaskRegisterModel taskRegisterModel);
        Task<TaskRegisterModel> UpdateTask(TaskRegisterModel taskRegisterModel);
        Task<List<TaskUpdateModel>> GetAllTasksByUser(TaskFilterModel taskFilterModelm, int userId);
        Task<TaskUpdateModel> UpdateIsCompletedTask(int id);
        Task<TaskUpdateModel> DeleteUpdateTask(int id);

    }
}
