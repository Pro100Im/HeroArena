using Settings.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Code.Game.Input.Service
{
    // To do rework
    public class InputService : IInputService
    {
        private Camera _mainCamera;
        private Vector3 _screenPosition;
        private NewInputSystemApi _newInputSystemApi;

        public InputService()
        {
            _newInputSystemApi = new NewInputSystemApi();
            _mainCamera = Camera.main;
        }

        public Vector2 GetScreenMousePosition() =>
            _mainCamera ? Mouse.current?.position.ReadValue() ?? Vector2.zero : Vector2.zero;

        public Vector2 GetWorldMousePosition()
        {
            if(_mainCamera == null || Mouse.current == null)
                return Vector2.zero;

            _screenPosition = Mouse.current.position.ReadValue();

            return _mainCamera.ScreenToWorldPoint(_screenPosition);
        }

        public void EnableInput() => _newInputSystemApi.Player.Enable();
        public void DisableInput() => _newInputSystemApi.Player.Disable();

        public bool HasAxisInput() => GetInputAxis().magnitude > 0;

        public float GetVerticalAxis() => GetInputAxis().y;

        public float GetHorizontalAxis() => GetInputAxis().x;


        public bool GetLeftMouseButton() => Mouse.current?.leftButton.isPressed == true && !IsPointerOverUI();

        public bool GetLeftMouseButtonDown() => Mouse.current?.leftButton.wasPressedThisFrame == true && !IsPointerOverUI();

        public bool GetLeftMouseButtonUp() => Mouse.current?.leftButton.wasReleasedThisFrame == true && !IsPointerOverUI();


        private Vector2 GetInputAxis() => _newInputSystemApi.Player.Move.ReadValue<Vector2>();

        private bool IsPointerOverUI() => EventSystem.current == null ? false : EventSystem.current.IsPointerOverGameObject();
    }
}