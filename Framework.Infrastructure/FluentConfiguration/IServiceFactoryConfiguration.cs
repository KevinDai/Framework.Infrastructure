using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Container;
using Framework.Infrastructure.Logger;

namespace Framework.Infrastructure.FluentConfiguration
{
    public interface IServiceFactoryConfiguration
    {
        IServiceFactoryConfiguration WithContainer(IContainer container);

        IServiceFactoryConfiguration WithLoggerProvider(ILoggerProvider loggerProvider);
    }

    public class ServiceFactoryConfiguration : IServiceFactoryConfiguration
    {
        /// <summary>
        /// IOC容器对象
        /// </summary>
        public IContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// 日志记录对象提供者
        /// </summary>
        public ILoggerProvider LoggerProvider
        {
            get;
            private set;
        }

        public IServiceFactoryConfiguration WithContainer(IContainer container)
        {
            Preconditions.CheckNotNull(container, "container");

            Container = container;
            return this;
        }

        public IServiceFactoryConfiguration WithLoggerProvider(ILoggerProvider loggerProvider)
        {
            Preconditions.CheckNotNull(loggerProvider, "loggerProvider");

            LoggerProvider = loggerProvider;
            return this;
        }
    }


}
