using AppCore.Interfaces.Services;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IAlbumService _albumService;
        private readonly DtoMapper _dtoMapper;
        private readonly IPublishService _publishService;
        public AlbumController(UserManager<UserEntity> userManager,IPublishService publishService,IAlbumService albumService)
        {
            _userManager = userManager;
            _albumService = albumService;
            _publishService = publishService;
            _dtoMapper = new DtoMapper(userManager,publishService);
        }

        [HttpPost]
        [Route("AddAlbum")]
        [Authorize]
        public async Task<IActionResult> AddAlbum(PublishAlbumInputDto inputDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var album = _dtoMapper.Map(inputDto);
            var user = await GetCurrentUser();

            if (user is null)
                return BadRequest();

            var created = await _albumService.Create(Guid.Parse(user.Id),album);
            var output = _dtoMapper.Map(created);
            return Created(output.Name, output);
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
