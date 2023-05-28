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

            return new PublishAlbumOutputDto(
                name: p.Name,
                status: p.Status,
                publishes: null
                );
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

            return new PublishOutputDto(
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                imgPath: "",
                uploadDate: p.UploadDate,
                status: p.Status,
                likes: p.UserPublishLikes is null ? 0 : (uint)p.UserPublishLikes.Count(),
                tags: p.PublishTags is null ? null : Map(p.PublishTags),
                comments: p.Comments is null ? null : Map(p.Comments)
                );
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

            return new CommentOutputDto(
                userLogin: "",
                commentContent: p.Content
                );
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
            return new PublishTagOutputDto(name: p.Name);
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
                Publishes = null
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
                PublishTags = p.Tags is null ? null : Map(p.Tags).ToHashSet(),
                Comments = null
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
    }
}
