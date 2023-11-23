using BlazingCuisine.Shared.Models;

namespace BlazingCuisine.Server.Services.FileService
{
    public interface IFileService
    {
        public Task<UploadResult> UploadImage(IFormFile file, int id, UserInformation userInformation);
    }
}
