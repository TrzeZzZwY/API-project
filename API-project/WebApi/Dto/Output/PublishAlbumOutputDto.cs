using AppCore.Models;
using AppCore.Models.Enums;
using WebApi.Dto.Mappers;

namespace WebApi.Dto.Output
{
    public class PublishAlbumOutputDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public Status Status { get; set; }
        public IEnumerable<PublishOutputDto> Publishes { get; set; }
    }
}
