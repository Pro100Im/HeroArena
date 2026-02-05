using Entitas;
using System;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Code.Game.Features.Network.Systems
{
    public class ReceiveComponentRequestSystem : IInitializeSystem, IExecuteSystem
    {
        private readonly IGroup<NetworkEntity> _requests;

        public ReceiveComponentRequestSystem(NetworkContext networkContext)
        {
            _requests = networkContext
                .GetGroup(NetworkMatcher
                .AllOf(
                    NetworkMatcher.ClientId,
                    NetworkMatcher.EntityRequestType,
                    NetworkMatcher.EntityReceive
                    ));
        }

        public void Initialize()
        {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(RequestType.Add.ToString(), AddComponentRequest);
        }

        private void AddComponentRequest(ulong senderClientId, FastBufferReader messagePayload)
        {
            Debug.Log($"Received request of type Add from client {senderClientId}");

            var entity = Contexts.sharedInstance.game.CreateEntity();
            entity.AddDamage(10);
        }

        public void Execute()
        {
            
        }
    }
}