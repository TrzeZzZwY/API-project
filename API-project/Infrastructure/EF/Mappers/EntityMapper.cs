using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.services;
using Infrastructure.EF.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Mappers
{
    public class EntityMapper
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EfPublishService _publishService;
        private readonly EfTagService _tagService;

        public EntityMapper(UserManager<UserEntity> userManager, EfPublishService publishService, EfTagService tagService)
        {
            _userManager = userManager;
            _publishService = publishService;
            _tagService = tagService;
        }

        public CommentEntity MapToEntity(Comment p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            var user = _userManager.FindByIdAsync(p.Id.ToString()).Result;
            if (user is null)
                throw new Exception("User Not Foud");
            return new CommentEntity()
            {
                User = user,
                publish = MapToEntity(_publishService.GetOne(p.PublishId)),
                CommentContent = p.Content,
                IsEdited = p.IsEdited                
            };
        }
        public IEnumerable<CommentEntity> MapToEntity(IEnumerable<Comment> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return MapToEntity(item);
        }
        public PublishEntity MapToEntity(Publish p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new PublishEntity()
            {
                Camera = p.Camera,
                Comments = MapToEntity(p.Comments).ToHashSet(),
                Description = p.Description,
                ImageName = p.ImageName,
                PublishTags = MapToEntity(p.PublishTags).ToHashSet(),
                Status = p.Status,
                UploadDate = p.UploadDate,
                User =_userManager.FindByIdAsync(p.UserId.ToString()).Result,
                UserLikes = p.UserPublishLikes.Select(
                    e => _userManager.FindByIdAsync(
                        e.ToString()
                        )
                    .Result)
                .ToHashSet(),// XD
            };
            throw new NotImplementedException();
        }
        public IEnumerable<PublishEntity> MapToEntity(IEnumerable<Publish> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return MapToEntity(item);
        }
        public PublishTagEntity MapToEntity(PublishTag p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new PublishTagEntity()
            {
                Name = p.Name
                //Publishes = MapToEntity(_tagService.GetAllPublishesForTag(p.Id)).ToHashSet()
            };

        }
        public IEnumerable<PublishTagEntity> MapToEntity(IEnumerable<PublishTag> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return MapToEntity(item);
        }
        public PublishAlbumEntity MapToEntity(PublishAlbum p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            //return new PublishAlbumEntity(
            //    );
            throw new NotImplementedException();
        }
        public IEnumerable<PublishAlbumEntity> MapToEntity(IEnumerable<PublishAlbum> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return MapToEntity(item);
        }

        public Comment MapFromEntity(CommentEntity p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            //return new Comment(
            //    );
            throw new NotImplementedException();
        }
        public IEnumerable<Comment> MapFromEntity(IEnumerable<CommentEntity> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return MapFromEntity(item);
        }
        public Publish MapFromEntity(PublishEntity p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new Publish(
                id: p.Id,
                userId: Guid.Parse(p.User.Id),
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                uploadDate: p.UploadDate,
                status: p.Status,
                userLikes: p.UserLikes.Select(e =>Guid.Parse(e.Id)).ToHashSet(),
                publishTags: MapFromEntity(p.PublishTags).ToHashSet(),
                comments: MapFromEntity(p.Comments).ToHashSet()
                );
        }
        public IEnumerable<Publish> MapFromEntity(IEnumerable<PublishEntity> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return MapFromEntity(item);
        }
        public PublishTag MapFromEntity(PublishTagEntity p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new PublishTag(
                    id:p.Id,
                    name:p.Name,
                    publishes:null
                );
        }
        public IEnumerable<PublishTag> MapFromEntity(IEnumerable<PublishTagEntity> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return MapFromEntity(item);
        }
        public PublishAlbum MapFromEntity(PublishAlbumEntity p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new PublishAlbum(
                    id: p.Id,
                    name: p.Name,
                    status:p.Status,
                    publishes:MapFromEntity(p.Publishes).ToHashSet()
                );
        }
        public IEnumerable<PublishAlbum> MapFromEntity(IEnumerable<PublishAlbumEntity> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return MapFromEntity(item);
        }
    }
}
