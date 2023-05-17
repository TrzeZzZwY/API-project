using AppCore.Models;
using AppCore.Models.Enums;

namespace WebApi.Dto.Input
{
    public class PublishInputDto
    {
        public PublishInputDto(string imageName, string camera, string description, Status status, ISet<Tag> tags, IFormFile image)
        {
            ImageName = imageName;
            Camera = camera;
            Description = description;
            Status = status;
            Tags = tags;
            Image = image;
        }

        public string ImageName { get; set; }
        public string Camera { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public ISet<Tag> Tags { get; set; }
        public IFormFile Image { get; set; }
    }
}
