using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [Route("AddTag")]
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
        [Route("UpdateTag/{tagName}")]
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

        [HttpGet]
        [Route("GetAllTags")]
        public async Task<ActionResult<IEnumerable<PublishTagOutputDto>>> GetAll([FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            return Ok(DtoMapper.Map(await _tagService.GetAll(Guid.Parse(user.Id),(int)page,(int)take)));
        }

        [HttpGet]
        [Route("GetOneTag/{tagName}")]
        public async Task<ActionResult<IEnumerable<PublishTagOutputDto>>> GetOne([FromRoute] string tagName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            return Ok(DtoMapper.Map(await _tagService.GetOne(tagName)));
        }
        [HttpGet]
        [Route("GetPublishesForTag/{tagName}")]
        public async Task<ActionResult<IEnumerable<PublishTagOutputDto>>> GetAllPublishesForTag([FromRoute] string tagName, [FromQuery] int? page = 1, [FromQuery] int? take = 10)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            return Ok(DtoMapper.Map(await _tagService.GetAllPublishesForTag(Guid.Parse(user.Id),tagName,(int)page,(int)take)));
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
