using AppCore.Models;
using AppCore.Models.Enums;

namespace WebApi.Dto.Input
{
    public class PublishInputDto
    {
        public PublishInputDto(Guid userId, string imageName, Cameras? camera, string? description, Status? status, ISet<PublishTag>? tags, IFormFile image)
        {
            UserId = userId;
            ImageName = imageName;
            Camera = camera ?? Cameras.None;
            Description = description?? String.Empty;
            Status = status ?? Status.private_publish;
            Tags = tags ?? new HashSet<PublishTag>();
            Image = image;
        }
        public Guid UserId { get; set; }
        public string ImageName { get; set; }
        public Cameras Camera { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public ISet<PublishTag> Tags { get; set; } //TODO: Dopisać metodę w serwisie
        public IFormFile Image { get; set; } //TODO: Dopisać metodę w serwisie

    }
}
