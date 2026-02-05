using Entitas;

namespace Code.Game.Features.Network
{
    [Network] public class ClientId : IComponent { public int Value; }
    [Network] public class OwnerClientId : IComponent { public int Value; }
    [Network] public class EntityRequestType : IComponent { public RequestType Value; }
    [Network] public class EntitySend : IComponent { public object Value; }
    [Network] public class EntityReceive : IComponent { public object Value; }

    public enum RequestType
    {
        None,
        Add,
        Replace,
        Remove
    }
}