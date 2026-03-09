using Code.Game.Features.Network.Systems;
using Code.Infrastructure.Systems;

namespace Code.Game.Features.Network
{
    public class NetworkFixedFeature : Feature
    {
        public NetworkFixedFeature(ISystemFactory systems)
        {
            Add(systems.Create<NetworkTickSystem>());
        }
    }
}