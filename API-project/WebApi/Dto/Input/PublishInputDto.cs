using AppCore.Models;
using AppCore.Models.Enums;

namespace WebApi.Dto.Input
{
    public class PublishInputDto
    {
        public string ImageName { get; set; }
        public string? AlbumName { get; set; }
        public Cameras Camera { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public IFormFile Image { get; set; }

    }
}
