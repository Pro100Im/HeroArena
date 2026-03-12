using Code.Game.Features.Network.Data;
using Entitas;
using UnityEngine;

namespace Code.Game.Features.Network
{
    [Network][Input][Game] public class ClientId : IComponent { public ulong Value; }
    [Network][Game] public class ObjectId : IComponent { public ulong Value; }
    [Network][Input] public class LocalPlayer : IComponent { }

    [Network] public class TickRate : IComponent { public int Value; }
    [Network] public class CurrentTick : IComponent { public int Value; }
    [Network] public class TickTime : IComponent { public float Value; }
    [Network] public class TimeComponent : IComponent { public float Value; }
    [Network][Game] public class HistoryBufferSize : IComponent { public int Value; }
    [Network][Game] public class MovementHistory : IComponent { public MovementHistoryData[] Value; }
  
    public enum RequestTypes
    {
        None,
        CreatePlayerEntity,
        ReceiveObjectId,
        ReceiveSpawnPosition,
        MovementHistory,
        RollBackMove
    }
}