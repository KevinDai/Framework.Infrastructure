using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Cache
{
    /// <summary>
    /// 缓存对象提供者接口定义
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// 获取指定名称的缓存对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>缓存对象</returns>
        ICache GetCache(string name);
    }
}
