using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Infrastructure.MessageBus;
using Framework.Infrastructure.MessageBus.RabbitMQ;
using Framework.Infrastructure.MessageBus.Topology;
using Framework.Infrastructure;
using Framework.Infrastructure.Container;
using Moq;
using Microsoft.Practices.Unity;
using Framework.Infrastructure.Logger;
using Framework.Infrastructure.Logger.Log4net;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBus.RabbitMQ.Test
{
    [TestClass]
    public class PublishSubscribeTests
    {

        public static IMessageBus _messageBus;


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            var containner = new UnityContainer();
            containner.RegisterType(typeof(ILoggerProvider), typeof(Log4netLoggerProvider), new ContainerControlledLifetimeManager());
            ServiceFactory.Initialize(new Framework.Infrastructure.Container.UnityContainer.UnityContainer(containner));
            //container.Container
            //ServiceFactory.Initialize(new
            _messageBus = new RabbitMessageBus(@"host=localhost:5672;virtualHost=/;username=guest;password=guest");
        }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _messageBus.Dispose();
        }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Should_be_able_to_publish()
        {
            var message = new Message<string>("Hello! " + Guid.NewGuid().ToString().Substring(0, 5));
            message.Properties.DeliveryMode = 2;
            using (var publishChannel = _messageBus.OpenPublishChannel())
            {
                var queue = Queue.DeclareDurable("Test");
                publishChannel.Publish(queue, message);
            }
            Console.Out.WriteLine("message.Text = {0}", message.Body);
        }

        [TestMethod]
        public void Should_also_send_messages_to_second_subscriber()
        {
            var autoResetEvent = new AutoResetEvent(false);
            var queue = Queue.DeclareDurable("Test");
            _messageBus.Subscribe<string>(queue, (msg, messageReceivedInfo) =>
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Got Message: {0}", msg.Body);
                    Console.WriteLine("ConsumerTag: {0}", messageReceivedInfo.ConsumerTag);
                    Console.WriteLine("DeliverTag: {0}", messageReceivedInfo.DeliverTag);
                    Console.WriteLine("Redelivered: {0}", messageReceivedInfo.Redelivered);
                    Console.WriteLine("Exchange: {0}", messageReceivedInfo.Exchange);
                    Console.WriteLine("RoutingKey: {0}", messageReceivedInfo.RoutingKey);
                    autoResetEvent.Set();
                }));

            // allow time for messages to be consumed
            autoResetEvent.WaitOne(500);

            Console.WriteLine("Stopped consuming");
        }

        [TestMethod]
        public void Should_two_subscriptions_from_the_same_app_should_also_both_get_all_messages()
        {
            var countdownEvent = new CountdownEvent(8);

            var queue = Queue.DeclareDurable("Test");
            _messageBus.Subscribe<string>(queue, (msg, messageReceivedInfo) =>
                Task.Factory.StartNew(() =>
                {
                    throw new Exception("test");
                    Console.WriteLine("1:" + msg.Body);
                    countdownEvent.Signal();
                }));
            _messageBus.Subscribe<string>(queue, (msg, messageReceivedInfo) =>
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("2:" + msg.Body);
                    countdownEvent.Signal();
                }));
            _messageBus.Subscribe<string>(queue, (msg, messageReceivedInfo) =>
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("3:" + msg.Body);
                    countdownEvent.Signal();
                }));
            _messageBus.Subscribe<string>(queue, (msg, messageReceivedInfo) =>
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("4:" + msg.Body);
                    countdownEvent.Signal();
                }));
            using (var publishChannel = _messageBus.OpenPublishChannel())
            {
                for (int i = 1; i < 5; i++)
                {
                    var message = new Message<string>("Hello! " + i.ToString());
                    publishChannel.Publish(queue, message);
                }

            }

            // allow time for messages to be consumed
            countdownEvent.Wait(1000);

            Console.WriteLine("Stopped consuming");
        }

        [TestMethod]
        public void Should_subscribe_OK_before_connection_to_broker_is_complete()
        {
            var autoResetEvent = new AutoResetEvent(false);

            var queue = Queue.DeclareDurable("Test");
            _messageBus.Subscribe<string>(queue, (msg, messageReceivedInfo) =>
                 Task.Factory.StartNew(() =>
                 {
                     Console.WriteLine("1:" + msg.Body);
                     autoResetEvent.Set();
                 }));
            Console.WriteLine("--- subscribed ---");

            // allow time for bus to connect
            autoResetEvent.WaitOne();
        }
    }
}
