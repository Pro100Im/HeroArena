using Code.Game.Features.Player.Systems;
using Code.Infrastructure.Systems;
using Unity.Netcode;

namespace Code.Game.Features.Player
{
    public class PlayerFeature : Feature
    {
        public PlayerFeature(ISystemFactory systemFactory)
        {
            if(NetworkManager.Singleton.IsHost)
            {
                Add(systemFactory.Create<PlayerSpawnSystem>());
                Add(systemFactory.Create<PlayerAnimatorSystem>());
            }

            Add(systemFactory.Create<PlayerCameraInitSystem>());
            Add(systemFactory.Create<PlayerDiractionalByInputSystem>());
            Add(systemFactory.Create<PlayerSpeedSetupSystem>());
        }
    }
}