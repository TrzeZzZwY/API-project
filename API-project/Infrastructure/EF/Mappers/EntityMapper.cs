using AppCore.Models;
using Infrastructure.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Mappers
{
    public class EntityMapper
    {
        public static Publish FromEntityToPublish(PublishEntity entity)
        {
            return new Publish(
                entity.ImageName, 
                entity.Camera, 
                entity.Description, 
                entity.Status, 
                entity.UserPublishLikes, 
                entity.PublishTags, 
                entity.Comments
                );
        }

        public static PublishAlbum FromEntityToPublishAlbum(PublishAlbumEntity entity)
        {
            throw new NotImplementedException();

            // TODO: Dopisać listę publishes do PublishAlbumEntity

            //return new PublishAlbum(
            //    entity.Name,
            //    entity.Status
            //    );
        }

        public static Comment FromEntityToComment(CommentEntity entity)
        {
            throw new NotImplementedException();

            //TODO: Brakuje listy podkomentarzy w CommentEntity

            //return new Comment(
            //    entity.User,
            //    entity.Content,
            //    entity.IsEdited,
            //    entity.
            //    );
        }
    }
}
