using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Interfaces.Services
{
    public interface IPublishService
    {
        public IEnumerable<Publish> GetAll();
        public IEnumerable<Publish> GetAllFor(Guid ownerId, string? albumName);
        public Publish GetOne(Guid publishId);
        public Publish GetOne(Guid albumId, string imageName);
        public Publish GetOne(Guid ownerId, string? albumName, string imageName);

        public Publish Create(Guid userID, string? albumName, Publish publish);

        public Publish Delete(Guid ownerId, string? albumName, string imageName);
        public Publish Delete(Guid publishId);

        public Publish Update(Guid publishId, Publish publish);
 
        public bool Move(string? targetAlbumName, Guid publishId);

        public string GetImgPath(Guid publishId);

        public bool IsUserOwner(Guid userId, Guid publishId);

        public bool IsPrivate(Guid ownerId, string? albumName, string imageName);
        public bool IsPrivate(Guid publishId);

        public uint GetLikes(Guid ownerId, string? albumName, string imageName);
        public uint GetLikes(Guid publishId);

        public uint Like(Guid ownerId, string? albumName, string imageName,Guid userId);
        public uint Like(Guid userId, Guid publishId);
    }
}
