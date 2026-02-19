using Code.Infrastructure.Systems;
using Code.Infrastructure.View.Systems;
using Unity.Netcode;

namespace Code.Infrastructure.View
{
    public sealed class BindViewFeature : Feature
    {
        public BindViewFeature(ISystemFactory systems)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Add(systems.Create<CreateEntityViewFromPathSystem>());
                Add(systems.Create<CreateEntityViewFromPrefabSystem>());
                Add(systems.Create<PlayerCharacterLinkSystem>());
            }
        }
    }
}