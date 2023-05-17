using AppCore.Models.Enums;

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
