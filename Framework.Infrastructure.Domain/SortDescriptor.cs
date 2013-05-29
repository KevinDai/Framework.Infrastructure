using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Framework.Infrastructure.Domain
{
    public class SortDescriptor<T> where T : class
    {
        #region Members

        public Expression<Func<T, object>> SortKeySelector
        {
            get;
            private set;
        }

        public ListSortDirection SortDirection
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数，默认为升序排列
        /// </summary>
        /// <param name="sortKeySelector"></param>
        public SortDescriptor(Expression<Func<T, object>> sortKeySelector)
            : this(sortKeySelector, ListSortDirection.Ascending)
        {
        }

        public SortDescriptor(Expression<Func<T, object>> sortKeySelector, ListSortDirection sortDirection)
        {
            SortKeySelector = sortKeySelector;
            SortDirection = sortDirection;
        }

        #endregion
    }
}
