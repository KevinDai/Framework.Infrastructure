using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;

namespace Log4NetExtensionTest
{
    public class MyLog4NetAppender : AppenderSkeleton
    {

        public MyLog4NetAppender()
        {
        }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            //throw new NotImplementedException();
        }
    }
}
