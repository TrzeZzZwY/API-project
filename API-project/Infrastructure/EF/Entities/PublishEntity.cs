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

        public Guid Id { get; set; }
        public UserEntity User { get; set; }
        public PublishAlbumEntity? Album { get; set; }
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
