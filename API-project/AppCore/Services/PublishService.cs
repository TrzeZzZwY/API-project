using AppCore.Interfaces.Services;
using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services
{
    public class PublishService : IPublishService
    {
        public Task<Publish> Create(Guid userID, string? albumName, Publish publish)
        {
            throw new NotImplementedException();
        }

        public Task<Publish> Delete(Guid ownerId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public Task<Publish> Delete(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Publish>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Publish>> GetAllFor(Guid ownerId, string? albumName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetImgPath(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<uint> GetLikes(Guid ownerId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public Task<uint> GetLikes(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<Publish> GetOne(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<Publish> GetOne(Guid albumId, string imageName)
        {
            throw new NotImplementedException();
        }

        public Task<Publish> GetOne(Guid ownerId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPrivate(Guid ownerId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPrivate(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserOwner(Guid userId, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<uint> Like(Guid ownerId, string? albumName, string imageName, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<uint> Like(Guid userId, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Move(string? targetAlbumName, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<Publish> Update(Guid publishId, Publish publish)
        {
            throw new NotImplementedException();
        }
    }
}
