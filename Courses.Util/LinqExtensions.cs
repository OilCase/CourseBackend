using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Courses.Util.LinqExtensions;

public static class LinqExtensions
{
    /// <summary>
    /// Осуществляет фильтрацию при условии 
    /// выполнения условия, переданного в 
    /// condition, иначе фильтр игнорируется
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="condition"></param>
    /// <param name="whereClause"></param>
    /// <returns></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> whereClause)
    {
        if (condition)
        {
            return query.Where(whereClause);
        }
        return query;
    }

    /// <summary>
    /// Осуществляет сортировку по возрастанию,
    /// если параметр ascending равен true.
    /// В противном случае сортирует по убыванию
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="ascending"></param>
    /// <param name="orderByClause"></param>
    /// <returns></returns>
    public static IQueryable<T> ParametrizedOrderBy<T>(this IQueryable<T> query, bool? ascending, Expression<Func<T, object>> orderByClause)
    {
        if (ascending == null)
        {
            return query.OrderBy(orderByClause);
        }
        return query.OrderByDescending(orderByClause);
    }

    /// <summary>
    /// Выполняет загрузку связанной сущности, 
    /// если condition = true
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="query"></param>
    /// <param name="condition"></param>
    /// <param name="includeClause"></param>
    /// <returns></returns>
    public static IQueryable<TEntity> IncludeIf<TEntity>(
        this IQueryable<TEntity> query
      , bool condition
      , Expression<Func<TEntity, object>> includeClause
      ) where TEntity : class
    {
        if (condition)
        {
            return query.Include(includeClause);
        }
        return query;
    }
}