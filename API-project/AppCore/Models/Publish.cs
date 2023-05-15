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
        public Publish(string imageName, Cameras? camera,
            string? description,Status? status, ISet<User>? userLikes,
            ISet<PublishTag> publishTags, ISet<Comment>? comments)
        {
            ImageName = imageName;
            Camera = camera ?? Cameras.None;
            Description = description;
            //Słabe rozwiązanie, można za pomocą DI wstrzyknąć servis od daty
            UploadDate = DateTime.Now;
            Status = status ?? Status.private_publish;
            UserPublishLikes = userLikes ?? new HashSet<User>();
            PublishTags = publishTags;
            Comments = comments ?? new HashSet<Comment>();
        }

        public Guid Id { get; set; }
        public string ImageName { get; set; }
        public Cameras Camera { get; set; }
        public string? Description { get; set; }
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; }
        // opcja gdzie klasa User nie dziedziczy po IdentityUser<Guid>
        // ISet<Guid> UserLikes { get;set; }
        public ISet<User> UserPublishLikes { get;set; }
        public ISet<PublishTag> PublishTags { get; set; }
        public ISet<Comment> Comments { get; set; }
    }
}
