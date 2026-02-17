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
                    NetworkMatcher.EntityId,
                    NetworkMatcher.EntityRequestType,
                    NetworkMatcher.SendIntValue,
                    NetworkMatcher.ComponentId,
                    NetworkMatcher.ComponentContext,
                    NetworkMatcher.ComponentTypeName
                    ));
        }

        public void Execute()
        {
            foreach (var request in _requests)
            {
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                    request.entityRequestType.Value.ToString(),
                    NetworkManager.Singleton.ConnectedClientsIds,
                    SerializePayload(request)
                );

                Debug.Log($"Sent request of type {request.entityRequestType.Value.ToString()} with payload {request.sendIntValue.Value}");
            }

            foreach (var entity in _requests.GetEntities(_buffer))
            {
                entity.Destroy();
            }
        }

        private FastBufferWriter SerializePayload(NetworkEntity request)
        {
            var totalSize = sizeof(int)                 
                          + sizeof(int)                 
                          + sizeof(int)                 
                          + sizeof(int)                  
                          + sizeof(int)                  
                          + sizeof(int)
                          + (request.componentTypeName.Value.Length * sizeof(char) + sizeof(int));

            using var writer = new FastBufferWriter(totalSize, Allocator.Temp);

            writer.WriteValueSafe(request.entityId.Value);
            writer.WriteValueSafe(request.entityRequestType.Value);
            writer.WriteValueSafe(request.sendIntValue.Value);
            writer.WriteValueSafe(request.componentId.Value);
            writer.WriteValueSafe(request.componentContext.Value);
            writer.WriteValueSafe(request.componentTypeName.Value);

            return writer;
        }
    }
}
