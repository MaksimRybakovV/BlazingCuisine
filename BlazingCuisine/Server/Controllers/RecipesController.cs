using BlazingCuisine.Server.Services.RecipeService;
using BlazingCuisine.Shared.Dtos.Recipe;
using BlazingCuisine.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazingCuisine.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _service;
        private readonly IHttpContextAccessor _context;

        public RecipesController(IRecipeService service, IHttpContextAccessor context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetRecipeHeaderDto>>>> GetAll()
        {
            var response = await _service.GetAllRecipesAsync();
            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<ServiceResponse<List<GetRecipeHeaderDto>>>> GetAllSearch([FromQuery] string searchTerm)
        {
            var response = await _service.GetAllRequestedRecipesAsync(searchTerm);
            return Ok(response);
        }

        [HttpGet]
        [Route("category")]
        public async Task<ActionResult<ServiceResponse<List<GetRecipeHeaderDto>>>> GetCategory([FromQuery] string category)
        {
            var response = await _service.GetRecipesByCategoryAsync(category);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [Route("pagination")]
        public async Task<ActionResult<PageServiceResponse<List<GetRecipeHeaderDto>>>> GetPage([FromQuery] int page, [FromQuery] int pageSize)
        {
            var response = await _service.GetRecipesByPageAsync(page, pageSize);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [Route("category/pagination")]
        public async Task<ActionResult<PageServiceResponse<List<GetRecipeHeaderDto>>>> GetCategoryPage
            ([FromQuery] string category, [FromQuery] int page, [FromQuery] int pageSize, [FromQuery] RecipeFilterParameters parameters)
        {
            var response = await _service.GetCategoryRecipesByPageAsync(category, page, pageSize, parameters);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [Route("search/pagination")]
        public async Task<ActionResult<PageServiceResponse<List<GetRecipeHeaderDto>>>> GetSearchPage
            ([FromQuery] string searchTerm, [FromQuery] int page, [FromQuery] int pageSize, [FromQuery]RecipeFilterParameters parameters)
        {
            var response = await _service.GetRequestedRecipesByPageAsync(searchTerm, page, pageSize, parameters);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ServiceResponse<GetRecipeDto>>> GetSingle(int id)
        {
            var response = await _service.GetRecipeById(id);

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<int>>> PostRecipe(AddRecipeDto newRecipe)
        {
            var response = await _service.AddRecipeAsync(newRecipe);
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult<AuthServiceResponse<UpdateRecipeDto>>> PutRecipe(UpdateRecipeDto updatedRecipe)
        {
            var username = _context.HttpContext!.User.Identity!.Name;

            var response = await _service.UpdateRecipeAsync(updatedRecipe, username!);

            if (response.IsAuthorized == false)
                return Unauthorized();

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult<AuthServiceResponse<string>>> PutRecipe(int id)
        {
            var username = _context.HttpContext!.User.Identity!.Name;

            var response = await _service.DeleteRecipeAsync(id, username!);

            if (response.IsAuthorized == false)
                return Unauthorized();

            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }
    }
}
