using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppCore.Commons.Exceptions;
using AppCore.Models;
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
        public void AddAlbumCreated()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            var album = DtoMapper.Map(new PublishAlbumInputDto
            {
                Name = "albumName",
            });
            var inputDto = new PublishAlbumInputDto
            {
                Name = "albumName",
            };
            var user = new UserEntity
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "username",
                Email = "email",
            };
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);
            A.CallTo(() => _albumService.Create(Guid.Parse(user.Id), album)).Returns(album);
            // Act
            var result = controller.AddAlbum(inputDto);
            // Assert
            Assert.IsType<CreatedResult>(result.Result);
        }
        [Fact]
        public void UpdateAlbum_WhenModelIsNotValid_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            controller.ModelState.AddModelError("Name", "Name is required");
            // Act
            var result = controller.UpateAlbum(new PublishAlbumInputDto(), "albumName");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public void UpdateAlbum_WhenGetCurrentUser_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            var inputDto = new PublishAlbumInputDto
            {
                Name = "albumName",
            };
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns((UserEntity)null);
            // Act
            var result = controller.UpateAlbum(inputDto, "albumName");
            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
        [Fact]
        public void GetAllAlbums_WhenGetCurrentUser_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns((UserEntity)null);
            // Act
            var result = controller.GetAllAlbums();
            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
        [Fact]
        public void DeleteAlbum_WhenGetCurrentUser_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns((UserEntity)null);
            // Act
            var result = controller.Delete("albumName");
            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

    }
       
}
