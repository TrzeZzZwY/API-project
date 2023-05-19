using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class UserRoleEntity : IdentityRole
    {
        public UserRoleEntity() : base()
        {

        }

        public UserRoleEntity(string roleName) : base(roleName)
        {
        }

    }
}
