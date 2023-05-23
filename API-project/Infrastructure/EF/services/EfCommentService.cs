using AppCore.Interfaces.Services;
using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services
{
    public class EfCommentService : ICommentService
    {
        public Task<Comment> Create(Comment comment, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> Create(Comment comment, Guid albumId, string imageName)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> Create(Comment comment, Guid userId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> Create2(Comment comment, Guid commentId)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> Delete(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetAllForComment(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetAllForPublish(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetOne(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> Update(Guid commentId, Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
