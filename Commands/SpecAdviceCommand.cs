using System;
using System.Linq;
using CommandSystem;
using LabApi.Features.Enums;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using LabApi.Loader;
using MEC;
using PlayerRoles;
using RemoteAdmin;
using RueI.API;
using RueI.API.Elements;
using ServerADS.Configs;

namespace ServerADS.Commands;

public class SpecAdviceCommand : ICommand
{
    public string Command { get; } = "specAdvice";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Force spawn new advice for spectators.";
    
    AdvicesForSpectators Config => Core.Instance.AfsConfig;
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        // Check player permissions
        if (sender is PlayerCommandSender && !Player.Get(sender)!.HasPermission("serverads.specAdvice"))
        {
            response = "You don't have permission for execute this command.";
            return false;
        }
        
        var players = Player.GetAll(PlayerSearchFlags.AuthenticatedPlayers)
            .Where(pl => pl.Role == RoleTypeId.Spectator).ToList();

        if (!players.Any())
        {
            response = "Not enough players.";
            return false;
        }

        foreach (var player in players)
        {
            var display = RueDisplay.Get(player);
            
            display.Show(
                new Tag(),
                new BasicElement(Config.VerticalMessagePosition, Config.Messages.RandomItem()),
                Config.ShowDuration);
            
            Timing.CallDelayed(Config.MessageDelay + 0.2f, () => display.Update());
        }
        
        response = $"Advice has been spawned for {players.Count()} players.";
        return true;
    }
}