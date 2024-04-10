using Task_Manager.Services;
using Task_Manager.Models;
using Task_Manager.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task_Manager.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService){
            _userService = userService;
            _authService = authService;
        }


        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult<UserRegisterModel>> RegisterUser(UserRegisterModel newUser)
        {
            try
            {
                UserRegisterModel userRegistered = await _userService.RegisterUser(newUser);
                return Ok(userRegistered);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception) {
                return BadRequest("An error has occurred");
            }
            
        }
        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult<string>> LoginUser(UserLoginModel newLogin)
        {
            try
            {
                 var newToken = await _authService.GenerateToken(newLogin);
                return Ok(new
                {
                    Token = newToken,
                });
            }
            catch (ApiException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("An error has occurred");
            }

        }

        [HttpGet]
        [Route("/tokenValidated")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public async Task<ActionResult<bool>> GetTokenValidated(string token)
        {
            try
            {
                var tokenValidated = await _authService.ValidatedToken(token);
                return Ok(tokenValidated);
            }
            catch (ApiException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("An error has occurred");
            }
        }

    }
}
