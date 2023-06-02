using AppCore.Models;
using AppCore.Models.Enums;
using WebApi.Dto.Mappers;

namespace WebApi.Dto.Output
{
    public class PublishOutputDto
    {
        public string ImageName { get; set; }
        public string UserName { get; set; }
        public Cameras Camera { get; set; }
        public string? Description { get; set; }
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; }
        public uint Likes { get; set; }
        public IEnumerable<PublishTagOutputDto> Tags { get; set; }
        public IEnumerable<CommentOutputDto> Comments { get; set; }

    }
}
