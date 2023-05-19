using AppCore.Interfaces.Services;
using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services
{
    public class EfAlbumService : IAlbumService
    {
        public PublishAlbum Create(Guid userId, PublishAlbum album)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum Delete(Guid ownerId, string albumName)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum Delete(Guid albumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PublishAlbum> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PublishAlbum> GetAllFor(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum GetByOne(Guid publishAlbumId)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum GetByOne(Guid ownerId, string albumName)
        {
            throw new NotImplementedException();
        }

        public bool IsPrivate(Guid ownerId, string albumName)
        {
            throw new NotImplementedException();
        }

        public bool IsPrivate(Guid albumId)
        {
            throw new NotImplementedException();
        }

        public bool IsUserOwner(Guid userId, Guid albumId)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum Update(Guid ownerId, PublishAlbum album)
        {
            throw new NotImplementedException();
        }
    }
}
