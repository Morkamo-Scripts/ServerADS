using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using LabApi.Loader;
using RemoteAdmin;
using ServerADS.Configs;

namespace ServerADS.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
public sealed class ServerAdsCommand : ParentCommand
{
    public override string Command { get; } = "serverads";
    public override string[] Aliases { get; } = Array.Empty<string>();
    public override string Description { get; } = "ServerADS utility commands.";

    public ServerAdsCommand()
    {
        LoadGeneratedCommands();
    }

    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new DynamicReloadCommand());
        RegisterCommand(new SpecAdviceCommand());
    }
    
    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        response = "Available subcommands: reload, specAdvice";
        return false;
    }
}