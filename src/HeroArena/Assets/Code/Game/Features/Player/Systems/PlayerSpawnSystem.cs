using Code.Game.Features.Movement;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.Features.Player.Systems
{
    public class PlayerSpawnSystem : ReactiveSystem<GameEntity>
    {
        public PlayerSpawnSystem(GameContext game) : base(game)
        {

        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
          context.CreateCollector(GameMatcher
            .AllOf(
              GameMatcher.Player,
              GameMatcher.PlayerSpawnRequsted,
              GameMatcher.View,
              GameMatcher.PlayerId)
            .Added());

        protected override bool Filter(GameEntity entity) => entity.isPlayerSpawnRequsted && entity.hasView;

        protected override void Execute(List<GameEntity> players)
        {
            foreach (GameEntity player in players)
            {
                player.transform.Value.position = Vector3.up;
                player.isPlayerSpawnRequsted = false;
                player.isMovementAvailable = true;
            }
        }
    }
}