using Code.Common.Network;
using Code.Game.Features.Network;
using Code.Game.Features.Network.Data;
using Entitas;
using Unity.Netcode;
using UnityEngine;

namespace Code.Game.Features.Movement.Systems
{
    public class MoveByCharacterControllerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _movers;
        private readonly IGroup<NetworkEntity> networks;

        public MoveByCharacterControllerSystem(GameContext gameContext, NetworkContext networkContext)
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

        public void Execute()
        {
            foreach (var mover in _movers)
            {
                if (NetworkManager.Singleton != null && (!NetworkManager.Singleton.IsClient || mover.clientId.Value != NetworkManager.Singleton.LocalClientId))
                    continue;

                foreach(var network in networks)
                {
                    while(network.time.Value > network.tickTime.Value)
                    {
                        var newCurrentTick = network.currentTick.Value + 1;
                        var newTime = network.time.Value - network.tickTime.Value;

                        network.ReplaceCurrentTick(newCurrentTick);
                        network.ReplaceTime(newTime);

                        if (mover.direction.Value.magnitude > 0)
                            mover.characterController.Value.SimpleMove(mover.direction.Value * mover.currentSpeed.Value);

                        mover.movementHistory.Value[network.currentTick.Value % mover.historyBufferSize.Value]
                            = new MovementHistoryData(network.currentTick.Value, mover.direction.Value, mover.transform.Value.position);

                        if (network.currentTick.Value < 2)
                            return;

                        var currentData = mover.movementHistory.Value[network.currentTick.Value % mover.historyBufferSize.Value];
                        var lastData = mover.movementHistory.Value[(network.currentTick.Value - 1) % mover.historyBufferSize.Value];

                        SendMove(network.currentTick.Value, currentData.Direction, currentData.Position, lastData.Direction, lastData.Position);
                    }
                }
            }
        }

        private void SendMove(int tick, Vector2 currentDir, Vector3 currentPos, Vector2 lastDir, Vector3 lastPos)
        {
            var totalSize = sizeof(int) + sizeof(float) * 10;
            using var builder = new NetworkMessageBuilder(totalSize);
            var writer = builder.Write(tick).Write(currentDir).Write(currentPos).Write(lastDir).Write(lastPos).Build();

            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
            RequestTypes.MovementHistory.ToString(),
            NetworkManager.ServerClientId,
            writer);
        }
    }
}