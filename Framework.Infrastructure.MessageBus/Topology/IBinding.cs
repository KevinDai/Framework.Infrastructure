namespace Framework.Infrastructure.MessageBus.Topology
{
    public interface IBinding : ITopology
    {
        IBindable Bindable { get; }
        IExchange Exchange { get; }
        string[] RoutingKeys { get; }
    }
}