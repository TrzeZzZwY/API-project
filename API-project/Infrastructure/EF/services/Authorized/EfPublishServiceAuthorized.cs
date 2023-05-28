using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services.Authorized
{
    public class EfPublishServiceAuthorized : ServiceAuthorization
    {
        private readonly IPublishService _publishService;

        public EfPublishServiceAuthorized(UserManager<UserEntity> userManager, IPublishService publishService):base(userManager)
        {
            _publishService = publishService;
        }

        public async Task<Publish> Create(Guid user,string? albumName, Publish publish)
        {
            return await _publishService.Create(user, albumName, publish);
        }
    }
}
