using AppCore.Models.Enums;
using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class PublishAlbumEntity
    {
        public PublishAlbumEntity()
        {
            
        }
        public PublishAlbumEntity(Guid id, string name, Status status, ISet<PublishEntity>? publishes)
        {
            Id = id;
            Name = name;
            Status = status;
            Publishes = publishes ?? new HashSet<PublishEntity>();
        }

        public Guid Id { get; set; }
        public UserEntity User { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public ISet<PublishEntity> Publishes { get; set; }
    }
}
