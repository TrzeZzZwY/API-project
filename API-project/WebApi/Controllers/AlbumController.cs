using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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
    public class AlbumController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EfAlbumServiceAuthorized _albumService;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AlbumController(UserManager<UserEntity> userManager, EfAlbumServiceAuthorized albumService, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _albumService = albumService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] PublishAlbumInputDto inputDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var album = DtoMapper.Map(inputDto);
            try
            {
                var created = await _albumService.Create(Guid.Parse(user.Id), album);
                var output = DtoMapper.Map(created);
                return Created(output.Name, output);
            }
            catch (NameDuplicateException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPatch]
        [Route("Update/{albumName}")]
        public async Task<IActionResult> Update([FromBody] PublishAlbumInputDto inputDto, [FromRoute] string albumName)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var album = DtoMapper.Map(inputDto);

            try
            {
                return Ok(DtoMapper.Map(await _albumService.Update(Guid.Parse(user.Id), Guid.Parse(user.Id), albumName, album)));
            }
            catch (AccessViolationException)
            {
                return Forbid();
            }
            catch (NameDuplicateException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return NotFound();
            }
        }

        /*[HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<PublishAlbumOutputDto>>> GetAll([FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var all = await _albumService.GetAll(Guid.Parse(user.Id), (int)page, (int)take);
            var mapped = DtoMapper.Map(all);
            return Ok(mapped);
        }*/
        [HttpGet]
        [Route("GetMany")]
        public async Task<ActionResult<IEnumerable<PublishAlbumOutputDto>>> GetAllFor([FromQuery] string? userLogin, [FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();
            try
            {
                if(userLogin is not null && userLogin.Length > 1)
                {
                    var targetUser = await GetTargetUser(userLogin);
                    if (targetUser is null)
                        return BadRequest();
                    var albums = (await _albumService.GetAllFor(Guid.Parse(user.Id), Guid.Parse(targetUser.Id), (int)page, (int)take)).ToList();
                    var mapped = DtoMapper.Map(albums);
                    return Ok(mapped);
                }
                var albumsAll = (await _albumService.GetAll(Guid.Parse(user.Id), (int)page, (int)take)).ToList();
                var mappedAll = DtoMapper.Map(albumsAll);
                return Ok(mappedAll);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("GetOne/{userLogin}/{albumName}")]
        public async Task<ActionResult<PublishAlbumOutputDto>> GetOne([FromRoute] string userLogin, [FromRoute] string albumName)
        {
            var user = await GetCurrentUser();
            var targetUser = await GetTargetUser(userLogin);
            if (user is null || targetUser is null)
                return BadRequest();

            try
            {
                return Ok(DtoMapper.Map(await _albumService.GetOne(Guid.Parse(user.Id), Guid.Parse(targetUser.Id), albumName)));
            }
            catch (AccessViolationException)
            {
                return Forbid();
            }
            catch
            {
                return NotFound();
            }

        }
        [HttpDelete]
        [Route("Delete/{albumName}")]
        public async Task<ActionResult<PublishAlbumOutputDto>> Delete([FromRoute] string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            try
            {
                var deleted = await _albumService.Delete(Guid.Parse(user.Id), Guid.Parse(user.Id), albumName);
                List<string> fileNames = new List<string>();
                foreach (var item in deleted.Publishes)
                {
                    fileNames.Add(item.FileName);
                }
                ImageManagement.DeleteImage(user, fileNames, _hostEnvironment);
                var mapped = DtoMapper.Map(deleted);
                return Ok();
            }
            catch (AccessViolationException)
            {
                return Forbid();
            }
            catch
            {
                return NotFound();
            }
        }
        /*[HttpDelete]
        [Route("DeleteAll")]
        public async Task<ActionResult<PublishAlbumOutputDto>> DeleteAll()
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            try
            {
                return Ok(DtoMapper.Map(await _albumService.DeleteAll(Guid.Parse(user.Id), Guid.Parse(user.Id))));
            }
            catch (AccessViolationException)
            {
                return Forbid();
            }
            catch
            {
                return NotFound();
            }
        }*/
        /*[HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("DeleteAllFor/{userLogin}")]
        public async Task<ActionResult<PublishAlbumOutputDto>> DeleteAll([FromRoute] string userLogin)
        {
            var user = await GetCurrentUser();
            var targetUser = await GetTargetUser(userLogin);
            if (user is null || targetUser is null)
                return BadRequest();

            try
            {
                var deleted = await _albumService.DeleteAll(Guid.Parse(user.Id), Guid.Parse(targetUser.Id));
                List<string> fileNames = new List<string>();
                foreach (var album in deleted)
                    foreach (var item in album.Publishes)
                    {
                        fileNames.Add(item.FileName);
                    }
                ImageManagement.DeleteImage(user, fileNames, _hostEnvironment);
                var mapped = DtoMapper.Map(deleted);
                return Ok();
            }
            catch (AccessViolationException)
            {
                return Forbid();
            }
            catch
            {
                return NotFound();
            }
        }*/


        /*[HttpGet]
        [Route("Test")]
        public async Task<IActionResult> UserTest()
        {
            var user = await GetCurrentUser();
            if (user is null)
                return Unauthorized();
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return Ok("Hello Admin");
            if (await _userManager.IsInRoleAsync(user, "User"))
                return Ok("Hello User");
            return Ok("Other Role");
        }*/

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
