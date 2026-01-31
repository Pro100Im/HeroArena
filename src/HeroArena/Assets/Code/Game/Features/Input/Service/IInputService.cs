using UnityEngine;

namespace Code.Game.Input.Service
{
    public interface IInputService
    {
        void EnableInput();
        void DisableInput();

        float GetVerticalAxis();
        float GetHorizontalAxis();

        bool HasAxisInput();
        bool GetLeftMouseButtonDown();
        bool GetLeftMouseButtonUp();

        Vector2 GetScreenMousePosition();
        Vector2 GetWorldMousePosition();
    }
}