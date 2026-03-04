using UnityEngine;

namespace Code.Game.Features.Network.Data
{
    public class MovementHistoryData
    {
        public int Tick;
        public Vector2 Direction;
        public Vector3 Position;

        public MovementHistoryData(int tick, Vector2 dir, Vector3 pos)
        {
            Tick = tick;
            Direction = dir;
            Position = pos;
        }
    }
}