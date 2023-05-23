using AppCore.Models;
using AppCore.Models.Enums;
using WebApi.Dto.Output;

namespace WebApi.Dto.Input
{
    public class PublishAlbumInputDto
    {
        public PublishAlbumInputDto(string name, Status status)
        {
            Name = name;
            Status = status;
        }
        public string Name { get; set; }
        public Status Status { get; set; }
    }
}
