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
        public IEnumerable<Comment> GetAll();
        public IEnumerable<Comment> GetAllForPublish(Guid publishId);
        public IEnumerable<Comment> GetAllForComment(Guid commentId);

        public Comment GetOne(Guid commentId);

        public Comment Create(Comment comment, Guid publishId);
        public Comment Create(Comment comment, Guid albumId, string imageName);
        public Comment Create(Comment comment, Guid userId,string? albumName, string imageName);
        public Comment Create2(Comment comment, Guid commentId);

        public Comment Delete(Guid commentId);

        public Comment Update(Guid commentId, Comment comment);

    }
}
