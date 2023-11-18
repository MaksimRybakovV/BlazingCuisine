using BlazingCuisine.Shared.Dtos.Recipe;
using BlazingCuisine.Shared.Models;

namespace BlazingCuisine.Server.Services.RecipeService
{
    public interface IRecipeService
    {
        public Task<ServiceResponse<List<GetRecipeHeaderDto>>> GetAllRecipesAsync();
        public Task<ServiceResponse<GetRecipeDto>> GetRecipeById(int id);
        public Task<PageServiceResponse<List<GetRecipeHeaderDto>>> GetRecipesByPageAsync(int page, int pageSize);
        public Task<ServiceResponse<List<GetRecipeHeaderDto>>> GetRecipesByCategoryAsync(string category);
        public Task<PageServiceResponse<List<GetRecipeHeaderDto>>> GetCategoryRecipesByPageAsync(string category, int page, int pageSize, RecipeFilterParameters parameters);
        public Task<ServiceResponse<List<GetRecipeHeaderDto>>> GetAllRequestedRecipesAsync(string searchTerm);
        public Task<PageServiceResponse<List<GetRecipeHeaderDto>>> GetRequestedRecipesByPageAsync(string searchTerm, int page, int pageSize, RecipeFilterParameters parameters);
        public Task<ServiceResponse<int>> AddRecipeAsync(AddRecipeDto newRecipe);
        public Task<ServiceResponse<UpdateRecipeDto>> UpdateRecipeAsync(UpdateRecipeDto updatedRecipe);
    }
}
