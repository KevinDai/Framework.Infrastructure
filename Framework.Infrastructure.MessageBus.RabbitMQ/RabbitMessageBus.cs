using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Logger;
using System.Collections.Concurrent;
using RabbitMQ.Client.Exceptions;
using Framework.Infrastructure.MessageBus.FluentConfiguration;
using Framework.Infrastructure.MessageBus.RabbitMQ.ConnectionString;
using Framework.Infrastructure.MessageBus.Topology;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using System.Configuration;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public class RabbitMessageBus : IMessageBus
    {
        internal const string MessageBusLoggerName = "MessageBusLogger";

        private readonly IConnectionConfiguration _connectionConfiguration;
        private readonly ConcurrentBag<SubscriptionAction> _subscribeActions = new ConcurrentBag<SubscriptionAction>();

        public const bool NoAck = false;
        public virtual event Action Connected;
        public virtual event Action Disconnected;

        public virtual SerializeType SerializeType
        {
            get;
            private set;
        }

        public virtual ISerializer Serializer
        {
            get;
            private set;
        }

        public IPersistentConnection PersistentConnection
        {
            get;
            private set;
        }

        public IConsumerFactory ConsumerFactory
        {
            get;
            private set;
        }

        public ILogger Logger
        {
            get;
            private set;
        }

        public Func<string> GetCorrelationId
        {
            get;
            private set;
        }

        public IConventions Conventions
        {
            get;
            private set;
        }

        public RabbitMessageBus(
            IConnectionConfiguration connectionConfiguration,
            IPersistentConnection persistentConnection,
            IPersistentConnection persistentConnection1,
            ISerializer serializer,
            IConsumerFactory consumerFactory,
            IConventions conventions)
        {
            Preconditions.CheckNotNull(connectionConfiguration, "connectionConfiguration");
            Preconditions.CheckNotNull(serializer, "serializer");
            Preconditions.CheckNotNull(consumerFactory, "consumerFactory");
            Preconditions.CheckNotNull(conventions, "conventions");

            Logger = Application.Current.LoggerProvider.GetLogger(MessageBusLoggerName);
            SerializeType = TypeNameSerializer.Serialize;
            GetCorrelationId = CorrelationIdGenerator.GetCorrelationId;

            _connectionConfiguration = connectionConfiguration;
            Serializer = serializer;
            Conventions = conventions;
            ConsumerFactory = consumerFactory;
            PersistentConnection = persistentConnection;

            PersistentConnection.Connected += OnConnected;
            PersistentConnection.Disconnected += ConsumerFactory.ClearConsumers;
            PersistentConnection.Disconnected += OnDisconnected;

        }

        //public RabbitMessageBus(IConnectionConfiguration connectionConfiguration)
        //{
        //    Preconditions.CheckNotNull(connectionConfiguration, "connectionConfiguration");

        //    Logger = Application.Current.LoggerProvider.GetLogger(MessageBusLoggerName);
        //    SerializeType = TypeNameSerializer.Serialize;
        //    GetCorrelationId = CorrelationIdGenerator.GetCorrelationId;
        //    Serializer = Application.Current.Container.Resolve<ISerializer>();
        //    Conventions = Application.Current.Container.Resolve<IConventions>();
        //    ConsumerFactory = Application.Current.Container.Resolve<IConsumerFactory>(connectionConfiguration);
        //    PersistentConnection = Application.Current.Container.Resolve<IPersistentConnection>(connectionConfiguration);
        //    PersistentConnection.Connected += OnConnected;
        //    PersistentConnection.Disconnected += ConsumerFactory.ClearConsumers;
        //    PersistentConnection.Disconnected += OnDisconnected;
        //}

        public virtual void Subscribe<T>(IQueue queue, Func<IMessage<T>, MessageReceivedInfo, Task> onMessage)
        {
            Preconditions.CheckNotNull(queue, "queue");
            Preconditions.CheckNotNull(onMessage, "onMessage");

            Subscribe(queue, (body, properties, messageRecievedInfo) =>
            {
                CheckMessageType<T>(properties);

                var messageBody = Serializer.BytesToMessage<T>(body);
                var message = new Message<T>(messageBody);
                message.SetProperties(properties);
                return onMessage(message, messageRecievedInfo);
            });
        }

        public virtual void Subscribe(IQueue queue, Func<Byte[], MessageProperties, MessageReceivedInfo, Task> onMessage)
        {
            Preconditions.CheckNotNull(queue, "queue");
            Preconditions.CheckNotNull(onMessage, "onMessage");

            if (disposed)
            {
                throw new Exception("This bus has been disposed");
            }

            var subscriptionAction = new SubscriptionAction(queue.IsSingleUse);

            subscriptionAction.Action = () =>
            {
                var channel = PersistentConnection.CreateModel();
                channel.ModelShutdown += (model, reason) => Logger.Debug(string.Format("Model Shutdown for queue: '{0}'", queue.Name));

                queue.Visit(new TopologyBuilder(channel));

                //channel.BasicQos(0, connectionConfiguration.PrefetchCount, false);
                channel.BasicQos(0, 50, false);

                var consumer = ConsumerFactory.CreateConsumer(subscriptionAction, channel, queue.IsSingleUse,
                    (consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body) =>
                    {
                        var messageRecievedInfo = new MessageReceivedInfo
                        {
                            ConsumerTag = consumerTag,
                            DeliverTag = deliveryTag,
                            Redelivered = redelivered,
                            Exchange = exchange,
                            RoutingKey = routingKey
                        };
                        MessageProperties messageProperties = new MessageProperties();
                        properties.CopyTo(messageProperties);
                        return onMessage(body, messageProperties, messageRecievedInfo);
                    });

                channel.BasicConsume(
                    queue.Name,             // queue
                    NoAck,                  // noAck 
                    consumer.ConsumerTag,   // consumerTag
                    consumer);              // consumer

                Logger.Debug(string.Format("Declared Consumer. queue='{0}', prefetchcount={1}",
                    queue.Name,
                    _connectionConfiguration.PrefetchCount));
            };

            AddSubscriptionAction(subscriptionAction);
        }

        private void AddSubscriptionAction(SubscriptionAction subscriptionAction)
        {
            if (subscriptionAction.IsMultiUse)
            {
                _subscribeActions.Add(subscriptionAction);
            }

            try
            {
                subscriptionAction.Action();
            }
            catch (OperationInterruptedException)
            {

            }
            catch (Exception)
            {
                // Looks like the channel closed between our IsConnected check
                // and the subscription action. Do nothing here, when the 
                // connection comes back, the subcription action will be run then.
            }
        }

        private void CheckMessageType<TMessage>(MessageProperties properties)
        {
            var typeName = SerializeType(typeof(TMessage));
            if (properties.Type != typeName)
            {
                Logger.Error(string.Format("Message type is incorrect. Expected '{0}', but was '{1}'",
                    typeName, properties.Type));

                throw new Exception(string.Format("Message type is incorrect. Expected '{0}', but was '{1}'",
                    typeName, properties.Type));
            }
        }

        public virtual IPublishChannel OpenPublishChannel()
        {
            return OpenPublishChannel(x => { });
        }

        public virtual IPublishChannel OpenPublishChannel(Action<IChannelConfiguration> configure)
        {
            return new RabbitPublishChannel(this, configure);
        }

        public void Purge(IQueue queue)
        {
            Preconditions.CheckNotNull(queue, "queue");

            using (var channel = PersistentConnection.CreateModel())
            {
                channel.QueuePurge(queue.Name);
            }
        }

        protected void OnConnected()
        {
            if (Connected != null) Connected();

            Logger.Debug("Re-creating subscribers");
            try
            {
                foreach (var subscribeAction in _subscribeActions)
                {
                    subscribeAction.Action();
                }
            }
            catch (OperationInterruptedException operationInterruptedException)
            {
                Logger.Error(string.Format("Re-creating subscribers failed: reason: '{0}'\n{1}",
                    operationInterruptedException.Message,
                    operationInterruptedException.ToString()));
            }
        }

        protected void OnDisconnected()
        {
            if (Disconnected != null) Disconnected();
        }

        public virtual bool IsConnected
        {
            get { return PersistentConnection.IsConnected; }
        }

        private bool disposed = false;
        public virtual void Dispose()
        {
            if (disposed) return;

            ConsumerFactory.Dispose();
            PersistentConnection.Dispose();

            disposed = true;

            Logger.Debug("Connection disposed");
        }
    }
}
