using Code.Common.StaticData;
using Code.Common.Time;
using Code.Common.Windows;
using Code.Game.Features.Player.Factory;
using Code.Game.Input.Service;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.DI.EntryPoints;
using Code.Infrastructure.Helpers;
using Code.Infrastructure.Identifiers;
using Code.Infrastructure.Loading;
using Code.Infrastructure.States.Factory;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.Infrastructure.Systems;
using Code.Infrastructure.View.Factory;
using Entitas;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Infrastructure.DI
{
    public class BootstrapScope : LifetimeScope
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;

        protected override void Configure(IContainerBuilder builder)
        {
            BindSystemFactory(builder);
            BindUIFactories(builder);
            BindStateFactory(builder);
            BindGameFactories(builder);

            BindInputService(builder);
            BindCommonServices(builder);
            BindAssetManagementServices(builder);

            BindContexts(builder);
            BindGameStates(builder);
            BindStateMachine(builder);
            BindSystems(builder);

            builder.RegisterComponentInNewPrefab(_coroutineRunner, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();
            builder.RegisterEntryPoint<GameWorld>();
        }

        private void BindSystemFactory(IContainerBuilder builder)
        {
            builder.Register<ISystemFactory, SystemFactory>(Lifetime.Singleton);
        }

        private void BindUIFactories(IContainerBuilder builder)
        {
            builder.Register<IWindowFactory, WindowFactory>(Lifetime.Singleton);
        }

        private void BindStateFactory(IContainerBuilder builder)
        {
            builder.Register<IStateFactory, StateFactory>(Lifetime.Singleton);
        }

        private void BindGameFactories(IContainerBuilder builder)
        {
            builder.Register<IEntityViewFactory, EntityViewFactory>(Lifetime.Singleton);
            builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton);
        }

        private void BindInputService(IContainerBuilder builder)
        {
            builder.Register<IInputService, InputService>(Lifetime.Singleton);
        }

        private void BindCommonServices(IContainerBuilder builder)
        {
            builder.Register<ITimeService, UnityTimeService>(Lifetime.Singleton);
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            builder.Register<IStaticDataService, StaticDataService>(Lifetime.Singleton);
            builder.Register<IWindowService, WindowService>(Lifetime.Singleton);
            builder.Register<IIdentifierService, IdentifierService>(Lifetime.Singleton);
        }

        private void BindAssetManagementServices(IContainerBuilder builder)
        {
            builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
        }

        private void BindContexts(IContainerBuilder builder)
        {
            builder.RegisterInstance(Contexts.sharedInstance);
            builder.RegisterInstance(Contexts.sharedInstance.game);
            builder.RegisterInstance(Contexts.sharedInstance.input);
            builder.RegisterInstance(Contexts.sharedInstance.meta);
        }

        private void BindStateMachine(IContainerBuilder builder)
        {
            builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindGameStates(IContainerBuilder builder)
        {
            builder.Register<BootstrapState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<LoadingHomeScreenState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<HomeScreenState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<LoadingGameState>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
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
    }
}
