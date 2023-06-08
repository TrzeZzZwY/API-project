using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Models;
using AppCore.Models.Enums;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Mappers;
using Xunit.Sdk;

namespace UnitTest
{
    public class EntityMapperTest
    {
        private UserEntity entityUser1 = new UserEntity()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            Albums = new HashSet<PublishAlbumEntity>(),
            Comments = new HashSet<CommentEntity>(),
            PublishLikes = new HashSet<PublishEntity>(),
            Publishes = new HashSet<PublishEntity>()
        };
        private UserEntity entityUser2 = new UserEntity()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User2",
            Albums = new HashSet<PublishAlbumEntity>(),
            Comments = new HashSet<CommentEntity>(),
            Publishes = new HashSet<PublishEntity>()
        };

        private PublishAlbumEntity entityAlbum1 = new PublishAlbumEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Album1",
            Status = Status.Visible,
            Publishes = new HashSet<PublishEntity>(),
        };
        private PublishAlbumEntity entityAlbum2 = new PublishAlbumEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Album2",
            Status = Status.Visible,
            Publishes = new HashSet<PublishEntity>(),
        };

        private PublishEntity entityPublish1 = new PublishEntity()
        {
            Id = Guid.NewGuid(),
            ImageName = "Image1",
            Description = "Image1Description",
            FileName = Guid.NewGuid().ToString(),
            UploadDate = DateTime.Now,
            Camera = Cameras.Canon,
            Status = Status.Visible,
            Comments = new HashSet<CommentEntity>(),
            PublishTags = new HashSet<PublishTagEntity>(),
            UserLikes = new HashSet<UserEntity>()
        };
        private PublishEntity entityPublish2 = new PublishEntity()
        {
            Id = Guid.NewGuid(),
            ImageName = "Image2",
            Description = "Image2Description",
            FileName = Guid.NewGuid().ToString(),
            UploadDate = DateTime.Now,
            Camera = Cameras.Canon,
            Status = Status.Visible,
            Comments = new HashSet<CommentEntity>(),
            PublishTags = new HashSet<PublishTagEntity>(),
            UserLikes = new HashSet<UserEntity>()
        };
        private PublishEntity entityPublish3 = new PublishEntity()
        {
            Id = Guid.NewGuid(),
            ImageName = "Image3",
            Description = "Image1Description",
            FileName = Guid.NewGuid().ToString(),
            UploadDate = DateTime.Now,
            Camera = Cameras.Canon,
            Status = Status.Visible,
            Comments = new HashSet<CommentEntity>(),
            PublishTags = new HashSet<PublishTagEntity>(),
            UserLikes = new HashSet<UserEntity>()
        };
        private PublishEntity entityPublish4 = new PublishEntity()
        {
            Id = Guid.NewGuid(),
            ImageName = "Image4",
            Description = "Image2Description",
            FileName = Guid.NewGuid().ToString(),
            UploadDate = DateTime.Now,
            Camera = Cameras.Canon,
            Status = Status.Visible,
            Comments = new HashSet<CommentEntity>(),
            PublishTags = new HashSet<PublishTagEntity>(),
            UserLikes = new HashSet<UserEntity>()
        };

        private PublishTagEntity entityTag1 = new PublishTagEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Animal",
            Publishes = new HashSet<PublishEntity>()
        };
        private PublishTagEntity entityTag2 = new PublishTagEntity()
        {
            Id = Guid.NewGuid(),
            Name = "Food",
            Publishes = new HashSet<PublishEntity>()
        };

        private CommentEntity entityComment1 = new CommentEntity()
        {
            Id = Guid.NewGuid(),
            CommentContent = "Nice photo!",
            IsEdited = false,
        };
        private CommentEntity entityComment2 = new CommentEntity()
        {
            Id = Guid.NewGuid(),
            CommentContent = "Haha",
            IsEdited = false,
        };
        public EntityMapperTest()
        {
            InitEnityModels();
        }
        private void InitEnityModels()
        {
            entityUser1.Albums.Add(entityAlbum1);
            entityUser2.Albums.Add(entityAlbum2);

            entityAlbum1.User = entityUser1;
            entityAlbum2.User = entityUser2;

            entityUser1.Publishes.Add(entityPublish1);
            entityUser1.Publishes.Add(entityPublish2);
            entityUser2.Publishes.Add(entityPublish3);
            entityUser2.Publishes.Add(entityPublish4);

            entityPublish1.User = entityUser1;
            entityPublish2.User = entityUser1;
            entityPublish3.User = entityUser2;
            entityPublish4.User = entityUser2;

            entityPublish1.Album = entityAlbum1;
            entityPublish3.Album = entityAlbum2;

            entityAlbum1.Publishes.Add(entityPublish1);
            entityAlbum2.Publishes.Add(entityPublish3);

            entityPublish1.PublishTags.Add(entityTag1);
            entityPublish3.PublishTags.Add(entityTag2);

            entityTag1.Publishes.Add(entityPublish1);
            entityTag2.Publishes.Add(entityPublish3);

            entityUser1.Comments.Add(entityComment1);
            entityUser2.Comments.Add(entityComment2);

            entityComment1.User = entityUser1;
            entityComment2.User = entityUser2;

            entityComment1.Publish = entityPublish3;
            entityComment2.Publish = entityPublish1;

            entityPublish1.Comments.Add(entityComment2);
            entityPublish3.Comments.Add(entityComment1);

            entityUser1.PublishLikes.Add(entityPublish3);
            entityPublish3.UserLikes.Add(entityUser1);
        }

        [Fact]
        public void EntityMapperEntityCommentTest()
        {
            var mapped = EntityMapper.Map(entityComment1);
            Assert.Equal(mapped.Id, entityComment1.Id);
            Assert.Equal(mapped.UserName, entityComment1.User.UserName);
            Assert.Equal(mapped.Content, entityComment1.CommentContent);
            Assert.Equal(mapped.IsEdited, entityComment1.IsEdited);
        }
        [Fact]
        public void EntityMapperEntityTagTest()
        {
            var mapped = EntityMapper.Map(entityTag2);
            Assert.Equal(mapped.Id, entityTag2.Id);
            Assert.Equal(mapped.Name, entityTag2.Name);
        }
        [Fact]
        public void EntityMapperEntityPublishTest()
        {
            var mapped = EntityMapper.Map(entityPublish3);
            Assert.Equal(mapped.Id, entityPublish3.Id);
            Assert.Equal(mapped.ImageName, entityPublish3.ImageName);
            Assert.Equal(mapped.Comments.Count(), entityPublish3.Comments.Count());
            Assert.Equal(mapped.Camera, entityPublish3.Camera);
            Assert.Equal(mapped.UserPublishLikes.Count(), 1);
        }
        [Fact]
        public void EntityMapperFromEntityAlbumTest()
        {
            var mapped = EntityMapper.Map(entityAlbum1);
            Assert.Equal(mapped.Id, entityAlbum1.Id);
            Assert.Equal(mapped.Name, entityAlbum1.Name);
            Assert.Equal(mapped.UserName, entityAlbum1.User.UserName);
        }

        [Fact]
        public void EntityMapperToEntityCommentTest()
        {
            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                Content = "Haha",
                IsEdited = false,
            };
            var mapped = EntityMapper.Map(comment);
            Assert.Equal(comment.Content, mapped.CommentContent);
            Assert.Equal(comment.IsEdited, mapped.IsEdited);
        }

        [Fact]
        public void EntityMapperToEntityAlbumTest()
        {
            var album = new PublishAlbum()
            {
                Id = Guid.NewGuid(),
                Name = "Album1",
                Status = Status.Visible
            };
            var mapped = EntityMapper.Map(album);
            Assert.Equal(album.Name, mapped.Name);
            Assert.Equal(album.Status, mapped.Status);
        }

        [Fact]
        public void EntityMapperToEntityPublishTest()
        {
            var publish = new Publish()
            {
                Id = Guid.NewGuid(),
                Description = "Test1",
                ImageName = "Image1",
                Camera = Cameras.Nokia,
                Status = Status.Visible,
                UploadDate = DateTime.Now
            };
            var mapped = EntityMapper.Map(publish);
            Assert.Equal(publish.Camera, mapped.Camera);
            Assert.Equal(publish.Description, mapped.Description);
            Assert.Equal(publish.Status, mapped.Status);
            Assert.Equal(publish.UploadDate, mapped.UploadDate);
        }
        [Fact]
        public void EntityMapperToEntityTagTest()
        {
            var tag = new PublishTag()
            {
                Id = Guid.NewGuid(),
                Name = "Funny"
            };
            var mapped = EntityMapper.Map(tag);
            Assert.Equal(tag.Name, mapped.Name);
        }

    }
}
