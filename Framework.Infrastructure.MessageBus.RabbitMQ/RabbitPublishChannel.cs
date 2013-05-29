using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.MessageBus.FluentConfiguration;
using RabbitMQ.Client;
using Framework.Infrastructure.MessageBus.Topology;
using RabbitMQ.Client.Exceptions;
using System.IO;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public class RabbitPublishChannel : IPublishChannel
    {
        private readonly IModel _channel;
        private readonly ChannelConfiguration _channelConfiguration;
        private readonly RabbitMessageBus _rabbitMessageBus;
        private readonly IPublisherConfirms _publisherConfirms;

        public RabbitPublishChannel(RabbitMessageBus rabbitMessageBus, Action<IChannelConfiguration> configure)
        {
            Preconditions.CheckNotNull(rabbitMessageBus, "rabbitMessageBus");

            if (!rabbitMessageBus.Connection.IsConnected)
            {
                throw new Exception("Cannot open channel for publishing, the broker is not connected");
            }

            this._rabbitMessageBus = rabbitMessageBus;
            _channel = rabbitMessageBus.Connection.CreateModel();

            _channelConfiguration = new ChannelConfiguration();
            configure(_channelConfiguration);

            _publisherConfirms = ConfigureChannel(_channelConfiguration, _channel);
        }

        private IPublisherConfirms ConfigureChannel(ChannelConfiguration configuration, IModel channel)
        {
            if (configuration.PublisherConfirmsOn)
            {
                channel.ConfirmSelect();
                var publisherConfirms = new PublisherConfirms();
                channel.BasicAcks += publisherConfirms.SuccessfulPublish;
                channel.BasicNacks += publisherConfirms.FailedPublish;
                return publisherConfirms;
            }
            return null;
        }

        protected void Publish<T>(string exchange, string routingKey, ITopology topology, IMessage<T> message, Action<IPublishConfiguration> configure)
        {
            var typeName = _rabbitMessageBus.SerializeType(typeof(T));
            var messageBody = _rabbitMessageBus.Serializer.MessageToBytes(message.Body);

            message.Properties.Type = typeName;
            message.Properties.CorrelationId =
                string.IsNullOrEmpty(message.Properties.CorrelationId) ?
                _rabbitMessageBus.GetCorrelationId() :
                message.Properties.CorrelationId;

            Publish(exchange, routingKey, topology, message.Properties, messageBody, configure);
        }

        protected void Publish(string exchange, string routingKey, ITopology topology, MessageProperties properties,
            byte[] messageBody, Action<IPublishConfiguration> configure)
        {
            if (disposed)
            {
                throw new Exception("PublishChannel is already disposed");
            }
            if (!_rabbitMessageBus.Connection.IsConnected)
            {
                throw new Exception("Publish failed. No rabbit server connected.");
            }

            try
            {
                var configuration = new AdvancedPublishConfiguration();
                if (configure != null)
                {
                    configure(configuration);
                }

                if (_publisherConfirms != null)
                {
                    if (configuration.SuccessCallback == null || configuration.FailureCallback == null)
                    {
                        throw new Exception("When pulisher confirms are on, you must supply success and failure callbacks in the publish configuration");
                    }

                    _publisherConfirms.RegisterCallbacks(_channel, configuration.SuccessCallback, configuration.FailureCallback);
                }

                var defaultProperties = _channel.CreateBasicProperties();
                properties.CopyTo(defaultProperties);

                if (topology != null)
                {
                    topology.Visit(new TopologyBuilder(_channel));
                }

                _channel.BasicPublish(
                    exchange,      // exchange
                    routingKey,         // routingKey 
                    defaultProperties,  // basicProperties
                    messageBody);       // body

                _rabbitMessageBus.Logger.Debug(string.Format("Published to exchange: '{0}', routing key: '{1}', correlationId: '{2}'",
                    exchange, routingKey, defaultProperties.CorrelationId));
            }
            catch (OperationInterruptedException exception)
            {
                throw new Exception(string.Format("Publish Failed: '{0}'", exception.Message));
            }
            catch (IOException exception)
            {
                throw new Exception(string.Format("Publish Failed: '{0}'", exception.Message));
            }
        }


        #region IPublishChannel接口实现

        public void Publish<T>(IExchange exchange, string routingKey, IMessage<T> message, Action<IPublishConfiguration> configure)
        {
            Preconditions.CheckNotNull(routingKey, "routingKey");
            Preconditions.CheckNotNull(message, "message");
            Preconditions.CheckNotNull(configure, "configure");

            Publish(exchange.Name, routingKey, exchange, message, configure);
        }

        public void Publish(IExchange exchange, string routingKey, MessageProperties properties, byte[] messageBody, Action<IPublishConfiguration> configure)
        {
            Preconditions.CheckNotNull(exchange, "exchange");
            Preconditions.CheckNotNull(routingKey, "routingKey");
            Preconditions.CheckNotNull(properties, "properties");
            Preconditions.CheckNotNull(messageBody, "messageBody");
            Preconditions.CheckNotNull(configure, "configure");

            Publish(exchange.Name, routingKey, exchange, properties, messageBody, configure);
        }


        public void Publish<T>(IQueue queue, IMessage<T> message, Action<IPublishConfiguration> configure = null)
        {
            Publish(string.Empty, queue.Name, queue, message, configure);
        }

        public void Publish(IQueue queue, MessageProperties properties, byte[] messageBody, Action<IPublishConfiguration> configure = null)
        {
            Publish(string.Empty, queue.Name, queue, properties, messageBody, configure);
        }

        private IExchange BindDefaultExchange(IQueue queue)
        {
            var exchange = Exchange.GetDefault();
            queue.BindTo(exchange);
            return exchange;
        }

        private bool disposed;

        public virtual void Dispose()
        {
            if (disposed) return;
            _channel.Abort();
            _channel.Dispose();
            disposed = true;
        }

        #endregion
    }
}
