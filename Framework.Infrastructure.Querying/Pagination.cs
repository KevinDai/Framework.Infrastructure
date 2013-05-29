using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Framework.Infrastructure.Querying
{
    /// <summary>
    /// 分页信息对象
    /// </summary>
    [DataContract]
    public class Pagination
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Pagination()
        {
            //默认每页记录数为10
            PageSize = 10;
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember]
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        [DataMember]
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 记录总数
        /// </summary>
        [DataMember]
        public int TotalCount
        {
            get;
            set;
        }
    }
}
