using Content.Server.DeviceLinking.Systems;
using Content.Shared._Wormix.CommandSend;
using Content.Shared.DeviceLinking.Events;
using Robust.Shared.Console;

namespace Content.Server._Wormix.CommandSend;
public sealed class CommandSendSystem : EntitySystem

{
    [Dependency] private readonly DeviceLinkSystem _signalSystem = default!;
    [Dependency] private readonly IConsoleHost _consoleHost = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CommandSendComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<CommandSendComponent, SignalReceivedEvent>(OnSignalReceived);
    }

    private void OnInit(EntityUid uid, CommandSendComponent component, ComponentInit args)
    {
        _signalSystem.EnsureSinkPorts(uid, component.Trigger);
    }


    private void OnSignalReceived(EntityUid uid, CommandSendComponent component, ref SignalReceivedEvent args)
    {
        if (args.Port == component.Trigger)
        {
            _consoleHost.ExecuteCommand(component.ConsoleCommand);
        }
    }

}
