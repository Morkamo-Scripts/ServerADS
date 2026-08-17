using System;
using System.Collections.Generic;
using System.ComponentModel;
using PlayerRoles;

namespace ServerADS.Configs;

[Serializable]
public class AdvicesForSpectators
{
    [Description("State of module (if disabled then manual only")]
    public bool IsEnabled { get; set; } = true;
    
    [Description("Time for show message.")]
    public ushort ShowDuration { get; set; } = 5;
    
    [Description("Delay between showing messages.")]
    public ushort MessageDelay { get; set; } = 10;
    
    [Description("Message vertical position on screen")]
    public ushort VerticalMessagePosition { get; set; } = 80;
    
    public List<string> Messages { get; set; } =
    [
        "SCP-173 is very fast, because is dangerous.",
        "SCP-500 is the best medical item.",
        "SCP-1344 is the only legal wallhack item."
    ];
    
    [Description("Roles that don't see these messages.")]
    public List<RoleTypeId> ExcludedRoles { get; set; } = new()
    {
        RoleTypeId.Filmmaker
    };
}