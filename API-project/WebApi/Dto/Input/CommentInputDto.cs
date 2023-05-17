using AppCore.Models;
using WebApi.Dto.Output;

namespace WebApi.Dto.Input
{
    public class CommentInputDto
    {
        public CommentInputDto(Guid userId, Guid targetUserId, string albumName, string publishName, string commentContent)
        {
            UserId = userId;
            TargetUserId = targetUserId;
            AlbumName = albumName;
            PublishName = publishName;
            CommentContent = commentContent;
        }

        public Guid UserId { get; set; }
        public Guid? TargetUserId { get; set; } // TODO-Jasiek: Tymczasowe rozwiązanie (nie ma TargetUsera w metodzie of)
        public string AlbumName { get; set; } //TODO: Brak AlbumName w Comment w AppCore
        public string PublishName { get; set; } //TODO: Brak PublishName w AppCore
        public string CommentContent { get; set; }

        public static CommentInputDto of(Comment comment)
        {
            if (comment is null)
            {
                throw new ArgumentException();
            }
            throw new NotImplementedException();
        }
    }
}
