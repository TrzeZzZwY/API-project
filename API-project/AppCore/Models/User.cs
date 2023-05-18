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
        public User(string login, ISet<Publish>? publishes, ISet<Publish>? userPublishLikes, Guid? id = null)
        {
            Id = id ?? Guid.Empty;
            Login = login;
            Publishes = publishes ?? new HashSet<Publish>();
            UserPublishLikes = userPublishLikes ?? new HashSet<Publish>();
        }

        public Guid Id { get; set; }
        //mozna dodać znajomch
        //public ISet<User> Friends { get; set; }
        public string Login { get; set; }
        public ISet<Publish> Publishes { get; set; }
        public ISet<Publish> UserPublishLikes { get; set; }
    }
}
