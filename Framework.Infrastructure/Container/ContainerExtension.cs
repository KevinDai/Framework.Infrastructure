using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Container
{
    /// <summary>
    /// IOC容器扩展
    /// </summary>
    public static class ContainerExtension
    {
        /// <summary>
        /// 根据类型获取实例对象
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="resolver">Ioc容器对象</param>
        /// <returns>服务对象</returns>
        public static TService GetService<TService>(this IContainer resolver)
        {
            return (TService)resolver.GetService(typeof(TService));
        }

        /// <summary>
        /// 根据名称和服务类型获取实例对象
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="resolver">Ioc容器对象</param>
        /// <param name="name">服务名称</param>
        /// <returns>服务对象</returns>
        public static TService GetService<TService>(this IContainer resolver, string name)
        {
            return (TService)resolver.GetService(typeof(TService), name);
        }

        /// <summary>
        /// 根据类型获取多个实例对象
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="resolver">Ioc容器对象</param>
        /// <returns>服务对象集合</returns>
        public static IEnumerable<TService> GetServices<TService>(this IContainer resolver)
        {

            return resolver.GetServices(typeof(TService)).Cast<TService>();
        }
    }
}
