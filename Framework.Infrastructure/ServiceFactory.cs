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
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

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
        [Import]
        public IContainer Container
        {
            get;
            private set;
        }

        //[ImportMany]
        //public IEnumerable<IComponent> Components
        //{
        //    get;
        //    private set;
        //}

        internal ComponentHost ComponentHost
        {
            get;
            private set;
        }

        protected CompositionContainer CompositionContainer
        {
            get;
            private set;
        }

        private ServiceFactory()
        {
            var directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);
            CompositionContainer = new CompositionContainer(directoryCatalog);

            //var directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);
            ////var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            ////var catalog = new AggregateCatalog(assemblyCatalog, directoryCatalog);

            //container = new CompositionContainer(directoryCatalog);
            //container.ComposeParts(this);
        }


        protected void InternalInitialize(IContainer container = null)
        {
            if (container != null)
            {
                Container = container;
            }
            else
            {
                CompositionContainer.ComposeParts(this);
            }

            ComponentHost = new ComponentHost(CompositionContainer);
            ComponentHost.Initialize();
            //var directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);
            ////var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            ////var catalog = new AggregateCatalog(assemblyCatalog, directoryCatalog);

            //container = new CompositionContainer(directoryCatalog);
            //container.ComposeParts(this);

        }

        /// <summary>
        /// 初始化服务工厂
        /// </summary>
        /// <param name="container"></param>
        public static void Initialize(IContainer container = null)
        {
            //Preconditions.CheckNotNull(container, "container");

            if (_instance != null)
            {
                throw new InvalidOperationException("服务工厂已经初始化");
            }
            _instance = new ServiceFactory();
            _instance.InternalInitialize();
        }

        /// <summary>
        /// 默认配置提供者
        /// </summary>
        public IConfigurationProvider GetDefaultConfigurationProvider()
        {
            return Container.Resolve<IConfigurationProvider>();
        }

        /// <summary>
        /// 默认缓存对象提供者
        /// </summary>
        public ICacheProvider GetDefaultCacheProvider()
        {
            return Container.Resolve<ICacheProvider>();
        }

        /// <summary>
        /// 默认日志对象提供者
        /// </summary>
        public ILoggerProvider GetDefaultLoggerProvider()
        {
            return Container.Resolve<ILoggerProvider>();
        }

    }
}
