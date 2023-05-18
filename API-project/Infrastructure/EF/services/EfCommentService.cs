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
        public Comment Create(Comment comment, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Comment Create(Comment comment, Guid albumId, string imageName)
        {
            throw new NotImplementedException();
        }

        public Comment Create(Comment comment, Guid userId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public Comment Create2(Comment comment, Guid commentId)
        {
            throw new NotImplementedException();
        }

        public Comment Delete(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetAllForComment(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetAllForPublish(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Comment GetOne(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public Comment Update(Guid commentId, Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
