using BlazingCuisine.Shared.Enums;

namespace BlazingCuisine.Shared.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public List<Ingredient> Ingredients { get; set; } = new();
        public string Instruction { get; set; } = string.Empty;
        public int CookingTimeInMinutes { get; set; }
        public string TimeFormatter => $"{CookingTimeInMinutes / 60}h, {CookingTimeInMinutes % 60}m";
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
        public string Owner { get; set; } = string.Empty;
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
    }
}
