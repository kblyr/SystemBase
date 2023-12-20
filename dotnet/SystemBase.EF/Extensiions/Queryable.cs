using System.Linq.Expressions;

namespace SystemBase;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, bool condition)
    {
        if (condition)
        {
            return query.Where(predicate);
        }

        return query;
    }

    public static IQueryable<T> SkipAboveZero<T>(this IQueryable<T> query, int count)
    {
        if (count > 0)
        {
            return query.Skip(count);
        }

        return query;
    }

    public static IQueryable<T> TakeAboveZero<T>(this IQueryable<T> query, int count)
    {
        if (count > 0)
        {
            return query.Take(count);
        }

        return query;
    }

    public static IQueryable<T> WithPagination<T>(this IQueryable<T> query, int page, int pageSize)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return query;
        }

        return query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }

    public static async Task<int> CountPage<T>(this IQueryable<T> query, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await query.CountAsync(cancellationToken);
        return Convert.ToInt32(Math.Ceiling((count / pageSize) + 0M));
    }
}