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
        public CommentEntity(Guid id, UserEntity user, string commentContent,
            bool isEdited, ISet<CommentEntity>? comments)
        {
            Id = id;
            User = user;
            CommentContent = commentContent;
            IsEdited= isEdited;
            Comments = comments ?? new HashSet<CommentEntity>();
        }

        public Guid Id { get; set; }
        public UserEntity User { get; set; }
        public string CommentContent { get; set; }
        public bool IsEdited { get; set; }
        public PublishEntity? publish { get; set; }
        public CommentEntity? comment { get; set; }
        public ISet<CommentEntity> Comments { get; set; }
    }
}
