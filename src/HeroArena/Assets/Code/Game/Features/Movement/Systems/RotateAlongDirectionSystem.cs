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
                if(entity.direction.Value.sqrMagnitude >= 0.01f)
                {
                    float angle = Mathf.Atan2(entity.direction.Value.y, entity.direction.Value.x) * Mathf.Rad2Deg;
                    entity.transform.Value.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }
}