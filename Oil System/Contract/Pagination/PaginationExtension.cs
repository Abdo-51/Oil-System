using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Oil_System.Contract.Pagination
{
    public static class PaginationExtension
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            return query.Skip(skip).Take(pageSize);
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string sortBy, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortBy))
                return query;
            if (sortDirection?.ToLower() == "desc")
            {
                return query.OrderByDescending(e => EF.Property<object>(e, sortBy));
            }
            else
            {
                return query.OrderBy(e => EF.Property<object>(e, sortBy));
            }
        }

        public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, List<Expression<Func<T, bool>>>? filters)
        {
            if (filters != null && filters.Any())
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }
            return query;
        }

        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var totalCount = query.Count();
            var items = query.ApplyPagination(pageNumber, pageSize).ToList();
            var pagedResult = new PagedResult<T>(items, totalCount, pageNumber, pageSize);
            return pagedResult;
        }
    }
}
