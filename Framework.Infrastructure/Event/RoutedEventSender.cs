using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Event
{
    /// <summary>
    /// 路由事件触发对象信息类
    /// </summary>
    public class RoutedEventSender
    {
        /// <summary>
        /// 应用程序ID
        /// </summary>
        public string ApplicationId
        {
            get;
            set;
        }

        /// <summary>
        /// 应用程序类型
        /// </summary>
        public string ApplicationType
        {
            get;
            set;
        }

        /// <summary>
        /// 应用程序分组
        /// </summary>
        public string ApplicationGroup
        {
            get;
            set;
        }

        /// <summary>
        /// 触发对象类型名称
        /// </summary>
        public string SenderTypeName
        {
            get;
            set;
        }

    }
}
