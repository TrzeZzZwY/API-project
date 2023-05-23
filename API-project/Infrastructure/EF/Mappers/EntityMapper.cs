﻿using AppCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.EF.services;
using Infrastructure.EF.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Mappers
{
    public static class EntityMapper
    {


        public static CommentEntity Map(Comment p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new CommentEntity()
            {
                publish = Map(p.Publish),
                CommentContent = p.Content,
                IsEdited = p.IsEdited
            };
        }
        public static IEnumerable<CommentEntity> Map(IEnumerable<Comment> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return Map(item);
        }
        public static PublishEntity Map(Publish p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new PublishEntity()
            {
                Camera = p.Camera,
                Description = p.Description,
                ImageName = p.ImageName,
                Status = p.Status,
                UploadDate = p.UploadDate
            };
        }
        public static IEnumerable<PublishEntity> Map(IEnumerable<Publish> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return Map(item);
        }
        public static PublishTagEntity Map(PublishTag p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new PublishTagEntity()
            {
                Name = p.Name
            };

        }
        public static IEnumerable<PublishTagEntity> Map(IEnumerable<PublishTag> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return Map(item);
        }
        public static PublishAlbumEntity Map(PublishAlbum p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new PublishAlbumEntity()
            {
                Name = p.Name,
                Status = p.Status
            };

        }
        public static IEnumerable<PublishAlbumEntity> Map(IEnumerable<PublishAlbum> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return Map(item);
        }

        public static Comment Map(CommentEntity p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new Comment(

                id: p.Id,
                content: p.CommentContent,
                isEdited: p.IsEdited,
                publish: null
            );
        }
        public static IEnumerable<Comment> Map(IEnumerable<CommentEntity> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return Map(item);
        }
        public static Publish Map(PublishEntity p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new Publish(
                id: p.Id,
                imageName: p.ImageName,
                camera: p.Camera,
                description: p.Description,
                uploadDate: p.UploadDate,
                status: p.Status,
                userLikes: p.UserLikes.Select(e => Guid.Parse(e.Id)).ToHashSet(),
                publishTags: Map(p.PublishTags).ToHashSet(),
                comments: Map(p.Comments).ToHashSet()
                );
        }
        public static IEnumerable<Publish> Map(IEnumerable<PublishEntity> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return Map(item);
        }
        public static PublishTag Map(PublishTagEntity p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new PublishTag(
                    id: p.Id,
                    name: p.Name
                );
        }
        public static IEnumerable<PublishTag> Map(IEnumerable<PublishTagEntity> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return Map(item);
        }
        public static PublishAlbum Map(PublishAlbumEntity p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");

            return new PublishAlbum(
                    id: p.Id,
                    name: p.Name,
                    status: p.Status,
                    publishes: p.Publishes is null ? null:Map(p.Publishes).ToHashSet()
                );
        }
        public static IEnumerable<PublishAlbum> Map(IEnumerable<PublishAlbumEntity> p)
        {
            if (p is null)
                throw new ArgumentException("Argument can't be null!");
            foreach (var item in p)
                yield return Map(item);
        }
    }
}
