using Code.Game.Features.Network;
using Entitas;
using Unity.Netcode;
using UnityEngine;

namespace Code.Game.Features.Movement.Systems
{
    public class RollBackByCharacterControllerSystem : IInitializeSystem, ITearDownSystem
    {
        private readonly IGroup<GameEntity> _movers;
        private readonly IGroup<NetworkEntity> _networks;

        public RollBackByCharacterControllerSystem(GameContext gameContext, NetworkContext networkContext)
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

            _networks = networkContext
                .GetGroup(NetworkMatcher
                .AllOf(
                NetworkMatcher.CurrentTick,
                NetworkMatcher.TickRate,
                NetworkMatcher.TickTime,
                NetworkMatcher.Time
                ));
        }

        public void Initialize()
        {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(RequestTypes.RollBackMove.ToString(), RollBackMoveHandler);
        }

        private void RollBackMoveHandler(ulong senderClientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out int activateTick);
            reader.ReadValueSafe(out ulong clientId);

            foreach (NetworkEntity network in _networks)
            {
                foreach (GameEntity mover in _movers)
                {
                    if (mover.clientId.Value == clientId)
                    {
                        Debug.LogWarning($"RollBackMoveHandler senderClientId {clientId}");


                        var correctPos = mover.movementHistory.Value[(activateTick - 1) % mover.historyBufferSize.Value].Position;

                        mover.isMovementRollback = activateTick <= network.currentTick.Value;

                        while (activateTick <= network.currentTick.Value)
                        {
                            var moveDir = mover.movementHistory.Value[(activateTick - 1) % mover.historyBufferSize.Value].Direction;

                            mover.transform.Value.position = correctPos;
                            mover.characterController.Value.SimpleMove(moveDir * mover.currentSpeed.Value);

                            correctPos = mover.transform.Value.position;

                            mover.movementHistory.Value[activateTick % mover.historyBufferSize.Value].Position = correctPos;

                            activateTick++;
                        }

                        mover.isMovementRollback = false;
                        mover.transform.Value.position = correctPos;
                    }
                }
            }   
        }

        public void TearDown()
        {
            NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler(RequestTypes.RollBackMove.ToString());
        }
    }
}