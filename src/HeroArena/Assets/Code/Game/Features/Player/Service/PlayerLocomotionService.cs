using Code.Game.Input.Service;
using UnityEngine;
using VContainer;

namespace Code.Game.Features.Player.Service
{
    public class PlayerLocomotionService : MonoBehaviour
    {
        private Transform _cameraObject;
        private Vector3 _moveDirection;
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Start()
        {
            
        }
    }
}