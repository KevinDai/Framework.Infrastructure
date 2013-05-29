using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace Framework.Infrastructure.Cache.Memcached
{
    /// <summary>
    /// 自定义的Memcached缓存配置信息
    /// </summary>
    public class MemcachedConfiguration
    {
        public List<string> Address
        {
            get;
            set;
        }

        public MemcachedClientConfiguration ConvertTo()
        {
            MemcachedClientConfiguration configuration = new MemcachedClientConfiguration();
            configuration.Protocol = MemcachedProtocol.Text;
            foreach (var item in Address)
            {
                configuration.AddServer(item);
            }
            return configuration;
        }
    }
}
