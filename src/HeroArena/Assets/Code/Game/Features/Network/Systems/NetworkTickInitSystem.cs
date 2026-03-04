using Code.Common.Entity;
using Entitas;

namespace Code.Game.Features.Network.Systems
{
    public class NetworkTickInitSystem : IInitializeSystem
    {
        public void Initialize()
        {
            var entity = CreateNetworkEntity.Empty();

            entity.AddCurrentTick(0);
            entity.AddTickRate(60);
            entity.AddTickTime(1f / 60);
            entity.AddTime(0);
        }
    }
}