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
        public Guid Id { get; set; }
        public string ImageName { get; set; }
        public Cameras Camera { get; set; }
        public string? Description { get; set; }
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; }
        // TODO: ISet<Guid> UserLikes { get;set; } 
        public ISet<User> UserPublishLikes { get; set; }
        public ISet<PublishTag> PublishTags { get; set; }
        public ISet<Comment> Comments { get; set; }
    }
}
