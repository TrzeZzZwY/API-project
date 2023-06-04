using Infrastructure.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Services
{
    public static class QueryFilter
    {
        public static IQueryable<T> Paginate<T>(IQueryable<T> query, int page, int take) where T : class
        {
           return query.Skip(take * (page - 1)).Take(take);
        }
        public static IQueryable<PublishAlbumEntity> AccessCheck<PublishAlbumEntity>(IQueryable<PublishAlbumEntity> query, Guid userId)
        {
            return query;
        }
    }
}
