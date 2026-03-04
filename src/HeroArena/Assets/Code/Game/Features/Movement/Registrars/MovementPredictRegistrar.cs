using Code.Game.Features.Network.Data;
using Code.Infrastructure.View.Registrars;
using Unity.Netcode;

namespace Code.Game.Features.Movement.Registrars
{
    public class MovementPredictRegistrar : EntityComponentRegistrar
    {
        public int HistoryBufferSize;

        public override void RegisterComponents()
        {
            //var networkObject = GetComponent<NetworkObject>();
            //if (networkObject.IsOwner)

            Entity.AddHistoryBufferSize(HistoryBufferSize);
            Entity.AddMovementHistory(new MovementHistoryData[HistoryBufferSize]);
        }

        public override void UnregisterComponents()
        {
            Entity.RemoveHistoryBufferSize();
            Entity.RemoveMovementHistory();
        }
    }
}