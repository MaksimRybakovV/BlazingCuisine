using AutoMapper;
using BlazingCuisine.Server.Data;
using BlazingCuisine.Shared.Dtos.Recipe;
using BlazingCuisine.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazingCuisine.Server.Services.RecipeService
{
    public class RecipeService : BaseService<Recipe>, IRecipeService
    {
        public RecipeService(ApplicationDataContext context, IMapper mapper, ILogger<Recipe> logger) 
            : base(context, mapper, logger) { }

        public async Task<ServiceResponse<int>> AddRecipeAsync(AddRecipeDto newRecipe)
        {
            var response = new ServiceResponse<int>();
            await _context.Recipes.AddAsync(_mapper.Map<Recipe>(newRecipe));
            await _context.SaveChangesAsync();
            var recipe = await _context.Recipes
                .MaxAsync(t => t.Id);
            response.Data = recipe;
            _logger.LogInformation("The recipe was created with the values {@newRecipe}.", newRecipe);
            return response;
        }

        public async Task<ServiceResponse<List<GetRecipeHeaderDto>>> GetAllRecipesAsync()
        {
            var response = new ServiceResponse<List<GetRecipeHeaderDto>>();

            response.Data = await _context.Recipes
                .Include(r => r.Category)
                .Select(r => _mapper.Map<GetRecipeHeaderDto>(r))
                .ToListAsync();

            return response;
        }

        public async Task<ServiceResponse<List<GetRecipeHeaderDto>>> GetAllRequestedRecipesAsync(string searchTerm)
        {
            var response = new ServiceResponse<List<GetRecipeHeaderDto>>();

            response.Data = await _context.Recipes
                .Include(r => r.Category)
                .Where(r => EF.Functions.Like(r.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(r.Instruction, $"%{searchTerm}%"))
                .Select(r => _mapper.Map<GetRecipeHeaderDto>(r))
                .ToListAsync();

            return response;
        }

        public async Task<PageServiceResponse<List<GetRecipeHeaderDto>>> GetCategoryRecipesByPageAsync(string category, int page, int pageSize)
        {
            var response = new PageServiceResponse<List<GetRecipeHeaderDto>>();

            try
            {
                var pageCount = Math.Ceiling(_context.Recipes.Count() / (float)pageSize);
                pageCount = Math.Max(pageCount, 1);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var recipes = await _context.Recipes
                    .Include(r => r.Category)
                    .Where(r => r.Category!.Name == category)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(r => _mapper.Map<GetRecipeHeaderDto>(r))
                    .ToListAsync()
                    ?? throw new Exception($"Category with name '{category}' not found!");

                response.Data = recipes;
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

        public async Task<ServiceResponse<GetRecipeDto>> GetRecipeById(int id)
        {
            var response = new ServiceResponse<GetRecipeDto>();

            try
            {
                var recipe = await _context.Recipes
                    .SingleOrDefaultAsync(r => r.Id == id)
                    ?? throw new Exception($"Recipe with Id '{id}' not found!");

                response.Data = _mapper.Map<GetRecipeDto>(recipe);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetRecipeHeaderDto>>> GetRecipesByCategoryAsync(string category)
        {
            var response = new ServiceResponse<List<GetRecipeHeaderDto>>();

            try
            {
                var recipes = await _context.Recipes
                    .Include(r => r.Category)
                    .Where(r => r.Category!.Name == category)
                    .Select(r => _mapper.Map<GetRecipeHeaderDto>(r))
                    .ToListAsync()
                    ?? throw new Exception($"Category with name '{category}' not found!");

                response.Data = recipes;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<PageServiceResponse<List<GetRecipeHeaderDto>>> GetRecipesByPageAsync(int page, int pageSize)
        {
            var response = new PageServiceResponse<List<GetRecipeHeaderDto>>();

            try
            {
                var pageCount = Math.Ceiling(_context.Recipes.Count() / (float)pageSize);
                pageCount = Math.Max(pageCount, 1);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var recipes = await _context.Recipes
                    .Include(r => r.Category)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(r => _mapper.Map<GetRecipeHeaderDto>(r))
                    .ToListAsync();

                response.Data = recipes;
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

        public async Task<PageServiceResponse<List<GetRecipeHeaderDto>>> GetRequestedRecipesByPageAsync(string searchTerm, int page, int pageSize)
        {
            var response = new PageServiceResponse<List<GetRecipeHeaderDto>>();

            try
            {
                var pageCount = Math.Ceiling(_context.Recipes.Count() / (float)pageSize);
                pageCount = Math.Max(pageCount, 1);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var recipes = await _context.Recipes
                    .Include(r => r.Category)
                    .Where(r => EF.Functions.Like(r.Name, $"%{searchTerm}%") ||
                        EF.Functions.Like(r.Instruction, $"%{searchTerm}%"))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(r => _mapper.Map<GetRecipeHeaderDto>(r))
                    .ToListAsync();

                response.Data = recipes;
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
    }
}