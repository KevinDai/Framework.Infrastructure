using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Logger;
using Castle.Core;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace Framework.Infrastructure
{
    /// <summary>
    /// 组件接口定义
    /// </summary>
    public interface IComponent : IWindsorInstaller
    {
        /// <summary>
        /// 启动组件
        /// </summary>
        void Start();

        /// <summary>
        /// 停止组件
        /// </summary>
        void Stop();
    }
}
