using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class CommentEntity
    {
        public CommentEntity()
        {
            
        }

        public Guid Id { get; set; }
        public UserEntity User { get; set; }
        public PublishEntity Publish { get; set; }
        public string CommentContent { get; set; }
        public bool IsEdited { get; set; }
    }
}
