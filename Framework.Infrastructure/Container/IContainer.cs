using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Framework.Infrastructure.Container
{
    /// <summary>
    /// IOC容器接口定义
    /// </summary>
    public interface IContainer
    {

        /// <summary>
        /// 注册指定类型的对象
        /// </summary>
        /// <param name="t">指定的类型</param>
        /// <param name="name">注册名称</param>
        /// <param name="instance">对象</param>
        /// <returns>IOC容器对象</returns>
        IContainer RegisterInstance(Type t, string name, object instance);

        /// <summary>
        /// 注册指定类型的具体实现类型
        /// </summary>
        /// <param name="from">指定的类型</param>
        /// <param name="to">具体实现类型</param>
        /// <param name="name">注册名称</param>
        /// <param name="lifeTime">创建对象的生存周期</param>
        /// <returns>IOC容器对象</returns>
        IContainer RegisterType(Type from, Type to, string name, LifeTime lifeTime);

        /// <summary>
        /// 获取指定类型的对象
        /// </summary>
        /// <param name="t">指定类型</param>
        /// <param name="name">注册名称</param>
        /// <param name="arguments">参数信息</param>
        /// <returns>对象</returns>
        object Resolve(Type t, string name, IDictionary<string, object> arguments);

        /// <summary>
        /// 获取指定类型的对象集合
        /// </summary>
        /// <param name="t">指定类型</param>
        /// <param name="arguments">参数信息</param>
        /// <returns>对象集合</returns>
        IEnumerable<object> ResolveAll(Type t, IDictionary<string, object> arguments);

        /// <summary>
        /// 是否存在指定类型以及注册名称的注册信息
        /// </summary>
        /// <param name="typeToCheck">指定类型</param>
        /// <param name="nameToCheck">注册名称</param>
        /// <returns>是否存在注册信息</returns>
        bool IsRegistered(Type typeToCheck, string nameToCheck);
    }
}
