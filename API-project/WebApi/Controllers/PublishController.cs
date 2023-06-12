using AppCore.Interfaces.Services;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;
using WebApi.Dto.Output;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
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
        [Route("Create")]
        public async Task<ActionResult<PublishOutputDto>> Create([FromForm] PublishInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null || input.Image is null)
                return BadRequest();

            try
            {
                string extention = Path.GetExtension(input.Image.FileName);

                if (!(extention == ".jpg" || extention == ".png")) return BadRequest("Not allowed file type");

                var entity = await _publishService.Create(Guid.Parse(user.Id), input.AlbumName, DtoMapper.Map(input));

                ImageManagement.SaveImage(user, input.Image, entity.FileName, _hostEnvironment);

                var mapped = DtoMapper.Map(entity);
                return Created("", mapped);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPatch]
        [Route("Update/{userLogin}/{imageName}")]
        public async Task<ActionResult<PublishOutputDto>> Update([FromBody] PublishUpdateInputDto input, [FromRoute] string userLogin, [FromRoute] string imageName, [FromQuery] string? albumName)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            var target = await GetTargetUser(userLogin);
            if (user is null || target is null)
                return BadRequest();

            try
            {
                var entity = await _publishService.Update(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName, DtoMapper.Map(input));
                var mapped = DtoMapper.Map(entity);
                return Ok(mapped);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetMany")]
        public async Task<ActionResult<IEnumerable<PublishOutputDto>>> GetAllPublishes([FromQuery] string? userLogin, [FromQuery] string[]? tagNames, [FromQuery] string? albumName, [FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            var user = await GetCurrentUser();
            if(userLogin is null && albumName is not null)
                return BadRequest("Type user login");

            if (user is null)
                return BadRequest();

            try
            {   
                if(userLogin is not null)
                {
                    var target = await GetTargetUser(userLogin);
                    var publish = await _publishService.GetAll(Guid.Parse(user.Id), Guid.Parse(target.Id), albumName, tagNames, (int)page, (int)take);
                    return Ok(DtoMapper.Map(publish));
                }
                var publishAll = await _publishService.GetAll(Guid.Parse(user.Id),tagNames, (int)page, (int)take);
                return Ok(DtoMapper.Map(publishAll));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetOne/{userLogin}/{imageName}")]
        public async Task<ActionResult<PublishOutputDto>> GetImageDetails([FromRoute] string userLogin, [FromRoute] string imageName, [FromQuery] string? albumName)
        {
            var user = await GetCurrentUser();
            var target = await GetTargetUser(userLogin);
            if (user is null || target is null)
                return BadRequest();

            try
            {
                var publish = await _publishService.GetOne(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName);
                return Ok(DtoMapper.Map(publish));
            }
            catch (AccessViolationException)
            {
                return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetOneImage/{userLogin}/{imageName}")]
        public async Task<ActionResult<PublishOutputDto>> GetImage([FromRoute] string userLogin, [FromRoute] string imageName, [FromQuery] string? albumName)
        {
            var user = await GetCurrentUser();
            var target = await GetTargetUser(userLogin);
            if (user is null || target is null)
                return BadRequest();

            try
            {
                var publish = await _publishService.GetOne(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName);

                Byte[] b = await System.IO.File.ReadAllBytesAsync(
                        Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", userLogin, publish.FileName + ".png"));
                return File(b, "image/png");


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpGet]
        [Route("GetOneMiniImage/{userLogin}/{imageName}")]
        public async Task<ActionResult<PublishOutputDto>> GetMiniImage([FromRoute] string userLogin, [FromRoute] string imageName, [FromQuery] string? albumName)
        {
            var user = await GetCurrentUser();
            var target = await GetTargetUser(userLogin);
            if (user is null || target is null)
                return BadRequest();

            try
            {
                var publish = await _publishService.GetOne(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName);

                Byte[] b = await System.IO.File.ReadAllBytesAsync(
                        Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", userLogin, "mini" + publish.FileName + ".png"));
                return File(b, "image/png");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpGet]
        [Route("Like/{userLogin}/{imageName}")]
        public async Task<ActionResult<uint>> Like([FromRoute] string userLogin, [FromRoute] string imageName, [FromQuery] string? albumName)
        {
            var user = await GetCurrentUser();
            var target = await GetTargetUser(userLogin);
            if (user is null || target is null)
                return BadRequest();

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
        public async Task<ActionResult<uint>> Unlike([FromRoute] string userLogin, [FromRoute] string imageName, [FromQuery] string? albumName)
        {
            var user = await GetCurrentUser();
            var target = await GetTargetUser(userLogin);
            if (user is null || target is null)
                return BadRequest();

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
        [Route("MoveTo/{userLogin}/{imageName}")]
        public async Task<ActionResult<bool>> Move([FromRoute] string userLogin, [FromRoute] string imageName, [FromQuery] string? oldAlbumName, [FromQuery] string? newAlbumName)
        {
            var user = await GetCurrentUser();
            var target = await GetTargetUser(userLogin);
            if (user is null || target is null)
                return BadRequest();

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
        public async Task<ActionResult<PublishOutputDto>> Delete([FromRoute] string userLogin, [FromRoute] string imageName, [FromQuery] string? albumName)
        {
            var user = await GetCurrentUser();
            var target = await GetTargetUser(userLogin);
            if (user is null || target is null)
                return BadRequest();

            try
            {
                var publish = await _publishService.Delete(Guid.Parse(user.Id), Guid.Parse(target.Id), imageName, albumName);
                string[] a = new string[] { publish.FileName };
                ImageManagement.DeleteImage(target, a, _hostEnvironment);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteAll/{userLogin}")]
        public async Task<ActionResult<IEnumerable<PublishOutputDto>>> Delete([FromRoute] string userLogin, [FromQuery] string? albumName)
        {
            var user = await GetCurrentUser();
            var target = await GetTargetUser(userLogin);
            if (user is null || target is null)
                return BadRequest();

            try
            {
                var publishes = await _publishService.DeleteAll(Guid.Parse(user.Id), Guid.Parse(target.Id), albumName);
                List<string> files = new List<string>();
                foreach (var item in publishes)
                {
                    files.Add(item.FileName);
                }
                ImageManagement.DeleteImage(target, files, _hostEnvironment);
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

        private async Task<UserEntity?> GetTargetUser(string username)
        {
            var find = await _userManager.Users.FirstOrDefaultAsync(e => username.Equals(e.UserName));
            if (find.UserName != username)
                return null;
            return find;
        }
    }
}
