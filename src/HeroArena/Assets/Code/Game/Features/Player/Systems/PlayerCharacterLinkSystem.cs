using Code.Infrastructure.View;
using Entitas;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCharacterLinkSystem : ReactiveSystem<GameEntity>
{
    public PlayerCharacterLinkSystem(GameContext game) : base(game)
    {

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
      context.CreateCollector(GameMatcher
        .AllOf(
          GameMatcher.Player,
          GameMatcher.PlayerId)
        .NoneOf(
          GameMatcher.View)
        .Added());

    protected override bool Filter(GameEntity entity) => entity.isPlayerSpawnRequsted && entity.hasView;

    protected override void Execute(List<GameEntity> players)
    {
        foreach (GameEntity entity in players)
        {
            var netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[entity.playerId.Value]; 
            var view = netObj.GetComponent<EntityBehaviour>(); 

            view.SetEntity(entity);
        }
    }
}
