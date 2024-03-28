using AutoMapper;
using Etna_Data;
using Etna_Data.Entities;
using Etna_Data.Models;
using gringotts_application.Exceptions;
using System.Text.RegularExpressions;


namespace Etna_Business.Services.Imp
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly EtnaDbContext _context;
        private readonly IAuthService _authService;
        public UserService(IMapper mapper, EtnaDbContext context, IAuthService authService)
        {
            _mapper = mapper;
            _context = context;
            _authService = authService;
        }

        public async Task<UserRegisterModel> RegisterUser(UserRegisterModel userRegister)
        {
            try
            {
                if (userRegister.name.Length > 50)
                {
                    var msg = "Name must not exceed 50 characters.";
                    throw new ApiException(msg);
                }
                if (userRegister.lastName.Length > 50)
                {
                    var msg = "Last name must not exceed 50 characters.";
                    throw new ApiException(msg);
                }

                IsValidPassword(userRegister.password);


                UserEntity newUser = _mapper.Map<UserEntity>(userRegister);
                newUser.password = _authService.DataEncoder(userRegister.password);

                UserEntity registeredEntity = await _context.SaveNewUser(newUser);
                UserRegisterModel savedUser = _mapper.Map<UserRegisterModel>(registeredEntity);
                return savedUser;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error while registering the user: {ex.Message}");
            }
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 8)
            {
                var msg = "The password must have at least 8 characters.";
                throw new ApiException(msg);
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+{}\[\]:;,<.>?`\-=_\\|'""/]"))
            {
                var msg = "The password must contain at least 1 non-alphanumeric character.";
                throw new ApiException(msg);
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                var msg = "The password must contain at least 1 numeric digit.";
                throw new ApiException(msg);
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                var msg = "The password must contain at least 1 uppercase letter.";
                throw new ApiException(msg);
            }

            return true;
        }
    }
}
