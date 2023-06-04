using AppCore.Models.Enums;

namespace WebApi.Dto.Input
{
    public class PublishUpdateInputModel
    {
        public string NewImageName { get; set; }
        public Cameras NewCamera { get; set; }
        public string? NewDescription { get; set; }
        public Status NewStatus { get; set; }
        public PublishTagInputDto[] NewTags { get; set; }
    }
}
