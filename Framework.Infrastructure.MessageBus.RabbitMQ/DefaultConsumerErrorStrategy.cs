using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using Framework.Infrastructure.Logger;
using RabbitMQ.Client.Exceptions;
using Framework.Infrastructure.MessageBus.RabbitMQ.SystemMessages;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public class DefaultConsumerErrorStrategy : IConsumerErrorStrategy
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly ISerializer serializer;
        private readonly ILogger logger;
        private readonly IConventions conventions;
        private IConnection connection;
        private bool errorQueueDeclared = false;
        private readonly ConcurrentDictionary<string, string> errorExchanges = new ConcurrentDictionary<string, string>();

        public DefaultConsumerErrorStrategy(
            IConnectionFactory connectionFactory,
            ISerializer serializer,
            IConventions conventions)
        {
            Preconditions.CheckNotNull(conventions, "conventions");

            logger = ServiceFactory.Instance.LoggerProvider.GetLogger(RabbitMessageBus.MessageBusLoggerName);

            this.connectionFactory = connectionFactory;
            this.serializer = serializer;
            //this.logger = logger;
            this.conventions = conventions;
        }

        private void Connect()
        {
            if (connection == null || !connection.IsOpen)
            {
                connection = connectionFactory.CreateConnection();
            }
        }

        private void DeclareDefaultErrorQueue(IModel model)
        {
            if (!errorQueueDeclared)
            {
                model.QueueDeclare(
                    queue: conventions.ErrorQueueNamingConvention(),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                errorQueueDeclared = true;
            }
        }

        private string DeclareErrorExchangeAndBindToDefaultErrorQueue(IModel model, string originalRoutingKey)
        {
            return errorExchanges.GetOrAdd(originalRoutingKey, _ =>
            {
                var exchangeName = conventions.ErrorExchangeNamingConvention(originalRoutingKey);
                model.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true);
                model.QueueBind(conventions.ErrorQueueNamingConvention(), exchangeName, originalRoutingKey);
                return exchangeName;
            });
        }

        private string DeclareErrorExchangeQueueStructure(IModel model, string originalRoutingKey)
        {
            DeclareDefaultErrorQueue(model);
            return DeclareErrorExchangeAndBindToDefaultErrorQueue(model, originalRoutingKey);
        }

        public virtual void HandleConsumerError(BasicDeliverEventArgs devliverArgs, Exception exception)
        {
            try
            {
                Connect();

                using (var model = connection.CreateModel())
                {
                    var errorExchange = DeclareErrorExchangeQueueStructure(model, devliverArgs.RoutingKey);

                    var messageBody = CreateErrorMessage(devliverArgs, exception);
                    var properties = model.CreateBasicProperties();
                    properties.SetPersistent(true);
                    properties.Type = TypeNameSerializer.Serialize(typeof(Error));

                    model.BasicPublish(errorExchange, devliverArgs.RoutingKey, properties, messageBody);
                }
            }
            catch (BrokerUnreachableException)
            {
                // thrown if the broker is unreachable during initial creation.
                logger.Error("EasyNetQ Consumer Error Handler cannot connect to Broker\n" +
                    CreateConnectionCheckMessage());
            }
            catch (OperationInterruptedException interruptedException)
            {
                // thrown if the broker connection is broken during declare or publish.
                logger.Error("EasyNetQ Consumer Error Handler: Broker connection was closed while attempting to publish Error message.\n" +
                    string.Format("Message was: '{0}'\n", interruptedException.Message) +
                    CreateConnectionCheckMessage());
            }
            catch (Exception unexpecctedException)
            {
                // Something else unexpected has gone wrong :(
                logger.Error("EasyNetQ Consumer Error Handler: Failed to publish error message\nException is:\n"
                    + unexpecctedException);
            }
        }

        public virtual PostExceptionAckStrategy PostExceptionAckStrategy()
        {
            return RabbitMQ.PostExceptionAckStrategy.ShouldAck;
        }

        private byte[] CreateErrorMessage(BasicDeliverEventArgs devliverArgs, Exception exception)
        {
            var messageAsString = Encoding.UTF8.GetString(devliverArgs.Body);
            var error = new Error
            {
                RoutingKey = devliverArgs.RoutingKey,
                Exchange = devliverArgs.Exchange,
                Exception = exception.ToString(),
                Message = messageAsString,
                DateTime = DateTime.Now,
                BasicProperties = new MessageBasicProperties(devliverArgs.BasicProperties)
            };

            return serializer.MessageToBytes(error);
        }

        private string CreateConnectionCheckMessage()
        {
            return
                "Please check EasyNetQ connection information and that the RabbitMQ Service is running at the specified endpoint.\n" +
                string.Format("\tHostname: '{0}'\n", connectionFactory.CurrentHost.Host) +
                string.Format("\tVirtualHost: '{0}'\n", connectionFactory.Configuration.VirtualHost) +
                string.Format("\tUserName: '{0}'\n", connectionFactory.Configuration.UserName) +
                "Failed to write error message to error queue";
        }

        private bool disposed = false;
        public virtual void Dispose()
        {
            if (disposed) return;

            if (connection != null) connection.Dispose();

            disposed = true;
        }
    }
}
