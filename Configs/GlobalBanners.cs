using System;
using System.Collections.Generic;
using System.ComponentModel;
using PlayerRoles;

namespace ServerADS.Configs;

[Serializable]
public class GlobalBanners
{
    [Description("State of module")]
    public bool IsEnabled { get; set; } = true;
    
    [Description("Whether to show hints in lobby")]
    public bool ShowInLobby { get; set; } = true;
    
    [Description("Roles that don't see these messages.")]
    public List<RoleTypeId> ExcludedRoles { get; set; } = new()
    {
        RoleTypeId.Filmmaker
    };
    
    [Description("Placeholders:" +
                 "\n# %player% - player nickname." +
                 "\n# %roundtime% - round duration.")]
    public List<Banner> Banners { get; set; } = 
    [
        new Banner("<size=80%>FIRE-FOX [CLASSIC]</size>", 10f, 20),
        new Banner("<size=60%>Round time: %roundtime%</size>", 1f, 950),
    ];
}

public class Banner
{
    public string Message { get; set; }
    public float UpdateFrequency { get; set; }
    public ushort VerticalPosition { get; set; }

    public Banner()
    {
        
    }
    
    public Banner(string message, float updateFrequency, ushort verticalPosition)
    {
        Message = message;
        UpdateFrequency = updateFrequency;
        VerticalPosition = verticalPosition;
    }
}