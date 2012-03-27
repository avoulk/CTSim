
namespace SimLib.Messages.Types
{
    public class MSGNeighborDiscovery : Message
    {
        public MSGNeighborDiscovery(int Source)
            : base(Source, MessageTargets.ALL_IN_RANGE, MessageType.NEIGHBOR_DISCOVERY, 0)
        {
        }
    }
}
