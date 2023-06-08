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

        public EfPublishServiceAuthorized(UserManager<UserEntity> userManager, IPublishService publishService) : base(userManager)
        {
            _publishService = publishService;
        }

        public async Task<Publish> Create(Guid user, string? albumName, Publish publish)
        {
            return await _publishService.Create(user, publish, albumName);
        }
        public async Task<Publish> Delete(Guid userId, Guid publishId)
        {
            if (await _publishService.IsUserOwner(userId, publishId) || await UserIsAdmin(userId))
                return await _publishService.Delete(publishId);
            throw new AccessViolationException();
        }
        public async Task<Publish> Delete(Guid userId, Guid ownerId, string imageName, string? albumName)
        {
            var album = await _publishService.GetOne(ownerId, imageName, albumName);
            return await Delete(userId, album.Id);
        }
        public async Task<IEnumerable<Publish>> DeleteAll(Guid userId, Guid ownerId, string? albumName)
        {
            if (await UserIsAdmin(userId) || userId == ownerId)
                return await _publishService.DeleteAll(ownerId, albumName);
            throw new AccessViolationException();
        }

        public async Task<Publish> GetOne(Guid userId, Guid ownerId, string imageName, string? albumName)
        {
            var publish = await _publishService.GetOne(
                ownerId: ownerId,
                imageName: imageName,
                albumName: albumName);
            return await UserHaveAcces(userId, publish.Id) ? publish : throw new AccessViolationException();
        }
        public async Task<IEnumerable<Publish>> GetAll(Guid userId, string[]? tagNames, int page, int take)
        {
            var all = await _publishService.GetAll(userId, tagNames, page, take);
            //return all.Where(
            //    e => UserHaveAcces(userId, e.Id).Result).ToList();
            return all;
        }
        public async Task<IEnumerable<Publish>> GetAll(Guid userId, Guid ownerId, string? albumName, string[]? tagNames, int page, int take)
        {
            var all = await _publishService.GetAllFor(userId, ownerId, albumName, tagNames, page, take);
            //return all.Where(
            //    e => UserHaveAcces(userId, e.Id).Result).ToList();
            return all;
        }
        public async Task<Publish> Update(Guid userId, Guid albumId, Publish publish)
        {
            if (await _publishService.IsUserOwner(userId, albumId) || await UserIsAdmin(userId))
                return await _publishService.Update(albumId, publish);
            throw new AccessViolationException();
        }
        public async Task<Publish> Update(Guid userId, Guid ownerId, string imageName, string? albumName, Publish publish)
        {
            var find = await _publishService.GetOne(ownerId, imageName, albumName);
            return await Update(userId, find.Id, publish);

        }
        public async Task<uint> Like(Guid userId, Guid ownerId, string imageName, string? albumName)
        {
            var find = await _publishService.GetOne(ownerId, imageName, albumName);
            if (await UserHaveAcces(userId, find.Id) &&
                !await _publishService.IsUserOwner(userId, find.Id) &&
                !find.UserPublishLikes.Contains(userId))
                return await _publishService.Like(userId, find.Id);
            throw new AccessViolationException();
        }
        public async Task<uint> UnLike(Guid userId, Guid ownerId, string imageName, string? albumName)
        {
            var find = await _publishService.GetOne(ownerId, imageName, albumName);
            if (await UserHaveAcces(userId, find.Id) &&
                !await _publishService.IsUserOwner(userId, find.Id) &&
                find.UserPublishLikes.Contains(userId))
                return await _publishService.Unlike(userId, find.Id);
            throw new AccessViolationException();
        }
        public async Task<bool> Move(Guid userId, Guid ownerId, string imageName, string? oldAlbum, string? newAlbum)
        {
            var find = await _publishService.GetOne(ownerId, imageName, oldAlbum);
            if (await UserHaveAcces(userId, find.Id))
                return await _publishService.Move(newAlbum, find.Id);
            throw new AccessViolationException();
        }
        private async Task<bool> UserHaveAcces(Guid userId, Guid publishId)
        {
            return !await _publishService.IsPrivate(publishId) ||
                    await _publishService.IsUserOwner(userId, publishId) ||
                    await UserIsAdmin(userId);
        }

    }
}
