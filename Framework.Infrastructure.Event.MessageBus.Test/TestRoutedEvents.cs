using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Event.MessageBus.Test
{
    public class TestRoutedEvents
    {
        public static RoutedEvent SimpleRoutedEvent = new RoutedEvent("RoutedEvent");

        public static RoutedEvent<RoutedEventArgs<User>> UserRoutedEvent = new RoutedEvent<RoutedEventArgs<User>>("UserRoutedEvent");
    }

    public class User
    {
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
