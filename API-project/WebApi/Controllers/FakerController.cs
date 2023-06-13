using AppCore.Commons.Exceptions;
using AppCore.Models;
using AppCore.Models.Enums;
using FakeData;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FakerController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EfAlbumServiceAuthorized _AlbumService;
        private readonly EfTagServiceAuthorized _TagService;
        private readonly EfCommentServiceAuthorized _CommentService;
        private readonly EfPublishServiceAuthorized _PublishService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FakerController(UserManager<UserEntity> userManager, EfAlbumServiceAuthorized albumService,
            EfTagServiceAuthorized tagService, EfCommentServiceAuthorized commentService,
            EfPublishServiceAuthorized publishService, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _AlbumService = albumService;
            _TagService = tagService;
            _CommentService = commentService;
            _PublishService = publishService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Route("AddRandomUsers")]
        public async Task<IActionResult> AddRandomUsers([FromQuery] int count = 1)
        {
            try
            {
                var faker = new FakeDataGenerator();
                var users = faker.RandomUser(count);
                foreach (var user in users)
                {
                    var saved = await _userManager.CreateAsync(user, "1qazXSW@");
                    await _userManager.AddToRoleAsync(user, "USER");
                }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("AddRandomTags")]
        public async Task<IActionResult> AddRandomTags([FromQuery] int count = 1)
        {
            try
            {
                var user = await GetCurrentUser();
                if (user is null)
                    return BadRequest();

                var faker = new FakeDataGenerator();
                var Tags = faker.RandomTag(count);
                foreach (var tag in Tags)
                {
                    try
                    {
                        await _TagService.Create(Guid.Parse(user.Id), tag);
                    }
                    catch (NameDuplicateException)
                    {

                    }

                }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("AddRandomAlbum")]
        public async Task<IActionResult> AddRandomAlbum([FromQuery] int count = 1)
        {
            try
            {
                var user = await GetCurrentUser();
                if (user is null)
                    return BadRequest();

                var faker = new FakeDataGenerator();
                var Albums = faker.RandomAlbum(count);
                var max = await _userManager.Users.CountAsync();
                Random random = new Random();
                foreach (var album in Albums)
                {
                    try
                    {
                        var randomUser = await _userManager.Users.Skip(random.Next(max)).Take(1).FirstOrDefaultAsync();
                        await _AlbumService.Create(Guid.Parse(randomUser.Id), album);
                    }
                    catch (NameDuplicateException)
                    {

                    }

                }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("AddRandomPublish")]
        public async Task<IActionResult> AddRandomPublish([FromQuery] int count = 1)
        {
            try
            {
                var user = await GetCurrentUser();
                if (user is null)
                    return BadRequest();

                var faker = new FakeDataGenerator();
                var Publishes = faker.RandomPublish(count);
                var maxUsers = await _userManager.Users.CountAsync();
                Random random = new Random();
                foreach (var publish in Publishes)
                {
                    try
                    {
                        var randomUser = (await _userManager.Users.ToListAsync())[random.Next(0, maxUsers)];

                        var allAlbums = (await _AlbumService.GetAllFor(Guid.Parse(user.Id), Guid.Parse(randomUser.Id), 1, 30)).ToList();
                        Publish pub;
                        var tags = (await _TagService.GetAll(Guid.Parse(user.Id), 1, 40)).ToList();
                        publish.PublishTags = new HashSet<PublishTag>();
                        var tagsAmount = random.Next(0, 4);
                        for (int i = 0; i < tagsAmount; i++)
                        {
                            if (tags.Count() < 1)
                                break;
                            var tag = tags[random.Next(0, tags.Count())];
                            tags.Remove(tag);
                            publish.PublishTags.Add(tag);
                        }
                        if (allAlbums is not null && allAlbums.Count > 0)
                        {
                            var randomAlbum = allAlbums[random.Next(0,allAlbums.Count)];
                            if (randomAlbum.Status == Status.Hidden && publish.Status == Status.Visible)
                                pub = await _PublishService.Create(Guid.Parse(randomUser.Id), null, publish);
                            else
                                pub = await _PublishService.Create(Guid.Parse(randomUser.Id), randomAlbum.Name, publish);
                        }
                        else
                        {
                            pub = await _PublishService.Create(Guid.Parse(randomUser.Id), null, publish);
                        }

                        using (var stream = faker.RandomImage())
                        {
                            IFormFile formFile = new FormFile(stream, 0, stream.Length, "name", "FileName");
                            ImageManagement.SaveImage(randomUser, formFile, pub.FileName, _hostEnvironment);
                        }


                    }
                    catch (NameDuplicateException)
                    {

                    }

                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("AddRandomComment")]
        public async Task<IActionResult> AddRandomComment([FromQuery] int count = 1)
        {
            try
            {
                var user = await GetCurrentUser();
                if (user is null)
                    return BadRequest();

                var faker = new FakeDataGenerator();
                var Comments = faker.RandomComment(count);
                var maxUsers = await _userManager.Users.CountAsync();
                Random random = new Random();
                foreach (var comments in Comments)
                {
                    var randomUser = await _userManager.Users.Skip(random.Next(maxUsers)).Take(1).FirstOrDefaultAsync();

                    var allPublishes = (await _PublishService.GetAll(Guid.Parse(user.Id), Guid.Parse(randomUser.Id), 1, 40)).ToList();
                    if (allPublishes.Count == 0)
                        continue;
                    var randomPublishes = allPublishes[random.Next(allPublishes.Count)];
                    var randomUser2 = await _userManager.Users.Skip(random.Next(maxUsers)).Take(1).FirstOrDefaultAsync();
                    await _CommentService.Create(Guid.Parse(randomUser2.Id), randomPublishes.Id, comments);

                }
                return Ok();
            }
            catch
            {
                return BadRequest();
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
    }
}
