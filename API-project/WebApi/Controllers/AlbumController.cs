using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;
using WebApi.Dto.Output;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EfAlbumServiceAuthorized _albumService;
        public AlbumController(UserManager<UserEntity> userManager, EfAlbumServiceAuthorized albumService)
        {
            _userManager = userManager;
            _albumService = albumService;
        }

        [HttpPost]
        [Route("AddAlbum")]
        public async Task<IActionResult> AddAlbum(PublishAlbumInputDto inputDto)
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
        [Route("UpdateAlbum/{albumName}")]
        public async Task<IActionResult> UpateAlbum(PublishAlbumInputDto inputDto, string albumName)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var album = DtoMapper.Map(inputDto);

            try
            {
                return Ok(DtoMapper.Map(await _albumService.Update(Guid.Parse(user.Id), Guid.Parse(user.Id), albumName,album)));
            }
            catch (AccessViolationException)
            {
                return Forbid();
            }
            catch(NameDuplicateException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<PublishAlbumOutputDto>>> GetAllAlbums()
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            return Ok(DtoMapper.Map(await _albumService.GetAll(Guid.Parse(user.Id))));
        }
        [HttpGet]
        [Route("GetAllFor/{userLogin}")]
        public async Task<ActionResult<IEnumerable<PublishAlbumOutputDto>>> GetAllAlbumsFor(string userLogin)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();
            //if(userLogin) zawiera niedopuszcalne znaki return bad request
            var targetUser = await _userManager.FindByNameAsync(userLogin);
            if (targetUser is null)
                return BadRequest();
        
            return Ok(DtoMapper.Map(await _albumService.GetAllFor(Guid.Parse(user.Id), Guid.Parse(targetUser.Id))));
        }

        [HttpGet]
        [Route("GetOne/{userLogin}/{albumName}")]
        public async Task<ActionResult<PublishAlbumOutputDto>> GetOne(string userLogin, string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var targetUser = await _userManager.FindByNameAsync(userLogin);
            if (targetUser is null)
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
        public async Task<ActionResult<PublishAlbumOutputDto>> Delete(string albumName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();
         
            try
            {
                return Ok(DtoMapper.Map(await _albumService.Delete(Guid.Parse(user.Id), Guid.Parse(user.Id), albumName)));
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
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("DeleteAllFor/{userLogin}")]
        public async Task<ActionResult<PublishAlbumOutputDto>> DeleteAll(string userLogin)
        {
            var user = await GetCurrentUser();
            var targetUser = await _userManager.FindByNameAsync(userLogin);
            if (user is null || targetUser is null)
                return BadRequest();
         
            try
            {
                return Ok(DtoMapper.Map(await _albumService.DeleteAll(Guid.Parse(user.Id), Guid.Parse(targetUser.Id))));
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


        [HttpGet]
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
