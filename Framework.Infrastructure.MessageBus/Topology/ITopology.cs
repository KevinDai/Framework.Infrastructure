namespace Framework.Infrastructure.MessageBus.Topology
{
    public interface ITopology
    {
        void Visit(ITopologyVisitor visitor);
    }
}