using BlazingCuisine.Shared.Enums;
using BlazingCuisine.Shared.Models;
using FluentValidation;

namespace BlazingCuisine.Shared.Dtos.Recipe
{
    public class AddRecipeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public List<Ingredient> Ingredients { get; set; } = new();
        public string Instruction { get; set; } = string.Empty;
        public int CookingTimeInMinutes { get; set; }
        public string TimeFormatter => $"{CookingTimeInMinutes / 60}h, {CookingTimeInMinutes % 60}m";
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
    }

    public class RecipeAddValidator: AbstractValidator<AddRecipeDto>
    {
        public RecipeAddValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Please enter a name.");

            RuleFor(r => r.Instruction)
                .NotEmpty()
                .WithMessage("Please enter a instruction.");

            RuleFor(r => r.CookingTimeInMinutes)
                .GreaterThan(0)
                .WithMessage("Please enter a time.");

            RuleFor(r => r.Difficulty)
                .IsInEnum()
                .WithMessage("Please enter a difficulty.");

            RuleFor(r => r.CategoryId)
                .GreaterThan(0)
                .WithMessage("Please enter a category.");

            RuleFor(r => r.Ingredients)
                .NotEmpty()
                .WithMessage("Please enter a ingredients.");

            RuleForEach(r => r.Ingredients)
                .SetValidator(new IngredientValidator());
        }
    }

    public class IngredientValidator : AbstractValidator<Ingredient>
    {
        public IngredientValidator()
        {
            RuleFor(i => i.Text)
                .NotEmpty()
                .WithMessage("Please enter a ingredient.");
        }
    }
}
