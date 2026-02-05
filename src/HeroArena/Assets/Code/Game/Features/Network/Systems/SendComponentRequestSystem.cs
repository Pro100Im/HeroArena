using Code.Common.Entity;
using Entitas;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Code.Game.Features.Network.Systems
{
    public class SendComponentRequestSystem : IExecuteSystem
    {
        private readonly List<NetworkEntity> _buffer = new(16);
        private readonly IGroup<NetworkEntity> _requests;

        public SendComponentRequestSystem(NetworkContext networkContext)
        {
            _requests = networkContext
                .GetGroup(NetworkMatcher
                .AllOf(
                    NetworkMatcher.ClientId,
                    NetworkMatcher.EntityRequestType,
                    NetworkMatcher.EntitySend
                    ));
        }

        public void Execute()
        {
            foreach (var request in _requests)
            {
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                    request.entityRequestType.Value.ToString(),
                    NetworkManager.Singleton.ConnectedClientsIds,
                    SerializePayload(request.entitySend.Value)
                );

                Debug.Log($"Sent request of type {request.entityRequestType.Value.ToString()} with payload {request.entitySend.Value}");
            }

            foreach (var entity in _requests.GetEntities(_buffer))
            {
                entity.Destroy();
            }
        }

        private FastBufferWriter SerializePayload(object payload)
        {
            var writer = new FastBufferWriter(128, Allocator.Temp);

            return writer;
        }
    }
}
