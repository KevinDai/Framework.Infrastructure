using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Logger
{
    /// <summary>
    /// 日志记录对象提供者接口定义
    /// </summary>
    public interface ILoggerProvider
    {
        /// <summary>
        /// 根据名称获取日志对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>日志记录对象</returns>
        ILogger GetLogger(string name);

    }
}
