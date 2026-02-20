using Code.Game.Features.Player.Systems;
using Code.Infrastructure.Systems;
using Unity.Netcode;

namespace Code.Game.Features.Player
{
    public class PlayerFeature : Feature
    {
        public PlayerFeature(ISystemFactory systemFactory)
        {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            {
                Add(systemFactory.Create<PlayerSpawnSystem>());
                Add(systemFactory.Create<PlayerAnimatorSystem>());
            }
            else
            {
                Add(systemFactory.Create<PlayerCameraInitSystem>());
            }

            Add(systemFactory.Create<PlayerDiractionalByInputSystem>());
            Add(systemFactory.Create<PlayerSpeedSetupSystem>());
        }
    }
}