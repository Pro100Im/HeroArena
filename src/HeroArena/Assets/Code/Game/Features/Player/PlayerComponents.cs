using Code.Game.Features.Player.Service;
using Entitas;

namespace Code.Game.Features.Player
{
    [Game] public class PlayerComponent : IComponent { }
    [Game] public class PlayerIdComponent : IComponent { public ulong Value; }
    [Game] public class PlayerSpawnRequsted : IComponent { }
    [Game] public class PlayerAnimatorComponent : IComponent { public PlayerAnimatorService Value; }
}