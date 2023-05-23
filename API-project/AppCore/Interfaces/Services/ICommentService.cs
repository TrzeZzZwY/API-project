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
        public Task<IEnumerable<Comment>> GetAll();
        public Task<IEnumerable<Comment>> GetAllForPublish(Guid publishId);
        public Task<IEnumerable<Comment>> GetAllForComment(Guid commentId);
        public Task<Comment> GetOne(Guid commentId);
        public Task<Comment> Create(Comment comment, Guid publishId);
        public Task<Comment> Create(Comment comment, Guid albumId, string imageName);
        public Task<Comment> Create(Comment comment, Guid userId,string? albumName, string imageName);
        public Task<Comment> Create2(Comment comment, Guid commentId);
        public Task<Comment> Delete(Guid commentId);
        public Task<Comment> Update(Guid commentId, Comment comment);

    }
}
