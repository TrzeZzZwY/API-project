using AppCore.Interfaces.Services;
using AppCore.Models;
using AppCore.Models.Enums;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services.Authorized
{
    public class EfTagServiceAuthorized : ServiceAuthorization
    {
        private readonly ITagService _tagService;

        public EfTagServiceAuthorized(UserManager<UserEntity> manager, ITagService tagService):base(manager)
        {
            _tagService = tagService;
        }

        public async Task<PublishTag> Create(Guid userId, PublishTag tag)
        {
            if (await UserIsAdmin(userId))
                return await _tagService.Create(tag);
            throw new AccessViolationException();
        }
        public async Task<PublishTag> Update(Guid userId, Guid tagId, PublishTag tag)
        {
            if (await UserIsAdmin(userId))
                return await _tagService.Update(tagId,tag);
            throw new AccessViolationException();
        }
        public async Task<PublishTag> Update(Guid userId, string tagName, PublishTag tag)
        {
            if (await UserIsAdmin(userId))
               return  await _tagService.Update(tagName, tag);
            throw new AccessViolationException();
        }
        public async Task<IEnumerable<PublishTag>> GetAll(Guid userId, int page, int take)
        {
            return await _tagService.GetAll(userId,page,take);
        }
        public async Task<PublishTag> GetOne(Guid tagId)
        {
            return await _tagService.GetOne(tagId);
        }
        public async Task<PublishTag> GetOne(string tagName)
        {
            return await _tagService.GetOne(tagName);
        }
        public async Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid userId, Guid tagId, int page, int take)
        {
            //if (await UserIsAdmin(userId))
            //    return await _tagService.GetAllPublishesForTag(tagId);
            //return (await _tagService.GetAllPublishesForTag(tagId)).Where(e => e.Status == Status.Visible);
            return await _tagService.GetAllPublishesForTag(userId, tagId, page, take);
        }
        public async Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid userId, string tagName, int page, int take)
        {
            //if (await UserIsAdmin(userId))
            //    return await _tagService.GetAllPublishesForTag(tagName);
            //return (await _tagService.GetAllPublishesForTag(tagName)).Where(e => e.Status == Status.Visible);
            return await _tagService.GetAllPublishesForTag(userId, tagName, page, take);
        }
        public async Task<PublishTag> Delete(Guid userId,Guid tagId)
        {
            if (await UserIsAdmin(userId))
                return await _tagService.Delete(tagId);
            throw new AccessViolationException();
        }
        public async Task<PublishTag> Delete(Guid userId, string tagName)
        {
            if (await UserIsAdmin(userId))
                return await _tagService.Delete(tagName);
            throw new AccessViolationException();
        }

    }
}
