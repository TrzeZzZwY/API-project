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
        public Comment(Publish? publish, string content, bool? isEdited, Guid? id = null)
        {
            Id = id ?? Guid.Empty;
            Publish = publish;
            Content = content;
            IsEdited = isEdited ?? false;
        }

        public Guid Id { get; set; }
        public Publish? Publish { get; set; }
        public string Content { get; set; }
        public bool IsEdited { get; set; }
    }
}
