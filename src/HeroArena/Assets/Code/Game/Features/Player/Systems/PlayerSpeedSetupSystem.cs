using Code.Common.Time;
using Entitas;

namespace Code.Game.Features.Player.Systems
{
    public class PlayerSpeedSetupSystem : IExecuteSystem
    {
        private readonly ITimeService _timeService;
        private readonly IGroup<GameEntity> _players;

        public PlayerSpeedSetupSystem(GameContext gameContext, ITimeService timeService)
        {
            _timeService = timeService;

            _players = gameContext.GetGroup(GameMatcher
                .AllOf(
                GameMatcher.Player,
                GameMatcher.Speed,
                GameMatcher.MaxRunSpeed,
                GameMatcher.MaxWalkSpeed,
                GameMatcher.CurrentSpeed));
        }

        public void Execute()
        {
            foreach (var player in _players)
            {
                if (player.isMoving && player.currentSpeed.Value < player.maxWalkSpeed.Value)
                {
                    player.currentSpeed.Value += player.speed.Value * _timeService.DeltaTime;

                    if (player.currentSpeed.Value > player.maxWalkSpeed.Value)
                        player.currentSpeed.Value = player.maxWalkSpeed.Value;
                }
                else if (player.currentSpeed.Value > 0)
                {
                    player.currentSpeed.Value -= player.speed.Value * _timeService.DeltaTime * 2;

                    if (player.currentSpeed.Value < 0)
                        player.currentSpeed.Value = 0;
                }
            }
        }
    }
}
