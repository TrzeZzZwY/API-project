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
        //TODO
        public Task<IEnumerable<PublishTag>> GetAll();
        public Task<IEnumerable<PublishTag>> GetAll(IEnumerable<Guid> tagsId);
        public Task<IEnumerable<PublishTag>> GetAllFor(Guid publishId);
        public Task<IEnumerable<Publish>> GetAllPublishesForTag(Guid tagId);
        public Task<PublishTag> GetTag(Guid tagId);
    }
}
