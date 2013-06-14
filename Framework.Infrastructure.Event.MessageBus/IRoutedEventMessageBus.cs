using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Event.MessageBus
{
    public interface IRoutedEventMessageBus
    {
        void Send(RoutedEventMessage message, RaiseScope raiseScope);
        event Action<RoutedEventMessage> OnRecived;
    }
}
