using AppCore.Models;
using AppCore.Models.Enums;
using WebApi.Dto.Mappers;

namespace WebApi.Dto.Output
{
    public class PublishOutputDto
    {
        public PublishOutputDto(string imageName, Cameras? camera, string? description,
            string imgPath, DateTime uploadDate, Status status, uint likes, 
            IEnumerable<PublishTagOutputDto>? tags, IEnumerable<CommentOutputDto>? comments)
        {
            ImageName = imageName;
            Camera = camera?? Cameras.None;
            Description = description ?? String.Empty;
            ImgPath = imgPath;
            UploadDate = uploadDate;
            Status = status;
            Likes = likes;
            Tags = tags ?? new HashSet<PublishTagOutputDto>();
            Comments = comments ?? new HashSet<CommentOutputDto>();
        }

        public string ImageName { get; set; }
        public Cameras Camera { get; set; }
        public string? Description { get; set; }
        public string ImgPath { get; set; } //TODO: Brak ImgPath w AppCore, dopisać metodę w serwisie
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; }
        public uint Likes { get; set; }
        public IEnumerable<PublishTagOutputDto> Tags { get; set; }//TODO: DTO
        public IEnumerable<CommentOutputDto> Comments { get; set; }

    }
}
