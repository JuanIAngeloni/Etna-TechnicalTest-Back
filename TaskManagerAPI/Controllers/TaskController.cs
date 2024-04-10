using Task_Manager.Services;
using Task_Manager.Models;
using Task_Manager.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Task_Manager.Controllers
{
    [ApiController]
    [Route("task")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TaskController(ITaskService taskService, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _taskService = taskService;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpGet]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public async Task<ActionResult<List<TaskUpdateModel>>> GetListTaskByUser([FromQuery] TaskFilterModel taskFilterModel)
        {
            try
            {
                var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                List<TaskUpdateModel> taskList = await _taskService.GetAllTasksByUser(taskFilterModel,userId);
                return Ok(taskList);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("An error has occurred");
            }

        }


        [HttpPost]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public async Task<ActionResult<TaskRegisterModel>> RegisterTask(TaskRegisterModel newTask)
        {
            try
            {
                TaskRegisterModel taskRegistered = await _taskService.RegisterTask(newTask);
                return Ok(taskRegistered);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("An error has occurred");
            }

        }

        [HttpPut]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public async Task<ActionResult<TaskRegisterModel>> UpdateTask(TaskRegisterModel newTask)
        {
            try
            {
                TaskRegisterModel taskUpdated = await _taskService.UpdateTask(newTask);
                return Ok(taskUpdated);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("An error has occurred");
            }

        }

        [HttpPut]
        [Route("iscompleted")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public async Task<ActionResult<TaskUpdateModel>> UpdateIsCompletedTask(int idTask)
        {
            try
            {
                TaskUpdateModel taskUpdated = await _taskService.UpdateIsCompletedTask(idTask);
                return Ok(taskUpdated);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("An error has occurred");
            }

        }

        [HttpPut]
        [Route("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public async Task<ActionResult<TaskUpdateModel>> DeleteTask(int idTask)
        {
            try
            {
                TaskUpdateModel taskUpdated = await _taskService.DeleteUpdateTask(idTask);
                return Ok(taskUpdated);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("An error has occurred");
            }

        }



    }
}
