using Code.Common.Time;
using Entitas;

namespace Code.Game.Features.Movement.Systems
{
    public class MoveByCharacterControllerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _movers;

        public MoveByCharacterControllerSystem(GameContext gameContext)
        {
            _movers = gameContext
                .GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.CharacterController,
                    GameMatcher.CurrentSpeed,
                    GameMatcher.Direction,
                    GameMatcher.MovementAvailable
                    ));
        }

        public void Execute()
        {
            foreach (var mover in _movers)
            {
                if (mover.direction.Value.magnitude > 0)
                    mover.characterController.Value.SimpleMove(mover.direction.Value * mover.currentSpeed.Value);
            }
        }
    }
}