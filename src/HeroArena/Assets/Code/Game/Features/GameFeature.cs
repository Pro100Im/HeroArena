using Code.Game.Features.Input;
using Code.Game.Features.Movement;
using Code.Game.Features.Network;
using Code.Game.Features.Player;
using Code.Infrastructure.Systems;
using Code.Infrastructure.View;

namespace Code.Game.Features
{
    public class GameFeature : Feature
    {
        public GameFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<CreateViewFeature>());
            Add(systemFactory.Create<NetworkFeature>());
            Add(systemFactory.Create<InputFeature>());
            Add(systemFactory.Create<PlayerFeature>());
            Add(systemFactory.Create<MovementFeature>());
        }
    }
}
