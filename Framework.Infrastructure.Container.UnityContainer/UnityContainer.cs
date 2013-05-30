using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Container;
using Unity = Microsoft.Practices.Unity;
using Microsoft.Practices.Unity;
using Framework.Infrastructure.Logger;
using System.Collections;
using System.ComponentModel.Composition;

namespace Framework.Infrastructure.Container.UnityContainer
{
    /// <summary>
    /// Unity容器实现类
    /// </summary>
    [Export(typeof(IContainer))]
    public class UnityContainer : IContainer
    {
        #region 字段属性

        /// <summary>
        /// Unity容器对象
        /// </summary>
        protected Unity.IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        /// Unity容器对象
        /// </summary>
        private Unity.IUnityContainer _container;

        #endregion

        #region Constructor

        public UnityContainer()
        {
            _container = new Unity.UnityContainer();
        }

        public UnityContainer(Unity.IUnityContainer unityContainer)
        {
            if (unityContainer == null)
            {
                throw new ArgumentNullException("unityContainer");
            }
            _container = unityContainer;
        }

        #endregion

        #region 方法

        private LifetimeManager GetLifetimeManager(LifeTime lifeTime)
        {
            switch (lifeTime)
            {
                case LifeTime.Transient:
                    return new TransientLifetimeManager();
                case LifeTime.Singleton:
                    return new ContainerControlledLifetimeManager();
                default:
                    throw new NotImplementedException(string.Format("不支持对象生存周期{0}", lifeTime.ToString()));
            }
        }

        private static ResolverOverride[] CreateParameterOverride(IDictionary<string, object> arguments)
        {
            if (arguments == null || !arguments.Any())
            {
                return new ResolverOverride[0];
            }

            return arguments.Select(a => new ParameterOverride(a.Key, a.Value)).ToArray();
        }

        #endregion

        #region IContainer接口实现

        public IContainer RegisterInstance(Type t, string name, object instance)
        {
            Container.RegisterInstance(t, name, instance);
            return this;
        }

        public IContainer RegisterType(Type from, Type to, string name, Container.LifeTime lifeTime)
        {
            Container.RegisterType(from, to, name, GetLifetimeManager(lifeTime));
            return this;
        }

        public object Resolve(Type t, string name, IDictionary<string, object> arguments)
        {
            return Container.Resolve(t, name, CreateParameterOverride(arguments));
        }

        public IEnumerable<object> ResolveAll(Type t, IDictionary<string, object> arguments)
        {
            return Container.ResolveAll(t, CreateParameterOverride(arguments));
        }

        public bool IsRegistered(Type typeToCheck, string nameToCheck)
        {
            return Container.IsRegistered(typeToCheck, nameToCheck);
        }

        #endregion
    }
}
