using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure
{
    using Container;
    using Cache;
    using Configuration;
    using Logger;

    /// <summary>
    /// 服务工厂，从IOC容器中获取各种服务对象
    /// </summary>
    public class ServiceFactory
    {
        /// <summary>
        /// 服务工厂静态实例对象
        /// </summary>
        private static ServiceFactory _instance;

        /// <summary>
        /// 服务工厂静态实例对象
        /// </summary>
        public static ServiceFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// IOC容器对象
        /// </summary>
        public IContainer Container
        {
            get;
            private set;
        }

        private ServiceFactory(IContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// 初始化服务工厂
        /// </summary>
        /// <param name="container"></param>
        public static void Initialize(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (_instance != null)
            {
                throw new InvalidOperationException("服务工厂的IOC容器已经初始化");
            }
            _instance = new ServiceFactory(container);
        }

        /// <summary>
        /// 默认配置提供者
        /// </summary>
        public IConfigurationProvider GetDefaultConfigurationProvider()
        {
            return Container.GetService<IConfigurationProvider>();
        }

        /// <summary>
        /// 默认缓存对象提供者
        /// </summary>
        public ICacheProvider GetDefaultCacheProvider()
        {
            return Container.GetService<ICacheProvider>();
        }

        /// <summary>
        /// 默认日志对象提供者
        /// </summary>
        public ILoggerProvider GetDefaultLoggerProvider()
        {
            return Container.GetService<ILoggerProvider>();
        }

    }
}
