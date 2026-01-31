using Code.Game.Features.Player.Service;
using Code.Infrastructure.View.Registrars;
using UnityEngine;

namespace Code.Game.Features.Player.Registrars
{
    public class PlayerLocomotionRegistrar : EntityComponentRegistrar
    {
        [SerializeField] private PlayerLocomotionService _locomotionService;

        public override void RegisterComponents()
        {
            Entity.AddPlayerLocomotion(_locomotionService);
        }

        public override void UnregisterComponents()
        {
            if (Entity.hasPlayerLocomotion)
                Entity.RemovePlayerLocomotion();
        }
    }
}