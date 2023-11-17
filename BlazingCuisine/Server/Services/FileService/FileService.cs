using AutoMapper;
using BlazingCuisine.Server.Data;
using BlazingCuisine.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlazingCuisine.Server.Services.FileService
{
    public class FileService : BaseService<UploadResult>, IFileService
    {
        private readonly IWebHostEnvironment _enviroment;

        public FileService(ApplicationDataContext context, IMapper mapper, ILogger<UploadResult> logger, IWebHostEnvironment environment) 
            : base(context, mapper, logger) 
        {
            _enviroment = environment;
        }

        public async Task<UploadResult> UploadImage(IFormFile file, int id)
        {
            var result = new UploadResult();

            Recipe? recipe = new();
            long maxFileSize = 1024 * (1024 * 5);
            string trustedFileNameForFileStorage;
            var untrustedFileName = file.FileName;
            result.FileName = untrustedFileName;
            var trustedFileNameForDisplay =
                WebUtility.HtmlEncode(untrustedFileName);
            
            if(file.Length == 0)
            {
                var message = $"{trustedFileNameForDisplay} is empty.";
                _logger.LogError(message);
                result.IsUploaded = false;
                result.Message = message;
            }
            else if(file.Length > maxFileSize)
            {
                var message = $"{trustedFileNameForDisplay} of {file.Length} bytes is larger than the limit of {maxFileSize} bytes.";
                _logger.LogError(message);
                result.IsUploaded = false;
                result.Message = message;
            }
            else
            {
                try
                {
                    recipe = await _context.Recipes
                        .FirstOrDefaultAsync(r => r.Id == id);
                }
                catch (Exception ex)
                {
                    result.IsUploaded = false;
                    result.Message = ex.Message;
                }

                try
                {
                    trustedFileNameForFileStorage = $"img{id}.jpg";
                    var path = Path.Combine(_enviroment.WebRootPath,
                        "images", "recipe", trustedFileNameForFileStorage);

                    await using FileStream fs = new(path, FileMode.Create);

                    var resizeOptions = new ResizeOptions
                    {
                        Mode = ResizeMode.Pad,
                        Size = new Size(1920, 1080)
                    };

                    using var image = Image.Load(file.OpenReadStream());
                    image.Mutate(x => x.Resize(resizeOptions));
                    await image.SaveAsJpegAsync(fs);

                    var message = $"{trustedFileNameForDisplay} saved at {path}";
                    _logger.LogInformation(message);

                    recipe!.Photo = $"images/recipe/{trustedFileNameForFileStorage}";
                    await _context.SaveChangesAsync();

                    result.IsUploaded = true;
                    result.StoredFileName = trustedFileNameForFileStorage;
                }
                catch (IOException ex)
                {
                    var message = $"{trustedFileNameForDisplay} error on upload. {ex.Message}";
                    _logger.LogError(message);
                    result.IsUploaded = false;
                    result.Message = message;
                }
            }

            return result;
        }
    }
}
