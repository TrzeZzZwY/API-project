using AppCore.Models;
using WebApi.Dto.Output;

namespace WebApi.Dto.Input
{
    public class CommentInputDto
    {
        public CommentInputDto(Guid userId, Guid publishId, string commentContent)
        {
            UserId = userId;
            PublishId = publishId;
            CommentContent = commentContent;
        }

        public Guid UserId { get; set; }
        public Guid PublishId { get; set; }
        public string CommentContent { get; set; }
    }
}
