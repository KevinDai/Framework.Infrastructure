using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Container
{
    /// <summary>
    /// 容器注册服务的生存周期
    /// </summary>
    public enum LifeTime
    {
        Singleton = 1,

        Transient = 2
    }
}
