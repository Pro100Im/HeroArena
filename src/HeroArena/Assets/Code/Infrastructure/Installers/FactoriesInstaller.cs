using Code.Common.Windows;
using Code.Game.Features.Player.Factory;
using Code.Infrastructure.States.Factory;
using Code.Infrastructure.Systems;
using Code.Infrastructure.View.Factory;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class FactoriesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSystemFactory();
            BindUIFactories();
            BindStateFactory();
            BindGameFactories();
        }

        private void BindSystemFactory()
        {
            Container.Bind<ISystemFactory>().To<SystemFactory>().AsSingle();
        }

        private void BindUIFactories()
        {
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();
        }

        private void BindStateFactory()
        {
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
        }

        private void BindGameFactories()
        {
            Container.Bind<IEntityViewFactory>().To<EntityViewFactory>().AsSingle();
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
        }
    }
}
