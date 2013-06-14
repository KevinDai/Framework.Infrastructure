using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Repository;

namespace Framework.Infrastructure.Logger.Log4net
{
    /// <summary>
    /// log4net日志对象提供者
    /// </summary>
    public class Log4netLoggerProvider : ILoggerProvider
    {
        public Log4netLoggerProvider()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public ILogger GetLogger(string name)
        {
            return new Log4netLogger(LogManager.GetLogger(name));
        }
    }
}
