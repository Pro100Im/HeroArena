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
                if (!NetworkManager.Singleton.IsClient || mover.clientId.Value != NetworkManager.Singleton.LocalClientId)
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

                        //var newHistory = mover.movementHistory.Value;

                        //newHistory[network.currentTick.Value % mover.historyBufferSize.Value]
                        //    = new MovementHistoryData(network.currentTick.Value, mover.direction.Value, mover.transform.Value.position);

                        //mover.ReplaceMovementHistory(newHistory);

                        mover.movementHistory.Value[network.currentTick.Value % mover.historyBufferSize.Value]
                            = new MovementHistoryData(network.currentTick.Value, mover.direction.Value, mover.transform.Value.position);
                    }
                }
            }
        }
    }
}