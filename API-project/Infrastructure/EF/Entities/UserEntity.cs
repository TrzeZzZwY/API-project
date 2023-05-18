using AppCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class UserEntity : IdentityUser
    {
        public UserEntity(string login, ISet<PublishAlbumEntity>? publishes, ISet<PublishAlbumEntity>? userPublishLikes):base()
        {
            Login = login;
            Publishes = publishes ?? new HashSet<PublishAlbumEntity>();
            UserPublishLikes = userPublishLikes ?? new HashSet<PublishAlbumEntity>();
        }

        public string Login { get; set; }
        public ISet<PublishAlbumEntity> Publishes { get; set; }
        public ISet<PublishAlbumEntity> UserPublishLikes { get; set; }
    }
}
