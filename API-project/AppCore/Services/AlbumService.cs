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
        public PublishAlbum Create(Guid userId, PublishAlbum album)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum Create(string userLogin, PublishAlbum album)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum Delete(Guid userId, string albumName)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum Delete(string userLogin, string albumName)
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

        public IEnumerable<PublishAlbum> GetAllFor(Guid userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PublishAlbum> GetAllFor(string userLogin)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum GetByOne(Guid publishAlbumId)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum GetByOne(Guid userID, string albumName)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum GetByOne(string userLogin, string albumName)
        {
            throw new NotImplementedException();
        }

        public PublishAlbum Update(Guid albumId, PublishAlbum album)
        {
            throw new NotImplementedException();
        }
    }
}
