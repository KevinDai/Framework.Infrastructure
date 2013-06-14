using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.MessageBus.FluentConfiguration;
using Framework.Infrastructure.MessageBus.Topology;

namespace Framework.Infrastructure.MessageBus
{
    /// <summary>
    /// 消息发送管道接口定义
    /// </summary>
    public interface IPublishChannel : IDisposable
    {
        void Publish<T>(IExchange exchange, string routingKey, IMessage<T> message, Action<IPublishConfiguration> configure = null);

        void Publish(IExchange exchange, string routingKey, MessageProperties properties, byte[] messageBody, Action<IPublishConfiguration> configure = null);

        void Publish<T>(IQueue queue, IMessage<T> message, Action<IPublishConfiguration> configure = null);

        void Publish(IQueue queue, MessageProperties properties, byte[] messageBody, Action<IPublishConfiguration> configure = null);

        //bool WaitForConfirms();

        //bool WaitForConfirms(TimeSpan timeout, out bool timedOut);
    }

}
