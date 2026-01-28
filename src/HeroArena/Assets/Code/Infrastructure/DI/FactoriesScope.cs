using Code.Common.Windows;
using Code.Game.Features.Player.Factory;
using Code.Infrastructure.States.Factory;
using Code.Infrastructure.Systems;
using Code.Infrastructure.View.Factory;
using VContainer;
using VContainer.Unity;

namespace Code.Infrastructure.DI
{
    public class FactoriesScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            BindSystemFactory(builder);
            BindUIFactories(builder);
            BindStateFactory(builder);
            BindGameFactories(builder);
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
    }
}
