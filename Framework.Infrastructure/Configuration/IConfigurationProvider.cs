using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Configuration
{
    public interface IConfigurationProvider
    {

        /// <summary>
        /// 根据键值从配置中取出一项配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>配置对象</returns>
        T GetItem<T>(string key);

        /// <summary>
        /// 根据配置类型名从配置中取出一组配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="typeName">配置类型名称</param>
        /// <returns>配置对象</returns>
        IEnumerable<T> GetItems<T>(string typeName);
    }
}
