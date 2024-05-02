using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace MonkeyBuisness.Models.Extensions;
public static class QueryExtension
{
    //WhereIf - расширение метода Where
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition,
        Expression<Func<T, bool>> predicate)
    {
        if (condition)
        {
            return source.Where(predicate);
        }
        return source;
    }
}

