using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AppCore.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;

namespace AppCore.Models
{
    public class User : IIdentity<Guid>
    {
        public Guid Id { get; set; }
        //mozna dodać znajomch
        //public ISet<User> Friends { get; set; }
        public string Login { get; set; }
        public ISet<Publish> Publishes { get; set; } = new HashSet<Publish>();
        public ISet<Publish> UserPublishLikes { get; set; } = new HashSet<Publish>();
        public ISet<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ISet<PublishAlbum> Albums { get; set; } = new HashSet<PublishAlbum>();
    }
}
