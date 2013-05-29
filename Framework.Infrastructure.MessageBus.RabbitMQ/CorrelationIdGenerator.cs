using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public class CorrelationIdGenerator
    {
        public static string GetCorrelationId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
