using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public interface IConsumerFactory : IDisposable
    {
        DefaultBasicConsumer CreateConsumer(SubscriptionAction subscriptionAction, IModel model, bool modelIsSingleUse, MessageCallback callback);
        void ClearConsumers();
    }
}
