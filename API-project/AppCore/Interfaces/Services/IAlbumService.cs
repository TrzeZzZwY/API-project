using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Interfaces.Services
{
    public interface IAlbumService
    {
        public Task<IEnumerable<PublishAlbum>> GetAll();
        public Task<IEnumerable<PublishAlbum>> GetAllFor(Guid ownerId);

        public Task<PublishAlbum> GetOne(Guid publishAlbumId);
        public Task<PublishAlbum> GetOne(Guid ownerId, string albumName);
            
        public Task<PublishAlbum> Create(Guid userId,PublishAlbum album);
               
        public Task<PublishAlbum> Delete(Guid ownerId, string albumName);
        public Task<PublishAlbum> Delete(Guid albumId);

        public Task<PublishAlbum> Update(Guid albumId, PublishAlbum album);
        public Task<PublishAlbum> Update(Guid ownerId, string albumName, PublishAlbum album);
        public Task<bool> IsUserOwnerOrAdmin(Guid userId, Guid albumId);

        public Task<bool> IsPrivate(Guid ownerId, string albumName);
        public Task<bool> IsPrivate(Guid albumId);
    }
}
