using Code.Game.Features.Player.Service;
using Entitas;

namespace Code.Game.Features.Player
{
    [Game] public class PlayerComponent : IComponent { }
    [Game] public class PlayerSpawnRequsted : IComponent { }
    [Game] public class PlayerLocomotionComponent : IComponent { public PlayerLocomotionService Value; }
}