using AppCore.Interfaces.Identity;
using AppCore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class Publish : IIdentity<Guid>
    {
        public Publish()
        {
            UploadDate = DateTime.Now;
        }
        public Guid Id { get; set; }
        public string ImageName { get; set; }
        public string FileName { get; set; }
        public Cameras Camera { get; set; } = Cameras.None;
        public string? Description { get; set; }
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; } = Status.Hidden;
        public ISet<Guid> UserPublishLikes { get; set; } = new HashSet<Guid>();
        public ISet<PublishTag> PublishTags { get; set; } = new HashSet<PublishTag>();
        public ISet<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
