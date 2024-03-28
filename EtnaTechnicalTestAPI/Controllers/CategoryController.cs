using Etna_Business.Services;
using Etna_Data.Models;
using gringotts_application.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EtnaTechnicalTestAPI.Controllers
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
