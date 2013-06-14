using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Infrastructure.Cache.Memcached.Test
{
    [TestClass]
    public class MemcachedCacheProviderTest
    {

        private const string TestCacheName = "Test";

        private MemcachedCacheProvider CreateMemcachedCacheProvider()
        {
            return new MemcachedCacheProvider();
        }

        [TestMethod]
        public void GetCache_Test()
        {
            var provider = CreateMemcachedCacheProvider();
            var cache = provider.GetCache(TestCacheName);
            Assert.IsNotNull(cache);
        }

        [TestMethod]
        public void Cache_Get_Test()
        {
            var provider = CreateMemcachedCacheProvider();
            var cache = provider.GetCache(TestCacheName);
            var key = "test";
            var value = "test";
            cache.Add(key, value);

            var result = cache.Get(key).ToString();
            Assert.AreEqual(result, value);
        }

        [TestMethod]
        public void Cache_Remove_Test()
        {
            var provider = CreateMemcachedCacheProvider();
            var cache = provider.GetCache(TestCacheName);
            var key = "test";
            var value = "test";
            cache.Add(key, value);

            var result = cache.Get(key);
            Assert.AreEqual(result, value);

            cache.Remove(key);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Cache_ExpireTime_Test()
        {
            var provider = CreateMemcachedCacheProvider();
            var cache = provider.GetCache(TestCacheName);
            var key = "test";
            var value = "test";
            cache.Add(key, value, 1000);

            System.Threading.Thread.Sleep(5000);

            var result = cache.Get(key);
            Assert.IsNull(result);
        }
    }
}
