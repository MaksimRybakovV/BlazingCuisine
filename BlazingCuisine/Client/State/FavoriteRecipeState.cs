using BlazingCuisine.Shared.Dtos.Recipe;
using Blazored.LocalStorage;

namespace BlazingCuisine.Client.State
{
    public class FavoriteRecipeState
    {
        private const string _favoriteRecipeKey = "favoriteRecipe";

        private readonly ILocalStorageService _localStorage;

        private bool _isInitialized;
        private List<GetRecipeHeaderDto> _favoriteRecipes = new();

        public event Action? OnChange;

        public IReadOnlyList<GetRecipeHeaderDto> FavoriteRecipes => _favoriteRecipes.AsReadOnly();

        public FavoriteRecipeState(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task Initialize()
        {
            if(!_isInitialized)
            {
                _favoriteRecipes = await _localStorage.GetItemAsync<List<GetRecipeHeaderDto>>(_favoriteRecipeKey)
                    ?? new List<GetRecipeHeaderDto>();
                _isInitialized = true;
                NotifyStateHasChange();
            }
        }

        public async Task AddFavorite(GetRecipeHeaderDto recipe)
        {
            if (_favoriteRecipes.Any(r => r.Id == recipe.Id))
                return;
            _favoriteRecipes.Add(recipe);
            await _localStorage.SetItemAsync(_favoriteRecipeKey, _favoriteRecipes);
            NotifyStateHasChange();
        }

        public async Task RemoteFavorite(GetRecipeHeaderDto recipe)
        {
            var existingRecipe = _favoriteRecipes
                .SingleOrDefault(r => r.Id == recipe.Id);

            if(existingRecipe is null)
                return;

            _favoriteRecipes.Remove(existingRecipe);
            await _localStorage.SetItemAsync(_favoriteRecipeKey, _favoriteRecipes);
            NotifyStateHasChange();
        }

        public bool IsFavorite(GetRecipeHeaderDto recipe)
        {
            return _favoriteRecipes.Any(r => r.Id == recipe.Id);
        }

        public void NotifyStateHasChange()
        {
            OnChange?.Invoke();
        }
    }
}