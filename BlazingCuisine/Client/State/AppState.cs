using Blazored.LocalStorage;

namespace BlazingCuisine.Client.State
{
    public class AppState
    {
        private bool _isInitialized;
        public NewRecipeState NewRecipeState { get; }
        public FavoriteRecipeState FavoriteRecipeState { get; }

        public AppState(ILocalStorageService localStorage)
        {
            NewRecipeState = new();
            FavoriteRecipeState = new(localStorage);
        }

        public async Task Initialize()
        {
            if(!_isInitialized)
            {
                await FavoriteRecipeState.Initialize();
                _isInitialized = true;
            }
        }
    }
}
