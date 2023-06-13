using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using AppCore.Models;
using AppCore.Models.Enums;
using Azure;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            if (duplicate is not null)
            {
                await _context.Entry(duplicate).Reference(e => e.Album).LoadAsync();
                if ((duplicate.Album is null && albumName is null) || (duplicate.Album is not null && albumName is not null && duplicate.Album.Name == albumName))
                    throw new NameDuplicateException($"name: {entity.ImageName} is already in use in that album");
            }

            entity.User = user;
            //sprtawdzenie czy podany album istnieje
            if (albumName is not null)
            {
                var album = await _context.Albums.FirstOrDefaultAsync(e => e.User.Id == user.Id && e.Name == albumName)
                    ?? throw new ArgumentException(message: $"Invalid album name {albumName}");
                if (album.Status == Status.Hidden && entity.Status == Status.Visible)
                    throw new Exception("cannot put visible publish to hidden album");
                entity.Album = album;
            }

            //przypisanie tagów
            HashSet<PublishTagEntity> tags = new HashSet<PublishTagEntity>();
            foreach (var item in publish.PublishTags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(e => e.Name == item.Name);
                if (tag is not null)
                    tags.Add(tag);
            }
            entity.PublishTags = tags;

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

        public async Task<IEnumerable<Publish>> DeleteAll(Guid ownerId, string? albumName)
        {
            var all = findAll(ownerId, albumName);
            var copy = all.ToList();
            _context.Publishes.RemoveRange(all);
            await _context.SaveChangesAsync();
            return EntityMapper.Map(copy);
        }

        public async Task<IEnumerable<Publish>> GetAll(Guid userId, IEnumerable<string>? tagNames, int page, int take)
        {
            var query = _context.Publishes.Include(e => e.UserLikes)
                                          .Include(e => e.Comments)
                                          .Include(e => e.Album)
                                          .Include(e => e.PublishTags)
                                          .Include(e => e.User);
            var acces = query.Where(e => (e.Status == Status.Visible) ||
                                        (e.User.Id == userId.ToString()) ||
                                        (_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId.ToString()).Result, "Admin").Result));

            if (tagNames is not null && tagNames.Count() > 0)
            {
                var tags = _context.Tags.Where(e => tagNames.Contains(e.Name)).ToList();
                acces = acces.Where(e => e.PublishTags.Any(x => tags.Contains(x)));
            }


            var publishes = QueryFilter.Paginate(acces, page, take);

            foreach (var item in publishes)
            {
                await _context.Entry(item).Collection(e => e.Comments).LoadAsync();
                foreach (var comment in item.Comments)
                {
                    await _context.Entry(comment).Reference(e => e.User).LoadAsync();
                }
            }
            return EntityMapper.Map(publishes.ToList());
        }
        public async Task<IEnumerable<Publish>> GetAll(Guid userId, Guid targetId, int page, int take)
        {
            IQueryable<PublishEntity> query = _context.Publishes
                        .Include(e => e.UserLikes)
                        .Include(e => e.Comments)
                        .Include(e => e.Album)
                        .Include(e => e.PublishTags)
                        .Include(e => e.User)
                        .Where(e => e.User.Id == targetId.ToString());

            var test1 = query.ToList();
            var acces = query.Where(e => (e.Status == Status.Visible) ||
                                         (e.User.Id == userId.ToString()) ||
                                         (_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId.ToString()).Result, "Admin").Result));
            var test2 = acces.ToList();
            var publishes = QueryFilter.Paginate(acces, page, take);

            foreach (var item in publishes)
            {
                await _context.Entry(item).Collection(e => e.Comments).LoadAsync();
                foreach (var comment in item.Comments)
                {
                    await _context.Entry(comment).Reference(e => e.User).LoadAsync();
                }
            }
            var list = publishes.ToList();
            return EntityMapper.Map(list);
        }
        public async Task<IEnumerable<Publish>> GetAllFor(Guid userId, Guid ownerId, string? albumName, IEnumerable<string>? tagNames, int page, int take)
        {
            IQueryable<PublishEntity> query =
                (albumName is not null) ?
                _context.Publishes.Include(e => e.UserLikes)
                                  .Include(e => e.Comments)
                                  .Include(e => e.Album)
                                  .Include(e => e.PublishTags)
                                  .Include(e => e.User)
                                  .Where(e => e.Album.Name == albumName && e.User.Id == ownerId.ToString())
                                  :
                _context.Publishes.Include(e => e.UserLikes)
                                  .Include(e => e.Comments)
                                  .Include(e => e.Album)
                                  .Include(e => e.PublishTags)
                                  .Include(e => e.User)
                                  .Where(e => e.Album == null && e.User.Id == ownerId.ToString());
            var acces = query.Where(e => (e.Status == Status.Visible) ||
                                         (e.User.Id == userId.ToString()) ||
                                         (_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId.ToString()).Result, "Admin").Result));
            if (tagNames is not null && tagNames.Count() > 0)
            {
                var tags = _context.Tags.Where(e => tagNames.Contains(e.Name)).ToList();
                acces = acces.Where(e => e.PublishTags.Any(x => tags.Contains(x)));
            }

            var publishes = QueryFilter.Paginate(acces, page, take);

            foreach (var item in publishes)
            {
                await _context.Entry(item).Collection(e => e.Comments).LoadAsync();
                foreach (var comment in item.Comments)
                {
                    await _context.Entry(comment).Reference(e => e.User).LoadAsync();
                }
            }
            var list = publishes.ToList();
            return EntityMapper.Map(list);
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
            PublishAlbumEntity album;
            bool duplucate = false;
            if (targetAlbumName is not null)
            {
                album = await _context.Albums.FirstOrDefaultAsync(e => e.Name == targetAlbumName && e.User.Id == find.User.Id)
                   ?? throw new ArgumentException($"User don't have album named {targetAlbumName}");
                await _context.Entry(album).Collection(e => e.Publishes).LoadAsync();
                if (album.Publishes.Any(e => e.ImageName == find.ImageName))
                    throw new ArgumentException($"album {album.Name} already contrains publish named {find.ImageName}");
                if (album.Status == Status.Hidden && find.Status == Status.Visible)
                    throw new Exception("cannot put visible publish to hidden album");
            }
            else
            {
                album = null;
                if (_context.Publishes.Any(e => e.User.Id == find.User.Id && e.Album == null && e.ImageName == find.ImageName))
                    throw new ArgumentException($"primary album already contrains publish named {find.ImageName}");
            }

            find.Album = album;
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
            if (_context.Publishes.Any(e => e.User.Id == find.User.Id && e.Album == find.Album && e.ImageName == publish.ImageName && e.Id != find.Id))
                throw new ArgumentException($"name {publish.ImageName} is already takien in that folder");
            find.Status = publish.Status;
            find.ImageName = publish.ImageName;
            find.Description = publish.Description;
            find.Camera = publish.Camera;
            HashSet<PublishTagEntity> tags = new HashSet<PublishTagEntity>();
            foreach (var item in publish.PublishTags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(e => e.Name == item.Name);
                if (tag is not null)
                    tags.Add(tag);
            }
            find.PublishTags = tags;
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
            await _context.Entry(find).Reference(e => e.User).LoadAsync();
            await _context.Entry(find).Reference(e => e.Album).LoadAsync();
            foreach (var item in find.Comments)
            {
                await _context.Entry(item).Reference(e => e.User).LoadAsync();
            }
            return find;
        }
        private async Task<PublishEntity> FindPublish(Guid ownerId, string imageName, string? albumName = null)
        {
            PublishEntity find;
            if (albumName is not null)
                find = await _context.Publishes.FirstOrDefaultAsync(e =>
                        e.User.Id == ownerId.ToString() &&
                        e.ImageName == imageName &&
                        e.Album.Name == albumName);
            else
                find = await _context.Publishes.FirstOrDefaultAsync(e =>
                        e.User.Id == ownerId.ToString() &&
                        e.ImageName == imageName &&
                        e.Album == null);
            if (find is null)
                throw new Exception("Publish not found");
            return await FindPublish(find.Id);
        }
        private IEnumerable<PublishEntity> findAll(Guid ownerId, string? albumName)
        {
            List<PublishEntity> find = new List<PublishEntity>();
            if (albumName is not null)
                find = _context.Publishes.Where(e =>
                        e.User.Id == ownerId.ToString() &&
                        e.Album.Name == albumName).ToList();
            else
                find = _context.Publishes.Where(e =>
                        e.User.Id == ownerId.ToString() &&
                        e.Album == null).ToList();
            if (find is null)
                throw new Exception("Publish not found");
            return find;
        }
    }
}
