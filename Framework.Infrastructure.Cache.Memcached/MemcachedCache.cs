using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Framework.Infrastructure.Cache;
using System.Net;

namespace Framework.Infrastructure.Cache.Memcached
{
    /// <summary>
    /// Memcached缓存对象
    /// </summary>
    public class MemcachedCache : ICache
    {
        /// <summary>
        /// 缓存键值的最大长度
        /// </summary>
        public static readonly int KeyMaxLength = 250;

        /// <summary>
        /// 缓存对象名称
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Memcached客户端对象
        /// </summary>
        protected MemcachedClient MemcachedClient
        {
            get;
            private set;
        }

        public MemcachedCache(string name, MemcachedClient memcachedClient)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (memcachedClient == null)
            {
                throw new ArgumentNullException("memcachedClient");
            }

            Name = name;
            MemcachedClient = memcachedClient;
        }

        /// <summary>
        /// 获取完整缓存键值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>完整缓存键值</returns>
        private string GetKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            var result = string.Format("{0}_{1}", Name, key);
            if (key.Length > KeyMaxLength)
            {
                throw new ArgumentException(string.Format("缓存的键值{0}超过键值的最大长度{1}", result, KeyMaxLength));
            }
            return result;
        }

        /// <summary>
        /// <see cref="ICache"/>
        /// </summary>
        /// <param name="key"><see cref="ICache"/></param>
        /// <param name="value"><see cref="ICache"/></param>
        /// <returns><see cref="ICache"/></returns>
        public bool Add(string key, object value)
        {
            return MemcachedClient.Store(StoreMode.Set, GetKey(key), value);
        }

        /// <summary>
        /// <see cref="ICache"/>
        /// </summary>
        /// <param name="key"><see cref="ICache"/></param>
        /// <param name="value"><see cref="ICache"/></param>
        /// <param name="expireTime"><see cref="ICache"/></param>
        /// <returns><see cref="ICache"/></returns>
        public bool Add(string key, object value, long expireTime)
        {
            return MemcachedClient.Store(StoreMode.Set, GetKey(key), value, TimeSpan.FromMilliseconds(expireTime));
        }

        /// <summary>
        /// <see cref="ICache"/>
        /// </summary>
        /// <param name="key"><see cref="ICache"/></param>
        /// <returns><see cref="ICache"/></returns>
        public object Get(string key)
        {
            return MemcachedClient.Get(GetKey(key));
        }

        /// <summary>
        /// <see cref="ICache"/>
        /// </summary>
        /// <param name="key"><see cref="ICache"/></param>
        public void Remove(string key)
        {
            MemcachedClient.Remove(GetKey(key));
        }
    }
}
