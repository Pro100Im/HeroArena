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

        public Camera CameraMain
        {
            get
            {
                if(_mainCamera == null && Camera.main != null)
                    _mainCamera = Camera.main;

                return _mainCamera;
            }
        }

        public Vector2 GetScreenMousePosition() =>
            CameraMain ? Mouse.current?.position.ReadValue() ?? Vector2.zero : Vector2.zero;

        public Vector2 GetWorldMousePosition()
        {
            if(CameraMain == null || Mouse.current == null)
                return Vector2.zero;

            _screenPosition = Mouse.current.position.ReadValue();
            return CameraMain.ScreenToWorldPoint(_screenPosition);
        }

        public bool HasAxisInput() => GetHorizontalAxis() != 0 || GetVerticalAxis() != 0;

        public float GetVerticalAxis()
        {
            var keyboard = Keyboard.current;

            if(keyboard == null) return 0f;

            var value = 0f;

            if(keyboard.wKey.isPressed) 
                value += 1f;

            if(keyboard.sKey.isPressed) 
                value -= 1f;

            return value;
        }

        public float GetHorizontalAxis()
        {
            var keyboard = Keyboard.current;

            if(keyboard == null) return 0f;

            float value = 0f;

            if(keyboard.dKey.isPressed) 
                value += 1f;

            if(keyboard.aKey.isPressed) 
                value -= 1f;

            return value;
        }

        public bool GetLeftMouseButton() =>
            Mouse.current?.leftButton.isPressed == true && !IsPointerOverUI();

        public bool GetLeftMouseButtonDown() =>
            Mouse.current?.leftButton.wasPressedThisFrame == true && !IsPointerOverUI();

        public bool GetLeftMouseButtonUp() =>
            Mouse.current?.leftButton.wasReleasedThisFrame == true && !IsPointerOverUI();

        private bool IsPointerOverUI()
        {
            if(EventSystem.current == null) return false;
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}