using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.MessageBus
{
    public interface IMessage<T>
    {
        MessageProperties Properties { get; }
        T Body { get; }
    }
}
