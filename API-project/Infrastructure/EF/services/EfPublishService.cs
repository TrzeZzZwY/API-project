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

        public Publish Create(string userLogin, string? albumName, Publish publish)
        {
            throw new NotImplementedException();
        }

        public Publish Delete(Guid userId, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public Publish Delete(string userLogin, string? albumName, string imageName)
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

        public IEnumerable<Publish> GetAllFor(Guid userId, string? albumName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Publish> GetAllFor(string userLogin, string? albumName)
        {
            throw new NotImplementedException();
        }

        public Publish GetByOne(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Publish GetByOne(Guid albumId, string imageName)
        {
            throw new NotImplementedException();
        }

        public Publish GetByOne(Guid userID, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public Publish GetByOne(string userLogin, string? albumName, string imageName)
        {
            throw new NotImplementedException();
        }

        public string GetImgPath(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public bool Move(Guid userId, string? targetAlbumName, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public bool Move(string userLogin, string? targetAlbumName, Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Publish Update(Guid publishId, Publish publish)
        {
            throw new NotImplementedException();
        }
    }
}
