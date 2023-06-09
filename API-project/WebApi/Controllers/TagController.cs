using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;
using WebApi.Dto.Output;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class TagController : Controller
    {
        private readonly EfTagServiceAuthorized _tagService;
        private readonly UserManager<UserEntity> _userManager;

        public TagController(EfTagServiceAuthorized tagService, UserManager<UserEntity> userManager)
        {
            _tagService = tagService;
            _userManager = userManager;
        }
        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTag([FromBody]PublishTagInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var tag = DtoMapper.Map(input);
            try
            {
                var created = await _tagService.Create(Guid.Parse(user.Id), tag);
                var output = DtoMapper.Map(created);
                return Created(output.Name, output);
            }
            catch (NameDuplicateException e)
            {
                return BadRequest(e.Message);
            }            
        }
        [HttpPatch]
        [Route("Update/{tagName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTag([FromRoute]string tagName,[FromBody] PublishTagInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var tag = DtoMapper.Map(input);
            try
            {
                var updated = await _tagService.Update(Guid.Parse(user.Id), tagName, tag);
                var output = DtoMapper.Map(updated);
                return Created(output.Name, output);
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

        [HttpGet]
        [Route("GetMany")]
        public async Task<ActionResult<IEnumerable<PublishTagOutputDto>>> GetAll([FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            return Ok(DtoMapper.Map(await _tagService.GetAll(Guid.Parse(user.Id),(int)page,(int)take)));
        }

        [HttpGet]
        [Route("GetOne/{tagName}")]
        public async Task<ActionResult<IEnumerable<PublishTagOutputDto>>> GetOne([FromRoute] string tagName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();
            
            return Ok(DtoMapper.Map(await _tagService.GetOne(tagName)));
        }
        /*[HttpGet]
        [Route("GetPublishesForTag/{tagName}")]
        public async Task<ActionResult<IEnumerable<PublishTagOutputDto>>> GetAllPublishesForTag([FromRoute] string tagName,[FromQuery]string? userLogin, [FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            var user = await GetCurrentUser();
            UserEntity? target = null;
            if(userLogin is not null)
            {
                target = await GetTargetUser(userLogin);
                if (target is null)
                    return BadRequest();
            }
            if (user is null)
                return BadRequest();

            Guid? targetId = target is null ? null : Guid.Parse(target.Id);
            return Ok(DtoMapper.Map(await _tagService.GetAllPublishesForTag(Guid.Parse(user.Id),targetId ,tagName,(int)page,(int)take)));
        }*/
        [HttpDelete]
        [Route("DeleteTag/{tagName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTag([FromRoute] string tagName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            try
            {
                var updated = await _tagService.Delete(Guid.Parse(user.Id), tagName);
                var output = DtoMapper.Map(updated);
                return Created(output.Name, output);
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
