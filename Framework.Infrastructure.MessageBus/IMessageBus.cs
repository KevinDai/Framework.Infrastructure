using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Infrastructure.MessageBus.FluentConfiguration;
using Framework.Infrastructure.MessageBus.Topology;
using System.Threading.Tasks;

namespace Framework.Infrastructure.MessageBus
{
    public interface IMessageBus : IDisposable
    {

        void Subscribe<T>(IQueue queue, Func<IMessage<T>, MessageReceivedInfo, Task> onMessage);

        void Subscribe(IQueue queue, Func<Byte[], MessageProperties, MessageReceivedInfo, Task> onMessage);

        IPublishChannel OpenPublishChannel();

        IPublishChannel OpenPublishChannel(Action<IChannelConfiguration> configure);

        //void Purge(IQueue queue);

        event Action Connected;

        event Action Disconnected;

        bool IsConnected { get; }
    }
}
