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
        public ISet<Tag> Tags { get; set; } //TODO: Dopisać metodę w serwisie
        public IFormFile Image { get; set; } //TODO: Dopisać metodę w serwisie

        public static PublishInputDto of(Publish publish)
        {
            if (publish is null)
            {
                throw new ArgumentException();
            }
            throw new NotImplementedException();
            //return new PublishInputDto(publish.ImageName, publish.Camera, publish.Description, publish.Status);
        }
    }
}
