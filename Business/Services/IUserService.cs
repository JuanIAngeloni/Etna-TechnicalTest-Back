using Etna_Data.Models;

namespace Etna_Business.Services
{
    public interface IUserService
    {
        Task<UserRegisterModel> RegisterUser(UserRegisterModel userRegisterDTO);
    }
}
