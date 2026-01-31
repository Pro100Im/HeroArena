using Entitas;
using UnityEngine;

namespace Code.Game.Features.Movement.Systems
{
    public class RotateAlongDirectionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public RotateAlongDirectionSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher
              .AllOf(
                GameMatcher.Transform,
                GameMatcher.RotationAlignedAlongDirection,
                GameMatcher.Direction));
        }

        public void Execute()
        {
            foreach(GameEntity entity in _entities)
            {
                if (entity.direction.Value.sqrMagnitude >= 0.01f)
                {
                    var dir = entity.direction.Value;
                    var targetRotation = Quaternion.LookRotation(dir, Vector3.up);
                    var moveSpeed = dir.magnitude;
                    var rotationSpeed = 180f + 90f * moveSpeed;

                    entity.transform.Value.rotation = Quaternion.RotateTowards(
                        entity.transform.Value.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );
                }
            }
        }
    }
}