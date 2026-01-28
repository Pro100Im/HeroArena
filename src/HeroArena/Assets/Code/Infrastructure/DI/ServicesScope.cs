using Code.Common.StaticData;
using Code.Common.Time;
using Code.Common.Windows;
using Code.Game.Input.Service;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Identifiers;
using Code.Infrastructure.Loading;
using VContainer;
using VContainer.Unity;

namespace Code.Infrastructure.DI
{
    public class ServicesScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //BindInputService(builder);
            //BindCommonServices(builder);
            //BindAssetManagementServices(builder);
        }

        //private void BindInputService(IContainerBuilder builder)
        //{
        //    builder.Register<IInputService, InputService>(Lifetime.Singleton);
        //}

        //private void BindCommonServices(IContainerBuilder builder)
        //{
        //    builder.Register<ITimeService, UnityTimeService>(Lifetime.Singleton);
        //    builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
        //    builder.Register<IStaticDataService, StaticDataService>(Lifetime.Singleton);
        //    builder.Register<IWindowService, WindowService>(Lifetime.Singleton);
        //    builder.Register<IIdentifierService, IdentifierService>(Lifetime.Singleton);
        //}

        //private void BindAssetManagementServices(IContainerBuilder builder)
        //{
        //    builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
        //}
    }
}