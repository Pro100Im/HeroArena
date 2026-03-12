using Entitas;
using Unity.Netcode;
using UnityEngine;

namespace Code.Game.Features.Player.Systems
{
    public class PlayerAnimatorSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;

        public PlayerAnimatorSystem(GameContext gameContext)
        {
            _players = gameContext.GetGroup(GameMatcher
                .AllOf(
                GameMatcher.Player,
                GameMatcher.ClientId,
                GameMatcher.PlayerAnimator,
                GameMatcher.CurrentSpeed,
                GameMatcher.MaxSpeed));
        }

        public void Execute()
        {
            foreach (var player in _players)
            {
                if (player.clientId.Value != NetworkManager.Singleton.LocalClientId)
                    continue;

                var normalizedSpeed = Mathf.Clamp01(player.currentSpeed.Value / player.maxSpeed.Value);

                player.playerAnimator.Value.SetMoveParameters(normalizedSpeed, 0);
            }
        }
    }
}
