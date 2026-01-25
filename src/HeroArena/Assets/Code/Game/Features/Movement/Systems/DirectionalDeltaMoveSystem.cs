using Code.Common.Time;
using Entitas;

namespace Code.Game.Features.Movement.Systems
{
    public class DirectionalDeltaMoveSystem : IExecuteSystem
    {
        private readonly ITimeService _timeService;
        private readonly IGroup<GameEntity> _movers;

        public DirectionalDeltaMoveSystem(GameContext gameContext, ITimeService timeService)
        {
            _timeService = timeService;

            _movers = gameContext
                .GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.WorldPosition, 
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
                mover.ReplaceWorldPosition(mover.worldPosition.Value + mover.direction.Value * mover.speed.Value * _timeService.DeltaTime);
            }
        }
    }
}