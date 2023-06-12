using AppCore.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.Input
{
    public class PublishUpdateInputDto
    {
        [Required(ErrorMessage = "Name of the image is required.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Name of the image must be between 5 and 25 characters.")]
        public string NewImageName { get; set; }
        [Required(ErrorMessage = "You have to choose the camera type.")]
        public Cameras NewCamera { get; set; }
        [MaxLength(100, ErrorMessage = "Description must be at most 100 characters.")]
        public string? NewDescription { get; set; }
        [Required(ErrorMessage = "You have to choose the status of the publication")]
        public Status NewStatus { get; set; }
        public PublishTagInputDto[] NewTags { get; set; }
    }
}
