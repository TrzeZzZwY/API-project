using AppCore.Commons.Exceptions;
using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services
{
    public class EfPublishService : IPublishService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserEntity> _userManager;
        public EfPublishService(AppDbContext context, UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Publish> Create(Guid userId, string? albumName, Publish publish)
        {
            var entity = EntityMapper.Map(publish);
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentException();

            //sprawdzenie czy nazwa jest wolna w danym folderze
            var duplicate = await _context.Publishes.FirstOrDefaultAsync(e =>e.User.Id == user.Id && e.ImageName == entity.ImageName);
            if(duplicate is not null)
                if((duplicate.Album is null && albumName is null) || (duplicate.Album.Name == albumName))
                    throw new NameDuplicateException($"name: {entity.ImageName} is already in use in that album");

            entity.User = user;
            if(albumName is not null)
            {
                var album = await _context.Albums.FirstOrDefaultAsync(e => e.User.Id == user.Id && e.Name == albumName) 
                    ?? throw new ArgumentException(message:$"Invalid album name {albumName}");
                entity.Album = album;
            }


            var saved = await _context.Publishes.AddAsync(entity);
            await _context.SaveChangesAsync();
            var mapped = EntityMapper.Map(saved.Entity);
            return mapped;
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
