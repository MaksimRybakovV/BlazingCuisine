using BlazingCuisine.Shared.Dtos.Recipe;
using BlazingCuisine.Shared.Enums;

namespace BlazingCuisine.Client.State
{
    public class AppState
    {
        private AddRecipeDto _unsavedNewRecipe = new();
        public AddRecipeDto UnsavedNewRecipe => _unsavedNewRecipe;

        public void SaveRecipe(AddRecipeDto newRecipe)
        {
            _unsavedNewRecipe = newRecipe;
        }

        public void ClearRecipe()
        {
            _unsavedNewRecipe = new()
            {
                CategoryId = 1,
                Ingredients = new(),
                Difficulty = Difficulty.Easy
            };
        }
    }
}
