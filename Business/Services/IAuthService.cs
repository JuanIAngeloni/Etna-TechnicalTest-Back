
using Etna_Data.Models;

namespace Etna_Business.Services
{
    public interface IAuthService
    {
        Task<string> GenerateToken(UserLoginModel userLogin);

        Task<bool> ValidatedToken(string token);
        
        string DataEncoder(string varToEncrypt);
    }
}
