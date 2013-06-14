using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Logger;
using Castle.Core;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;
using System.IO;

namespace Framework.Infrastructure
{
    /// <summary>
    /// 组件管理对象类
    /// </summary>
    internal class ComponentHost : InstallerFactory
    {
        /// <summary>
        /// 所有加载的组件集合
        /// </summary>
        protected IList<IComponent> Components
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

        public ComponentHost(ILogger logger)
        {
            Logger = logger;
            Components = new List<IComponent>();
        }

        /// <summary>
        /// 加载组件
        /// </summary>
        /// <param name="container"></param>
        public void Install(IWindsorContainer container)
        {
            string componentsPath = AppDomain.CurrentDomain.BaseDirectory;
            if (AppSettings.ComponentsPath != null)
            {
                if (Path.IsPathRooted(AppSettings.ComponentsPath))
                {
                    componentsPath = AppSettings.ComponentsPath;
                }
                else
                {
                    componentsPath = Path.Combine(componentsPath, AppSettings.ComponentsPath);
                }
            }
            var installer = FromAssembly.InDirectory(new AssemblyFilter(componentsPath), this);
            container.Install(installer);

            ComponentsExecute(c =>
            {
                Logger.Info(string.Format("{0}组件加载", c.GetType().FullName));
            });
        }

        /// <summary>
        /// 启动所有组件
        /// </summary>
        public void Start()
        {
            ComponentsExecute(c =>
            {
                c.Start();
                Logger.Debug(string.Format("{0}组件启动", c.GetType().FullName));
            });
        }

        /// <summary>
        /// 停止所有组件
        /// </summary>
        public void Stop()
        {
            ComponentsExecute(c =>
            {
                c.Stop();
                Logger.Debug(string.Format("{0}组件停止", c.GetType().FullName));
            });
        }

        /// <summary>
        /// 所有组件执行指定操作
        /// </summary>
        /// <param name="action">指定操作</param>
        private void ComponentsExecute(Action<IComponent> action)
        {
            foreach (var component in Components)
            {
                action(component);
            }
        }

        /// <summary>
        /// 创建组件实例对象
        /// </summary>
        /// <param name="installerType"></param>
        /// <returns></returns>
        public override IWindsorInstaller CreateInstance(Type installerType)
        {
            var instance = base.CreateInstance(installerType);
            Components.Add(instance as IComponent);
            return instance;
        }

        /// <summary>
        /// 筛选符合条件的组件
        /// </summary>
        /// <param name="installerTypes"></param>
        /// <returns></returns>
        public override IEnumerable<Type> Select(IEnumerable<Type> installerTypes)
        {
            return installerTypes.Where(t => typeof(IComponent).IsAssignableFrom(t));
        }
    }

}
