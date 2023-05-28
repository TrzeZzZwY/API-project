using AppCore.Interfaces.Services;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly EfPublishServiceAuthorized _publishService;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PublishController(UserManager<UserEntity> userManager, EfPublishServiceAuthorized publishService,
            EfAlbumServiceAuthorized albumService, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _publishService = publishService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        [Route("SavePost")]
        public async Task<ActionResult<PublishOutputDto>> SavePublish(PublishInputDto input)
        {
            var user = await GetCurrentUser();
            if (user is null || input.Image is null)
                return BadRequest();

            string extention = Path.GetExtension(input.Image.FileName);
            if (!(extention == ".jpg" || extention == ".png"))
                return BadRequest("Not allowed file type");

            var entity = await _publishService.Create(
                Guid.Parse(user.Id),
                input.AlbumName,
                DtoMapper.Map(input)); // TODO zapisywanie jakie ma tagi

            SaveImage(user, input, entity.FileName);

            var mapped = DtoMapper.Map(entity);
            return Created("", mapped);
        }
        [HttpGet]
        [Route("GetImgage/{userLogin}/{imageName}")]
        public async Task<ActionResult<PublishOutputDto>> GetImg(string userLogin, string imageName,[FromQuery] string albumName = null)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();

            try
            {
                var publish = await _publishService.GetOne(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName);

                Byte[] b;
                b = await System.IO.File.ReadAllBytesAsync(
                    Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", userLogin, publish.FileName + ".jpg"));
                return File(b,"image/jpeg");
            }
            catch (AccessViolationException e)
            {
                return BadRequest(e.Message);
            }

        }

        private async Task<UserEntity?> GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null)
                return null;
            var userId = identity.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

            return userId is null ? null : await _userManager.FindByIdAsync(userId);
        }
        private async void SaveImage(UserEntity user, PublishInputDto input, string FileName)
        {
            string dir = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", user.UserName);
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, FileName + Path.GetExtension(input.Image.FileName));
            using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                await input.Image.CopyToAsync(fileStream);
            }

        }
    }
}
