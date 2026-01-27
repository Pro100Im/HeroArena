using Code.Infrastructure.View.Registrars;
using UnityEngine;

namespace Code.Game.Common.Registrars
{
    public class CharacterControllerRegistrar : EntityComponentRegistrar
    {
        [SerializeField] private CharacterController _characterController;

        public override void RegisterComponents()
        {
            Entity.AddCharacterController(_characterController);
        }

        public override void UnregisterComponents()
        {
            if (Entity.hasCharacterController)
                Entity.RemoveCharacterController();
        }
    }
}