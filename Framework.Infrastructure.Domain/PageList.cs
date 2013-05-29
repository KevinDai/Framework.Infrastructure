using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Domain
{
    public class PageList<T>
    {
        #region Members

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// 对象总数
        /// </summary>
        public int TotalCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 分页页数
        /// </summary>
        public int PageCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前页对象列表
        /// </summary>
        public IEnumerable<T> List
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        public PageList(IEnumerable<T> list, int pageIndex, int pageSize, int totalCount)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentException("分页的当前页码必须大于零",
                    "pageIndex");
            }

            List = list;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageCount =
                TotalCount % PageIndex == 0
                ?
                TotalCount / PageIndex
                :
                TotalCount / PageIndex + 1;
        }

        #endregion

    }
}
