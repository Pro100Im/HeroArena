using Entitas;
using UnityEngine;

namespace Code.Game.Input
{
  [Input] public class Input : IComponent { }
  [Input] public class AxisInput : IComponent { public Vector2 Value; }
}