using Entitas;


namespace Code.Game.Features.Movement.Systems
{
    public class UpdateTransformPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _movers;

        public UpdateTransformPositionSystem(GameContext gameContext)
        {
            _movers = gameContext
                .GetGroup(GameMatcher
                .AllOf(GameMatcher.WorldPosition, GameMatcher.Transform));
        }

        public void Execute()
        {
            foreach(var mover in _movers)
            {
                mover.transform.Value.position = mover.worldPosition.Value;
            }
        }
    }
}