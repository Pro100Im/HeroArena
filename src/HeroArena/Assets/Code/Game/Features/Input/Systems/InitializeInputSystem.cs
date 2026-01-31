using Code.Common.Entity;
using Code.Game.Input.Service;
using Entitas;

namespace Code.Game.Features.Input.Systems
{
    public class InitializeInputSystem : IInitializeSystem
    {
        public InitializeInputSystem(IInputService inputService)
        {
            inputService.EnableInput();
        }

        public void Initialize()
        {
            CreateInputEntity.Empty().isInput = true;
        }
    }
}