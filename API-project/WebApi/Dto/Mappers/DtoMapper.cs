using AppCore.Interfaces.Services;
using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services;
using Microsoft.AspNetCore.Identity;
using WebApi.Dto.Input;
using WebApi.Dto.Output;

namespace WebApi.Dto.Mappers
{
    public class DtoMapper
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EfPublishService _publishService;
        public DtoMapper(UserManager<UserEntity> userManager, EfPublishService publishService)// TODO: zamisat IPublishService konkretna implementacja !
        {
            _userManager = userManager;
            _publishService = publishService;
        }

        public PublishAlbumOutputDto MapToOutput(PublishAlbum p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishAlbumOutputDto(
                name: p.Name,
                status: p.Status,
                publishes: MapToOutput(p.Publishes)
                );
        }
        public IEnumerable<PublishAlbumOutputDto> MapToOutput(IEnumerable<PublishAlbum> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var item in p)
                yield return MapToOutput(item);
        }
        public PublishOutputDto MapToOutput(Publish p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishOutputDto(
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                imgPath: _publishService.GetImgPath(p.Id),
                uploadDate: p.UploadDate,
                status: p.Status,
                likes: (uint)p.UserPublishLikes.Count(),
                tags:MapToOutput(p.PublishTags),
                comments: MapToOutput( p.Comments )
                );
            throw new NotImplementedException();
        }
        public IEnumerable<PublishOutputDto> MapToOutput(IEnumerable<Publish> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var publish in p)
                yield return MapToOutput(publish);
        }
        public CommentOutputDto MapToOutput(Comment p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            var user = _userManager.FindByIdAsync(p.Id.ToString()).Result;
            if (user is null)
                throw new Exception();

            return new CommentOutputDto(
                userLogin: user.Login,
                commentContent: p.Content
                );
        }
        public IEnumerable<CommentOutputDto> MapToOutput(IEnumerable<Comment> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return MapToOutput(item);
        }
        public PublishTagOutputDto MapToOutput(PublishTag p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            return new PublishTagOutputDto(name: p.Name);
        }
        public IEnumerable<PublishTagOutputDto> MapToOutput(IEnumerable<PublishTag> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var item in p)
                yield return MapToOutput(item);
        }
        public PublishAlbum MapFromInput(PublishAlbumInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishAlbum(
                name: p.Name,
                status: p.Status,
                publishes: null
                );
        }
        public IEnumerable<PublishAlbum> MapFromInput(IEnumerable<PublishAlbumInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return MapFromInput(item);
        }
        public Publish MapFromInput(PublishInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            return new Publish(
                userId:p.UserId,
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
        public IEnumerable<Publish> MapFromInput(IEnumerable<PublishInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return MapFromInput(item);
        }
        public Comment MapFromInput(CommentInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
             return new Comment(
                 userId: p.UserId,
                 publishId:p.PublishId,
                 content: p.CommentContent,
                 isEdited: false
            );
        }
        public IEnumerable<Comment> MapFromInput(IEnumerable<CommentInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return MapFromInput(item);
        }
        public PublishTag MapFromInput(PublishTagInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishTag(name: p.TagName,publishes:null);
        }
        public IEnumerable<PublishTag> MapFromInput(IEnumerable<PublishTagInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return MapFromInput(item);
        }
    }
}
