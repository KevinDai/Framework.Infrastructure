using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.MessageBus;
using Framework.Infrastructure.MessageBus.Topology;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Event.MessageBus
{
    internal class RoutedEventMessageBus : IRoutedEventMessageBus
    {
        public const string RoutedEventExchangeName = "Framework.RoutedEvent";

        /// <summary>
        /// 路由事件的队列名称格式：Framework.RoutedEvent.{应用程序Id}
        /// </summary>
        public const string RoutedEventQueueNameFormat = "Framework.RoutedEvent.{0}";

        /// <summary>
        /// 路由事件的所有应用程序范围路由关键字
        /// </summary>
        public const string AllAppScopeRoutingKey = "AllApplication";

        /// <summary>
        /// 路由事件的应用程序Id范围路由关键字格式：ApplicationId.{应用程序Id}
        /// </summary>
        public const string AppIdScopeRoutingKeyFormat = "ApplicationId.{0}";

        /// <summary>
        /// 路由事件的应用程序类型范围路由关键字格式：ApplicationType.{应用程序类型}
        /// </summary>
        public const string AppTypeScopeRoutingKeyFormat = "ApplicationType.{0}";

        /// <summary>
        /// 路由事件的应用程序分组范围路由关键字格式：ApplicationGroup.{应用程序分组}
        /// </summary>
        public const string AppGroupScopeRoutingKeyFormat = "ApplicationGroup.{0}";

        protected IMessageBus MessageBus
        {
            get;
            private set;
        }

        protected ISerializer Serializer
        {
            get;
            private set;
        }

        protected IQueue RoutedEventQueue
        {
            get;
            private set;
        }

        protected IExchange RoutedEventExchange
        {
            get;
            private set;
        }

        public RoutedEventMessageBus(IMessageBusProvider messageBusProvider, ISerializer serializer)
        {
            Preconditions.CheckNotNull(messageBusProvider, "messageBusProvider");
            Preconditions.CheckNotNull(serializer, "serializer");

            Serializer = serializer;
            MessageBus = messageBusProvider.GetMessageBus();
            InitMessageBusTopology();

            Subscribe();
        }

        protected virtual void InitMessageBusTopology()
        {
            RoutedEventExchange = Exchange.DeclareTopic(RoutedEventExchangeName);
            RoutedEventQueue = Queue.DeclareTransient(GetRoutedEventQueueName(Application.Current.ApplicationId));
            RoutedEventQueue.BindTo(RoutedEventExchange,
                AllAppScopeRoutingKey,   //全局范围的消息路由关键字
                GetAppIdScopeRoutingKey(Application.Current.ApplicationId), //当前应用Id范围的消息路由关键字
                GetAppTypeScopeRoutingKey(Application.Current.ApplicationType), //当前应用类型范围的消息路由关键字
                GetAppGroupScopeRoutingKey(Application.Current.ApplicationGroup //当前应用分组范围的消息路由关键字
                ));
        }

        protected virtual void Publish(RoutedEventMessage message, RaiseScope raiseScope = null)
        {
            using (IPublishChannel channel = MessageBus.OpenPublishChannel())
            {
                var messageProperties = new MessageProperties()
                {
                    DeliveryMode = 2
                };
                var body = Serializer.MessageToBytes(message);
                channel.Publish(RoutedEventExchange, GetRoutingKey(raiseScope), messageProperties, body);
            }
        }

        protected void Subscribe()
        {
            MessageBus.Subscribe(RoutedEventQueue, (messageBody, messageProperties, receivedInfo) =>
                    Task.Factory.StartNew(() =>
                    {
                        if (OnRecived == null)
                        {
                            return;
                        }
                        RoutedEventMessage message = Serializer.BytesToMessage<RoutedEventMessage>(messageBody);
                        if (message == null)
                        {
                            throw new Exception("无效的消息");
                        }

                        var errors = message.Valid();
                        if (errors.Any())
                        {
                            throw new Exception(string.Format("无效的消息:{0}", string.Join("；", errors)));
                        }

                        OnRecived(message);
                    })
                );
        }

        private string GetRoutingKey(RaiseScope raiseScope = null)
        {
            if (raiseScope == null)
            {
                return AllAppScopeRoutingKey;
            }

            if (raiseScope is AppIdRasieScope)
            {
                return GetAppIdScopeRoutingKey((raiseScope as AppIdRasieScope).ApplicationId);
            }

            if (raiseScope is AppTypeRasieScope)
            {
                return GetAppTypeScopeRoutingKey((raiseScope as AppTypeRasieScope).ApplicationType);
            }

            if (raiseScope is AppGroupRasieScope)
            {
                return GetAppTypeScopeRoutingKey((raiseScope as AppGroupRasieScope).ApplicationGroup);
            }

            throw new NotImplementedException(string.Format("暂不支持{0}类型的事件范围", raiseScope.GetType().FullName));
        }

        private string GetRoutedEventQueueName(string applicationId)
        {
            return string.Format(RoutedEventQueueNameFormat, applicationId);
        }

        private string GetAppIdScopeRoutingKey(string applicationId)
        {
            return string.Format(AppIdScopeRoutingKeyFormat, applicationId);
        }

        private string GetAppTypeScopeRoutingKey(string applicationType)
        {
            return string.Format(AppTypeScopeRoutingKeyFormat, applicationType);
        }

        private string GetAppGroupScopeRoutingKey(string applicationGroup)
        {
            return string.Format(AppGroupScopeRoutingKeyFormat, applicationGroup);
        }

        #region IRoutedEventMessageBus接口实现

        public void Send(RoutedEventMessage message, RaiseScope raiseScope = null)
        {
            Preconditions.CheckNotNull(message, "message");

            Publish(message, raiseScope);
        }

        public event Action<RoutedEventMessage> OnRecived;

        #endregion
    }
}
