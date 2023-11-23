using BlazingCuisine.Server.Services.FileService;
using BlazingCuisine.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazingCuisine.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _service;
        private readonly IHttpContextAccessor _context;

        public FilesController(IFileService service, IHttpContextAccessor context)
        {
            _service = service;       
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UploadResult>> PostImage([FromQuery] int id)
        {
            var info = new UserInformation();
            info.Username = _context.HttpContext!.User.Identity!.Name!;
            info.IsAdministrator = _context.HttpContext!.User.IsInRole("Administrator");

            var formCollection = await Request.ReadFormAsync();
            var result = await _service.UploadImage(formCollection.Files[0], id, info);

            if(result.IsAuthorized == false)
                return Unauthorized();

            if(result.IsUploaded == false)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
