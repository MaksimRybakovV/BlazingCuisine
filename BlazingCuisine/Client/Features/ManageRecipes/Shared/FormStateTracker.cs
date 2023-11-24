using BlazingCuisine.Client.State;
using BlazingCuisine.Shared.Dtos.Recipe;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazingCuisine.Client.Features.ManageRecipes.Shared
{
    public class FormStateTracker : ComponentBase, IDisposable
    {
        [Inject]
        private AppState? _appState { get; set; }

        [CascadingParameter]
        public EditContext? CascadingEditContext { get; set; }

        protected override void OnInitialized()
        {
            CascadingEditContext!.OnFieldChanged += SaveInput;
        }

        private void SaveInput(object? sender, FieldChangedEventArgs e)
        {
            var recipe = e.FieldIdentifier.Model;

            if (recipe is AddRecipeDto)
            {
                _appState?.SaveRecipe((AddRecipeDto)recipe);
            }
        }

        public void Dispose()
        {
            CascadingEditContext!.OnFieldChanged -= SaveInput;
        }
    }
}
