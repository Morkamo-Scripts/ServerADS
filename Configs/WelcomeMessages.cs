using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ServerADS.Configs;

[Serializable]
public class WelcomeMessages
{
    [Description("State of module")]
    public bool IsEnabled { get; set; } = true;
    
    [Description("Time for show welcome message.")]
    public ushort ShowDuration { get; set; } = 10;
    
    [Description("%player% - nickname of joined player.")]
    public List<string> Messages { get; set; } =
    [
        "Welcome to MagicSCPSL! Read rules and start game!",
        "Welcome, %player%! Read rules and have fun",
        "Welcome back, %player%!"
    ];
}