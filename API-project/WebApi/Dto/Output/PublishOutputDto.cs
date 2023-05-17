using AppCore.Models;
using AppCore.Models.Enums;
using WebApi.Dto.Mappers;

namespace WebApi.Dto.Output
{
    public class PublishOutputDto
    {
        public PublishOutputDto(string imageName, Cameras camera, string description,
            string imgPath, DateTime uploadDate, Status status, uint likes, IEnumerable<PublishTag> tags, IEnumerable<Comment> comments)
        {
            ImageName = imageName;
            Camera = camera;
            Description = description;
            ImgPath = imgPath;
            UploadDate = uploadDate;
            Status = status;
            Likes = likes;
            Tags = tags;
            Comments = comments;
        }

        public string ImageName { get; set; }
        public Cameras Camera { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; } //TODO: Brak ImgPath w AppCore, dopisać metodę w serwisie
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; }
        public uint Likes { get; set; } //TODO: Metoda w serwisie do obliczania liczby polubień
        public IEnumerable<PublishTag> Tags { get; set; } //TODO: Brak listy Tags w AppCore, dopisać metodę w serwisie
        public IEnumerable<Comment> Comments { get; set; }

    }
}
