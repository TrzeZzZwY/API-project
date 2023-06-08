using AppCore.Interfaces.Services;
using AppCore.Models;
using AppCore.Models.Enums;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services
{
    public class EfCommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public EfCommentService(AppDbContext context, UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Comment> Create(Guid userId, Guid publishId, Comment comment)
        {
            var entity = EntityMapper.Map(comment);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var publish = await _context.Publishes.FirstOrDefaultAsync(e => e.Id == publishId) ?? throw new ArgumentException("No such publish");
            await _context.Entry(publish).Collection(e => e.Comments).LoadAsync();
            entity.User = user;

            publish.Comments.Add(entity);
            _context.Publishes.Update(publish);
            await _context.SaveChangesAsync();
            return EntityMapper.Map(entity);
        }

        public async Task<Comment> Create(Guid userId, Guid ownerId, string? albumName, string imageName, Comment comment)
        {
            var publish = _context.Publishes.FirstOrDefault(e => e.User.Id == ownerId.ToString() && e.Album.Name == albumName && e.ImageName == imageName)
                ?? throw new ArgumentException("no such publish");
            return await Create(userId, publish.Id, comment);
        }

        public async Task<Comment> Delete(Guid commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(e => e.Id == commentId) ?? throw new ArgumentException("No such comment");
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return EntityMapper.Map(comment);
        }

        public Task<IEnumerable<Comment>> GetAll(Guid userId, int page, int take)
        {
            var query = _context.Comments;
            var acces = query.Where(e =>
              (e.Publish.Status == Status.Visible) ||
              (e.Publish.User.Id == userId.ToString()) ||
              (_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId.ToString()).Result, "Admin").Result));

            var publishes = QueryFilter.Paginate(acces, page, take).ToList();
            return Task.FromResult(EntityMapper.Map(publishes));
        }

        public Task<IEnumerable<Comment>> GetAllForPublish(Guid userId, Guid publishId, int page, int take)
        {
            var query = _context.Comments.Where(e => e.Publish.Id == publishId);
            var acces = query.Where(e =>
              (e.Publish.Status == Status.Visible) ||
              (e.Publish.User.Id == userId.ToString()) ||
              (_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId.ToString()).Result, "Admin").Result));

            var publishes = QueryFilter.Paginate(acces, page, take).ToList();
            return Task.FromResult(EntityMapper.Map(publishes));
        }

        public async Task<IEnumerable<Comment>> GetAllForPublish(Guid userId, Guid ownerId, string? albumName, string imageName, int page, int take)
        {
            var publish = await _context.Publishes.FirstOrDefaultAsync(e => e.User.Id == ownerId.ToString() && e.Album.Name == albumName && e.ImageName == imageName)
                ?? throw new Exception("no such publish");
            return await GetAllForPublish(userId, publish.Id, page, take);
        }

        public Task<IEnumerable<Comment>> GetAllForUser(Guid userId,Guid TargetUserId, int page, int take)
        {
            var query = _context.Comments.Where(e => e.User.Id == TargetUserId.ToString());
            var acces = query.Where(e =>
              (e.Publish.Status == Status.Visible) ||
              (e.Publish.User.Id == userId.ToString()) ||
              (_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId.ToString()).Result, "Admin").Result));

            var publishes = QueryFilter.Paginate(acces, page, take).ToList();
            return Task.FromResult(EntityMapper.Map(publishes));
        }

        public async Task<Comment> GetOne(Guid commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(e => e.Id == commentId) ?? throw new ArgumentException("no such comment");
            return EntityMapper.Map(comment);
        }

        public async Task<bool> IsUserOwner(Guid userId, Guid commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(e => e.Id == commentId) ?? throw new ArgumentException("no such comment");
            return comment.User.Id == userId.ToString();
        }

        public async Task<Comment> Update(Guid commentId, Comment comment)
        {
            var old = await _context.Comments.FirstOrDefaultAsync(e => e.Id == commentId) ?? throw new ArgumentException("no such comment");
            old.CommentContent = comment.Content;
            old.IsEdited = true;
            _context.Comments.Update(old);
            await _context.SaveChangesAsync();
            return EntityMapper.Map(old);
        }
    }
}
