using AppCore.Commons.Exceptions;
using AppCore.Models;
using Infrastructure.EF;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.EfServiceTest
{
    public class EfAlbumTest
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public EfAlbumTest(AppDbContext context, UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Fact]
        public async Task<PublishAlbum> Create()
        {
            var album = new PublishAlbum
            {
                Id = Guid.NewGuid(),
                Name = "Piotr",
            };
            var entity = EntityMapper.Map(album);
            var user = await _userManager.FindByIdAsync(album.Id.ToString()) ?? throw new ArgumentException();
            entity.User = user;

            var created = _context.Albums.Add(entity);

            await _context.SaveChangesAsync();
            var mapped = EntityMapper.Map(created.Entity);
            return mapped;
        }
        [Fact]
        public async Task<PublishAlbum> Delete()
        {
            var album = new PublishAlbum
            {
                Id = Guid.NewGuid(),
                Name = "Piotr",
            };
            var entity = EntityMapper.Map(album);
            var user = await _userManager.FindByIdAsync(album.Id.ToString()) ?? throw new ArgumentException();
            entity.User = user;

            var created = _context.Albums.Add(entity);

            await _context.SaveChangesAsync();
            var mapped = EntityMapper.Map(created.Entity);
            return mapped;
        }
        
    }
}
