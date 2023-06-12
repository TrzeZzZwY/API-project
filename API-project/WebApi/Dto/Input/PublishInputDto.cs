using AppCore.Models;
using AppCore.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.Input
{
    public class PublishInputDto
    {
        [Required(ErrorMessage = "Name of the image is required.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Name of the image must be between 5 and 25 characters.")]
        public string ImageName { get; set; }

        [StringLength(25, MinimumLength = 5, ErrorMessage = "Name of the album must be between 5 and 25 characters.")]
        public string? AlbumName { get; set; }

        [Required(ErrorMessage = "You have to choose the camera type.")]
        public Cameras Camera { get; set; }

        [MaxLength(100, ErrorMessage = "Description must be at most 100 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "You have to choose the status of the publication")]
        public Status Status { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        [Required(ErrorMessage = "Image is required.")]
        public IFormFile Image { get; set; }

    }
}
