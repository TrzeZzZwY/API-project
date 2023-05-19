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
        public UserEntity()
        {
            
        }
        public UserEntity(string login, ISet<PublishEntity>? publishes, ISet<PublishEntity>? userPublishLikes):base()
        {
            Login = login;
            Publishes = publishes ?? new HashSet<PublishEntity>();
            PublishLikes = userPublishLikes ?? new HashSet<PublishEntity>();
        }

        public string Login { get; set; }
        public ISet<PublishEntity> Publishes { get; set; }
        public ISet<PublishEntity> PublishLikes { get; set; }
        public ISet<CommentEntity> Comments { get; set; }
    }
}
