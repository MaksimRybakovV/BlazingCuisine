using BlazingCuisine.Shared.Enums;
using BlazingCuisine.Shared.Models;

namespace BlazingCuisine.Shared.Dtos.Recipe
{
    public class AddRecipeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
        public string Instruction { get; set; } = string.Empty;
        public int CookingTimeInMinutes { get; set; }
        public string TimeFormatter => $"{CookingTimeInMinutes / 60}h, {CookingTimeInMinutes % 60}m";
        public Category? Category { get; set; }
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
    }
}
