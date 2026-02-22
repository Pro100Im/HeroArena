using Code.Infrastructure.View.Registrars;

namespace Code.Game.Features.Spawn.Registrars
{
    public class PlayerSpawnPointRegistrar : EntityComponentRegistrar
    {
        public override void RegisterComponents()
        { 
            Entity.AddSpawnPosition(transform.position);
            Entity.isForPlayer = true;
            Entity.isFreePoint = true;
        }

        public override void UnregisterComponents()
        {
            Entity.RemoveAllComponents();
        }
    }
}