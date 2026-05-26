using Content.Server.Administration;
using Content.Shared._Wormix.MessageOverlay;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Player;


namespace Content.Server._Wormix.MessageOverlay;

[AdminCommand(AdminFlags.Fun)]
public sealed class MessageOverlayCommands : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly Robust.Server.Player.IPlayerManager _playerManager = default!;

    public string Command => "overlaymessage";
    public string Description => "Большой-текст Маленький-текст (Желательно пишите в кавычках)";
    public string Help => "ZALUPUS";
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        NewOverlayMessage newMessage = new NewOverlayMessage(args[0], args[1]);
        var filter = Filter.Empty().AddAllPlayers(_playerManager);


        _entityManager.System<MessageOverlaySystem>().CallMessage(newMessage, filter);
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return CompletionResult.Empty;
    }
}


