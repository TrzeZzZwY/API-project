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

namespace Infrastructure.EF.services
{
    public class EfAlbumServiceProtected
    {
        private readonly IAlbumService _albumService;

        public EfAlbumServiceProtected(IAlbumService albumService)
        {
            _albumService = albumService;
        }
        public async Task<PublishAlbum> Create(Guid userId, PublishAlbum album)
        {
            return await _albumService.Create(userId, album);
        }

        public async Task<PublishAlbum> Delete(Guid userId, Guid albumId)
        {
            if (await _albumService.IsUserOwnerOrAdmin(userId, albumId))
                return await _albumService.Delete(albumId);
            throw new AccessViolationException();
        }
        public async Task<IEnumerable<PublishAlbum>> GetAll(Guid userId)
        {
            var all = await _albumService.GetAll();
            return all.Where(
                e => UserHaveAcces(userId,e.Id).Result).ToList();
        }
        public async Task<IEnumerable<PublishAlbum>> GetAllFor(Guid userId,Guid ownerId)
        {
            var all = await _albumService.GetAllFor(ownerId);
            return all.Select(e => e).Where(
                e => UserHaveAcces(userId,e.Id).Result);
        }
        public async Task<PublishAlbum> GetOne(Guid userId, Guid publishAlbumId)
        {
            if (!await UserHaveAcces(userId,publishAlbumId))
                throw new AccessViolationException();
            return await _albumService.GetOne(publishAlbumId);
        }
        public async Task<PublishAlbum> GetOne(Guid userId,Guid ownerId, string albumName)
        {
            var album = await _albumService.GetOne(ownerId, albumName);
            return await GetOne(userId, album.Id);
        }
       
        public async Task<PublishAlbum> Update(Guid userId, Guid albumId, PublishAlbum album)
        {
            if (await _albumService.IsUserOwnerOrAdmin(userId, albumId))
               return await _albumService.Update(albumId, album);
            throw new AccessViolationException();
        }
        public async Task<PublishAlbum> Update(Guid userId,Guid ownerId, string albumName, PublishAlbum album)
        {
            var find = await _albumService.GetOne(ownerId, albumName);
            return await Update(userId, find.Id, album);

        }
        private async Task<bool> UserHaveAcces(Guid userId, Guid albumId)
        {
            return !await _albumService.IsPrivate(albumId) || await _albumService.IsUserOwnerOrAdmin(userId, albumId);
        }
    }
}
