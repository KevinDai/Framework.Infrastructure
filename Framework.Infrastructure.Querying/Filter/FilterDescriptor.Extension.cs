using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Framework.Infrastructure.Querying
{
    public partial class FilterDescriptor
    {
        /// <summary>
        /// 创建过滤描述对象
        /// </summary>
        /// <typeparam name="T">过滤对象类型</typeparam>
        /// <param name="expression">从元素中获取过滤属性的函数</param>
        /// <param name="value">过滤参数</param>
        /// <param name="filterOperator">过滤操作</param>
        /// <returns>过滤描述对象</returns>
        public static FilterDescriptor Create<T>(Expression<Func<T, object>> expression, object value, FilterOperator filterOperator)
        {
            string propertyName = PropertyNameHelper.ResolvePropertyName<T>(expression);
            FilterDescriptor filterDescriptor = new FilterDescriptor(propertyName, filterOperator, value);
            return filterDescriptor;
        }
    }
}
