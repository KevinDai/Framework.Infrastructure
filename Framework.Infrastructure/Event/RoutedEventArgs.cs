using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Event
{
    /// <summary>
    /// 路由事件参数
    /// </summary>
    public class RoutedEventArgs
    {
        public static readonly RoutedEventArgs Empty = new RoutedEventArgs();
    }

    public class RoutedEventArgs<T> : RoutedEventArgs
    {
        public T Parameter
        {
            get;
            set;
        }
    }
}
