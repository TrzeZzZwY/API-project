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
                publishes: Map(p.Publishes)
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
                imgPath: _publishService.GetImgPath(p.Id).Result,
                uploadDate: p.UploadDate,
                status: p.Status,
                likes: (uint)p.UserPublishLikes.Count(),
                tags:Map(p.PublishTags),
                comments: Map( p.Comments )
                );
            throw new NotImplementedException();
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

            return new PublishAlbum(
                name: p.Name,
                status: p.Status,
                publishes: null
                );
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
            return new Publish(
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                status: p.Status,
                userLikes: null,
                publishTags: p.Tags,
                comments: null,
                uploadDate: null
                );
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
             return new Comment(
                 publish: _publishService.GetOne(p.PublishId).Result,
                 content: p.CommentContent,
                 isEdited: false
            );
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

            return new PublishTag(name: p.TagName);
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
