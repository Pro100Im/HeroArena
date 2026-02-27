using Code.Game.Features.Network;
using Entitas;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Code.Game.Features.Input.Systems
{
    public class ReceiveInputSystem : IInitializeSystem, ITearDownSystem
    {
        private readonly IGroup<InputEntity> _inputs;
        private readonly List<InputEntity> _inputsBuffer = new(32);

        public ReceiveInputSystem(InputContext input)
        {
            _inputs = input.GetGroup(InputMatcher
                .AllOf(
                InputMatcher.Input,
                InputMatcher.AxisInput,
                InputMatcher.ClientId));
        }

        public void Initialize()
        {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(RequestTypes.ReceiveInput.ToString(), ReceiveInput);
        }

        private void ReceiveInput(ulong senderClientId, FastBufferReader reader)
        {
            foreach (var input in _inputs.GetEntities(_inputsBuffer))
            {
                if (input.clientId.Value != senderClientId)
                    continue;

                reader.ReadValueSafe(out float x);
                reader.ReadValueSafe(out float y);

                if (x == 0 && y == 0)
                    input.ReplaceAxisInput(Vector2.zero);
                else
                    input.ReplaceAxisInput(new Vector2(x, y));
            }
        }

        public void TearDown()
        {
            NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler(RequestTypes.ReceiveInput.ToString());
        }
    }
}