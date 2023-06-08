using AppCore.Models;
using AppCore.Models.Enums;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApi.Dto.Mappers;

namespace UnitTest
{
    public class DtoMapperTest
    {
        private Publish publish1 = new Publish()
        {
            Id = Guid.NewGuid(),
            ImageName = "publish1",
            Description = "publish1 desc",
            FileName = Guid.NewGuid().ToString(),
            UserName = "User1",
            Status = Status.Visible,
            Camera = Cameras.Nokia,
            UploadDate = DateTime.Now,
            PublishTags = new HashSet<PublishTag>(),
            Comments = new HashSet<Comment>(),
            UserPublishLikes = new HashSet<Guid>()
        };
        private Publish publish2 = new Publish()
        {
            Id = Guid.NewGuid(),
            ImageName = "publish2",
            Description = "publish2 desc",
            FileName = Guid.NewGuid().ToString(),
            UserName = "User2",
            Status = Status.Visible,
            Camera = Cameras.Nokia,
            UploadDate = DateTime.Now,
            PublishTags = new HashSet<PublishTag>(),
            Comments = new HashSet<Comment>(),
            UserPublishLikes = new HashSet<Guid>()
        };

        private PublishAlbum album1 = new PublishAlbum()
        {
            Id = Guid.NewGuid(),
            Name = "album1",
            Publishes = new HashSet<Publish>(),
            Status = Status.Visible,
            UserName = "User1"
        };
        private PublishAlbum album2 = new PublishAlbum()
        {
            Id = Guid.NewGuid(),
            Name = "album2",
            Publishes = new HashSet<Publish>(),
            Status = Status.Visible,
            UserName = "User2"
        };

        private PublishTag tag1 = new PublishTag()
        {
            Id = Guid.NewGuid(),
            Name = "tag1"
        };
        private PublishTag tag2 = new PublishTag()
        {
            Id = Guid.NewGuid(),
            Name = "tag2"
        };

        private Comment comment1 = new Comment()
        {
            Id = Guid.NewGuid(),
            Content = "comment1",
            IsEdited = false,
            UserName = "User1"
        };
        private Comment comment2 = new Comment()
        {
            Id = Guid.NewGuid(),
            Content = "comment2",
            IsEdited = false,
            UserName = "User2"
        };

        public DtoMapperTest()
        {
            InitCoreModels();
        }

        private void InitCoreModels()
        {
            album1.Publishes.Add(publish1);
            album2.Publishes.Add(publish2);

            publish1.PublishTags.Add(tag1);
            publish2.PublishTags.Add(tag2);

            publish1.Comments.Add(comment1);
            publish2.Comments.Add(comment2);

            comment1.Publish = publish1;
            comment2.Publish = publish2;
        }

        [Fact]
        public void DtoMapperFromCoreAlbumTest()
        {
            var mapped = DtoMapper.Map(album1);
            Assert.Equal(mapped.UserName, album1.UserName);
            Assert.Equal(mapped.Name, album1.Name);
            Assert.Equal(mapped.Status, album1.Status);
            Assert.Equal(mapped.Publishes.Count(), mapped.Publishes.Count());
        }
        [Fact]
        public void DtoMapperFromCorePublishTest()
        {
            var mapped = DtoMapper.Map(publish2);
            Assert.Equal(mapped.UserName, publish2.UserName);
            Assert.Equal(mapped.ImageName, publish2.ImageName);
            Assert.Equal(mapped.Comments.Count(), publish2.Comments.Count());
            Assert.Equal(mapped.Tags.First().Name, publish2.PublishTags.First().Name);
            Assert.Equal(mapped.Status, publish2.Status);
        }
        [Fact]
        public void DtoMapperFromCoreTagTest()
        {
            var mapped = DtoMapper.Map(tag1);
            Assert.Equal(mapped.Name, tag1.Name);
        }
        [Fact]
        public void DtoMapperFromCoreCommentTest()
        {
            var mapped = DtoMapper.Map(comment1);
            Assert.Equal(mapped.CommentContent, comment1.Content);
            Assert.Equal(mapped.IsEdited, comment1.IsEdited);
            Assert.Equal(mapped.UserLogin, comment1.UserName);
        }
    };
}
