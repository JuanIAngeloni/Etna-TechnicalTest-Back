
using Task_Manager.Models;

namespace Task_Manager.Services
{
    public interface IAuthService
    {
        Task<string> GenerateToken(UserLoginModel userLogin);

        Task<bool> ValidatedToken(string token);
        
        string DataEncoder(string varToEncrypt);
    }
}
