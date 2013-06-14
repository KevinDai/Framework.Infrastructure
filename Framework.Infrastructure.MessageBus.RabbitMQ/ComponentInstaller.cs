using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Lifestyle;
using Framework.Infrastructure.Logger;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public class ComponentInstaller : IComponent
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMessageBusProvider>().ImplementedBy<RabbitMessageBusProvider>().LifeStyle.Singleton,
                Component.For<IMessageBus>().ImplementedBy<RabbitMessageBus>().LifeStyle.Transient,
                Component.For<ISerializer>().ImplementedBy<JsonSerializer>().LifeStyle.Singleton,
                Component.For<IConventions>().ImplementedBy<Conventions>().LifeStyle.Singleton,
                Component.For<IConnectionConfiguration>().ImplementedBy<ConnectionConfiguration>().LifestyleScoped(),
                Component.For<IConnectionFactory>().ImplementedBy<ConnectionFactoryWrapper>().LifestyleBoundToNearest<IMessageBus>(),
                Component.For<IClusterHostSelectionStrategy<ConnectionFactoryInfo>>()
                    .ImplementedBy<DefaultClusterHostSelectionStrategy<ConnectionFactoryInfo>>().LifestyleBoundToNearest<IMessageBus>(),
                Component.For<IConsumerFactory>().ImplementedBy<QueueingConsumerFactory>().LifestyleBoundToNearest<IMessageBus>(),
                Component.For<IConsumerErrorStrategy>().ImplementedBy<DefaultConsumerErrorStrategy>().LifestyleBoundToNearest<IMessageBus>(),
                Component.For<IPersistentConnection>().ImplementedBy<PersistentConnection>().LifestyleBoundToNearest<IMessageBus>()
                );
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
