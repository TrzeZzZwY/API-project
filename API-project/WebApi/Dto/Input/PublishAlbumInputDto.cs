using AppCore.Models;
using AppCore.Models.Enums;
using WebApi.Dto.Output;

namespace WebApi.Dto.Input
{
    public class PublishAlbumInputDto
    {
        public PublishAlbumInputDto(Guid userId,string name, Status status)
        {
            UserId = userId;
            Name = name;
            Status = status;
        }
        public Guid UserId { get; set; }

        public string Name { get; set; }
        public Status Status { get; set; }
    }
}
