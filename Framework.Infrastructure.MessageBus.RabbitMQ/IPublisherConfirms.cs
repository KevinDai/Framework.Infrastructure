using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public interface IPublisherConfirms
    {
        void RegisterCallbacks(IModel channel, Action successCallback, Action failureCallback);
        void SuccessfulPublish(IModel channel, BasicAckEventArgs args);
        void FailedPublish(IModel channel, BasicNackEventArgs args);
    }
}
