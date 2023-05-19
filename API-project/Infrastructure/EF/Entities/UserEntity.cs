﻿using AppCore.Models;
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
        public string Login { get; set; }
        public ISet<PublishEntity> Publishes { get; set; }
        public ISet<PublishEntity> PublishLikes { get; set; }
        public ISet<CommentEntity> Comments { get; set; }
    }
}
