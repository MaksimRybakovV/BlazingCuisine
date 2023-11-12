using AutoMapper;
using BlazingCuisine.Shared.Dtos.Recipe;
using BlazingCuisine.Shared.Models;

namespace BlazingCuisine.Server
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddRecipeDto, Recipe>();
            CreateMap<Recipe, GetRecipeHeaderDto>();
        }
    }
}
