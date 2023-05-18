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
        public PublishTag(string name, Guid? id = null)
        {
            Id = id ?? Guid.Empty;
            Name = name;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
