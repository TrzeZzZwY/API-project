using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using AppCore.Services;
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
        private readonly DtoMapper _dtoMapper;

        public TagController(EfTagServiceAuthorized tagService, IPublishService publishService, UserManager<UserEntity> userManager)
        {
            _tagService = tagService;
            _userManager = userManager;
            _dtoMapper = new DtoMapper(userManager, publishService);
        }
        [HttpPost]
        [Route("AddTag")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTag(PublishTagInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            var tag = _dtoMapper.Map(input);
            try
            {
                var created = await _tagService.Create(Guid.Parse(user.Id), tag);
                var output = _dtoMapper.Map(created);
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

            var tag = _dtoMapper.Map(input);
            try
            {
                var updated = await _tagService.Update(Guid.Parse(user.Id), tagName, tag);
                var output = _dtoMapper.Map(updated);
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
                var output = _dtoMapper.Map(updated);
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
        public async Task<ActionResult<IEnumerable<PublishTagOutputDto>>> GetAll()
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            return Ok(_dtoMapper.Map(await _tagService.GetAll()));
        }

        [HttpGet]
        [Route("GetOneTag/{tagName}")]
        public async Task<ActionResult<IEnumerable<PublishTagOutputDto>>> GetOne([FromRoute] string tagName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            return Ok(_dtoMapper.Map(await _tagService.GetOne(tagName)));
        }
        [HttpGet]
        [Route("GetPublishesForTag/{tagName}")]
        public async Task<ActionResult<IEnumerable<PublishTagOutputDto>>> GetAllPublishesForTag([FromRoute] string tagName)
        {
            var user = await GetCurrentUser();
            if (user is null)
                return BadRequest();

            return Ok(_dtoMapper.Map(await _tagService.GetAllPublishesForTag(Guid.Parse(user.Id),tagName)));
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
