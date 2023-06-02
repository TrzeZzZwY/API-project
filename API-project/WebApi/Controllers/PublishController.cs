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
        [Route("CreatePost")]
        public async Task<ActionResult<PublishOutputDto>> SavePublish(PublishInputDto input)
        {
            var user = await GetCurrentUser();
            if (user is null || input.Image is null)
                return BadRequest();

            string extention = Path.GetExtension(input.Image.FileName);
            if (!(extention == ".jpg" || extention == ".png"))
                return BadRequest("Not allowed file type");
            try
            {
                var entity = await _publishService.Create(
                    Guid.Parse(user.Id),
                    input.AlbumName,
                    DtoMapper.Map(input));

                SaveImage(user, input, entity.FileName);

                var mapped = DtoMapper.Map(entity);
                return Created("", mapped);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPatch]
        [Route("UpdatPost/{userLogin}/{imageName}")]
        public async Task<ActionResult<PublishOutputDto>> Update(PublishUpdateInputModel input, string userLogin, string imageName, [FromQuery] string? albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();
            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();
            try
            {
                var entity = await _publishService.Update(Guid.Parse(user.Id),Guid.Parse(target.Id),imageName,albumName,DtoMapper.Map(input));
                var mapped = DtoMapper.Map(entity);
                return Ok(mapped);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpGet]
        [Route("GetAllPostsFor/{userLogin}")]
        public async Task<ActionResult<IEnumerable<PublishOutputDto>>> GetAllPublishes(string userLogin, [FromQuery] string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();

            try
            {
                var publish = await _publishService.GetAll(Guid.Parse(user.Id), Guid.Parse(target.Id), albumName);
                return Ok(DtoMapper.Map(publish));
            }
            catch (AccessViolationException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetPost/{userLogin}/{imageName}")]
        public async Task<ActionResult<PublishOutputDto>> GetImageDetails(string userLogin, string imageName, [FromQuery] string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();

            try
            {
                var publish = await _publishService.GetOne(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName);
                return Ok(DtoMapper.Map(publish));
            }
            catch (AccessViolationException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetPostImage/{userLogin}/{imageName}")]
        public async Task<ActionResult<PublishOutputDto>> GetImage(string userLogin, string imageName, [FromQuery] string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();

            try
            {
                var publish = await _publishService.GetOne(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName);

                Byte[] b;
                try
                {
                    b = await System.IO.File.ReadAllBytesAsync(
                        Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", userLogin, publish.FileName + ".jpg"));
                    return File(b, "image/jpeg");
                }
                catch
                {
                    b = await System.IO.File.ReadAllBytesAsync(
                        Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", userLogin, publish.FileName + ".png"));
                    return File(b, "image/png");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpGet]
        [Route("Like/{userLogin}/{imageName}")]
        public async Task<ActionResult<uint>> Like(string userLogin, string imageName, [FromQuery] string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();

            try
            {
                return Ok(await _publishService.Like(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("Unlike/{userLogin}/{imageName}")]
        public async Task<ActionResult<uint>> Unlike(string userLogin, string imageName, [FromQuery] string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();

            try
            {
                return Ok(await _publishService.UnLike(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("MovePostTo/{userLogin}/{imageName}")]
        public async Task<ActionResult<bool>> Move(string userLogin, string imageName, [FromQuery] string oldAlbumName, [FromQuery] string newAlbumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();

            try
            {
                return Ok(await _publishService.Move(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, oldAlbumName, newAlbumName));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
        [Route("Delete/{userLogin}/{imageName}")]
        public async Task<ActionResult<PublishOutputDto>> Delete(string userLogin, string imageName, [FromQuery] string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();

            try
            {
                await _publishService.Delete(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteAll/{userLogin}")]
        public async Task<ActionResult<IEnumerable<PublishOutputDto>>> Delete(string userLogin, [FromQuery] string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var target = await _userManager.FindByNameAsync(userLogin) ?? throw new ArgumentException();

            try
            {
                await _publishService.DeleteAll(Guid.Parse(user.Id), Guid.Parse(target.Id), albumName);
                return NoContent();
            }
            catch (Exception e)
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
