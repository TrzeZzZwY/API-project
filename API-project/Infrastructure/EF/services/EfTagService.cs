using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Interfaces.Services;
using AppCore.Models;

namespace Infrastructure.EF.Services
{
    public class EfTagService : ITagService
    {
        public Task<IEnumerable<PublishTag>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PublishTag>> GetAll(IEnumerable<Guid> tagsId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PublishTag>> GetAllFor(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid tagId)
        {
            throw new NotImplementedException();
        }

        public Task<PublishTag> GetTag(Guid tagId)
        {
            throw new NotImplementedException();
        }
    }
}
