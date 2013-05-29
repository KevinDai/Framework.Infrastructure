using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Framework.Infrastructure.Logger.Log4net
{
    /// <summary>
    /// log4net日志类
    /// </summary>
    public class Log4netLogger : ILogger
    {
        #region Members

        /// <summary>
        /// log4net日志对象
        /// </summary>
        protected ILog Log
        {
            get
            {
                return _log;
            }
        }
        
        /// <summary>
        /// log4net日志对象
        /// </summary>
        private ILog _log;

        #endregion

        #region Constructor

        public Log4netLogger(ILog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _log = log;
        }

        #endregion

        #region ILogger implementation

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        public void Debug(string message)
        {
            Log.Debug(message);
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="ex"><see cref="ILogger"/></param>
        public void Debug(string message, Exception ex)
        {
            Log.Debug(message, ex);
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        public void Error(string message)
        {
            Log.Error(message);
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="ex"><see cref="ILogger"/></param>
        public void Error(string message, Exception ex)
        {
            Log.Error(message, ex);
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        public void Fatal(string message)
        {
            Log.Fatal(message);
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="ex"><see cref="ILogger"/></param>
        public void Fatal(string message, Exception ex)
        {
            Log.Fatal(message, ex);
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        public void Info(string message)
        {
            Log.Info(message);
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="ex"><see cref="ILogger"/></param>
        public void Info(string message, Exception ex)
        {
            Log.Info(message, ex);
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        public void Warn(string message)
        {
            Log.Warn(message);
        }

        /// <summary>
        /// <see cref="ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="ILogger"/></param>
        /// <param name="ex"><see cref="ILogger"/></param>
        public void Warn(string message, Exception ex)
        {
            Log.Warn(message, ex);
        }

        #endregion
    }
}
