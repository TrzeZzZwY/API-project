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
    public class DtoMapper
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IPublishService _publishService;
        public DtoMapper(UserManager<UserEntity> userManager, IPublishService publishService)
        {
            _userManager = userManager;
            _publishService = publishService;
        }

        public PublishAlbumOutputDto Map(PublishAlbum p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishAlbumOutputDto(
                name: p.Name,
                status: p.Status,
                publishes: null
                );
        }
        public IEnumerable<PublishAlbumOutputDto> Map(IEnumerable<PublishAlbum> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var item in p)
                yield return Map(item);
        }
        public PublishOutputDto Map(Publish p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishOutputDto(
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                imgPath: " ",//_publishService.GetImgPath(p.Id).Result,
                uploadDate: p.UploadDate,
                status: p.Status,
                likes: p.UserPublishLikes is null ? 0 : (uint)p.UserPublishLikes.Count(),
                tags: p.PublishTags is null ? null : Map(p.PublishTags),
                comments: p.Comments is null ? null : Map(p.Comments)
                );
        }
        public IEnumerable<PublishOutputDto> Map(IEnumerable<Publish> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var publish in p)
                yield return Map(publish);
        }
        public CommentOutputDto Map(Comment p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            var user = _userManager.FindByIdAsync(p.Id.ToString()).Result;
            if (user is null)
                throw new Exception();

            return new CommentOutputDto(
                userLogin: user.UserName,
                commentContent: p.Content
                );
        }
        public IEnumerable<CommentOutputDto> Map(IEnumerable<Comment> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
        public PublishTagOutputDto Map(PublishTag p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            return new PublishTagOutputDto(name: p.Name);
        }
        public IEnumerable<PublishTagOutputDto> Map(IEnumerable<PublishTag> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var item in p)
                yield return Map(item);
        }
        public PublishAlbum Map(PublishAlbumInputDto p)
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
        public IEnumerable<PublishAlbum> Map(IEnumerable<PublishAlbumInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
        public Publish Map(PublishInputDto p)
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
                PublishTags = null, //Map(p.Tags).ToHashSet(),
                Comments = null
            };
        }
        public IEnumerable<Publish> Map(IEnumerable<PublishInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
        public Comment Map(CommentInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            return new Comment()
            {
                Publish = _publishService.GetOne(p.PublishId).Result,
                Content = p.CommentContent,
                IsEdited = false
            };
        }
        public IEnumerable<Comment> Map(IEnumerable<CommentInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
        public PublishTag Map(PublishTagInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishTag() { Name = p.TagName };
        }
        public IEnumerable<PublishTag> Map(IEnumerable<PublishTagInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return Map(item);
        }
    }
}
