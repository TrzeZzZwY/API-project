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
        public IEnumerable<PublishTag> GetAll();
        public IEnumerable<PublishTag> GetAll(IEnumerable<Guid> tagsId);
        public IEnumerable<PublishTag> GetAllFor(Guid publishId);
        public IEnumerable<Publish> GetAllPublishesForTag(Guid tagId);
        public PublishTag GetTag(Guid tagId);
    }
}
