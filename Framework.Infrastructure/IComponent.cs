using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Container;
using Framework.Infrastructure.Logger;

namespace Framework.Infrastructure
{
    /// <summary>
    /// 组件接口定义
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 组件初始化时处理方法
        /// </summary>
        /// <param name="ontainer"></param>
        void OnComponentsInitializing(IContainer ontainer, ILogger logger);

        /// <summary>
        /// 组件完成初始化时处理方法（所有组件）
        /// </summary>
        void OnComponentsInitialized();
    }
}
