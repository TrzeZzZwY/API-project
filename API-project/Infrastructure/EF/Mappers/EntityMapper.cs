using AppCore.Models;
using Infrastructure.EF.Entities;
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

        public EntityMapper(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public CommentEntity MapToEntity(Comment p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            var user = _userManager.FindByIdAsync(p.Id.ToString()).Result;
            if (user is null)
                throw new Exception("User Not Foud");
            return new CommentEntity(
                    id: p.Id,
                    user: user,
                    commentContent: p.Content,
                    isEdited: p.IsEdited,
                    comments: MapToEntity(p.Comments).ToHashSet()
                );
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

            return new PublishEntity(
                id: p.Id,
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                uploadDate: p.UploadDate,
                status: p.Status,
                userPublishLikes: p.UserPublishLikes.Select(
                    e => _userManager.FindByIdAsync(
                        e.ToString()
                        )
                    .Result)
                .ToHashSet(),// XD
                publishTags: MapToEntity(p.PublishTags).ToHashSet(),
                comments: MapToEntity(p.Comments).ToHashSet()
                );
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

            return new PublishTagEntity(
                id:p.Id,
                name:p.Name
                );
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

            return new PublishAlbumEntity(
                id: p.Id,
                name: p.Name,
                status: p.Status,
                publishes: MapToEntity(p.Publishes).ToHashSet()
                );
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

            return new Comment(
                    userId: Guid.Parse(p.User.Id),
                    content: p.CommentContent,
                    isEdited: p.IsEdited,
                    comments: MapFromEntity(p.Comments).ToHashSet()
                );
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
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                uploadDate: p.UploadDate,
                status: p.Status,
                userLikes: p.UserPublishLikes.Select(e =>Guid.Parse(e.Id)).ToHashSet(),
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
                    name:p.Name
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
