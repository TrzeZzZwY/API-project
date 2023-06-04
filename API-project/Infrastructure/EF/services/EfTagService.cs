using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using AppCore.Models;
using AppCore.Models.Enums;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Services
{
    public class EfTagService : ITagService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public EfTagService(AppDbContext context, UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PublishTag> Create(PublishTag tag)
        {
            if (_context.Tags.Any(e => e.Name == tag.Name))
                throw new NameDuplicateException($"name: {tag.Name} is already in use");
            var entity = EntityMapper.Map(tag);
            var added = await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
            var mapped = EntityMapper.Map(added.Entity);
            return mapped;
        }

        public async Task<PublishTag> Delete(Guid tagId)
        {
            var find = await FindTag(tagId);
            var deleted = _context.Tags.Remove(find);
            await _context.SaveChangesAsync();

            var mapped = EntityMapper.Map(deleted.Entity);
            return mapped;
        }

        public async Task<PublishTag> Delete(string tagName)
        {
            var tag = await FindTag(tagName);
            return await Delete(tag.Id);
        }

        public async Task<IEnumerable<PublishTag>> GetAll(Guid userId, int page, int take)
        {
            var query = _context.Tags.Include(e => e.Publishes);
            var tags = await QueryFilter.Paginate(query, page, take).ToListAsync();
            return EntityMapper.Map(tags);
        }

        public async Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid userId, Guid tagId, int page, int take)
        {
            var tag = await FindTag(tagId);
            var query = _context.Publishes.Where(e => e.PublishTags.Contains(tag))
                    .Include(e => e.UserLikes)
                    .Include(e => e.Comments)
                    .Include(e => e.Album)
                    .Include(e => e.PublishTags);
            var acces = query.Where(e =>
                    (e.Status == Status.Visible) ||
                    (e.User.Id == userId.ToString()) ||
                    (_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId.ToString()).Result, "Admin").Result));
            var tags = await QueryFilter.Paginate(acces, page, take).ToListAsync();
            return EntityMapper.Map(tags);
        }

        public async Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid userId, string tagName, int page, int take)
        {
            var tag = await FindTag(tagName);
            return await GetAllPublishesForTag(userId, tag.Id, page, take);
        }

        public async Task<PublishTag> GetOne(Guid tagId)
        {
            var tag = await FindTag(tagId);
            return EntityMapper.Map(tag);
        }

        public async Task<PublishTag> GetOne(string tagName)
        {
            var tag = await FindTag(tagName);
            return await GetOne(tag.Id);
        }

        public async Task<PublishTag> Update(Guid tagId, PublishTag tag)
        {
            var find = await FindTag(tagId);
            var sameName = _context.Tags.FirstOrDefault(e => e.Name == tag.Name);
            if (sameName is not null && sameName.Id != tagId)
                throw new NameDuplicateException($"name: {tag.Name} is already in use");

            find.Name = tag.Name;
            var updated = _context.Tags.Update(find);
            await _context.SaveChangesAsync();
            return EntityMapper.Map(updated.Entity);
        }

        public async Task<PublishTag> Update(string tagName, PublishTag tag)
        {
            var find = await FindTag(tagName);
            return await Update(find.Id, tag);
        }

        private async Task<PublishTagEntity> FindTag(Guid tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag is null)
                throw new ArgumentException();
            return tag;
        }
        private async Task<PublishTagEntity> FindTag(string tagName)
        {
            var tag = _context.Tags.FirstOrDefault(e => e.Name == tagName);
            if (tag is null)
                throw new ArgumentException();
            await _context.Entry(tag).Collection(e => e.Publishes).LoadAsync();
            return tag;
        }
    }
}
