using AppCore.Models;
using AppCore.Models.Enums;
using System.ComponentModel.DataAnnotations;
using WebApi.Dto.Output;

namespace WebApi.Dto.Input
{
    public class PublishAlbumInputDto
    {
        [Required(ErrorMessage = "Name of the publication is required.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Name of the publication must be between 5 and 25 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You have to choose the status of the publication")]
        public Status Status { get; set; }
    }
}
