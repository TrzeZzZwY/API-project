using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.services;
using Infrastructure.EF.Services;
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
        private readonly EfAlbumServiceProtected _albumService;
        private readonly DtoMapper _dtoMapper;
        private readonly IPublishService _publishService;
        public AlbumController(UserManager<UserEntity> userManager,IPublishService publishService, EfAlbumServiceProtected albumService)
        {
            _userManager = userManager;
            _albumService = albumService;
            _publishService = publishService;
            _dtoMapper = new DtoMapper(userManager,publishService);
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

            var album = _dtoMapper.Map(inputDto);
            var created = await _albumService.Create(Guid.Parse(user.Id),album);
            var output = _dtoMapper.Map(created);

            return Created(output.Name, output);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<PublishAlbumOutputDto>>> GetAllAlbums()
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            return Ok(_dtoMapper.Map(await _albumService.GetAll(Guid.Parse(user.Id))));
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
