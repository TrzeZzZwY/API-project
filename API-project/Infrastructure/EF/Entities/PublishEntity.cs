using AppCore.Models.Enums;
using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class PublishEntity
    {
        public PublishEntity()
        {
            
        }
        public PublishEntity(Guid id, string imageName, Cameras camera,
            string? description, DateTime uploadDate, Status status,
            ISet<UserEntity> userPublishLikes, ISet<PublishTagEntity>? publishTags,
            ISet<CommentEntity>? comments)
        {
            Id = id;
            ImageName = imageName;
            Camera = camera;
            Description = description;
            UploadDate = uploadDate;
            Status = status;
            UserLikes = userPublishLikes;
            PublishTags = publishTags ?? new HashSet<PublishTagEntity>();
            Comments = comments ?? new HashSet<CommentEntity>();
        }

        public Guid Id { get; set; }
        public UserEntity User { get; set; }
        public string ImageName { get; set; }
        public Cameras Camera { get; set; }
        public string? Description { get; set; }
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; }
        public ISet<UserEntity> UserLikes { get; set; }
        public ISet<PublishTagEntity> PublishTags { get; set; }
        public ISet<CommentEntity> Comments { get; set; }
    }
}
