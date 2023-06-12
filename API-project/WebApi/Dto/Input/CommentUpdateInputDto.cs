using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.Input
{
    public class CommentUpdateInputDto
    {
        public Guid CommnentId { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [MaxLength(100, ErrorMessage = "Content of the comment must be at most 100 characters.")]
        public string NewCommentContent { get; set; }
    }
}
