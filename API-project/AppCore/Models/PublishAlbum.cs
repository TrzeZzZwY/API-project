using AppCore.Interfaces.Identity;
using AppCore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class PublishAlbum : IIdentity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public Status Status { get; set; } = Status.Hidden;
        public ISet<Publish> Publishes { get; set; } = new HashSet<Publish>();
    }
}
