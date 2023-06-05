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
        public Guid Id { get; set; }
        public Publish? Publish { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public bool IsEdited { get; set; } = false;
    }
}
