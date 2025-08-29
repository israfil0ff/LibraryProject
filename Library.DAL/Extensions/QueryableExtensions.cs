using Library.DBO.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, PaginationRequest request)
        {
            return query.Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize);
        }
    }
}
