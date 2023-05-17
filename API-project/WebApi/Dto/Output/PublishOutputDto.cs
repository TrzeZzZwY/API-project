using AppCore.Models;
using AppCore.Models.Enums;

namespace WebApi.Dto.Output
{
    public class PublishOutputDto
    {
        public PublishOutputDto(string imageName, string camera, string description, string imgPath, DateTime uploadDate, Status status, uint likes, IEnumerable<Tag> tags, IEnumerable<Comment> comments)
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
        public string Camera { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; }
        public uint Likes { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
