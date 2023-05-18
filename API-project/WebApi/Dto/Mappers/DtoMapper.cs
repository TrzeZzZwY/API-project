using AppCore.Models;
using Infrastructure.EF.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApi.Dto.Input;
using WebApi.Dto.Output;

namespace WebApi.Dto.Mappers
{
    public static class DtoMapper
    {
        public static PublishAlbumOutputDto MapToOutput(PublishAlbum p)
        {
            if(p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishAlbumOutputDto(
                name: p.Name,
                status: p.Status,
                publishes: MapToOutput(p.Publishes)
                );
        }
        public static IEnumerable<PublishAlbumOutputDto> MapToOutput(IEnumerable<PublishAlbum> p)
        {
            if(p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var item in p)
                yield return MapToOutput(item);
        }
        public static PublishOutputDto MapToOutput(Publish p)
        {
            if (p is null)
                throw new ArgumentException(message:"Argument can't be null");

            /*return new PublishOutputDto(
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                //imgPath: // TODO,
                uploadDate: p.UploadDate,
                status: p.Status
                //likes: // TODO,
                //tags:// TODO,
                //comments://TODO
                );*/
            throw new NotImplementedException();
        }
        public static IEnumerable<PublishOutputDto> MapToOutput(IEnumerable<Publish> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            foreach (var publish in p)
                yield return MapToOutput(publish);      
        }
        public static CommentOutputDto MapToOutput(Comment p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            /* return new CommentOutputDto(
                 //userLogin: p.UserId,//TODO
                 commentContent: p.Content
                 );*/
            throw new NotImplementedException();
        }
        public static IEnumerable<CommentOutputDto> MapToOutput(IEnumerable<Comment> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return MapToOutput(item);
        }

        public static PublishAlbum MapFromInput(PublishAlbumInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");

            return new PublishAlbum(
                name: p.Name,
                status: p.Status,
                publishes: null
                );
        }
        public static IEnumerable<PublishAlbum> MapFromInput(IEnumerable<PublishAlbumInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return MapFromInput(item);
        }
        public static Publish MapFromInput(PublishInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            return new Publish(
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                status: p.Status,
                userLikes:null,
                publishTags: p.Tags,
                comments:null
                );
        }
        public static IEnumerable<Publish> MapFromInput(IEnumerable<PublishInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return MapFromInput(item);
        }
        public static Comment MapFromInput(CommentInputDto p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            // return new Comment(

            //);
            throw new NotImplementedException();
        }
        public static IEnumerable<Comment> MapFromInput(IEnumerable<CommentInputDto> p)
        {
            if (p is null)
                throw new ArgumentException(message: "Argument can't be null");
            foreach (var item in p)
                yield return MapFromInput(item);
        }
    }
}
