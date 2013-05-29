using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Framework.Infrastructure.Querying
{

    using Querying.Expressions;

    public static class QueryableExtensions
    {
        //internal static IQueryable<T> FindBy<T>(this ObjectQuery<T> objectQuery, Query query, IEnumerable<String> includes = null)
        //{
        //    //关联导航属性的加载
        //    objectQuery = objectQuery.Include<T>(includes);

        //    if (query == null)
        //    {
        //        return objectQuery;
        //    }

        //    //排序
        //    objectQuery = objectQuery.OrderBy(query.SortDescriptors);

        //    //根据过滤条件进行过滤
        //    var exp = ExpressionBuilder.Expression<T>(query.FilterDescriptors);
        //    var source = objectQuery.Where(exp);
        //    source = source.OrderBy(query.SortDescriptors);

        //    //分页
        //    source = source.Pagination<T>(query.Pagination);
        //    return source;
        //}

        public static IQueryable<T> Pagination<T>(this IQueryable<T> source, Pagination pagination)
        {
            //分页
            if (pagination != null)
            {
                pagination.TotalCount = source.Count();
                if (pagination.PageIndex <= 0)
                {
                    pagination.PageIndex = 1;
                }
                source = source
                    .Skip(pagination.PageSize * (pagination.PageIndex - 1))
                    .Take(pagination.PageSize);
            }
            return source;
        }


        public static IQueryable<T> FindBy<T>(this IQueryable<T> source, Query query)
        {
            //关联导航属性的加载
            if (query == null)
            {
                return source;
            }

            //根据过滤条件进行过滤
            var exp = ExpressionBuilder.Expression<T>(query.FilterDescriptors);
            source = source.Where(exp);
            source = source.OrderBy(query.SortDescriptors);

            //分页
            source = source.Pagination<T>(query.Pagination);
            return source;
        }

        //public static ObjectQuery<T> Include<T>(this ObjectQuery<T> objectQuery, IEnumerable<string> includes)
        //{
        //    if (includes != null && includes.Any())
        //    {
        //        foreach (var include in includes)
        //        {
        //            objectQuery = objectQuery.Include(include);
        //        }
        //    }
        //    return objectQuery;
        //}

        //public static ObjectQuery<T> OrderBy<T>(this ObjectQuery<T> objectQuery, IEnumerable<SortDescriptor> sortDesciptors)
        //{
        //    if (sortDesciptors != null && sortDesciptors.Any())
        //    {
        //        foreach (var sort in sortDesciptors)
        //        {
        //            objectQuery = objectQuery.OrderBy(sort.SortDirection == ListSortDirection.Ascending
        //                    ? string.Format("{0}.{1}", objectQuery.Name, sort.Member)
        //                    : string.Format("{0}.{1} desc", objectQuery.Name, sort.Member));
        //        }

        //    }
        //    return objectQuery;
        //}

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, IEnumerable<SortDescriptor> sortDesciptors)
        {
            if (sortDesciptors != null && sortDesciptors.Any())
            {
                var sortBuilder = new SortDescriptorCollectionExpressionBuilder(query, sortDesciptors);
                query = sortBuilder.Sort() as IQueryable<T>;
            }
            return query;
        }

    }
}
