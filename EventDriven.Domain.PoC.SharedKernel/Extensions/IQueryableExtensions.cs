using System.Linq;

namespace EventDriven.Domain.PoC.SharedKernel.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TModel> Paging<TModel>(this IQueryable<TModel> query, int pageSize = 0,
            int pageNumber = 0) where TModel : class
        {
            return pageSize > 0 && pageNumber > 0 ? query.Skip((pageNumber - 1) * pageSize).Take(pageSize) : query;
        }
    }
}