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
        public Guid TargetUserId { get; set; }
        public string AlbumName { get; set; }
        public string PublishName { get; set; }
        public string CommentContent { get; set; }
    }
}
