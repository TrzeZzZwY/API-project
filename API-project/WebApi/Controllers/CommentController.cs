using AppCore.Commons.Exceptions;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EfCommentServiceAuthorized _commentService;

        public CommentController(UserManager<UserEntity> userManager, EfCommentServiceAuthorized commentService)
        {
            _userManager = userManager;
            _commentService = commentService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] CommentInputDto inputDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            var targetUser = await GetTargetUser(inputDto.UserName);
            if (user is null || targetUser is null)
                return BadRequest();

            try
            {
                var comment = DtoMapper.Map(inputDto);
                var created = await _commentService.Create(Guid.Parse(user.Id), Guid.Parse(targetUser.Id),inputDto.PublishName,inputDto.AlbumName, comment);
                var output = DtoMapper.Map(created);
                return Created(output.UserLogin, output);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPatch]
        [Route("Update")]
        public async Task<IActionResult> Update([FromForm] CommentUpdateInputDto inputDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            try
            {
                var comment = DtoMapper.Map(inputDto);
                var created = await _commentService.Update(Guid.Parse(user.Id), inputDto.CommnentId, comment);
                var output = DtoMapper.Map(created);
                return Ok(output);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            try
            {
                var all = await _commentService.GetAll(Guid.Parse(user.Id), (int)page, (int)take);
                var output = DtoMapper.Map(all);
                return Ok(output);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetAllForUser/{userLogin}")]
        public async Task<IActionResult> GetAllForUser([FromRoute] string userLogin,[FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");


            var user = await GetCurrentUser();
            var targetUser = await GetTargetUser(userLogin);
            if (user is null || targetUser is null)
                return BadRequest();

            try
            {
                var all = await _commentService.GetAllForUser(Guid.Parse(user.Id), Guid.Parse(targetUser.Id), (int)page, (int)take);
                var output = DtoMapper.Map(all);
                return Ok(output);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetAllForPublish/{userLogin}/{imageName}")]
        public async Task<IActionResult> GetAllForPublish([FromRoute] string userLogin, [FromRoute] string imageName, [FromQuery] string? albumName, [FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");


            var user = await GetCurrentUser();
            var targetUser = await GetTargetUser(userLogin);
            if (user is null || targetUser is null)
                return BadRequest();

            try
            {
                var all = await _commentService.GetAllForPublish(Guid.Parse(user.Id), Guid.Parse(targetUser.Id),imageName,albumName, (int)page, (int)take);
                var output = DtoMapper.Map(all);
                return Ok(output);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("GetOne/{commentId}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid commentId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            try
            {
                var all = await _commentService.GetOne(Guid.Parse(user.Id), commentId);
                var output = DtoMapper.Map(all);
                return Ok(output);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
        [Route("Delete/{commentId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid commentId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            try
            {
                var all = await _commentService.Delete(Guid.Parse(user.Id), commentId);
                var output = DtoMapper.Map(all);
                return Ok(output);
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
