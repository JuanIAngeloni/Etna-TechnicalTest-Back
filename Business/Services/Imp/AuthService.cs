using Etna_Data;
using Etna_Data.Models;
using gringotts_application.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Etna_Business.Services.Imp
{
    public class AuthService : IAuthService
    {
        private readonly EtnaDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _salt;

        public AuthService(EtnaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _salt = _configuration.GetSection("AuthenticationSettings:Salt").Value;
        }

        public async Task<string> GenerateToken(UserLoginModel userLoginModel)
        {
            var userFind = await _context.GetUserLogged(userLoginModel.email);

            var encodedJwt = "";

            if (userFind != null && VerifyPassword(userLoginModel.password, userFind.password))
            {
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,userFind.userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("userId",userFind.userId.ToString()),
                    new Claim("name",userFind.name),
                    new Claim("lastName", userFind.lastName),
                    new Claim("email", userFind.email),
                    new Claim("role", userFind.role)

                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AuthenticationSettings:SigningKey").Value));
                var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var actualDate = DateTime.UtcNow;
                var expireDate = DateTime.UtcNow.AddMinutes(90);
                var jwt = new JwtSecurityToken(
                    issuer: "Peticionario",
                    audience: "Public",
                    claims: claims,
                    notBefore: actualDate,
                    expires: expireDate,
                    signingCredentials: credential
                );
                encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return encodedJwt;
            }
            else
            {
                var msg = "Invalid Credentials";
                throw new ApiException(msg);
            }
        }
        public bool VerifyPassword(string inputPassword, string storedPassword)
        {

            string Salt = _salt.Substring(0, 64);

            var hashedInputPassword = DataEncoder(inputPassword);
            return hashedInputPassword == storedPassword;
        }


        public string DataEncoder(string varToEncrypt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(varToEncrypt);
                byte[] saltBytes = Encoding.UTF8.GetBytes(_salt);

                byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
                Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

                byte[] hashedBytes = sha256.ComputeHash(combinedBytes);

                string hashedPasswordWithSalt = Convert.ToHexString(Encoding.UTF8.GetBytes(Convert.ToHexString(hashedBytes) + saltBytes));

                return hashedPasswordWithSalt;
            }
        }

        public async Task<bool> ValidatedToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AuthenticationSettings:SigningKey").Value);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = "Peticionario",
                    ValidAudience = "Public",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                var msg = "Invalid token";
                throw new ApiException(msg);
            }
        }
    }
}
