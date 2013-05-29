using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.MessageBus.FluentConfiguration
{
    public interface IChannelConfiguration
    {
        IChannelConfiguration WithPublisherConfirms();
    }

    public class ChannelConfiguration : IChannelConfiguration
    {
        public bool PublisherConfirmsOn { get; private set; }

        public ChannelConfiguration()
        {
            PublisherConfirmsOn = false;
        }

        public IChannelConfiguration WithPublisherConfirms()
        {
            PublisherConfirmsOn = true;
            return this;
        }
    }
}
