using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Framework.Infrastructure.Cache;

namespace Framework.Infrastructure.Cache.Memcached
{
    /// <summary>
    /// Memcached的缓存对象提供者
    /// </summary>
    public class MemcachedCacheProvider : ICacheProvider
    {

        /// <summary>
        /// Memcached缓存配置信息键值
        /// </summary>
        public const string MemcachedConfigurationItemKey = "Memcached";

        /// <summary>
        /// Memcached客户端对象
        /// </summary>
        protected MemcachedClient MemcachedClient
        {
            get;
            private set;
        }

        public MemcachedCacheProvider()
        {
            MemcachedClient = new MemcachedClient("memcached");
            //var item = ServiceFactory.Instance.GetDefaultConfigurationProvider().GetItem<MemcachedConfiguration>(MemcachedConfigurationItemKey);
            //MemcachedClient = new MemcachedClient(item.ConvertTo());
        }

        //public MemcachedCacheProvider(MemcachedConfiguration configuration)
        //{
        //    if (configuration == null)
        //    {
        //        throw new ArgumentNullException("configuration");
        //    }
        //    MemcachedClient = new MemcachedClient(configuration.ConvertTo());
        //    var test = MemcachedClient.Stats();
        //}

        /// <summary>
        /// <see cref="ICacheProvider"/>
        /// </summary>
        /// <param name="name"><see cref="ICacheProvider"/></param>
        /// <returns><see cref="ICacheProvider"/></returns>
        public ICache GetCache(string name)
        {
            return new MemcachedCache(name, MemcachedClient);
        }
    }
}