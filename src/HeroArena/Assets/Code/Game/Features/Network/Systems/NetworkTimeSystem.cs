using Code.Common.Time;
using Entitas;

namespace Code.Game.Features.Network.Systems
{
    public class NetworkTimeSystem : IExecuteSystem
    {
        private readonly IGroup<NetworkEntity> _networks;
        private readonly ITimeService _timeService;

        public NetworkTimeSystem(NetworkContext networkContext, ITimeService timeService)
        {
            _timeService = timeService;
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
            foreach (var entity in _networks)
            {
                var newTime = entity.time.Value + _timeService.DeltaTime;
                entity.ReplaceTime(newTime);
            }
        }
    }
}