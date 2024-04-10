using Task_Manager.Services;
using Task_Manager.Models;
using Task_Manager.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task_Manager.Controllers
{
    [ApiController]
    [Route("category")]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService category)
        {
            _categoryService = category;
        }

        [HttpGet]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public async Task<ActionResult<List<CategoryModel>>> GetListCategory()
        {
            try
            {
                List<CategoryModel> categories = await _categoryService.GetCategories();
                return Ok(categories);
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
