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

        public async Task<PageServiceResponse<List<GetRecipeHeaderDto>>> GetCategoryRecipesByPageAsync
            (string category, int page, int pageSize, RecipeFilterParameters parameters)
        {
            var response = new PageServiceResponse<List<GetRecipeHeaderDto>>();

            try
            {
                var query = _context.Recipes.AsQueryable();

                if (!string.IsNullOrEmpty(parameters.Category))
                {
                    query = query
                        .Include(r => r.Category)
                        .Where(e => e.Category!.Name.Contains(parameters.Category));
                }

                if (parameters.TimeInMinutes.HasValue)
                {
                    query = query.Where(e => e.CookingTimeInMinutes <= parameters.TimeInMinutes);
                }

                if (parameters.Difficulty.HasValue)
                {
                    query = query.Where(e => (int)e.Difficulty == parameters.Difficulty);
                }

                query = query.Where(r => r.Category!.Name == category);

                var pageCount = Math.Ceiling(query.Count() / (float)pageSize);
                pageCount = Math.Max(pageCount, 1);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var recipes = await query
                    .Include(r => r.Category)
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

        public async Task<PageServiceResponse<List<GetRecipeHeaderDto>>> GetRequestedRecipesByPageAsync
            (string searchTerm, int page, int pageSize, RecipeFilterParameters parameters)
        {
            var response = new PageServiceResponse<List<GetRecipeHeaderDto>>();

            try
            {
                var query = _context.Recipes.AsQueryable();

                if (!string.IsNullOrEmpty(parameters.Category))
                {
                    query = query
                        .Include(r => r.Category)
                        .Where(e => e.Category!.Name.Contains(parameters.Category));
                }

                if (parameters.TimeInMinutes.HasValue)
                {
                    query = query.Where(e => e.CookingTimeInMinutes <= parameters.TimeInMinutes);
                }

                if (parameters.Difficulty.HasValue)
                {
                    query = query.Where(e => (int)e.Difficulty == parameters.Difficulty);
                }

                query = query.Where(r => EF.Functions.Like(r.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(r.Instruction, $"%{searchTerm}%"));

                var pageCount = Math.Ceiling(query.Count() / (float)pageSize);
                pageCount = Math.Max(pageCount, 1);

                if (page > pageCount)
                    throw new Exception($"The page {page} does not exist. The maximum number of pages is {pageCount}.");

                var recipes = await query
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

        public async Task<AuthServiceResponse<UpdateRecipeDto>> UpdateRecipeAsync(UpdateRecipeDto updatedRecipe, UserInformation userInformation)
        {
            var response = new AuthServiceResponse<UpdateRecipeDto>();

            try
            {
                var recipe = await _context.Recipes
                    .SingleOrDefaultAsync(r => r.Id == updatedRecipe.Id)
                    ?? throw new Exception($"Recipe with Id '{updatedRecipe.Id}' not found!");

                if(!recipe.Owner.Equals(userInformation.Username) && !userInformation.IsAdministrator)
                {
                    response.IsAuthorized = false;
                    response.IsSuccessful = false;
                    response.Message = $"The user is not the author of the recipe with id {recipe.Id}. Access is denied.";
                    _logger.LogError("The user is not the author of the recipe with id {recipe.Id}. Access is denied.", recipe.Id);

                    return response;
                }

                recipe.Instruction = updatedRecipe.Instruction;
                recipe.Ingredients = updatedRecipe.Ingredients;
                recipe.Name = updatedRecipe.Name;
                recipe.Photo = updatedRecipe.Photo;
                recipe.CookingTimeInMinutes = updatedRecipe.CookingTimeInMinutes;
                recipe.Category = updatedRecipe.Category;
                recipe.CategoryId = updatedRecipe.CategoryId;
                recipe.Difficulty = updatedRecipe.Difficulty;

                await _context.SaveChangesAsync();
                response.Data = updatedRecipe;
                _logger.LogInformation("The recipe with ID '{updatedRecipe.Id}' has been updated " +
                    "with values {@updatedRecipe}.", updatedRecipe.Id, updatedRecipe);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
                _logger.LogError("The recipe with ID '{updatedRecipe.Id}' not found.", updatedRecipe.Id);
            }

            return response;
        }

        public async Task<AuthServiceResponse<string>> DeleteRecipeAsync(int id, UserInformation userInformation)
        {
            var response = new AuthServiceResponse<string>();

            try
            {
                var recipe = await _context.Recipes
                    .FirstOrDefaultAsync(x => x.Id == id)
                    ?? throw new Exception($"Recipe with Id '{id}' not found!");

                if (!recipe.Owner.Equals(userInformation.Username) && !userInformation.IsAdministrator)
                {
                    response.IsAuthorized = false;
                    response.IsSuccessful = false;
                    response.Message = $"The user is not the author of the recipe with id {recipe.Id}. Access is denied.";
                    _logger.LogError("The user is not the author of the recipe with id {recipe.Id}. Access is denied.", recipe.Id);

                    return response;
                }

                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();

                response.Data = $"Company with Id '{id}' deleted!";

                _logger.LogInformation("The recipe with ID '{id}' has been deleted.", id);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
                _logger.LogError("The recipe with ID '{updatedRecipe.Id}' not found.", id);
            }

            return response;
        }
    }
}