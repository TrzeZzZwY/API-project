using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using AppCore.Models;
using AppCore.Models.Enums;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services
{
    public class EfPublishService : IPublishService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserEntity> _userManager;
        public EfPublishService(AppDbContext context, UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Publish> Create(Guid userId, Publish publish, string? albumName)
        {
            var entity = EntityMapper.Map(publish);
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentException();

            //sprawdzenie czy nazwa jest wolna w danym folderze
            var duplicate = await _context.Publishes.FirstOrDefaultAsync(e => e.User.Id == user.Id && e.ImageName == entity.ImageName);
            await _context.Entry(duplicate).Reference(e => e.Album).LoadAsync();
            if (duplicate is not null)
                if ((duplicate.Album is null && albumName is null) ||
                    (duplicate.Album is not null && albumName is not null && duplicate.Album.Name == albumName))
                    throw new NameDuplicateException($"name: {entity.ImageName} is already in use in that album");

            entity.User = user;
            if (albumName is not null)
            {
                var album = await _context.Albums.FirstOrDefaultAsync(e => e.User.Id == user.Id && e.Name == albumName)
                    ?? throw new ArgumentException(message: $"Invalid album name {albumName}");
                entity.Album = album;
            }
            entity.FileName = Guid.NewGuid().ToString();

            var saved = await _context.Publishes.AddAsync(entity);
            await _context.SaveChangesAsync();
            var mapped = EntityMapper.Map(saved.Entity);
            return mapped;
        }

        public async Task<Publish> Delete(Guid ownerId, string imageName, string? albumName)
        {
            var find = await FindPublish(ownerId, imageName, albumName);
            var removed = EntityMapper.Map(_context.Publishes.Remove(find).Entity);
            await _context.SaveChangesAsync();
            return removed;
        }

        public async Task<Publish> Delete(Guid publishId)
        {
            var find = await FindPublish(publishId);
            var removed = EntityMapper.Map(_context.Publishes.Remove(find).Entity);
            await _context.SaveChangesAsync();
            return removed;
        }

        public Task<IEnumerable<Publish>> GetAll()
        {
            return Task.FromResult(EntityMapper.Map(_context.Publishes
                        .Include(e => e.UserLikes)
                        .Include(e => e.Comments)
                        .Include(e => e.Album)
                        .Include(e => e.PublishTags)));
        }

        public Task<IEnumerable<Publish>> GetAllFor(Guid ownerId, string? albumName)
        {
            if (albumName is not null)
                return Task.FromResult(
                    EntityMapper.Map(
                        _context.Publishes
                        .Where(e => e.Album.Name == albumName && e.User.Id == ownerId.ToString())
                        .Include(e => e.UserLikes)
                        .Include(e => e.Album)
                        .Include(e => e.PublishTags)));
            return Task.FromResult(EntityMapper.Map(_context.Publishes
                        .Where(e => e.Album == null && e.User.Id == ownerId.ToString())
                        .Include(e => e.UserLikes)
                        .Include(e => e.Album)
                        .Include(e => e.PublishTags)));
        }

        public async Task<uint> GetLikes(Guid ownerId, string imageName, string? albumName)
        {
            var find = await FindPublish(ownerId, imageName, albumName);
            return (uint)find.UserLikes.Count;
        }

        public async Task<uint> GetLikes(Guid publishId)
        {
            var find = await FindPublish(publishId);
            return (uint)find.UserLikes.Count;
        }

        public async Task<Publish> GetOne(Guid publishId)
        {
            return EntityMapper.Map(await FindPublish(publishId));
        }

        public async Task<Publish> GetOne(Guid albumId, string imageName)
        {
            throw new NotImplementedException();
        }

        public async Task<Publish> GetOne(Guid ownerId, string imageName, string? albumName)
        {
            return EntityMapper.Map(await FindPublish(ownerId, imageName, albumName));
        }

        public async Task<bool> IsPrivate(Guid ownerId, string imageName, string? albumName)
        {
            var find = await FindPublish(ownerId, imageName, albumName);
            return find.Status == Status.Hidden;
        }

        public async Task<bool> IsPrivate(Guid publishId)
        {
            var find = await FindPublish(publishId);
            return find.Status == Status.Hidden;
        }

        public async Task<bool> IsUserOwner(Guid userId, Guid publishId)
        {
            var find = await FindPublish(publishId);
            return find.User.Id == userId.ToString();
        }

        public async Task<uint> Like(Guid ownerId, string imageName, string? albumName, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentException("user not found");
            var find = await FindPublish(ownerId, imageName, albumName);
            find.UserLikes.Add(user);
            _context.Update(find);
            await _context.SaveChangesAsync();
            return (uint)find.UserLikes.Count;
        }

        public async Task<uint> Like(Guid userId, Guid publishId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentException("user not found");
            var find = await FindPublish(publishId);
            find.UserLikes.Add(user);
            _context.Publishes.Update(find);
            await _context.SaveChangesAsync();
            return (uint)find.UserLikes.Count;
        }

        public async Task<bool> Move(string? targetAlbumName, Guid publishId)
        {
            var find = await FindPublish(publishId);
            if ((find.Album is null && targetAlbumName is null) ||
                (find.Album is not null && targetAlbumName is not null && find.Album.Name == targetAlbumName))
                return false;
            if (targetAlbumName is not null)
            {
                var album = await _context.Albums.FirstOrDefaultAsync(e => e.Name == targetAlbumName && e.User.Id == find.User.Id)
                    ?? throw new ArgumentException($"User don't have album named {targetAlbumName}");
                find.Album = album;
            }
            else
                find.Album = null;
            _context.Publishes.Update(find);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<uint> Unlike(Guid ownerId, string imageName, string? albumName, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentException("user not found");
            var find = await FindPublish(ownerId, imageName, albumName);
            find.UserLikes.Remove(user);
            _context.Publishes.Update(find);
            await _context.SaveChangesAsync();
            return (uint)find.UserLikes.Count;
        }

        public async Task<uint> Unlike(Guid publishId, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentException("user not found");
            var find = await FindPublish(publishId);
            find.UserLikes.Remove(user);
            _context.Publishes.Update(find);
            await _context.SaveChangesAsync();
            return (uint)find.UserLikes.Count;
        }

        public async Task<Publish> Update(Guid publishId, Publish publish)
        {
            var find = await FindPublish(publishId);
            find.Status = publish.Status;
            find.ImageName = publish.ImageName;
            find.Description = publish.Description;
            find.Camera = publish.Camera;
            find.PublishTags = EntityMapper.Map(publish.PublishTags).ToHashSet();
            _context.Publishes.Update(find);
            _context.SaveChanges();
            return EntityMapper.Map(find);
        }
        private async Task<PublishEntity> FindPublish(Guid id)
        {
            var find = await _context.Publishes.FindAsync(id) ?? throw new ArgumentException("Publish not found");
            await _context.Entry(find).Collection(e => e.Comments).LoadAsync();
            await _context.Entry(find).Collection(e => e.PublishTags).LoadAsync();
            await _context.Entry(find).Collection(e => e.UserLikes).LoadAsync();
            await _context.Entry(find).Reference(e => e.Album).LoadAsync();
            return find;
        }
        private async Task<PublishEntity> FindPublish(Guid ownerId, string imageName, string? albumName = null)
        {
            PublishEntity find;
            if (albumName is not null)
                find = await _context.Publishes.FirstOrDefaultAsync(e =>
                        e.User.Id == ownerId.ToString() &&
                        e.ImageName == imageName &&
                        e.Album.Name == albumName) ?? throw new ArgumentException("Publish not found");
            else
                find = await _context.Publishes.FirstOrDefaultAsync(e =>
                        e.User.Id == ownerId.ToString() &&
                        e.ImageName == imageName &&
                        e.Album == null) ?? throw new ArgumentException("Publish not found");
            return await FindPublish(find.Id);
        }
    }
}
