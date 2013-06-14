using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Framework.Infrastructure.Event.MessageBus.Test
{
    [TestClass]
    public class UserRoutedEventTest
    {

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            Application.InitCurrent();
            Application.Current.Start();

            //_messageBus = Application.Current.Container.Resolve<IMessageBusProvider>().GetMessageBus("test");
            //ServiceFactory.Initialize(new Framework.Infrastructure.Container.UnityContainer.UnityContainer(containner));
            //container.Container
            //ServiceFactory.Initialize(new
        }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Application.Current.Stop();
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
        public void UserRoutedEvent_routed_event_handle_test()
        {
            var autoResetEvent = new AutoResetEvent(false);

            RoutedEventArgs<User> actualRoutedEventArgs = new RoutedEventArgs<User>()
            {
                Parameter = new User() { Id = Guid.NewGuid().ToString(), Name = "Test" }
            };

            var routedEventContainer = Application.Current.Container.Resolve<IRoutedEventContainer>();
            RoutedEventSender expectedRoutedEventSender = null;
            RoutedEventArgs<User> expectedRoutedEventArgs = null;
            routedEventContainer.AddHandler(TestRoutedEvents.UserRoutedEvent, (sender, args) =>
            {
                expectedRoutedEventSender = sender;
                expectedRoutedEventArgs = args;
                autoResetEvent.Set();
            });

            routedEventContainer.Raise(TestRoutedEvents.UserRoutedEvent, this, actualRoutedEventArgs);

            autoResetEvent.WaitOne(5000);

            Assert.IsNotNull(expectedRoutedEventSender);
            Assert.IsNotNull(expectedRoutedEventArgs);

            Assert.AreEqual(expectedRoutedEventArgs.Parameter.Id, actualRoutedEventArgs.Parameter.Id);
        }
    }
}
