using Code.Common.Network;
using Code.Game.Features.Network;
using Entitas;
using System.Collections.Generic;
using Unity.Netcode;

namespace Code.Game.Features.Player.Systems
{
    public class SelectPlayerSpawnPositionSystem : ReactiveSystem<GameEntity>
    {
        public SelectPlayerSpawnPositionSystem(GameContext game) : base(game)
        {

        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
          context.CreateCollector(GameMatcher
            .AllOf(
              GameMatcher.Player,
              GameMatcher.ClientId,
              GameMatcher.ObjectId,
              GameMatcher.WaitingToSpawn,
              GameMatcher.View)
            .Added());

        protected override bool Filter(GameEntity entity) => entity.isPlayer && entity.isWaitingToSpawn && entity.hasView && entity.hasObjectId && entity.hasClientId;

        protected override void Execute(List<GameEntity> players)
        {
            foreach (GameEntity player in players)
            {
                //test
                var clientId = player.clientId.Value;
                var objectId = player.objectId.Value;
                var posX = clientId * 2f;
                var posY = 0.5f;
                var posZ = 0f;

                var totalSize = sizeof(float) + sizeof(float) + sizeof(float) + sizeof(ulong) + sizeof(ulong);
                using var builder = new NetworkMessageBuilder(totalSize);
                var writer = builder.Write(posX).Write(posY).Write(posZ).Write(clientId).Write(objectId).Build();

                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                    RequestTypes.ReceiveSpawnPosition.ToString(),
                    NetworkManager.Singleton.ConnectedClientsIds,
                    writer);
            }
        }
    }
}
