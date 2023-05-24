using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;

namespace Infrastructure.EF.Services
{
    public class EfTagService : ITagService
    {
        private readonly AppDbContext _context;

        public EfTagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PublishTag> Create(PublishTag tag)
        {
            if (_context.Tags.Any(e => e.Name == tag.Name))
                throw new NameDuplicateException($"name: {tag.Name} is already in use");
            var entity = EntityMapper.Map(tag);
            var added = await _context.Tags.AddAsync(entity);
            _context.SaveChanges();
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
            var tag = FindTag(tagName);
            return await Delete(tag.Id);
        }

        public Task<IEnumerable<PublishTag>> GetAll()
        {
            return Task.FromResult(EntityMapper.Map(_context.Tags));
        }

        public async Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid tagId)
        {
            var tag = await FindTag(tagId);
            await _context.Entry(tag).Collection(e => e.Publishes).LoadAsync();
            return EntityMapper.Map(tag.Publishes);
        }

        public async Task<IEnumerable<Publish>> GetAllPublishesForTag(string tagName)
        {
            var tag = FindTag(tagName);
            return await GetAllPublishesForTag(tag.Id);
        }

        public async Task<PublishTag> GetOne(Guid tagId)
        {
            var tag = await FindTag(tagId);
            return EntityMapper.Map(tag);
        }

        public async Task<PublishTag> GetOne(string tagName)
        {
            var tag = FindTag(tagName);
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
            var find = FindTag(tagName);
            return await Update(find.Id, tag);
        }

        private async Task<PublishTagEntity> FindTag(Guid tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag is null)
                throw new ArgumentException();
            return tag;
        }
        private PublishTagEntity FindTag(string tagName)
        {
            var tag = _context.Tags.FirstOrDefault(e => e.Name == tagName);
            if (tag is null)
                throw new ArgumentException();
            return tag;
        }
    }
}
