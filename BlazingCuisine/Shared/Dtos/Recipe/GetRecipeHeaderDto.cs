using BlazingCuisine.Shared.Enums;
using BlazingCuisine.Shared.Models;

namespace BlazingCuisine.Shared.Dtos.Recipe
{
    public class GetRecipeHeaderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public int CookingTimeInMinutes { get; set; }
        public string TimeFormatter => $"{CookingTimeInMinutes / 60}h, {CookingTimeInMinutes % 60}m";
        public Category? Category { get; set; }
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
    }
}
