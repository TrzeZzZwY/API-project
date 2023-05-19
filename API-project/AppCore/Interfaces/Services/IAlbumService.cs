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
        public IEnumerable<PublishAlbum> GetAll();
        public IEnumerable<PublishAlbum> GetAllFor(Guid ownerId);

        public PublishAlbum GetByOne(Guid publishAlbumId);
        public PublishAlbum GetByOne(Guid ownerId, string albumName);

        public PublishAlbum Create(Guid userId,PublishAlbum album);

        public PublishAlbum Delete(Guid ownerId, string albumName);
        public PublishAlbum Delete(Guid albumId);

        public PublishAlbum Update(Guid ownerId, PublishAlbum album);

        public bool IsUserOwner(Guid userId, Guid albumId);

        public bool IsPrivate(Guid ownerId, string albumName);
        public bool IsPrivate(Guid albumId);
    }
}
