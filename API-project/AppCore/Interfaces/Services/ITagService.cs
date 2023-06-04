using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Interfaces.Services
{
    public interface ITagService
    {
        public Task<IEnumerable<PublishTag>> GetAll(Guid userId, int page, int take);
        public Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid userId, Guid tagId, int page, int take);
        public Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid userId, string tagName, int page, int take);
        public Task<PublishTag> GetOne(Guid tagId);
        public Task<PublishTag> GetOne(string tagName);
        public Task<PublishTag> Create(PublishTag tag);
        public Task<PublishTag> Delete(Guid tagId);
        public Task<PublishTag> Delete(string tagName);
        public Task<PublishTag> Update(Guid tagId, PublishTag tag);
        public Task<PublishTag> Update(string tagName, PublishTag tag);
    }
}
