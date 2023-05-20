using AppCore.Interfaces.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class PublishTag : IIdentity<Guid>
    {
        public PublishTag(string name,ISet<Publish>? publishes, Guid? id = null)
        {
            Id = id ?? Guid.Empty;
            Name = name;
            Publishes = publishes ?? new HashSet<Publish>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ISet<Publish> Publishes {get;set;}
    }
}
