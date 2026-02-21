using Code.Common.Network;
using Code.Game.Features.Network;
using Code.Infrastructure.AssetManagement;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Code.Infrastructure.View.Factory
{
    public class EntityViewFactory : IEntityViewFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IObjectResolver _objectResolver;
        private readonly Vector3 _farAway = new(-999, 999, 0);

        public EntityViewFactory(IAssetProvider assetProvider, IObjectResolver objectResolver)
        {
            _assetProvider = assetProvider;
            _objectResolver = objectResolver;
        }

        public EntityBehaviour CreateViewForEntity(GameEntity entity)
        {
            var viewPrefab = _assetProvider.LoadAsset<EntityBehaviour>(entity.viewPath.Value);
            var view = GameObject.Instantiate<EntityBehaviour>(viewPrefab, Vector3.zero, Quaternion.identity, null);
            var networkObject = view.GetComponent<NetworkObject>();

            networkObject.SpawnWithOwnership(entity.clientId.Value);

            var totalSize = UnsafeUtility.SizeOf<ulong>() + UnsafeUtility.SizeOf<ulong>();
            using var builder = new NetworkMessageBuilder(totalSize);
            var writer = builder.Write(networkObject.OwnerClientId).Write(networkObject.NetworkObjectId).Build(); 

            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                       RequestTypes.ReceiveObjectId.ToString(),
                       NetworkManager.Singleton.ConnectedClientsIds,
                       writer);

            _objectResolver.Inject(view);

            return view;
        }

        public EntityBehaviour CreateViewForEntityFromPrefab(GameEntity entity)
        {
            var view = GameObject.Instantiate<EntityBehaviour>(entity.viewPrefab.Value, Vector3.zero, Quaternion.identity, null);
            view.GetComponent<NetworkObject>().SpawnWithOwnership(entity.clientId.Value);

            _objectResolver.Inject(view);

            return view;
        }
    }
} 