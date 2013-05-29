using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading;
using log4net.Config;
using log4net.Repository.Hierarchy;
using log4net.Appender;

namespace Log4NetExtensionTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //XmlConfigurator.Configure();
            //TestConfig1();
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            //LogManager.GetLoggerRepository(
            //hierarchy.Configured = false;
            //hierarchy.Exists(
            Logger logger = (Logger)hierarchy.GetLogger("Test");
            logger.EffectiveLevel
            //hierarchy.get

            //logger.AddAppender(new MyLog4NetAppender());
            //logger.AddAppender(new MyLog4NetAppender());
            //hierarchy.Configured = true;
            //var appender = new RollingFileAppender();
            //appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            //appender.Name = "Test";
            //logger.AddAppender(appender);
            //appender.

            //hierarchy.
            //hierarchy.ResetConfiguration();
            hierarchy.Shutdown();
            logger = (Logger)hierarchy.GetLogger("Test1");
            //hierarchy.get

            //logger.AddAppender(new MyLog4NetAppender());
            //logger.AddAppender(new MyLog4NetAppender());
            //hierarchy.Configured = true;
            appender = new RollingFileAppender();
            appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            appender.Name = "Test1";
            logger.AddAppender(appender);

            //logger.
            //var appenders = test.GetAppenders();
            //test.ConfigurationMessages.add
            //new log4net.log
            var log = LogManager.GetLogger("Test");
            log.Debug(new TestMessage() { Name = "111", Test = "222" });

            //while (!Console.KeyAvailable)
            //{
            //    log.Debug(new TestMessage() { Name = "111", Test = "222" });
            //    Thread.Sleep(1000);
            //}

        }

        public static void TestConfig1()
        {
            var hierarchy = LogManager.GetRepository();
            hierarchy.Configured = false;
            Logger logger = (Logger)hierarchy.GetLogger("Test1");
            //logger.AddAppender(new MyLog4NetAppender());
            logger.AddAppender(new MyLog4NetAppender());
            hierarchy.Configured = true;

            //logger.
            //var appenders = test.GetAppenders();
            //test.ConfigurationMessages.add
            //new log4net.log
            var log = LogManager.GetLogger("Test");
            log.Debug(new TestMessage() { Name = "111", Test = "222" });
        }

        public class TestMessage
        {
            public string Name
            {
                get;
                set;
            }

            public string Test
            {
                get;
                set;
            }
        }
    }
}
