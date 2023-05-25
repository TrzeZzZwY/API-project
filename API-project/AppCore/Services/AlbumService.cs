using AppCore.Interfaces.Services;
using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services
{
    public class AlbumService : IAlbumService
    {
        public Task<PublishAlbum> Create(Guid userId, PublishAlbum album)
        {
            throw new NotImplementedException();
        }

        public Task<PublishAlbum> Delete(Guid ownerId, string albumName)
        {
            throw new NotImplementedException();
        }

        public Task<PublishAlbum> Delete(Guid albumId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PublishAlbum>> DeleteAll(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PublishAlbum>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PublishAlbum>> GetAllFor(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public Task<PublishAlbum> GetOne(Guid publishAlbumId)
        {
            throw new NotImplementedException();
        }

        public Task<PublishAlbum> GetOne(Guid ownerId, string albumName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPrivate(Guid ownerId, string albumName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPrivate(Guid albumId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserOwner(Guid userId, Guid albumId)
        {
            throw new NotImplementedException();
        }

        public Task<PublishAlbum> Update(Guid ownerId, PublishAlbum album)
        {
            throw new NotImplementedException();
        }

        public Task<PublishAlbum> Update(Guid ownerId, string albumName, PublishAlbum album)
        {
            throw new NotImplementedException();
        }
    }
}
