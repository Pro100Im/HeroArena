using Entitas;

namespace Code.Game.Features.Network.Systems
{
    public class NetworkTickSystem : IExecuteSystem
    {
        private readonly IGroup<NetworkEntity> _networks;

        public NetworkTickSystem(NetworkContext networkContext)
        {
            _networks = networkContext.GetGroup(NetworkMatcher
           .AllOf(
           NetworkMatcher.CurrentTick,
           NetworkMatcher.TickRate,
           NetworkMatcher.TickTime,
           NetworkMatcher.Time
           ));
        }

        public void Execute()
        {
            
        }
    }
}