using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Logger;
using System.Collections;
using Framework.Infrastructure.MessageBus;

namespace Framework.Infrastructure.Event.MessageBus
{
    /// <summary>
    /// 路由事件容器
    /// </summary>
    public class RoutedEventContainer : IRoutedEventContainer, IDisposable
    {

        #region 字段属性

        private ConcurrentDictionary<string, Delegate> _routedEvents = new ConcurrentDictionary<string, Delegate>();

        public IRoutedEventMessageBus RoutedEventMessageBus
        {
            get;
            private set;
        }

        #endregion

        #region 构造函数

        public RoutedEventContainer(IRoutedEventMessageBus routedEventMessageBus)
        {
            Preconditions.CheckNotNull(routedEventMessageBus, "routedEventMessageBus");

            RoutedEventMessageBus = routedEventMessageBus;
            RoutedEventMessageBus.OnRecived += OnRecivedRoutedEventMessage;
        }

        #endregion

        #region 方法

        protected virtual void AddHandler(string eventName, Delegate handler)
        {
            _routedEvents.AddOrUpdate(eventName, handler, (key, value) => Delegate.Combine(value, handler));
        }

        protected virtual void RemoveHandler(string eventName, Delegate handler)
        {
            _routedEvents.AddOrUpdate(eventName, handler, (key, value) => Delegate.Remove(value, handler));
        }

        protected virtual void SendRoutedEventMessage(RoutedEventBase routedEvent, object sender,
            RoutedEventArgs args, RaiseScope raiseScope = null)
        {
            var message = new RoutedEventMessage(routedEvent, CreateCurrentApplicationSender(sender), args);
            RoutedEventMessageBus.Send(message, raiseScope);
        }


        protected virtual void OnRecivedRoutedEventMessage(RoutedEventMessage message)
        {
            Delegate handlers = null;
            if (_routedEvents.TryGetValue(message.RoutedEventName, out handlers))
            {
                handlers.DynamicInvoke(message.Sender, message.Args);
            }
        }

        protected RoutedEventSender CreateCurrentApplicationSender(object sender)
        {
            return new RoutedEventSender()
            {
                ApplicationId = Application.Current.ApplicationId,
                ApplicationType = Application.Current.ApplicationType,
                ApplicationGroup = Application.Current.ApplicationGroup,
                SenderTypeName = sender == null ? string.Empty : sender.GetType().FullName
            };
        }

        #endregion

        #region IRoutedEventContainer接口实现

        public void AddHandler(RoutedEvent routedEvent, RoutedEventHandler handler)
        {
            Preconditions.CheckNotNull(routedEvent, "routedEvent");
            Preconditions.CheckNotNull(handler, "handler");

            AddHandler(routedEvent.Name, handler);
        }

        public void AddHandler<T>(RoutedEvent<T> routedEvent, RoutedEventHandler<T> handler)
             where T : RoutedEventArgs
        {
            Preconditions.CheckNotNull(routedEvent, "routedEvent");
            Preconditions.CheckNotNull(handler, "handler");

            AddHandler(routedEvent.Name, handler);
        }

        public void RemoveHandler(RoutedEvent routedEvent, RoutedEventHandler handler)
        {
            Preconditions.CheckNotNull(routedEvent, "routedEvent");
            Preconditions.CheckNotNull(handler, "handler");

            RemoveHandler(routedEvent.Name, handler);
        }

        public void RemoveHandler<T>(RoutedEvent<T> routedEvent, RoutedEventHandler<T> handler)
             where T : RoutedEventArgs
        {
            Preconditions.CheckNotNull(routedEvent, "routedEvent");
            Preconditions.CheckNotNull(handler, "handler");

            RemoveHandler(routedEvent.Name, handler);
        }

        public void Raise(RoutedEvent routedEvent, object sender, RoutedEventArgs args)
        {
            SendRoutedEventMessage(routedEvent, sender, args);
        }

        public void Raise<T>(RoutedEvent<T> routedEvent, object sender, T args) where T : RoutedEventArgs
        {
            SendRoutedEventMessage(routedEvent, sender, args);
        }

        public void Raise(RoutedEvent routedEvent, object sender, RoutedEventArgs args, RaiseScope scope)
        {
            SendRoutedEventMessage(routedEvent, sender, args, scope);
        }

        public void Raise<T>(RoutedEvent<T> routedEvent, object sender, T args, RaiseScope scope) where T : RoutedEventArgs
        {
            SendRoutedEventMessage(routedEvent, sender, args, scope);
        }

        #endregion

        private bool disposed = false;
        public virtual void Dispose()
        {
            if (disposed) return;

            _routedEvents = null;
            disposed = true;
        }
    }
}
