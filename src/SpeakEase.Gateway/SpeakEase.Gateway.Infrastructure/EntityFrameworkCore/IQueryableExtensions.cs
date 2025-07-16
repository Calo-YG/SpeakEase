using System.Linq.Expressions;

namespace SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;

public static class IQueryableExtensions
{
    /// <summary>
    /// whereif
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}