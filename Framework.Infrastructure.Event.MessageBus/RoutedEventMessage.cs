using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Event.MessageBus
{

    /// <summary>
    /// 路由事件消息
    /// </summary>
    public class RoutedEventMessage
    {
        public string RoutedEventName
        {
            get;
            set;
        }

        public RoutedEventSender Sender
        {
            get;
            set;
        }

        public object Args
        {
            get;
            set;
        }

        public RoutedEventMessage()
        {
        }

        public RoutedEventMessage(RoutedEventBase routedEvent, RoutedEventSender sender, object args)
        {
            Preconditions.CheckNotNull(routedEvent, "routedEvent");
            Preconditions.CheckNotNull(sender, "sender");

            RoutedEventName = routedEvent.Name;
            Sender = sender;
            Args = args;
        }

        public IEnumerable<string> Valid()
        {
            if (RoutedEventName == null)
            {
                yield return "路由事件的名称不能为空";
            }
            if (Sender == null)
            {
                yield return "路由事件发送者信息不能为空";
            }
        }
    }
}
