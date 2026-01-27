using Code.Common.Time;
using Entitas;

namespace Code.Game.Features.Movement.Systems
{
    public class MoveByCharacterControllerSystem : IExecuteSystem
    {
        private readonly ITimeService _timeService;
        private readonly IGroup<GameEntity> _movers;

        public MoveByCharacterControllerSystem(GameContext gameContext, ITimeService timeService)
        {
            _timeService = timeService;

            _movers = gameContext
                .GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.CharacterController, 
                    GameMatcher.Speed, 
                    GameMatcher.Direction, 
                    GameMatcher.Moving,
                    GameMatcher.MovementAvailable
                    ));
        }

        public void Execute()
        {
            foreach(var mover in _movers) 
            {
                mover.characterController.Value.SimpleMove(mover.direction.Value * mover.speed.Value);
            }
        }
    }
}