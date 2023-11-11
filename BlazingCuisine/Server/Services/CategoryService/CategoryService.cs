using AutoMapper;
using BlazingCuisine.Server.Data;
using BlazingCuisine.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazingCuisine.Server.Services.CategoryService
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(ApplicationDataContext context, IMapper mapper, ILogger<Category> logger) 
            : base(context, mapper, logger) { }

        public async Task<ServiceResponse<List<Category>>> GetAllCategoriesAsync()
        {
            var response = new ServiceResponse<List<Category>>();

            response.Data = await _context.Categories
                .ToListAsync();

            return response;
        }

        public async Task<PageServiceResponse<List<Category>>> GetCategoriesByPageAsync(int page, int pageSize)
        {
            var response = new PageServiceResponse<List<Category>>();

            try
            {
                var pageCount = Math.Ceiling(_context.Categories.Count() / (float)pageSize);
                pageCount = Math.Max(pageCount, 1);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var categories = await _context.Categories
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                response.Data = categories;
                response.CurrentPage = page;
                response.PageCount = (int)pageCount;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<Category>> GetCategoryById(int id)
        {
            var response = new ServiceResponse<Category>();

            try
            {
                var category = await _context.Categories
                    .SingleOrDefaultAsync(c => c.Id == id)
                    ?? throw new Exception($"Category with Id '{id}' not found!");

                response.Data = category;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
