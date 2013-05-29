using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Container;
using Unity = Microsoft.Practices.Unity;
using Microsoft.Practices.Unity;

namespace Framework.Infrastructure.Container.UnityContainer
{
    /// <summary>
    /// Unity容器实现类
    /// </summary>
    public class UnityContainer : IContainer
    {
        #region Members

        /// <summary>
        /// Unity容器对象
        /// </summary>
        public Unity.IUnityContainer Container
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

        #region Methods

        #endregion

        #region IContainer Members

        /// <summary>
        /// <see cref="IContainer"/>
        /// </summary>
        /// <param name="serviceType"><see cref="IContainer"/></param>
        /// <returns><see cref="IContainer"/></returns>
        public object GetService(Type serviceType)
        {
            if (!Container.IsRegistered(serviceType))
            {
                return null;
            }
            return Container.Resolve(serviceType);
        }

        /// <summary>
        /// <see cref="IContainer"/>
        /// </summary>
        /// <param name="serviceType"><see cref="IContainer"/></param>
        /// <returns><see cref="IContainer"/></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!Container.IsRegistered(serviceType))
            {
                return null;
            }
            return Container.ResolveAll(serviceType);
        }

        /// <summary>
        /// <see cref="IContainer"/>
        /// </summary>
        /// <param name="serviceType"><see cref="IContainer"/></param>
        /// <param name="name"><see cref="IContainer"/></param>
        /// <returns><see cref="IContainer"/></returns>
        public object GetService(Type serviceType, string name)
        {
            if (!Container.IsRegistered(serviceType, name))
            {
                return null;
            }
            return Container.Resolve(serviceType, name);
        }

        #endregion

    }
}
