using AppCore.Interfaces.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class Comment : IIdentity<Guid>
    {
        public Comment(Guid userId, string content, bool? isEdited, ISet<Comment>? comments, Guid? id = null)
        {
            Id = id ?? Guid.Empty;
            UserId = userId;
            Content = content;
            IsEdited = isEdited ?? false;
            Comments = comments ?? new HashSet<Comment>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public bool IsEdited { get; set; }
        public ISet<Comment> Comments { get; set; }
    }
}
