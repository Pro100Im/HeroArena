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
        private NewInputSystemApi _newInputSystemApi;

        public InputService()
        {
            _newInputSystemApi = new NewInputSystemApi();
            _mainCamera = Camera.main;
        }

        public Vector2 GetPointer() => _newInputSystemApi.Player.Point.ReadValue<Vector2>();

        public Vector2 GetWorldPointer()
        {
            if(_mainCamera == null || Mouse.current == null)
                return Vector2.zero;

            return _mainCamera.ScreenToWorldPoint(GetPointer());
        }

        public Ray GetRayWorldPointer()
        {
            if (_mainCamera == null || Mouse.current == null)
                return new Ray();

            return _mainCamera.ScreenPointToRay(GetPointer());
        }

        public void EnableInput() => _newInputSystemApi.Player.Enable();
        public void DisableInput() => _newInputSystemApi.Player.Disable();

        public bool HasAxisInput() => GetInputAxis().magnitude > 0;

        public float GetVerticalAxis() => GetInputAxis().y;

        public float GetHorizontalAxis() => GetInputAxis().x;

        //public bool GetLeftMouseButton() => Mouse.current?.leftButton.isPressed == true && !IsPointerOverUI();

        //public bool GetLeftMouseButtonDown() => Mouse.current?.leftButton.wasPressedThisFrame == true && !IsPointerOverUI();

        //public bool GetLeftMouseButtonUp() => Mouse.current?.leftButton.wasReleasedThisFrame == true && !IsPointerOverUI();

        private Vector2 GetInputAxis() => _newInputSystemApi.Player.Move.ReadValue<Vector2>();

        //private bool IsPointerOverUI() => EventSystem.current == null ? false : EventSystem.current.IsPointerOverGameObject();
    }
}