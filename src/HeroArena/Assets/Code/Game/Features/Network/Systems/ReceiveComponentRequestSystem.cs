using Entitas;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Code.Game.Features.Network.Systems
{
    public class ReceiveComponentRequestSystem : IInitializeSystem, IExecuteSystem
    {
        private readonly List<NetworkEntity> _buffer = new(16);
        private readonly NetworkContext _networkContext;
        private readonly IGroup<NetworkEntity> _requests;

        public ReceiveComponentRequestSystem(NetworkContext networkContext)
        {
            _networkContext = networkContext;
            _requests = networkContext
                .GetGroup(NetworkMatcher
                .AllOf(
                    //NetworkMatcher.ClientId,
                    NetworkMatcher.EntityId,
                    NetworkMatcher.EntityRequestType,
                    NetworkMatcher.ReceiveIntValue,
                    NetworkMatcher.ComponentId,
                    NetworkMatcher.ComponentContext,
                    NetworkMatcher.ComponentTypeName
                    ));
        }

        public void Initialize()
        {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(RequestTypes.Add.ToString(), AddComponentRequest);
        }

        private void AddComponentRequest(ulong senderClientId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out int entityIdValue);
            reader.ReadValueSafe(out int requestTypeValue);
            reader.ReadValueSafe(out int intValue);
            reader.ReadValueSafe(out int componentIdValue);
            reader.ReadValueSafe(out int componentContextValue);
            reader.ReadValueSafe(out string componentTypeNameValue);

            var entity = _networkContext.CreateEntity();

            //entity.AddClientId((int)senderClientId);
            entity.AddEntityId(entityIdValue);
            entity.AddEntityRequestType((RequestTypes)requestTypeValue);
            entity.AddReceiveIntValue(intValue);
            entity.AddComponentId(componentIdValue);
            entity.AddComponentContext((ComponentContexts)componentContextValue);
            entity.AddComponentTypeName(componentTypeNameValue);

            Debug.Log($"Received request of type {(RequestTypes)requestTypeValue} from client {senderClientId} for entity {entityIdValue} with int value {intValue} and component type {(ComponentContexts)componentContextValue}");
        }

        public void Execute()
        {
            foreach (var request in _requests)
            {
                switch (request.entityRequestType.Value)
                {
                    case RequestTypes.Add:
                        //Debug.Log($"Processing Add request from client {request.clientId.Value} for entity {request.entityId.Value}");

                        var entity = Contexts.sharedInstance.game.CreateEntity();
                        var componentIndex = request.componentId.Value;
                        var type = Type.GetType(request.componentTypeName.Value);
                        var component = entity.CreateComponent(componentIndex, type);

                        entity.AddComponent(componentIndex, component);

                        break;
                    case RequestTypes.Replace:

                        break;
                    case RequestTypes.Remove:

                        break;
                }
            }

            foreach (var entity in _requests.GetEntities(_buffer))
            {
                entity.Destroy();
            }
        }
    }
}