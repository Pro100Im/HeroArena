using Code.Infrastructure.DI.EntryPoints;
using Code.Infrastructure.Helpers;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using VContainer;
using VContainer.Unity;

namespace Code.Infrastructure.DI
{
    public class BootstrapScope : LifetimeScope/*, ICoroutineRunner*/
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //builder.RegisterInstance(this);

            BindContexts(builder);
            BindStateMachine(builder);
            BindGameStates(builder);

            builder.RegisterEntryPoint<GameWorld>();
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
            builder.Register<BootstrapState>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LoadingHomeScreenState>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<HomeScreenState>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LoadingGameState>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameEnterState>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameLoopState>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameOverState>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}