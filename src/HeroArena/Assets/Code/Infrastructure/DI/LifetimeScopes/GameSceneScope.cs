using Code.Game.Features.Player.Factory;
using Code.Game.Input.Service;
using Code.Infrastructure.DI.EntryPoints;
using Code.Infrastructure.Identifiers;
using Code.Infrastructure.States.Factory;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.Infrastructure.Systems;
using Code.Infrastructure.View.Factory;
using Entitas;
using System.Linq;
using System.Reflection;
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

        private void BindSystems(IContainerBuilder builder)
        {
            var systemType = typeof(ISystem);
            var implementations = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => systemType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (var impl in implementations)
            {
                builder.Register(impl, Lifetime.Transient).As<ISystem>().AsSelf();
            }
        }

        private void BindGameFactories(IContainerBuilder builder)
        {
            builder.Register<IEntityViewFactory, EntityViewFactory>(Lifetime.Singleton);
            builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton);
        }
    }
}