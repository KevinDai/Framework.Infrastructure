using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.Logger;

namespace Framework.Infrastructure.Event.MessageBus
{
    public class RoutedEventLogger
    {
        public const string RoutedEventLoggerName = "RoutedEventLogger";

        public readonly static ILogger Instance = Application.Current.LoggerProvider.GetLogger(RoutedEventLoggerName);

    }
}
