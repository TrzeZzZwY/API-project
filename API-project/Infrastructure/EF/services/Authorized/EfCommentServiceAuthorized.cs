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
    public class EfCommentServiceAuthorized : ServiceAuthorization
    {
        private readonly ICommentService _commentService;
        public EfCommentServiceAuthorized(UserManager<UserEntity> userManager, ICommentService commentService) : base(userManager)
        {
            _commentService = commentService;
        }

        public async Task<Comment> Create(Guid userId, Guid publishId, Comment comment)
        {
            return await _commentService.Create(userId, publishId, comment);
        }
        public async Task<Comment> Create(Guid userId, Guid ownerId, string imageName, string? albumName, Comment comment)
        {
            return await _commentService.Create(userId, ownerId, albumName, imageName, comment);
        }
        public async Task<Comment> Delete(Guid userId, Guid commentId)
        {
            if (await _commentService.IsUserOwner(userId, commentId) || await UserIsAdmin(userId))
                return await _commentService.Delete(commentId);
            throw new AccessViolationException();
        }
        public async Task<IEnumerable<Comment>> GetAll(Guid userId, int page, int take)
        {
            var all = await _commentService.GetAll(userId, page, take);
            return all;
        }
        public async Task<IEnumerable<Comment>> GetAllForPublish(Guid userId, Guid publishId, int page, int take)
        {
            var all = await _commentService.GetAllForPublish(userId, publishId, page, take);
            return all;
        }
        public async Task<IEnumerable<Comment>> GetAllForPublish(Guid userId, Guid ownerId, string imageName, string? albumName, int page, int take)
        {
            var all = await _commentService.GetAllForPublish(userId, ownerId, albumName, imageName, page, take);
            return all;
        }
        public async Task<IEnumerable<Comment>> GetAllForUser(Guid userId, Guid targerUserId, int page, int take)
        {
            var all = await _commentService.GetAllForUser(userId, targerUserId, page, take);
            return all;
        }
        public async Task<Comment> GetOne(Guid userId, Guid commentId)
        {
            if (await _commentService.IsUserOwner(userId, commentId) || await UserIsAdmin(userId))
                return await _commentService.GetOne(commentId);
            throw new AccessViolationException();
        }
        public async Task<Comment> Update(Guid userId, Guid commentId, Comment comment)
        {
            if (await _commentService.IsUserOwner(userId, commentId) || await UserIsAdmin(userId))
                return await _commentService.Update(commentId, comment);
            throw new AccessViolationException();
        }
    }
}
