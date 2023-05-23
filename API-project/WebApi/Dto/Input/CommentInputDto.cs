using AppCore.Models;
using WebApi.Dto.Output;

namespace WebApi.Dto.Input
{
    public class CommentInputDto
    {
        public CommentInputDto(Guid publishId, string commentContent)
        {
            PublishId = publishId;
            CommentContent = commentContent;
        }

        public Guid PublishId { get; set; }
        public string CommentContent { get; set; }
    }
}
