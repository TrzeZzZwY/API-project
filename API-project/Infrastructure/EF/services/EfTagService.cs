using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Interfaces.Services;
using AppCore.Models;

namespace Infrastructure.EF.services
{
    public class EfTagService : ITagService
    {
        public IEnumerable<PublishTag> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PublishTag> GetAll(IEnumerable<Guid> tagsId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PublishTag> GetAllFor(Guid publishId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Publish> GetAllPublishesForTag(Guid tagId)
        {
            throw new NotImplementedException();
        }

        public PublishTag GetTag(Guid tagId)
        {
            throw new NotImplementedException();
        }
    }
}
