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

        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;
        private IConnection _connection;

        public PersistentConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _logger = ServiceFactory.Instance.GetDefaultLoggerProvider().GetLogger(RabbitMessageBus.MessageBusLoggerName);

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
            return _connection.CreateModel();
        }

        public bool IsConnected
        {
            get { return _connection != null && _connection.IsOpen && !disposed; }
        }

        void StartTryToConnect()
        {
            var timer = new Timer(TryToConnect);
            timer.Change(connectAttemptIntervalMilliseconds, Timeout.Infinite);
        }

        void TryToConnect(object timer)
        {
            if (timer != null) ((Timer)timer).Dispose();

            _logger.Debug("Trying to connect");
            if (disposed) return;

            _connectionFactory.Reset();
            do
            {
                try
                {
                    _connection = _connectionFactory.CreateConnection();
                    _connectionFactory.Success();
                }
                catch (System.Net.Sockets.SocketException socketException)
                {
                    LogException(socketException);
                }
                catch (BrokerUnreachableException brokerUnreachableException)
                {
                    LogException(brokerUnreachableException);
                }
            } while (_connectionFactory.Next());

            if (_connectionFactory.Succeeded)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;

                OnConnected();
                _logger.Info(string.Format("Connected to RabbitMQ. Broker: '{0}', Port: {1}, VHost: '{2}'",
                    _connectionFactory.CurrentHost.Host,
                    _connectionFactory.CurrentHost.Port,
                    _connectionFactory.Configuration.VirtualHost));
            }
            else
            {
                _logger.Error(string.Format("Failed to connected to any Broker. Retrying in {0} ms\n",
                    connectAttemptIntervalMilliseconds));
                StartTryToConnect();
            }
        }

        void LogException(Exception exception)
        {
            _logger.Error(string.Format("Failed to connect to Broker: '{0}', Port: {1} VHost: '{2}'. " +
                    "ExceptionMessage: '{3}'",
                _connectionFactory.CurrentHost.Host,
                _connectionFactory.CurrentHost.Port,
                _connectionFactory.Configuration.VirtualHost,
                exception.Message));
        }

        void OnConnectionShutdown(IConnection _, ShutdownEventArgs reason)
        {
            if (disposed) return;
            OnDisconnected();

            // try to reconnect and re-subscribe
            _logger.Info("Disconnected from RabbitMQ Broker");

            TryToConnect(null);
        }

        public void OnConnected()
        {
            _logger.Debug("OnConnected event fired");
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
            if (_connection != null) _connection.Dispose();
        }
    }
}
