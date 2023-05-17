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
        public PublishAlbum(string name, Status? status, ISet<Publish>? publishes)
        {
            Name = name;
            Status = status ?? Status.private_publish;
            Publishes = publishes ?? new HashSet<Publish>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        ISet<Publish> Publishes { get; set; }
    }
}
