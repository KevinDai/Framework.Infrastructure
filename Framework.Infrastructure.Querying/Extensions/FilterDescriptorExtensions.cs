using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Framework.Infrastructure.Querying
{
    using Querying.Expressions;

    public static class FilterDescriptorExtensions
    {
        public static Expression<Func<T, bool>> Expression<T>(this IFilterDescriptor filter)
        {
            return ExpressionBuilder.Expression<T>(filter);
        }

        public static Expression<Func<T, bool>> Expression<T>(this IEnumerable<IFilterDescriptor> filters)
        {
            return ExpressionBuilder.Expression<T>(filters);
        }

    }
}
