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
                    if (input.clientId.Value != player.clientId.Value)
                        continue;

                    player.isMoving = input.axisInput.Value.magnitude > 0;

                    if (player.isMoving)
                    {
                        var x = input.axisInput.Value.x;
                        var y = input.axisInput.Value.y;
                        var transform = player.transform.Value;

                        _direction.x = (transform.right * x).x + (transform.forward * y).x;
                        _direction.z = (transform.right * x).z + (transform.forward * y).z;

                        player.ReplaceDirection(_direction.normalized);
                    }
                    else
                    {
                        if (player.currentSpeed.Value <= 0)
                        {
                            player.ReplaceDirection(Vector3.zero);
                        }
                    }

                    var plane = new Plane(Vector3.up, Vector3.zero);

                    if (plane.Raycast(input.pointerRay.Value, out float target))
                    {
                        var hitPoint = input.pointerRay.Value.GetPoint(target);
                        var direction = hitPoint - player.transform.Value.position;

                        direction.y = 0;

                        if (direction.sqrMagnitude > 0.01f)
                        {
                            player.ReplaceLookAtPoint(direction);
                        }
                    }
                }
            }
        }
    }
}