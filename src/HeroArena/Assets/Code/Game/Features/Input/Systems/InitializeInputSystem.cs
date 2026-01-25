using Code.Common.Entity;
using Entitas;

namespace Code.Game.Features.Input.Systems
{
    public class InitializeInputSystem : IInitializeSystem
    {
        public void Initialize()
        {
            CreateInputEntity.Empty().isInput = true;
        }
    }
}