using AppCore.Models;
using WebApi.Dto.Output;

namespace WebApi.Dto.Input
{
    public class CommentInputDto
    {      
        public string UserName { get; set; }
        public string PublishName { get; set; }
        public string? AlbumName { get; set; }
        public string CommentContent { get; set; }
    }
}
