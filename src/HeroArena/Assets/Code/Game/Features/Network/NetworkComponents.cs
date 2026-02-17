using Entitas;

namespace Code.Game.Features.Network
{
    [Network] public class ClientId : IComponent { public int Value; }
    [Network] public class OwnerClientId : IComponent { public int Value; }
    [Network] public class EntityId : IComponent { public int Value; }
    [Network] public class EntityRequestType : IComponent { public RequestTypes Value; }
    [Network] public class SendIntValue : IComponent { public int Value; }
    [Network] public class ReceiveIntValue : IComponent { public int Value; }
    [Network] public class ComponentContext : IComponent { public ComponentContexts Value; }
    [Network] public class ComponentId : IComponent { public int Value; }
    [Network] public class ComponentTypeName : IComponent { public string Value; }

    public enum RequestTypes
    {
        None,
        Add,
        Replace,
        Remove
    }

    public enum ComponentContexts
    {
        Game,
        Input,
        Meta,
        Network
    }
}