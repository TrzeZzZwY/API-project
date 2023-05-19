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
            //return new CommentEntity(
            //    );
            throw new NotImplementedException();
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

            /*return new PublishEntity(
                userPublishLikes: p.UserPublishLikes.Select(
                    e => _userManager.FindByIdAsync(
                        e.ToString()
                        )
                    .Result)
                .ToHashSet(),// XD

                );*/
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

            //return new PublishTagEntity(
            //    );
            throw new NotImplementedException();
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
