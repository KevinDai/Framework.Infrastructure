using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Framework.Infrastructure.Container
{
    /// <summary>
    /// IOC容器扩展
    /// </summary>
    public static class ContainerExtension
    {
        public static bool IsRegistered<T>(this IContainer container)
        {
            return container.IsRegistered(typeof(T), null);
        }

        public static bool IsRegistered<T>(this IContainer container, string nameToCheck)
        {
            return container.IsRegistered(typeof(T), nameToCheck);
        }

        public static IContainer RegisterInstance<T>(this IContainer container, T instance)
        {
            return container.RegisterInstance<T>(null, instance);
        }

        public static IContainer RegisterInstance<T>(this IContainer container, string name, T instance)
        {
            return container.RegisterInstance(typeof(T), name, instance);
        }

        public static IContainer RegisterType<TFrom, TTo>(this IContainer container, string name, LifeTime lifeTime = LifeTime.Transient) where TTo : TFrom
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), null, lifeTime);
        }

        public static IContainer RegisterType<TFrom, TTo>(this IContainer container, LifeTime lifeTime = LifeTime.Transient) where TTo : TFrom
        {
            return container.RegisterType<TFrom, TTo>(null, lifeTime);
        }

        public static T Resolve<T>(this IContainer container, IDictionary<string, object> arguments = null)
        {
            return container.Resolve<T>(null, arguments);
        }
        public static T Resolve<T>(this IContainer container, string name, IDictionary<string, object> arguments = null)
        {
            return (T)container.Resolve(typeof(T), name, arguments);
        }
    }
}
