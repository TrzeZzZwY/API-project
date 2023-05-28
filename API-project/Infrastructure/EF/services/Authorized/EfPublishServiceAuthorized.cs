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
            return await _publishService.Create(user, publish, albumName);
        }
        public async Task<Publish> GetOne(Guid userId,Guid ownerId,string imageName, string? albumName)
        {
            var publish = await _publishService.GetOne(
                ownerId: ownerId,
                imageName: imageName,
                albumName: albumName);
            return await UserHaveAcces(userId, publish.Id) ? publish : throw new AccessViolationException();
        }
        private async Task<bool> UserHaveAcces(Guid userId, Guid publishId)
        {
            return !await _publishService.IsPrivate(publishId) ||
                    await _publishService.IsUserOwner(userId, publishId) ||
                    await UserIsAdmin(userId);
        }

    }
}
