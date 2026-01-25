using Code.Game.Features;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.Systems;

namespace Code.Infrastructure.States.GameStates
{
    public class GameLoopState : IState, IUpdateable
    {
        private readonly ISystemFactory _systems;

        private readonly GameContext _gameContext;

        private GameFeature _gameFeature;

        public GameLoopState(ISystemFactory systems, GameContext gameContext)
        {
            _systems = systems;
            _gameContext = gameContext;
        }

        public void Enter()
        {
            _gameFeature = _systems.Create<GameFeature>();
            _gameFeature.Initialize();
        }

        public void Update()
        {
            _gameFeature?.Execute();
            _gameFeature?.Cleanup();
        }

        public void Exit()
        {
            _gameFeature.DeactivateReactiveSystems();
            _gameFeature.ClearReactiveSystems();

            DestructEntities();

            _gameFeature.Cleanup();
            _gameFeature.TearDown();
            _gameFeature = null;
        }

        private void DestructEntities()
        {
            foreach(GameEntity entity in _gameContext.GetEntities())
                entity.isDestructed = true;
        }
    }
}