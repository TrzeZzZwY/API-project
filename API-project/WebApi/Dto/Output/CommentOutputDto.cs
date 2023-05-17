using AppCore.Models;
using WebApi.Dto.Mappers;

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
    }
}
