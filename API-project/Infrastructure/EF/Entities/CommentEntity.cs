using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class CommentEntity
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public bool IsEdited { get; set; }
        ISet<Comment> Comments { get; set; }
    }
}
