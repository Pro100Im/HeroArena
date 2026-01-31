using Entitas;
using UnityEngine;

namespace Code.Game.Features.Movement.Systems
{
    public class RotateByCharacterControllerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public RotateByCharacterControllerSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher
              .AllOf(
                GameMatcher.CharacterController,
                GameMatcher.RotationAlignedAlongDirection,
                GameMatcher.Direction));
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.direction.Value.sqrMagnitude >= 0.01f)
                {
                    float angle = Mathf.Atan2(entity.direction.Value.y, entity.direction.Value.x) * Mathf.Rad2Deg;
                    entity.transform.Value.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }
}
