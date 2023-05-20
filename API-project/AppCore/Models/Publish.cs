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
        public Publish(string imageName, Cameras? camera, Guid userId,
            string? description, Status? status, ISet<Guid>? userLikes,
            ISet<PublishTag>? publishTags, ISet<Comment>? comments, DateTime? uploadDate, Guid? id = null)
        {
            Id = id ?? Guid.Empty;
            UserId = userId;
            ImageName = imageName;
            Camera = camera ?? Cameras.None;
            Description = description;
            // DateTime.Now to słabe rozwiązanie, można za pomocą DI wstrzyknąć servis od daty
            UploadDate = uploadDate ?? DateTime.Now;
            Status = status ?? Status.private_publish;
            UserPublishLikes = userLikes ?? new HashSet<Guid>();
            PublishTags = publishTags ?? new HashSet<PublishTag>();
            Comments = comments ?? new HashSet<Comment>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ImageName { get; set; }
        public Cameras Camera { get; set; }
        public string? Description { get; set; }
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; }
        public ISet<Guid> UserPublishLikes { get; set; }
        public ISet<PublishTag> PublishTags { get; set; }
        public ISet<Comment> Comments { get; set; }
    }
}
