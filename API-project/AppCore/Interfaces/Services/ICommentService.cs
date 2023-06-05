using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Interfaces.Services
{
    public interface ICommentService
    {
        public Task<IEnumerable<Comment>> GetAll(Guid userId, int page, int take);
        public Task<IEnumerable<Comment>> GetAllForPublish(Guid userId, Guid publishId, int page, int take);
        public Task<IEnumerable<Comment>> GetAllForPublish(Guid userId, Guid ownerId, string? albumName, string imageName, int page, int take);
        public Task<IEnumerable<Comment>> GetAllForUser(Guid userId, Guid TargetUserId, int page, int take);
        public Task<Comment> GetOne(Guid commentId);
        public Task<Comment> Create(Guid userId, Guid publishId, Comment comment);
        public Task<Comment> Create(Guid userId, Guid ownerId, string? albumName, string imageName, Comment comment);
        public Task<Comment> Delete(Guid commentId);
        public Task<Comment> Update(Guid commentId, Comment comment);
        public Task<bool> IsUserOwner(Guid userId, Guid commentId);
    }
}
