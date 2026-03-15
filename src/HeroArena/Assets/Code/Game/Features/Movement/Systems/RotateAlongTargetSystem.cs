using Entitas;
using UnityEngine;

namespace Code.Game.Features.Movement.Systems
{
    public class RotateAlongTargetSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public RotateAlongTargetSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher
              .AllOf(
                GameMatcher.CharacterController,
                GameMatcher.RotationAlignedAlongTarget,
                GameMatcher.LookAtPoint));
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                Quaternion targetRotation = Quaternion.LookRotation(entity.lookAtPoint.Value);
                entity.transform.Value.rotation = Quaternion.Slerp(entity.transform.Value.rotation, targetRotation, Time.fixedDeltaTime * 4f);
            }
        }
    }
}
