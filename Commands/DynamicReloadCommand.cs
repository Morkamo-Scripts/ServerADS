using System;
using System.Linq;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using LabApi.Loader;
using MEC;
using RemoteAdmin;
using RueI.API;
using ServerADS.Configs;
using ServerADS.Handlers;

namespace ServerADS.Commands;

public class DynamicReloadCommand : ICommand
{
    public string Command { get; } = "reload";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Dynamic reload plugin configs.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        // Check player permissions
        if (sender is PlayerCommandSender && !Player.Get(sender)!.HasPermission("serverads.reload"))
        {
            response = "You don't have permission for execute this command.";
            return false;
        }

        // Check current state of the WelcomeMessage config
        if (!Core.Instance.TryLoadConfig<WelcomeMessages>("WelcomeMessages", out var wmConfig))
        {
            response = "WelcomeMessages could not be loaded because has errors.";
            return false;
        }
        
        // Check current state of the WelcomeMessage config
        if (!Core.Instance.TryLoadConfig<AdvicesForSpectators>("AdvicesForSpectators", out var afsConfig))
        {
            response = "AdvicesForSpectators could not be loaded because has errors.";
            return false;
        }
        
        // Check current state of the GlobalBanners config
        if (!Core.Instance.TryLoadConfig<GlobalBanners>("GlobalBanners", out var gbConfig))
        {
            response = "GlobalBanners could not be loaded because has errors.";
            return false;
        }
        
        Core.Instance.WmConfig = wmConfig;
        Core.Instance.AfsConfig = afsConfig;
        Core.Instance.GlobalBanners = gbConfig;

        #region afsHandler
        
            var afsHandler = Core.Instance.AfsHandler;
            
            if (afsHandler.AdviceGeneratorCoroutine != null)
            {
                Timing.KillCoroutines((CoroutineHandle)afsHandler.AdviceGeneratorCoroutine);
                afsHandler.AdviceGeneratorCoroutine = null;
            }
            
            if (afsConfig.IsEnabled)
            {
                if (Round.IsRoundStarted && !Round.IsRoundEnded)
                {
                    afsHandler.AdviceGeneratorCoroutine = Timing.RunCoroutine(afsHandler.AdviceGenerator());
                }
            }
        
        #endregion

        #region gbHandler

            GbHandler.UnloadBanners();
            
            if (Core.Instance.GlobalBanners.IsEnabled)
                GbHandler.LoadBanners(Player.ReadyList.Where(pl => pl.IsPlayer).ToList());

        #endregion
        
        response = "Plugin seccessfully reloaded.";
        return true;
    }
}