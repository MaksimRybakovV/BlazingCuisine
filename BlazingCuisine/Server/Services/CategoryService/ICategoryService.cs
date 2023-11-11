using BlazingCuisine.Shared.Models;

namespace BlazingCuisine.Server.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ServiceResponse<List<Category>>> GetAllCategoriesAsync();
        public Task<ServiceResponse<Category>> GetCategoryById(int id);
        public Task<PageServiceResponse<List<Category>>> GetCategoriesByPageAsync(int page, int pageSize);
    }
}
