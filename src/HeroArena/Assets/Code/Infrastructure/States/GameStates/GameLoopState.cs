using Code.Game.Common;
using Code.Game.Features;
using Code.Game.Features.Network;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.Systems;
using Unity.Netcode;

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

            //if (NetworkManager.Singleton.IsHost)
            //{
            //    var entity = Contexts.sharedInstance.network.CreateEntity();

            //    entity.AddEntityId(entity.creationIndex);
            //    entity.AddEntityRequestType(RequestTypes.Add);
            //    entity.AddSendIntValue(10);
            //    entity.AddClientId(3);
            //    entity.AddComponentContext(ComponentContexts.Game);
            //    entity.AddComponentId(GameComponentsLookup.Damage);
            //    entity.AddComponentTypeName("Code.Game.Common.Damage");
            //}
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