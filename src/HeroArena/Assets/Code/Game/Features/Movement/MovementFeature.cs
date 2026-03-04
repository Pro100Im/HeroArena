using Code.Game.Features.Movement.Systems;
using Code.Infrastructure.Systems;
using Unity.Netcode;

namespace Code.Game.Features.Movement
{
    public class MovementFeature : Feature
    {
        public MovementFeature(ISystemFactory systemFactory)
        {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            {
        
            }

            if (NetworkManager.Singleton.IsClient)
            {
                Add(systemFactory.Create<MoveByCharacterControllerSystem>());
                Add(systemFactory.Create<UpdateTransformPositionSystem>());
                Add(systemFactory.Create<RotateAlongDirectionSystem>());
            }
        }
    }
}