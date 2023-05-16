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
        public IEnumerable<PublishAlbum> GetAllFor(Guid userId);
        public IEnumerable<PublishAlbum> GetAllFor(string userLogin);

        public PublishAlbum GetByOne(Guid publishAlbumId);
        public PublishAlbum GetByOne(Guid userID,string albumName);
        public PublishAlbum GetByOne(string userLogin,string albumName);

        public PublishAlbum Create(Guid userId,PublishAlbum album);
        public PublishAlbum Create(string userLogin, PublishAlbum album);

        public PublishAlbum Delete(Guid userId, string albumName);
        public PublishAlbum Delete(string userLogin, string albumName);
        public PublishAlbum Delete(Guid albumId);

        public PublishAlbum Update(Guid albumId, PublishAlbum album);
    }
}
