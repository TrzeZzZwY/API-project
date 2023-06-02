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
        private readonly Mock<HttpContext> _httpContextMock;


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
        public void AddAlbum_WhenUserIsNotAuthorized_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns((UserEntity)null);
            // Act
            var result = controller.AddAlbum(new PublishAlbumInputDto());
            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
        //[Fact]
        //public void AddAlbum_WhenAlbumIsCreated_ReturnsCreated()
        //{
        //    // Arrange
        //    var controller = new AlbumController(_userManager, _albumService);
        //    var user = new UserEntity();
        //    var album = new PublishAlbum();
        //    var inputDto = new PublishAlbumInputDto();
        //    var outputDto =  PublishAlbumOutputDto();
        //    A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);
        //    A.CallTo(() => _albumService.Create(A<Guid>._, A<PublishAlbum>._)).Returns(album);
        //    A.CallTo(() => DtoMapper.Map(A<PublishAlbum>._)).Returns(outputDto);
        //    // Act
        //    var result = controller.AddAlbum(inputDto);
        //    // Assert
        //    Assert.IsType<CreatedResult>(result.Result);
        //}
        //[Fact]
        //public void AddAlbum_WhenAlbumNameIsDuplicated_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var controller = new AlbumController(_userManager, _albumService);
        //    var user = new UserEntity();
        //    var album = new PublishAlbum();
        //    var inputDto = new PublishAlbumInputDto();
        //    var outputDto =  PublishAlbumOutputDto();
        //    A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);
        //    A.CallTo(() => _albumService.Create(A<Guid>._, A<PublishAlbum>._)).Throws<NameDuplicateException>();
        //    // Act
        //    var result = controller.AddAlbum(inputDto);
        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result.Result);
        //}
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
        //[Fact]
        //public void UpdateAlbum_WhenUserIsNotAuthorized_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var controller = new AlbumController(_userManager, _albumService);
        //    A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns((UserEntity)null);
        //    // Act
        //    var result = controller.UpateAlbum(new PublishAlbumInputDto(), "albumName");
        //    // Assert
        //    Assert.IsType<BadRequestResult>(result.Result);
        //}
        //[Fact]
        //public void UpdateAlbum_WhenAlbumIsUpdated_ReturnsOk()
        //{
        //    // Arrange
        //    var controller = new AlbumController(_userManager, _albumService);
        //    var user = new UserEntity();
        //    var album = new PublishAlbum();
        //    var inputDto = new PublishAlbumInputDto();
        //    var outputDto =  PublishAlbumOutputDto();
        //    A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns(user);
        //    A.CallTo(() => _albumService.Update(A<Guid>._, A<PublishAlbum>._)).Returns(album);
        //    A.CallTo(() => DtoMapper.Map(A<PublishAlbum>._)).Returns(outputDto);
        //    // Act
        //    var result = controller.UpateAlbum(inputDto, "albumName");
        //    // Assert
        //    Assert.IsType<OkObjectResult>(result.Result);
        //}
        [Fact]
        public void GetAll_WhenUserIsNull_ReturnsBadQequest()
        {
            // Arrange
            var controller = new AlbumController(_userManager, _albumService);
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>._)).Returns((UserEntity)null);
            // Act
            var result = controller.GetAllAlbums();
            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

    }
       
}
