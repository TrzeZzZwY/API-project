using System.ComponentModel.DataAnnotations;
using AppCore.Models;
using WebApi.Dto.Output;

namespace WebApi.Dto.Input
{
    public class CommentInputDto
    {
        [Required(ErrorMessage = "Name of the user is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Name of the publish is required")]
        public string PublishName { get; set; }

        public string AlbumName { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [MaxLength(100, ErrorMessage = "Content of the comment must be at most 100 characters.")]
        public string CommentContent { get; set; }
    }
}
