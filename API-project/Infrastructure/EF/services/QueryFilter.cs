using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Services
{
    public static class QueryFilter
    {
        public static IQueryable<T> Paginate<T>(IQueryable<T> query, int page, int take) where T : class
        {
           return query.Skip(take * (page - 1)).Take(take);
        }

    }
}
