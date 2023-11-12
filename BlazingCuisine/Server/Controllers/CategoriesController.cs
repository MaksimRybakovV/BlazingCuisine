using BlazingCuisine.Server.Services.CategoryService;
using BlazingCuisine.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazingCuisine.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetAll()
        {
            var response = await _service.GetAllCategoriesAsync();
            return Ok(response);
        }

        [HttpGet]
        [Route("pagination")]
        public async Task<ActionResult<PageServiceResponse<List<Category>>>> GetPage([FromQuery] int page, [FromQuery] int pageSize)
        {
            var response = await _service.GetCategoriesByPageAsync(page, pageSize);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponse<Category>>> GetSingle(int id)
        {
            var response = await _service.GetCategoryById(id);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }
    }
}
