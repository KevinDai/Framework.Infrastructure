using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Event
{

    public abstract class RoutedEventBase
    {
        public string Name
        {
            get;
            private set;
        }

        public RoutedEventBase(string name)
        {
            Preconditions.CheckNotBlank(name, "name");

            Name = name;
        }
    }

    /// <summary>
    /// 路由事件定义类
    /// </summary>
    public class RoutedEvent : RoutedEventBase
    {
        public RoutedEvent(string name)
            : base(name)
        {
        }
    }

    /// <summary>
    /// 带特定参数的路由事件定义类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RoutedEvent<T> : RoutedEventBase
        where T : RoutedEventArgs
    {
        public RoutedEvent(string name)
            : base(name)
        {
        }
    }
}
