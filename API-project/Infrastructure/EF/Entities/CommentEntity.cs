using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class CommentEntity
    {
        public Guid UserId { get; set; }
        public Guid TargetUserId { get; set; }
        public string AlbumName { get; set; }
        public string PublishName { get; set; }
        public string CommentContent { get; set; }
        public string UserLogin { get; set; }
    }
}
