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
    using System.Reflection;
    using Framework.Infrastructure.FluentConfiguration;

    /// <summary>
    /// 服务工厂类定义
    /// </summary>
    public class ServiceFactory
    {
        public const string FrameworkLoggerName = "FrameworkLogger";

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

        /// <summary>
        /// 日志记录对象提供者
        /// </summary>
        public ILoggerProvider LoggerProvider
        {
            get;
            private set;
        }

        /// <summary>
        /// 日志记录对象
        /// </summary>
        protected ILogger Logger
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
            var test = Assembly.GetExecutingAssembly().Location;
            var directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);
            CompositionContainer = new CompositionContainer(directoryCatalog);

            //var directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);
            ////var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            ////var catalog = new AggregateCatalog(assemblyCatalog, directoryCatalog);

            //container = new CompositionContainer(directoryCatalog);
            //container.ComposeParts(this);
        }


        protected void InternalInitialize(Action<IServiceFactoryConfiguration> configure = null)
        {
            var configuration = new ServiceFactoryConfiguration();
            if (configure != null)
            {
                configure(configuration);
            }

            LoggerProvider = configuration.LoggerProvider == null
                ?
                CompositionContainer.GetExportedValue<ILoggerProvider>()
                :
                configuration.LoggerProvider;

            Container = configuration.Container == null 
                ?
                CompositionContainer.GetExportedValue<IContainer>() 
                :
                configuration.Container;

            Logger = LoggerProvider.GetLogger(FrameworkLoggerName);

            ComponentHost = new ComponentHost(CompositionContainer, Logger);
            ComponentHost.Initialize(Container);

            Logger.Info("服务工厂完成初始化");
        }

        /// <summary>
        /// 初始化服务工厂
        /// </summary>
        /// <param name="container"></param>
        public static void Initialize(Action<IServiceFactoryConfiguration> configure = null)
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

        ///// <summary>
        ///// 默认缓存对象提供者
        ///// </summary>
        //public ICacheProvider GetDefaultCacheProvider()
        //{
        //    return Container.Resolve<ICacheProvider>();
        //}

        ///// <summary>
        ///// 默认日志对象提供者
        ///// </summary>
        //public ILoggerProvider GetDefaultLoggerProvider()
        //{
        //    return Container.Resolve<ILoggerProvider>();
        //}

    }
}
