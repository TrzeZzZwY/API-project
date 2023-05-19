using AppCore.Interfaces.Services;
using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services
{
    public class EfPublishService : IPublishService
    {
        public Publish Create(Guid userID, string? albumName, Publish publish)
        {
            throw new NotImplementedException();
        }

        public Publish Delete(Guid ownerId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public Publish Delete(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Publish> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Publish> GetAllFor(Guid ownerId, string? albumName)
        {
            throw new NotImplementedException();
        }

        public string GetImgPath(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public uint GetLikes(Guid ownerId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public uint GetLikes(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Publish GetOne(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Publish GetOne(Guid albumId, string imageName)
        {
            throw new NotImplementedException();
        }

        public Publish GetOne(Guid ownerId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public bool IsPrivate(Guid ownerId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public bool IsPrivate(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public bool IsUserOwner(Guid userId, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public uint Like(Guid ownerId, string? albumName, string imageName, Guid userId)
        {
            throw new NotImplementedException();
        }

        public uint Like(Guid userId, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public bool Move(string? targetAlbumName, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Publish Update(Guid publishId, Publish publish)
        {
            throw new NotImplementedException();
        }
    }
}
