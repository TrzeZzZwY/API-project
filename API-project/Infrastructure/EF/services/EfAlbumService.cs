using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppCore.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services
{
    public class EfAlbumService : IAlbumService
    {

        private readonly AppDbContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public EfAlbumService(AppDbContext context, UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PublishAlbum> Create(Guid userId, PublishAlbum album)
        {

            var entity = EntityMapper.Map(album);
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentException();
            entity.User = user;

            if (_context.Albums.Any(e => e.User.Id == userId.ToString() && e.Name == album.Name))
                throw new NameDuplicateException($"name: {album.Name} is already in use");

            var created = _context.Albums.Add(entity);

            await _context.SaveChangesAsync();
            var mapped = EntityMapper.Map(created.Entity);
            return mapped;
        }
        public async Task<PublishAlbum> Delete(Guid ownerId, string albumName)
        {
            var find = await GetAlbumAsync(ownerId, albumName);
            return await Delete(find.Id);
        }
        public async Task<PublishAlbum> Delete(Guid albumId)
        {
            var album = await GetAlbumAsync(albumId);
            await _context.Entry(album).Collection(e => e.Publishes).LoadAsync();
            var publishes = album.Publishes;
            foreach (var item in album.Publishes)
            {
                _context.Publishes.Remove(item);
            }
            var removed = _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            var entity = removed.Entity;
            entity.Publishes = publishes;
            return EntityMapper.Map(entity);
        }
        public async Task<IEnumerable<PublishAlbum>> DeleteAll(Guid ownerId)
        {
            var albums =await GetAllAlbums(ownerId);
            var a = albums.ToList();
            List<PublishEntity> publishEntities = new List<PublishEntity>();
            foreach (var album in albums)
            {
                foreach (var publish in album.Publishes)
                {
                    publishEntities.Add(publish);
                    _context.Publishes.Remove(publish);
                }
            }
            _context.Albums.RemoveRange(albums);
            await _context.SaveChangesAsync();
            return EntityMapper.Map(a);
        }
        public async Task<IEnumerable<PublishAlbum>> GetAll(Guid userId, int page, int take)
        {
            var query = _context.Albums.Include(e => e.Publishes).Include(e => e.User);
            var acces = query.Where(e =>
                (e.Status == Status.Visible) ||
                (e.User.Id == userId.ToString()) ||
                (_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId.ToString()).Result, "Admin").Result)
            );
            var albums = QueryFilter.Paginate(acces, page, take);

            foreach (var album in albums)
            {
                foreach (var item in album.Publishes)
                {
                    await _context.Entry(item).Collection(e => e.Comments).LoadAsync();
                }
            }

            return EntityMapper.Map(albums.ToList());
        }
        public async Task<IEnumerable<PublishAlbum>> GetAllFor(Guid userId, Guid ownerId, int page, int take)
        {
            var query = _context.Albums
                .Where(e => e.User.Id == ownerId.ToString())
                .Include(e => e.Publishes)
                .Include(e => e.User);

            var acces = query.Where(e =>     
                (e.Status == Status.Visible) ||
                (e.User.Id == userId.ToString()) ||
                (_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId.ToString()).Result, "Admin").Result)
            );

            var albums = QueryFilter.Paginate(acces, page, take);

            foreach (var album in albums)
            {
                foreach (var item in album.Publishes)
                {
                    await _context.Entry(item).Collection(e => e.Comments).LoadAsync();
                }
            }
            return EntityMapper.Map(albums.ToList());
        }
        public async Task<PublishAlbum> GetOne(Guid publishAlbumId)
        {
            return EntityMapper.Map(await GetAlbumAsync(publishAlbumId));
        }
        public async Task<PublishAlbum> GetOne(Guid ownerId, string albumName)
        {
            var find = await GetAlbumAsync(ownerId, albumName);
            return await GetOne(find.Id);
        }
        public async Task<bool> IsPrivate(Guid ownerId, string albumName)
        {
            var find = await GetAlbumAsync(ownerId, albumName);
            return await IsPrivate(find.Id);
        }
        public async Task<bool> IsPrivate(Guid albumId)
        {
            var album = await GetAlbumAsync(albumId);
            return album.Status == AppCore.Models.Enums.Status.Hidden ? true : false;
        }
        public async Task<bool> IsUserOwner(Guid userId, Guid albumId)
        {
            var album = await GetAlbumAsync(albumId);
            if (album.User.Id == userId.ToString())
                return true;

            return false;
        }
        public async Task<PublishAlbum> Update(Guid albumId, PublishAlbum album)
        {
            var find = await GetAlbumAsync(albumId);

            var sameNames = _context.Albums.FirstOrDefault(e => e.User.Id == find.User.Id.ToString() && e.Name == album.Name);
            if (sameNames is not null &&
                sameNames.Id != find.Id
                )
                throw new NameDuplicateException($"name: {album.Name} is already in use");

            find.Status = album.Status;
            find.Name = album.Name;
            var update = _context.Albums.Update(find);
            await _context.SaveChangesAsync();
            return EntityMapper.Map(update.Entity);
        }
        public async Task<PublishAlbum> Update(Guid ownerId, string albumName, PublishAlbum album)
        {
            var find = await GetAlbumAsync(ownerId, albumName);
            return await Update(find.Id, album);
        }
        private async Task<PublishAlbumEntity> GetAlbumAsync(Guid albumId)
        {
            var album = await _context.Albums.FindAsync(albumId);
            await _context.Entry(album).Reference(e => e.User).LoadAsync();
            await _context.Entry(album).Collection(e => e.Publishes).LoadAsync();
            if (album is null)
                throw new ArgumentException("Invalid album");
            return album;
        }
        private async Task<PublishAlbumEntity> GetAlbumAsync(Guid ownerId, string albumName)
        {
            var album = _context.Albums.FirstOrDefault(e => e.User.Id == ownerId.ToString() && e.Name == albumName);
            if (album is null)
                throw new ArgumentException("Invalid album");
            return await GetAlbumAsync(album.Id);
        }
        private async Task<IEnumerable<PublishAlbumEntity>> GetAllAlbums(Guid ownerId)
        {
            return _context.Albums.Where(e => e.User.Id == ownerId.ToString()).Include(e => e.Publishes).Include(e => e.User);
        } 
    }
}
