using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Event
{
    /// <summary>
    /// 路由事件容器接口定义
    /// </summary>
    public interface IRoutedEventContainer
    {
        /// <summary>
        /// 监听指定的事件
        /// </summary>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="handler">事件处理方法</param>
        void AddHandler(RoutedEvent routedEvent, RoutedEventHandler handler);

        /// <summary>
        /// 监听指定的事件
        /// </summary>
        /// <typeparam name="T">路由事件参数的参数类型</typeparam>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="handler">事件处理方法</param>
        void AddHandler<T>(RoutedEvent<T> routedEvent, RoutedEventHandler<T> handler) where T : RoutedEventArgs;

        /// <summary>
        /// 监听指定的事件
        /// </summary>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="handler">事件处理方法</param>
        void RemoveHandler(RoutedEvent routedEvent, RoutedEventHandler handler);

        /// <summary>
        /// 移除对指定的事件的处理
        /// </summary>
        /// <typeparam name="T">路由事件参数的参数类型</typeparam>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="handler">事件处理方法</param>
        void RemoveHandler<T>(RoutedEvent<T> routedEvent, RoutedEventHandler<T> handler) where T : RoutedEventArgs;

        /// <summary>
        /// 移除对指定的事件的处理
        /// </summary>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="sender">触发对象</param>
        void Raise(RoutedEvent routedEvent, object sender, RoutedEventArgs args);

        /// <summary>
        /// 触发所有应用指定的路由事件
        /// </summary>
        /// <typeparam name="T">路由事件参数的参数类型</typeparam>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="sender">触发对象</param>
        /// <param name="args">事件参数</param>
        void Raise<T>(RoutedEvent<T> routedEvent, object sender, T args) where T : RoutedEventArgs;

        /// <summary>
        /// 触发指定的范围内的应用的指定的路由事件
        /// </summary>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="sender">触发对象</param>
        /// <param name="scope">触发事件的范围</param>
        void Raise(RoutedEvent routedEvent, object sender, RoutedEventArgs args, RaiseScope scope);

        /// <summary>
        /// 触发指定的范围内的应用的指定的路由事件
        /// </summary>
        /// <typeparam name="T">路由事件参数的参数类型</typeparam>
        /// <param name="routedEvent">路由事件</param>
        /// <param name="sender">触发对象</param>
        /// <param name="args">事件参数</param>
        /// <param name="scope">触发事件的范围</param>
        void Raise<T>(RoutedEvent<T> routedEvent, object sender, T args, RaiseScope scope) where T : RoutedEventArgs;
    }
}
