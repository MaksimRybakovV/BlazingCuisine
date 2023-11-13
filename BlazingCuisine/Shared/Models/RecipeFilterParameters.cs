using BlazingCuisine.Shared.Enums;

namespace BlazingCuisine.Shared.Models
{
    public class RecipeFilterParameters
    {
        public string? Category { get; set; } 
        public int? TimeInMinutes { get; set; }
        public int? Difficulty { get; set; }
    }
}
