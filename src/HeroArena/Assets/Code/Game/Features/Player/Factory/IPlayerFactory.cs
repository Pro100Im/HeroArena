using UnityEngine;

namespace Code.Game.Features.Player.Factory
{
    public interface IPlayerFactory
    {
        GameEntity CreatePlayer(Vector3 at);
    }
}