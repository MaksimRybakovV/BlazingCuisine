using BlazingCuisine.Server.Services.FileService;
using BlazingCuisine.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazingCuisine.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _service;

        public FilesController(IFileService service)
        {
            _service = service;        
        }

        [HttpPost]
        public async Task<ActionResult<UploadResult>> PostImage([FromQuery] int id)
        {
            var formCollection = await Request.ReadFormAsync();
            var result = await _service.UploadImage(formCollection.Files[0], id);

            if(result.IsUploaded == false)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
