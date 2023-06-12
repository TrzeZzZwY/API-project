using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Interfaces.Services
{
    public interface IPublishService
    {
        public Task<IEnumerable<Publish>> GetAll(Guid userId,IEnumerable<string>? tagNames, int page, int take);
        public Task<IEnumerable<Publish>> GetAll(Guid userId, Guid targetId, int page, int take);
        public Task<IEnumerable<Publish>> GetAllFor(Guid userId, Guid ownerId, string? albumName, IEnumerable<string>? tagNames, int page, int take);
        public Task<Publish> GetOne(Guid publishId);
        public Task<Publish> GetOne(Guid albumId, string imageName);
        public Task<Publish> GetOne(Guid ownerId, string imageName, string? albumName);
        public Task<Publish> Create(Guid userID, Publish publish, string? albumName);
        public Task<Publish> Delete(Guid ownerId, string imageName, string? albumName);
        public Task<Publish> Delete(Guid publishId);
        public Task<IEnumerable<Publish>> DeleteAll(Guid ownerId,string? albumName);
        public Task<Publish> Update(Guid publishId, Publish publish);
        public Task<bool> Move(string? targetAlbumName, Guid publishId);
        public Task<bool> IsUserOwner(Guid userId, Guid publishId);
        public Task<bool> IsPrivate(Guid ownerId, string imageName, string? albumName);
        public Task<bool> IsPrivate(Guid publishId);
        public Task<uint> GetLikes(Guid ownerId, string imageName, string? albumName);
        public Task<uint> GetLikes(Guid publishId);
        public Task<uint> Like(Guid ownerId, string imageName, string? albumName, Guid userId);
        public Task<uint> Like(Guid userId, Guid publishId);
        public Task<uint> Unlike(Guid ownerId, string imageName, string? albumName, Guid userId);
        public Task<uint> Unlike(Guid publishId, Guid userId);
    }
}
