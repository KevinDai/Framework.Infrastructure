using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Cache
{
    /// <summary>
    /// 缓存对象扩展方法
    /// </summary>
    public static class CacheExtension
    {
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <typeparam name="T">缓存值的类型</typeparam>
        /// <param name="cache">缓存对象</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static T Get<T>(this ICache cache, string key)
        {
            return (T)cache.Get(key);
        }
    }
}
