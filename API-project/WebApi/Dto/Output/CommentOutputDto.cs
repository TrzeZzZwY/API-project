using AppCore.Models;
using WebApi.Dto.Mappers;

namespace WebApi.Dto.Output
{
    public class CommentOutputDto
    {
        public string UserLogin { get; set; }
        public string CommentContent { get; set; }
    }
}
