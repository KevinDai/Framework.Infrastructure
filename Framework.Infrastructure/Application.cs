using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Framework.Infrastructure.Cache;
using Framework.Infrastructure.Configuration;
using Framework.Infrastructure.Logger;
using System.Reflection;
using System.Configuration;
using Castle.Core;
using Castle.Windsor;
using Castle.Windsor.Configuration;
using Castle.Windsor.Configuration.Interpreters;

namespace Framework.Infrastructure
{

    /// <summary>
    /// 应用程序类
    /// </summary>
    public class Application
    {
        public const string ApplicationLoggerName = "ApplicationLogger";

        /// <summary>
        /// 当前应用程序对象
        /// </summary>
        private static Application _current;

        /// <summary>
        /// 当前应用程序对象
        /// </summary>
        public static Application Current
        {
            get
            {
                return _current;
            }
        }

        /// <summary>
        /// IOC容器对象
        /// </summary>
        public IWindsorContainer Container
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
        public ILogger Logger
        {
            get;
            private set;
        }

        /// <summary>
        /// 应用程序Id
        /// </summary>
        public string ApplicationId
        {
            get;
            private set;
        }

        /// <summary>
        /// 应用类型
        /// </summary>
        public string ApplicationType
        {
            get;
            private set;
        }

        /// <summary>
        /// 应用分组
        /// </summary>
        public string ApplicationGroup
        {
            get;
            private set;
        }

        /// <summary>
        /// 组件管理对象类
        /// </summary>
        internal ComponentHost ComponentHost
        {
            get;
            private set;
        }

        private Application(IWindsorContainer container)
        {
            ApplicationId = AppSettings.ApplicationId ?? Guid.NewGuid().ToString();
            ApplicationType = AppSettings.ApplicationType ?? string.Empty;
            ApplicationGroup = AppSettings.ApplicationGroup ?? string.Empty;

            Container = container;
            LoggerProvider = Container.Resolve<ILoggerProvider>();
            Logger = LoggerProvider.GetLogger(ApplicationLoggerName);

            Logger.Info(string.Format("初始化当前应用程序[{0}]", ApplicationId));

            ComponentHost = new ComponentHost(Logger);
            ComponentHost.Install(Container);
        }

        /// <summary>
        /// 启动应用程序
        /// </summary>
        public void Start()
        {
            Logger.Info("启动应用程序");

            ComponentHost.Start();
        }

        /// <summary>
        /// 关闭应用程序
        /// </summary>
        public void Stop()
        {
            Logger.Info("停止应用程序");

            ComponentHost.Stop();

            Container.Dispose();
        }

        /// <summary>
        /// 初始化当前应用程序
        /// </summary>
        public static void InitCurrent()
        {
            InitCurrent(new WindsorContainer(new XmlInterpreter()));
        }

        /// <summary>
        /// 初始化当前应用程序
        /// </summary>
        /// <param name="container"></param>
        public static void InitCurrent(IWindsorContainer container)
        {
            Preconditions.CheckNotNull(container, "container");

            if (_current != null)
            {
                throw new InvalidOperationException("当前应用程序已经初始化");
            }
            _current = new Application(container);
        }

        /// <summary>
        /// 默认配置提供者
        /// </summary>
        //public IConfigurationProvider GetDefaultConfigurationProvider()
        //{
        //    return Container.Resolve<IConfigurationProvider>();
        //}

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
