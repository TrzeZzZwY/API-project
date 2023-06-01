using WebApi.Controllers;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;
using WebApi.Dto.Output;


namespace UnitTest
{

    public class TestApi
    {
        private readonly AlbumController _albumController;

       public TestApi(AlbumController albumController)
        {
            _albumController = albumController;
        }
        [Fact]
        public async void AddAlbum()
        {
            // Arrange
            var albumName = "TestAlbum";
            var inputDto = new PublishAlbumInputDto { Name = albumName };

            // Act
            var result = await _albumController.AddAlbum(inputDto);

            // Assert
            Assert.NotNull(result);
            Assert.Same(inputDto, result);
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
