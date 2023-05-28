using AppCore.Interfaces.Services;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services.Authorized
{
    public class ServiceAuthorization
    {
        private readonly UserManager<UserEntity> _userManager;

        public ServiceAuthorization(UserManager<UserEntity> manager)
        {
            _userManager = manager;
        }
        protected async Task<UserEntity> FindUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user is not null ? user : throw new ArgumentException();
        }

        protected async Task<bool> UserIsAdmin(Guid userId)
        {
            var user = await FindUser(userId);
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
    }
}
