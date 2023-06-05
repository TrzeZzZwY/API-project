namespace WebApi.Dto.Input
{
    public class CommentUpdateInputDto
    {
        public Guid CommnentId { get; set; }
        public string NewCommentContent { get; set; }
    }
}
