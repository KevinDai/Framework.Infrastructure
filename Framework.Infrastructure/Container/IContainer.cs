using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Container
{
    /// <summary>
    /// 适配不同Ioc容器接口定义
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// 根据类型获取实例对象
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>服务对象</returns>
        object GetService(Type serviceType);

        /// <summary>
        /// 根据名称和服务类型获取实例对象
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="name">服务名称</param>
        /// <returns>服务对象</returns>
        object GetService(Type serviceType, string name);

        /// <summary>
        /// 根据类型获取多个实例对象
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>服务对象集合</returns>
        IEnumerable<object> GetServices(Type serviceType);

    }
}
