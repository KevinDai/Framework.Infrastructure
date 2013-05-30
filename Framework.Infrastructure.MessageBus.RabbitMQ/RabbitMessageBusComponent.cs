using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Framework.Infrastructure.Container;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    [Export(typeof(IComponent))]
    public class RabbitMessageBusComponent : IComponent
    {
        public string Name
        {
            get
            {
                return "RabbitMessageBusComponent";
            }
        }

        public void Initialize()
        {
            ServiceFactory.Instance.Container
                .RegisterType<ISerializer, JsonSerializer>(LifeTime.Singleton)
                .RegisterType<IConventions, Conventions>(LifeTime.Singleton)
                .RegisterType<IConnectionFactory, ConnectionFactoryWrapper>(LifeTime.Singleton)
                .RegisterType<IClusterHostSelectionStrategy<ConnectionFactoryInfo>, DefaultClusterHostSelectionStrategy<ConnectionFactoryInfo>>(LifeTime.Singleton)
                .RegisterType<IConsumerFactory, QueueingConsumerFactory>(LifeTime.Singleton)
                .RegisterType<IConsumerErrorStrategy, DefaultConsumerErrorStrategy>(LifeTime.Singleton);
        }
    }
}
