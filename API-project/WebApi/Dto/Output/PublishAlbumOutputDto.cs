using AppCore.Models;
using AppCore.Models.Enums;

namespace WebApi.Dto.Output
{
    public class PublishAlbumOutputDto
    {
        public PublishAlbumOutputDto(string name, Status status, IEnumerable<PublishOutputDto> publishes)
        {
            Name = name;
            Status = status;
            Publishes = publishes;
        }

        public string Name { get; set; }
        public Status Status { get; set; }
        public IEnumerable<PublishOutputDto> Publishes { get; set; } //TODO: dopisać metodę w serwisie

        public static PublishAlbumOutputDto of(PublishAlbum publishAlbum)
        {
            if (publishAlbum is null)
            {
                throw new ArgumentException();
            }
            throw new NotImplementedException();
            //return new PublishAlbumOutputDto(publishAlbum.Name, publishAlbum.Status);
        }
    }
}
