using Unity.Netcode;
using UnityEngine;

namespace Code.Game.Features.Network.Data
{
    public class MovementHistoryData : INetworkSerializable
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

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Tick);
            serializer.SerializeValue(ref Direction);
            serializer.SerializeValue(ref Position);
        }
    }
}