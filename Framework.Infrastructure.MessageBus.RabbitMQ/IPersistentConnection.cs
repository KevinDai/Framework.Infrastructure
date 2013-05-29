using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using Framework.Infrastructure.Logger;
using System.Threading;
using RabbitMQ.Client.Exceptions;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public interface IPersistentConnection : IDisposable
    {
        event Action Connected;
        event Action Disconnected;
        bool IsConnected { get; }
        IModel CreateModel();
    }

    /// <summary>
    /// A connection that attempts to reconnect if the inner connection is closed.
    /// </summary>
    public class PersistentConnection : IPersistentConnection
    {
        private const int connectAttemptIntervalMilliseconds = 5000;

        private readonly IConnectionFactory connectionFactory;
        private readonly ILogger logger;
        private IConnection connection;

        public PersistentConnection(IConnectionFactory connectionFactory, ILogger logger)
        {
            this.connectionFactory = connectionFactory;
            this.logger = logger;

            TryToConnect(null);
        }

        public event Action Connected;
        public event Action Disconnected;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new Exception("Rabbit server is not connected.");
            }
            return connection.CreateModel();
        }

        public bool IsConnected
        {
            get { return connection != null && connection.IsOpen && !disposed; }
        }

        void StartTryToConnect()
        {
            var timer = new Timer(TryToConnect);
            timer.Change(connectAttemptIntervalMilliseconds, Timeout.Infinite);
        }

        void TryToConnect(object timer)
        {
            if (timer != null) ((Timer)timer).Dispose();

            logger.Debug("Trying to connect");
            if (disposed) return;

            connectionFactory.Reset();
            do
            {
                try
                {
                    connection = connectionFactory.CreateConnection();
                    connectionFactory.Success();
                }
                catch (System.Net.Sockets.SocketException socketException)
                {
                    LogException(socketException);
                }
                catch (BrokerUnreachableException brokerUnreachableException)
                {
                    LogException(brokerUnreachableException);
                }
            } while (connectionFactory.Next());

            if (connectionFactory.Succeeded)
            {
                connection.ConnectionShutdown += OnConnectionShutdown;

                OnConnected();
                logger.Info(string.Format("Connected to RabbitMQ. Broker: '{0}', Port: {1}, VHost: '{2}'",
                    connectionFactory.CurrentHost.Host,
                    connectionFactory.CurrentHost.Port,
                    connectionFactory.Configuration.VirtualHost));
            }
            else
            {
                logger.Error(string.Format("Failed to connected to any Broker. Retrying in {0} ms\n",
                    connectAttemptIntervalMilliseconds));
                StartTryToConnect();
            }
        }

        void LogException(Exception exception)
        {
            logger.Error(string.Format("Failed to connect to Broker: '{0}', Port: {1} VHost: '{2}'. " +
                    "ExceptionMessage: '{3}'",
                connectionFactory.CurrentHost.Host,
                connectionFactory.CurrentHost.Port,
                connectionFactory.Configuration.VirtualHost,
                exception.Message));
        }

        void OnConnectionShutdown(IConnection _, ShutdownEventArgs reason)
        {
            if (disposed) return;
            OnDisconnected();

            // try to reconnect and re-subscribe
            logger.Info("Disconnected from RabbitMQ Broker");

            TryToConnect(null);
        }

        public void OnConnected()
        {
            logger.Debug("OnConnected event fired");
            if (Connected != null) Connected();
        }

        public void OnDisconnected()
        {
            if (Disconnected != null) Disconnected();
        }

        private bool disposed = false;
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            if (connection != null) connection.Dispose();
        }
    }
}
