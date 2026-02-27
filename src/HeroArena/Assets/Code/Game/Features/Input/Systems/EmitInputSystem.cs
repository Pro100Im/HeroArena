using Code.Common.Network;
using Code.Game.Features.Network;
using Code.Game.Input.Service;
using Entitas;
using Unity.Netcode;
using UnityEngine;

namespace Code.Game.Features.Input.Systems
{
    public class EmitInputSystem : IExecuteSystem
    {
        private readonly IInputService _inputService;
        private readonly IGroup<InputEntity> _inputs;

        public EmitInputSystem(InputContext input, IInputService inputService)
        {
            _inputService = inputService;
            _inputs = input.GetGroup(InputMatcher.Input);
        }

        public void Execute()
        {
            foreach (InputEntity input in _inputs)
            {
                if (!input.isLocalPlayer)
                    continue;

                if (_inputService.HasAxisInput())
                {
                    var x = _inputService.GetHorizontalAxis();
                    var y = _inputService.GetVerticalAxis();

                    if (x != input.axisInput.Value.x || y != input.axisInput.Value.y)
                        SendInput(x, y, input.clientId.Value);

                    input.ReplaceAxisInput(new Vector2(_inputService.GetHorizontalAxis(), _inputService.GetVerticalAxis()));
                }
                else
                {
                    if (input.axisInput.Value.x != 0 || input.axisInput.Value.y != 0)
                        SendInput(0, 0, input.clientId.Value);

                    input.ReplaceAxisInput(Vector2.zero);
                }
            }
        }

        private void SendInput(float x, float y, ulong clientId)
        {
            var totalSize = sizeof(float) + sizeof(float);
            using var builder = new NetworkMessageBuilder(totalSize);
            var writer = builder.Write(x).Write(y).Build();

            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
            RequestTypes.ReceiveInput.ToString(),
            NetworkManager.ServerClientId,
            writer);
        }
    }
}