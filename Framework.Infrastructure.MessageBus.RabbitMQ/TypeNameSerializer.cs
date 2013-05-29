using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.MessageBus.RabbitMQ
{
    public class TypeNameSerializer
    {
        public static string Serialize(Type type)
        {
            Preconditions.CheckNotNull(type, "type");
            return type.FullName.Replace('.', '_') + ":" + type.Assembly.GetName().Name.Replace('.', '_');
        }
    }
}
