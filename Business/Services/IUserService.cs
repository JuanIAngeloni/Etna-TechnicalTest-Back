using Task_Manager.Models;

namespace Task_Manager.Services
{
    public interface IUserService
    {
        Task<UserRegisterModel> RegisterUser(UserRegisterModel userRegisterDTO);
    }
}
