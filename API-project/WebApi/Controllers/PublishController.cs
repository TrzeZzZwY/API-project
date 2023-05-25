using AppCore.Interfaces.Services;
using AppCore.Services;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;
using WebApi.Dto.Output;

namespace WebApi.Controllers
{
    [Authorize]
    public class PublishController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EfAlbumServiceAuthorized _albumService;
        private readonly DtoMapper _dtoMapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PublishController(UserManager<UserEntity> userManager, IPublishService publishService,
            EfAlbumServiceAuthorized albumService, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _albumService = albumService;
            _hostEnvironment = hostEnvironment;
            _dtoMapper = new DtoMapper(userManager, publishService);
        }

        [HttpPost]
        [Route("SavePost")]
        public async Task<ActionResult<PublishOutputDto>> SavePublish(PublishInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            return await SavePublish("", input);

        }
        [HttpPost]
        [Route("SavePost/{albumName}")]
        public async Task<ActionResult<PublishOutputDto>> SavePublish(string albumName, PublishInputDto input)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            string dir = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", user.UserName);
            if (albumName != "")
                dir = Path.Combine(dir, albumName);
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, input.ImageName);
            using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                await input.Image.CopyToAsync(fileStream);
            }
            return Ok();
        }
        [HttpPost]
        [Route("SavePostTest/{albumName}")]
        public async Task<ActionResult<PublishOutputDto>> SavePublishTest(string albumName, IFormFile file)
        {
            return Ok();
        }
        private async Task<UserEntity?> GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null)
                return null;
            var userId = identity.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

            return userId is null ? null : await _userManager.FindByIdAsync(userId);
        }
    }
}
