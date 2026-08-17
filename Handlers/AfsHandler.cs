using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using RueI.API;
using RueI.API.Elements;
using ServerADS.Configs;
using Logger = LabApi.Features.Console.Logger;

namespace ServerADS.Handlers;

public class AfsHandler
{
    private AdvicesForSpectators Config => Core.Instance.AfsConfig;
    public CoroutineHandle? AdviceGeneratorCoroutine;

    public IEnumerator<float> AdviceGenerator()
    {
        while (Round.IsRoundStarted && !Round.IsRoundEnded)
        {
            yield return Timing.WaitForSeconds(Config.MessageDelay);

            var players = Player.GetAll(PlayerSearchFlags.AuthenticatedPlayers)
                .Where(pl => pl.Role == RoleTypeId.Spectator).ToList();

            if (!players.Any())
            {
                continue;
            }

            foreach (var player in players.Where(pl => !Core.Instance.AfsConfig.ExcludedRoles.Contains(pl.Role)))
            {
                var display = RueDisplay.Get(player);

                display.Show(
                    new Tag(),
                    new BasicElement(Config.VerticalMessagePosition, Config.Messages.RandomItem()),
                    Config.ShowDuration);

                Timing.CallDelayed(Config.MessageDelay + 0.2f, () => display.Update());
            }
        }
        AdviceGeneratorCoroutine = null;
    }

    public void OnRoundStarted()
    {
        if (!Config.IsEnabled)
            return;
        
        AdviceGeneratorCoroutine = Timing.RunCoroutine(AdviceGenerator());
    }

    public void OnWaitingForPlayers()
    {
        if (AdviceGeneratorCoroutine != null)
        {
            Timing.KillCoroutines((CoroutineHandle)AdviceGeneratorCoroutine);
            AdviceGeneratorCoroutine = null;
        }
    }
}