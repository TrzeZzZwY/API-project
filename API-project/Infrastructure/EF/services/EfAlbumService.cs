using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services
{
    public class EfAlbumService : IAlbumService
    {

        private readonly UserManager<UserEntity> _userManager;
        private readonly AppDbContext _context;

        public EfAlbumService(UserManager<UserEntity> userManager,AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<PublishAlbum> Create(Guid userId, PublishAlbum album)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new Exception();

            var entity = EntityMapper.Map(album);
            entity.User = user;
            var created = _context.Albums.Add(entity);
            _context.SaveChanges();
            var mapped = EntityMapper.Map(created.Entity);
            return mapped;
        }

        public Task<PublishAlbum> Delete(Guid ownerId, string albumName)
        {
            throw new NotImplementedException();
        }

        public Task<PublishAlbum> Delete(Guid albumId)
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

        public Task<PublishAlbum> GetByOne(Guid publishAlbumId)
        {
            throw new NotImplementedException();
        }

        public Task<PublishAlbum> GetByOne(Guid ownerId, string albumName)
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
    }
}
