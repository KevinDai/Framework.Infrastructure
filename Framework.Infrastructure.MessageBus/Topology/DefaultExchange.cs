using System;

namespace Framework.Infrastructure.MessageBus.Topology
{
    public class DefaultExchange : Exchange
    {
        public DefaultExchange() : base("", Topology.ExchangeType.Direct)
        {
        }

        public override void Visit(ITopologyVisitor visitor)
        {
            // default exchange already exists
        }

        public override void BindTo(IExchange exchange, params string[] routingKeys)
        {
            throw new Exception("Cannot bind to default exchange");
        }
    }
}