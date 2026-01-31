using Entitas;
using UnityEngine;

namespace Code.Game.Features.Player.Systems
{
    public class PlayerDiractionalByInputSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<InputEntity> _inputs;

        private Vector3 _direction;

        public PlayerDiractionalByInputSystem(GameContext gameContext, InputContext inputContext)
        {
            _players = gameContext.GetGroup(GameMatcher
                .AllOf(
                GameMatcher.Player,
                GameMatcher.MovementAvailable,
                GameMatcher.CurrentSpeed));

            _inputs = inputContext.GetGroup(InputMatcher.Input);
            _direction = Vector3.zero;
        }

        public void Execute()
        {
            foreach (var input in _inputs)
            {
                foreach (var player in _players)
                {
                    player.isMoving = input.hasAxisInput;

                    if (player.isMoving)
                    {
                        _direction.x = input.axisInput.Value.x;
                        _direction.z = input.axisInput.Value.y;

                        player.ReplaceDirection(_direction.normalized);
                    }
                    else
                    {
                        if (player.currentSpeed.Value <= 0)
                        {
                            player.ReplaceDirection(Vector3.zero);
                        }
                    }
                }
            }
        }
    }
}