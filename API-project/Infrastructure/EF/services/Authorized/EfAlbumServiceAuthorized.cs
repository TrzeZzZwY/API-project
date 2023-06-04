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
    public class EfAlbumServiceAuthorized : ServiceAuthorization
    {
        private readonly IAlbumService _albumService;
        public EfAlbumServiceAuthorized(UserManager<UserEntity> userManager, IAlbumService albumService):base(userManager)
        {
            _albumService = albumService;
        }
        public async Task<PublishAlbum> Create(Guid userId, PublishAlbum album)
        {
            return await _albumService.Create(userId, album);
        }

        public async Task<PublishAlbum> Delete(Guid userId, Guid albumId)
        {
            if (await _albumService.IsUserOwner(userId, albumId)||await UserIsAdmin(userId))
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
            if (await UserIsAdmin(userId) || userId == ownerId)
                return await _albumService.DeleteAll(ownerId);
            throw new AccessViolationException();
        }
        public async Task<IEnumerable<PublishAlbum>> GetAll(Guid userId,int page, int take)
        {
            var all = await _albumService.GetAll(userId,page,take);
            //return all.Where(
            //    e => UserHaveAcces(userId, e.Id).Result).ToList();
            return all;
        }
        public async Task<IEnumerable<PublishAlbum>> GetAllFor(Guid userId, Guid ownerId, int page, int take)
        {
            var all = await _albumService.GetAllFor(userId,ownerId,page,take);

            //return all.Select(e => e).Where(
            //    e => UserHaveAcces(userId, e.Id).Result);
            return all;
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
            if (await _albumService.IsUserOwner(userId, albumId) || await UserIsAdmin(userId))
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
                    await UserIsAdmin(userId);
        }
    }
}
