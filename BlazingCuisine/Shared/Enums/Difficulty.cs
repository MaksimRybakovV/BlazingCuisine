using System.Text.Json.Serialization;

namespace BlazingCuisine.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Difficulty
    {
        Easy, 
        Medium, 
        Hard
    }
}
