using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services.Authorized
{
    public class EfAlbumServiceAuthorized
    {
        private readonly IAlbumService _albumService;
        private readonly UserManager<UserEntity> _userManager;
        public EfAlbumServiceAuthorized(UserManager<UserEntity> userManager, IAlbumService albumService)
        {
            _userManager = userManager;
            _albumService = albumService;
        }
        public async Task<PublishAlbum> Create(Guid userId, PublishAlbum album)
        {
            return await _albumService.Create(userId, album);
        }

        public async Task<PublishAlbum> Delete(Guid userId, Guid albumId)
        {
            if (await _albumService.IsUserOwner(userId, albumId)||await IsUserAdmin(userId))
                return await _albumService.Delete(albumId);
            throw new AccessViolationException();
        }
        public async Task<PublishAlbum> Delete(Guid userId, Guid ownerId, string albumName)
        {
            var album = await _albumService.GetOne(ownerId, albumName);
            return await Delete(userId, album.Id);
        }
        public async Task<IEnumerable<PublishAlbum>> DeleteAll(Guid userId, Guid ownerId)
        {
            if (await IsUserAdmin(userId) || userId == ownerId)
                return await _albumService.DeleteAll(ownerId);
            throw new AccessViolationException();
        }
        public async Task<IEnumerable<PublishAlbum>> GetAll(Guid userId)
        {
            var all = await _albumService.GetAll();
            return all.Where(
                e => UserHaveAcces(userId, e.Id).Result).ToList();
        }
        public async Task<IEnumerable<PublishAlbum>> GetAllFor(Guid userId, Guid ownerId)
        {
            var all = await _albumService.GetAllFor(ownerId);
            return all.Select(e => e).Where(
                e => UserHaveAcces(userId, e.Id).Result);
        }
        public async Task<PublishAlbum> GetOne(Guid userId, Guid publishAlbumId)
        {
            if (!await UserHaveAcces(userId, publishAlbumId))
                throw new AccessViolationException();
            return await _albumService.GetOne(publishAlbumId);
        }
        public async Task<PublishAlbum> GetOne(Guid userId, Guid ownerId, string albumName)
        {
            var album = await _albumService.GetOne(ownerId, albumName);
            return await GetOne(userId, album.Id);
        }

        public async Task<PublishAlbum> Update(Guid userId, Guid albumId, PublishAlbum album)
        {
            if (await _albumService.IsUserOwner(userId, albumId) || await IsUserAdmin(userId))
                return await _albumService.Update(albumId, album);
            throw new AccessViolationException();
        }
        public async Task<PublishAlbum> Update(Guid userId, Guid ownerId, string albumName, PublishAlbum album)
        {
            var find = await _albumService.GetOne(ownerId, albumName);
            return await Update(userId, find.Id, album);

        }
        private async Task<bool> UserHaveAcces(Guid userId, Guid albumId)
        {
            return !await _albumService.IsPrivate(albumId) ||
                    await _albumService.IsUserOwner(userId, albumId) ||
                    await IsUserAdmin(userId);
        }
        private async Task<bool> IsUserAdmin(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new ArgumentException(message: "User not found");
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
    }
}
