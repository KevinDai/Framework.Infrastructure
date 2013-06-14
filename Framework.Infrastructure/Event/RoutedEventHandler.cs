using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Event
{
    /// <summary>
    /// 路由事件Handler
    /// </summary>
    /// <param name="sender"></param>
    public delegate void RoutedEventHandler(RoutedEventSender sender, RoutedEventArgs args);

    /// <summary>
    /// 路由事件Handler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void RoutedEventHandler<T>(RoutedEventSender sender, T args) where T : RoutedEventArgs;
}
