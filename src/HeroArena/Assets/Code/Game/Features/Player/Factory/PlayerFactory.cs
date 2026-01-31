using Code.Common.Entity;
using Code.Infrastructure.Identifiers;
using UnityEngine;

namespace Code.Game.Features.Player.Factory
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IIdentifierService _identifiers;

        public PlayerFactory(IIdentifierService identifiers)
        {
            _identifiers = identifiers;
        }

        public GameEntity CreatePlayer(Vector3 at)
        {
            var entity = CreateEntity.Empty();
            entity.AddId(_identifiers.Next());
            entity.AddDirection(Vector2.zero);
            entity.AddSpeed(1f);
            entity.AddMaxSpeed(3f);
            entity.AddCurrentSpeed(0f);
            entity.AddViewPath("Game/Player/Player");
            entity.isPlayerSpawnRequsted = true;
            entity.isPlayer = true;
            entity.isRotationAlignedAlongDirection = true;

            return entity;
        }
    }
}
