using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;

namespace Content.Shared._Wormix.CommandSend;

[RegisterComponent]
public sealed partial class CommandSendComponent : Component
{

    [DataField]
    public ProtoId<SinkPortPrototype> Trigger = "Trigger";

    [DataField]
    public string ConsoleCommand = string.Empty;

}
