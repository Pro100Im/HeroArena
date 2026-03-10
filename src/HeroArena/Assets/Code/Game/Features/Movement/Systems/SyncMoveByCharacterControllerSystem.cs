using Code.Common.Network;
using Code.Game.Features.Network;
using Entitas;
using Unity.Netcode;
using UnityEngine;

namespace Code.Game.Features.Movement.Systems
{
    public class SyncMoveByCharacterControllerSystem : IInitializeSystem, ITearDownSystem
    {
        private readonly IGroup<GameEntity> _movers;
        private readonly IGroup<NetworkEntity> networks;

        public SyncMoveByCharacterControllerSystem(GameContext gameContext, NetworkContext networkContext)
        {
            _movers = gameContext
                .GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.CharacterController,
                    GameMatcher.CurrentSpeed,
                    GameMatcher.Direction,
                    GameMatcher.MovementAvailable,
                    GameMatcher.MovementHistory
                    ));

            networks = networkContext.GetGroup(NetworkMatcher
                .AllOf(
                NetworkMatcher.CurrentTick,
                NetworkMatcher.TickRate,
                NetworkMatcher.TickTime,
                NetworkMatcher.Time
                ));
        }

        public void Initialize()
        {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(RequestTypes.MovementHistory.ToString(), ServerMoveHandler);
        }

        private void ServerMoveHandler(ulong senderClientId, FastBufferReader reader)
        {
            foreach (GameEntity mover in _movers)
            {
                if (mover.clientId.Value == senderClientId)
                {
                    reader.ReadValueSafe(out int tick);
                    reader.ReadValueSafe(out Vector2 currentDir);
                    reader.ReadValueSafe(out Vector3 currentPos);
                    reader.ReadValueSafe(out Vector2 lastDir);
                    reader.ReadValueSafe(out Vector3 lastPos);

                    var startPos = mover.transform.Value.position;

                    mover.transform.Value.position = lastPos;
                    mover.characterController.Value.SimpleMove(lastDir * mover.currentSpeed.Value);

                    var correctPos = mover.transform.Value.position;
                    mover.transform.Value.position = startPos;

                    if (Vector3.Distance(correctPos, currentPos) > 0.5f)
                    {
                        var totalSize = sizeof(int);
                        using var builder = new NetworkMessageBuilder(totalSize);
                        var writer = builder.Write(tick).Build();

                        NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                        RequestTypes.RollBackMove.ToString(),
                        senderClientId,
                        writer);
                    }

                    break;
                }
            }
        }

        public void TearDown()
        {
            NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler(RequestTypes.MovementHistory.ToString());
        }
    }
}