using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Framework.Infrastructure.MessageBus.RabbitMQ.ConnectionString;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Lifestyle.Scoped;
using Castle.MicroKernel.Lifestyle;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    /// <summary>
    /// 根据配置文件连接字符串信息提供消息总线对象类
    /// </summary>
    public class RabbitMessageBusProvider : IMessageBusProvider, IDisposable
    {
        /// <summary>
        /// 默认的连接字符串名称
        /// </summary>
        public const string DefaultConnectionStringName = "rabbitmq";

        /// <summary>
        /// 消息总线对象字典
        /// </summary>
        private IDictionary<string, IMessageBus> _messageBuses = new Dictionary<string, IMessageBus>();

        /// <summary>
        /// 连接字符串解析对象
        /// </summary>
        private IConnectionStringParser ConnectionStringParser = new ConnectionStringParser();

        /// <summary>
        /// 锁对象
        /// </summary>
        private object _lockObject = new object();

        /// <summary>
        /// 获取默认的消息总线对象
        /// </summary>
        /// <returns></returns>
        public IMessageBus GetMessageBus()
        {
            return GetMessageBus(DefaultConnectionStringName);
        }

        /// <summary>
        /// 获取指定连接字符串名称的消息总线对象
        /// </summary>
        /// <param name="name">连接字符串名称</param>
        /// <returns>消息总线名称</returns>
        public IMessageBus GetMessageBus(string name)
        {
            Preconditions.CheckNotNull(name, "name");

            IMessageBus messageBus = null;
            if (!_messageBuses.TryGetValue(name, out messageBus))
            {
                lock (_lockObject)
                {
                    if (!_messageBuses.TryGetValue(name, out messageBus))
                    {
                        using (Application.Current.Container.BeginScope())
                        {
                            IConnectionConfiguration connectionConfiguration =
                                Application.Current.Container.Resolve<IConnectionConfiguration>(
                                    new
                                    {
                                        connectionString = GetConfigConnectionString(name)
                                    }
                                );
                            messageBus = Application.Current.Container.Resolve<IMessageBus>();
                        }
                        _messageBuses.Add(name, messageBus);
                    }
                }
            }
            return messageBus;
        }

        protected virtual string GetConfigConnectionString(string name)
        {
            var rabbitConnectionString = ConfigurationManager.ConnectionStrings[name];
            if (rabbitConnectionString == null)
            {
                throw new Exception(
                    "无法在配置文件中找到RabbitMQ的连接字符串信息. " +
                    "请在<ConnectionStrings>中添加相关配置如:<add name=\"" + name +
                    "\" connectionString=\"host=localhost\" />");
            }
            return rabbitConnectionString.ConnectionString;
        }

        private bool disposed = false;
        public virtual void Dispose()
        {
            if (disposed) return;

            foreach (var item in _messageBuses)
            {
                item.Value.Dispose();
            }
        }
    }
}
