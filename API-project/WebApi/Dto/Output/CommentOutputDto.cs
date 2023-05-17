using AppCore.Models;

namespace WebApi.Dto.Output
{
    public class CommentOutputDto
    {
        public CommentOutputDto(string userLogin, string commentContent)
        {
            UserLogin = userLogin;
            CommentContent = commentContent;
        }

        public string UserLogin { get; set; }
        public string CommentContent { get; set; }

        public static CommentOutputDto of(Comment comment)
        {
            if(comment is null)
            {
                throw new ArgumentException();
            }
            return new CommentOutputDto(comment.User.Login, comment.Content);
        }
    }
}
