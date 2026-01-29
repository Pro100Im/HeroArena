using Code.Game.Features;
using Code.Game.Features.Input;
using Code.Game.Features.Input.Systems;
using Code.Game.Features.Movement;
using Code.Game.Features.Movement.Systems;
using Code.Game.Features.Player;
using Code.Game.Features.Player.Factory;
using Code.Game.Features.Player.Systems;
using Code.Game.Input.Service;
using Code.Infrastructure.DI.EntryPoints;
using Code.Infrastructure.Identifiers;
using Code.Infrastructure.States.Factory;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.Infrastructure.Systems;
using Code.Infrastructure.View;
using Code.Infrastructure.View.Factory;
using Code.Infrastructure.View.Systems;
using VContainer;
using VContainer.Unity;

namespace Code.Infrastructure.DI.LifetimeScopes
{
    public class GameSceneScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IIdentifierService, IdentifierService>(Lifetime.Singleton);

            BindContexts(builder);

            BindSystemFactory(builder);
            BindStateFactory(builder);

            BindServices(builder);
            BindGameStates(builder);
            BindStateMachine(builder);

            BindSystems(builder);
            BindFeatures(builder);

            BindGameFactories(builder);

            builder.RegisterEntryPoint<GameWorld>();
        }

        private void BindContexts(IContainerBuilder builder)
        {
            builder.RegisterInstance(Contexts.sharedInstance);
            builder.RegisterInstance(Contexts.sharedInstance.game);
            builder.RegisterInstance(Contexts.sharedInstance.input);
            builder.RegisterInstance(Contexts.sharedInstance.meta);
        }

        private void BindStateFactory(IContainerBuilder builder)
        {
            builder.Register<IStateFactory, StateFactory>(Lifetime.Singleton);
        }

        private void BindSystemFactory(IContainerBuilder builder)
        {
            builder.Register<ISystemFactory, SystemFactory>(Lifetime.Singleton);
        }

        private void BindServices(IContainerBuilder builder)
        {
            builder.Register<IInputService, InputService>(Lifetime.Singleton);
        }

        private void BindStateMachine(IContainerBuilder builder)
        {
            builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindGameStates(IContainerBuilder builder)
        {
            builder.Register<GameEnterState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<GameLoopState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<GameOverState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private void BindFeatures(IContainerBuilder builder)
        {
            builder.Register<GameFeature>(Lifetime.Singleton);
            builder.Register<BindViewFeature>(Lifetime.Singleton);
            builder.Register<InputFeature>(Lifetime.Singleton);
            builder.Register<PlayerFeature>(Lifetime.Singleton);
            builder.Register<MovementFeature>(Lifetime.Singleton);
        }

        private void BindSystems(IContainerBuilder builder)
        {
            builder.Register<BindEntityViewFromPathSystem>(Lifetime.Singleton);
            builder.Register<BindEntityViewFromPrefabSystem>(Lifetime.Singleton);

            builder.Register<InitializeInputSystem>(Lifetime.Singleton);
            builder.Register<EmitInputSystem>(Lifetime.Singleton);

            builder.Register<PlayerDiractionalByInputSystem>(Lifetime.Singleton);

            builder.Register<MoveByCharacterControllerSystem>(Lifetime.Singleton);
            builder.Register<UpdateTransformPositionSystem>(Lifetime.Singleton);
            builder.Register<RotateAlongDirectionSystem>(Lifetime.Singleton);
        }

        private void BindGameFactories(IContainerBuilder builder)
        {
            builder.Register<IEntityViewFactory, EntityViewFactory>(Lifetime.Singleton);
            builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton);
        }
    }
}