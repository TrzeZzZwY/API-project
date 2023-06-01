using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FakeItEasy;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;
using WebApi.Dto.Output;
namespace UnitTest
{
    public class TestApiAlbumController
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EfAlbumServiceAuthorized _albumService;

        public TestApiAlbumController()
        {
            _userManager = A.Fake<UserManager<UserEntity>>();
            _albumService = A.Fake<EfAlbumServiceAuthorized>();
        }

        [Fact]
        public void AddAlbum_WhenModelIsNotValid_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            controller.ModelState.AddModelError("Name", "Name is required");
            // Act
            var result = controller.AddAlbum(new PublishAlbumInputDto());
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task AddAlbum_WhenUserIsNotAuthorized_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns((UserEntity)null);
            // Act
            var result = await controller.AddAlbum(new PublishAlbumInputDto());
            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        
        public async Task AddAlbum()
        {
            // Arrange
            var userManagerMock = A.Fake<UserManager<UserEntity>>();
            var albumServiceMock = A.Fake<EfAlbumServiceAuthorized>();
            var controller = new AlbumController(userManagerMock, albumServiceMock);

            var user = new UserEntity { Id = Guid.NewGuid().ToString() };
            A.CallTo(() => userManagerMock.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);

            var albumEntity = new Infrastructure.EF.Entities.PublishAlbumEntity { Name = "Test" };
            var album = new AppCore.Models.PublishAlbum { Name = albumEntity.Name };
            A.CallTo(() => albumServiceMock.Create(Guid.Parse(user.Id), album)).Returns(Task.FromResult(album));

            var inputDto = new PublishAlbumInputDto { Name = "Test" };

            // Act
            var result = await controller.AddAlbum(inputDto);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }
        [Fact]
        public async Task GetAllAlbumAsync()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            var user = new UserEntity { Id = Guid.NewGuid().ToString() };
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);
            var albumEntity = new PublishAlbumEntity { Name = "Test" };
            var album = new AppCore.Models.PublishAlbum { Name = albumEntity.Name };
            A.CallTo(() => _albumService.GetAll(Guid.Parse(user.Id))).Returns(new List<AppCore.Models.PublishAlbum> { album });

            // Act
            var result = await controller.GetAllAlbums();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var okObjectResult = (OkObjectResult)result.Result;
            var albums = (List<PublishAlbumOutputDto>)okObjectResult.Value;

            Assert.Equal(1, albums.Count);
        }
    }
       
}
