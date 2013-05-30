using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure;
using Framework.Infrastructure.Container;

namespace Framework.Infrastructure.MessageBus
{
    public static class ServiceFactoryExtension
    {
        public static IMessageBus GetDefaultMessageQueue(this ServiceFactory serviceFactory)
        {
            return serviceFactory.Container.Resolve<IMessageBus>();
        }
    }
}
