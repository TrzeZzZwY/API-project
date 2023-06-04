using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Infrastructure.EF.Services;
using Microsoft.AspNetCore.Identity;
using WebApi.Dto.Input;
using WebApi.Dto.Output;

namespace WebApi.Dto.Mappers
{
    public static class DtoMapper
    {
        public static PublishAlbumOutputDto Map(PublishAlbum p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishAlbumOutputDto()
            {
                Name = p.Name,
                UserName = p.UserName,
                Status = p.Status,
                Publishes = p.Publishes is null ? new List<PublishOutputDto>() : Map(p.Publishes).ToHashSet()
            };
        }
        public static IEnumerable<PublishAlbumOutputDto> Map(IEnumerable<PublishAlbum> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var item in p)
                yield return Map(item);
        }
        public static PublishOutputDto Map(Publish p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishOutputDto()
            {
                ImageName = p.ImageName,
                UserName = p.UserName,
                Camera = p.Camera,
                Description = p.Description,
                UploadDate = p.UploadDate,
                Status = p.Status,
                Likes = p.UserPublishLikes is null ? 0 : (uint)p.UserPublishLikes.Count(),
                Tags = p.PublishTags is null ? new List<PublishTagOutputDto>() : Map(p.PublishTags),
                Comments = p.Comments is null ? new List<CommentOutputDto>() : Map(p.Comments)
            };
        }
        public static IEnumerable<PublishOutputDto> Map(IEnumerable<Publish> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var publish in p)
                yield return Map(publish);
        }
        public static CommentOutputDto Map(Comment p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new CommentOutputDto() {
                UserLogin = "",
                CommentContent = p.Content
                };
        }
        public static IEnumerable<CommentOutputDto> Map(IEnumerable<Comment> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
        public static PublishTagOutputDto Map(PublishTag p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            return new PublishTagOutputDto() { Name = p.Name };
        }
        public static IEnumerable<PublishTagOutputDto> Map(IEnumerable<PublishTag> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var item in p)
                yield return Map(item);
        }
        public static PublishAlbum Map(PublishAlbumInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishAlbum()
            {
                Name = p.Name,
                Status = p.Status,
                Publishes = new HashSet<Publish>()
            };
        }
        public static IEnumerable<PublishAlbum> Map(IEnumerable<PublishAlbumInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
        public static Publish Map(PublishInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            return new Publish()
            {
                ImageName = p.ImageName,
                Camera = p.Camera,
                Description = p.Description,
                Status = p.Status,
                UserPublishLikes = null,
                PublishTags = p.Tags is null ? new HashSet<PublishTag>() : Map(p.Tags.Select(e => new PublishTagInputDto() { TagName = e})).ToHashSet(),
                Comments = new HashSet<Comment>()
            };
        }
        public static IEnumerable<Publish> Map(IEnumerable<PublishInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
        public static Comment Map(CommentInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            return new Comment()
            {
                Publish = null,
                Content = p.CommentContent,
                IsEdited = false
            };
        }
        public static IEnumerable<Comment> Map(IEnumerable<CommentInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
        public static PublishTag Map(PublishTagInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishTag() { Name = p.TagName };
        }
        public static IEnumerable<PublishTag> Map(IEnumerable<PublishTagInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
        public static Publish Map(PublishUpdateInputModel p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            return new Publish()
            {
                Camera = p.NewCamera,
                Description = p.NewDescription,
                ImageName = p.NewImageName,
                Status = p.NewStatus,
                PublishTags = p.NewTags is null? new HashSet<PublishTag>() : Map(p.NewTags).ToHashSet()            
            };
        }
        public static IEnumerable<Publish> Map(IEnumerable<PublishUpdateInputModel> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
    }
}
