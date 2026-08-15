using LabApi.Events.Arguments.PlayerEvents;
using ServerADS.Configs;

namespace ServerADS.Handlers;

public class WmHandler
{
    private WelcomeMessages Config => Core.Instance.WmConfig;
    
    public void OnPlayerJoined(PlayerJoinedEventArgs ev)
    {
        if (!Config.IsEnabled)
            return;
        
        ev.Player.SendBroadcast(Config.Messages.RandomItem()
            .Replace("%player%", ev.Player.Nickname),
            Config.ShowDuration);
    }
}