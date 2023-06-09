using AppCore.Models.Enums;
using Infrastructure.EF;
using Infrastructure.EF.Entities;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Writers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace IntegrationTest
{
    internal class ImageAppTest
    {
        private readonly HttpClient _client;
        private readonly ImageAppTestFactory<Program> _app;
        private readonly AppDbContext _context;

        public ImageAppTest(ImageAppTestFactory<Program> app)
        {
            _app = app;
            _client = app.CreateClient();
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<UserEntity>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<UserRoleEntity>>();

                _context = scope.ServiceProvider.GetService<AppDbContext>();

                var AdminRole = new UserRoleEntity() { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" };
                var UserRole = new UserRoleEntity() { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" };

                roleManager.CreateAsync(AdminRole);
                roleManager.CreateAsync(UserRole);

                _context.SaveChanges();

                UserEntity UserEntity = new UserEntity() { Email = "user@app.com", UserName = "User" };
                UserEntity AdminEntity = new UserEntity() { Email = "admin@app.com", UserName = "Admin" };

                userManager.CreateAsync(UserEntity, "1qazXSW@");
                userManager.CreateAsync(AdminEntity, "1qazXSW@");
                userManager.AddToRoleAsync(UserEntity, "USER");
                userManager.AddToRoleAsync(AdminEntity, "ADMIN");

                InitData(userManager, _context);
            }
        }

        private void InitData(UserManager<UserEntity> userManager, AppDbContext context)
        {
            var admin = userManager.Users.FirstOrDefault(e => e.UserName == "Admin");
            var user = userManager.Users.FirstOrDefault(e => e.UserName == "User");

            //Albumy
            PublishAlbumEntity userPublicAlbum = new PublishAlbumEntity()
            {
                Id = Guid.NewGuid(),
                Name = "userPublicAlbum",
                Status = Status.Visible,
                Publishes = new HashSet<PublishEntity>(),
                User = user
            };
            PublishAlbumEntity userPrivateAlbum = new PublishAlbumEntity()
            {
                Id = Guid.NewGuid(),
                Name = "userPrivateAlbum",
                Status = Status.Hidden,
                Publishes = new HashSet<PublishEntity>(),
                User = user
            };
            PublishAlbumEntity adminPublicAlbum = new PublishAlbumEntity()
            {
                Id = Guid.NewGuid(),
                Name = "adminPublicAlbum",
                Status = Status.Visible,
                Publishes = new HashSet<PublishEntity>(),
                User = admin
            };
            PublishAlbumEntity adminPrivateAlbum = new PublishAlbumEntity()
            {
                Id = Guid.NewGuid(),
                Name = "adminPrivateAlbum",
                Status = Status.Hidden,
                Publishes = new HashSet<PublishEntity>(),
                User = admin
            };

            userPublicAlbum = _context.Albums.Add(userPublicAlbum).Entity;
            userPrivateAlbum = _context.Albums.Add(userPrivateAlbum).Entity;
            adminPublicAlbum = _context.Albums.Add(adminPublicAlbum).Entity;
            adminPrivateAlbum = _context.Albums.Add(adminPrivateAlbum).Entity;
            _context.SaveChanges();

            //Tagi
            PublishTagEntity tag1 = new PublishTagEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Animal",
                Publishes = new HashSet<PublishEntity>()
            };
            PublishTagEntity tag2 = new PublishTagEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Food",
                Publishes = new HashSet<PublishEntity>()
            };
            PublishTagEntity tag3 = new PublishTagEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Funny",
                Publishes = new HashSet<PublishEntity>()
            };

            tag1 = _context.Tags.Add(tag1).Entity;
            tag2 = _context.Tags.Add(tag2).Entity;
            tag3 = _context.Tags.Add(tag3).Entity;
            _context.SaveChanges();

            //Publikacje
            PublishEntity userPublicPublish = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "userPublicPublish",
                Description = "userPublicPublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Visible,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>()
                {
                    tag1,tag2
                },
                UserLikes = new HashSet<UserEntity>(),
                User = user,
                Album = userPublicAlbum
            };
            PublishEntity userPrivatePublish = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "userPrivatePublish",
                Description = "userPrivatePublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>(),
                UserLikes = new HashSet<UserEntity>(),
                User = user,
                Album = userPrivateAlbum
            };
            PublishEntity adminPublicPublish = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "adminPublicPublish",
                Description = "adminPublicPublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Visible,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag3 },
                UserLikes = new HashSet<UserEntity>(),
                User = admin,
                Album = adminPublicAlbum
            };
            PublishEntity adminPrivatePublish = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "adminPrivatePublish",
                Description = "adminPrivatePublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag2},
                UserLikes = new HashSet<UserEntity>(),
                User = admin,
                Album = adminPrivateAlbum
            };
            PublishEntity userPublicPublishNoAlbum = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "userPublicPublish",
                Description = "userPublicPublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Visible,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag1 },
                UserLikes = new HashSet<UserEntity>(),
                User = user
            };
            PublishEntity userPrivatePublishNoAlbum = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "userPrivatePublish",
                Description = "userPrivatePublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag3 },
                UserLikes = new HashSet<UserEntity>(),
                User = user
            };
            PublishEntity adminPublicPublishNoAlbum = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "adminPublicPublish",
                Description = "adminPublicPublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Visible,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>(),
                UserLikes = new HashSet<UserEntity>(),
                User = admin
            };
            PublishEntity adminPrivatePublishNoAlbum = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "adminPrivatePublish",
                Description = "adminPrivatePublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag1,tag3 },
                UserLikes = new HashSet<UserEntity>(),
                User = admin
            };

            userPublicPublish = _context.Publishes.Add(userPublicPublish).Entity;
            userPrivatePublish = _context.Publishes.Add(userPrivatePublish).Entity;
            adminPublicPublish = _context.Publishes.Add(adminPublicPublish).Entity;
            adminPrivatePublish = _context.Publishes.Add(adminPrivatePublish).Entity;
            userPublicPublishNoAlbum = _context.Publishes.Add(userPublicPublishNoAlbum).Entity;
            userPrivatePublishNoAlbum = _context.Publishes.Add(userPrivatePublishNoAlbum).Entity;
            adminPublicPublishNoAlbum = _context.Publishes.Add(adminPublicPublishNoAlbum).Entity;
            adminPrivatePublishNoAlbum = _context.Publishes.Add(adminPrivatePublishNoAlbum).Entity;
            _context.SaveChanges();



            //Komentarze
            CommentEntity entityComment1 = new CommentEntity()
            {
                Id = Guid.NewGuid(),
                CommentContent = "Nice photo!",
                IsEdited = false,
                User = user,
                Publish = adminPublicPublish
            };
            CommentEntity entityComment2 = new CommentEntity()
            {
                Id = Guid.NewGuid(),
                CommentContent = "Haha",
                IsEdited = false,
                User = user,
                Publish = adminPublicPublishNoAlbum
            };
            CommentEntity entityComment3 = new CommentEntity()
            {
                Id = Guid.NewGuid(),
                CommentContent = "XDD",
                IsEdited = false,
                User = admin,
                Publish = userPublicPublishNoAlbum
            };
            CommentEntity entityComment4 = new CommentEntity()
            {
                Id = Guid.NewGuid(),
                CommentContent = "Awesome",
                IsEdited = false,
                User = admin,
                Publish = userPublicPublish
            };



        }
    }
}
