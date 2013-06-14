using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Lifestyle;
using Framework.Infrastructure.Logger;

namespace Framework.Infrastructure.Event.MessageBus
{
    public class ComponentInstaller : IComponent
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ISerializer>().ImplementedBy<JsonSerializer>().LifeStyle.Singleton,
                Component.For<IRoutedEventMessageBus>().ImplementedBy<RoutedEventMessageBus>().LifestyleSingleton(),
                Component.For<IRoutedEventContainer>().ImplementedBy<RoutedEventContainer>().LifestyleSingleton()
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
