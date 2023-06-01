using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Dto.Input;
using WebApi.Dto.Output;
namespace UnitTest
{
    public class TestApi
    {
       public readonly AlbumController _albumController;

        public TestApi()
        {
            var userManagerMock = new Mock<UserManager<UserEntity>>();
            var albumServiceMock = new Mock<EfAlbumServiceAuthorized>();

            _albumController = new AlbumController(userManagerMock.Object, albumServiceMock.Object);
            _albumController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "userId")
                    }))
                }
            };
        }
        [Fact]
        public async Task AddAlbum()
        {
            // Arrange
            var inputDto = new PublishAlbumInputDto { Name = "TestAlbum" };

            // Act
            var result = await _albumController.AddAlbum(inputDto) as CreatedResult;
            var outputDto = result?.Value as PublishAlbumOutputDto;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(outputDto);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }
        [Fact]
        public async void UpateAlbum()
        {
            var albumName = "TestAlbum";
            var result = await _albumController.UpateAlbum(new PublishAlbumInputDto { Name = albumName }, "TestAlbum2");
            if (result != null)
            {
                Assert.Equal("TestAlbum2", albumName);
            }
        }
        [Fact]
        public async void GetAll()
        {
            var expectedAlbumName = "TestAlbum1";
            var expectedUserName = "TestUser1";
            var result = await _albumController.GetAllAlbums();
            if (result != null)
            {
                var album = result.Value.FirstOrDefault(a => a.Name == expectedAlbumName);
                Assert.NotNull(album);
                Assert.Equal(expectedAlbumName, album.Name);
                Assert.Equal(expectedUserName, expectedAlbumName);
            }
        }
    }
}
