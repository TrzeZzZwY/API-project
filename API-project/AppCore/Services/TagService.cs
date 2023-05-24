using AppCore.Interfaces.Services;
using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services
{
    public class TagService : ITagService
    {
        public Task<PublishTag> Create(PublishTag tag)
        {
            throw new NotImplementedException();
        }

        public Task<PublishTag> Delete(Guid tagId)
        {
            throw new NotImplementedException();
        }

        public Task<PublishTag> Delete(string tagName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PublishTag>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid tagId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Publish>> GetAllPublishesForTag(string tagName)
        {
            throw new NotImplementedException();
        }

        public Task<PublishTag> GetOne(Guid tagId)
        {
            throw new NotImplementedException();
        }

        public Task<PublishTag> GetOne(string tagName)
        {
            throw new NotImplementedException();
        }

        public Task<PublishTag> Update(Guid tagId, PublishTag tag)
        {
            throw new NotImplementedException();
        }

        public Task<PublishTag> Update(string tagName, PublishTag tag)
        {
            throw new NotImplementedException();
        }
    }
}
