using System;
using System.Collections.Generic;
using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using RueI.API;
using RueI.API.Elements;
using ServerADS.Configs;

namespace ServerADS.Handlers;

public class GbHandler
{
    public static Dictionary<Tag, Player> BannerList { get; private set; } = new();

    public static void LoadBanners(Player player)
    {
        if (Core.Instance.GlobalBanners.ExcludedRoles.Contains(player.Role))
            return;
        
        foreach (var banner in Core.Instance.GlobalBanners.Banners)
        {
            var dynamicElement = new DynamicElement(
                banner.VerticalPosition,
                () =>
                {
                    string bannerContent = banner.Message;
                    // Replace roundTime placeholder to current round duration.
                    if (bannerContent.Contains("%roundtime%"))
                    {
                        var duration = Round.Duration;

                        string time = duration.TotalHours >= 1
                            ? $"{(int)duration.TotalHours:00}:{duration.Minutes:00}:{duration.Seconds:00}"
                            : $"{duration.Minutes:00}:{duration.Seconds:00}";
                        
                        bannerContent = bannerContent.Replace("%roundtime%", time);
                    }

                    // Replace player placeholder to player nickname.
                    if (bannerContent.Contains("%player%"))
                    {
                        bannerContent = bannerContent.Replace("%player%", player.Nickname);
                    }

                    return bannerContent;
                })
            {
                UpdateInterval = TimeSpan.FromSeconds(banner.UpdateFrequency),
                ResolutionBasedAlign = true
            };

            var tag = new Tag(Guid.NewGuid().ToString("N")[..12]);
            RueDisplay.Get(player).Show(tag, dynamicElement);
            BannerList.Add(tag, player);
        }
    }

    public static void LoadBanners(List<Player> players)
    {
        foreach (var player in players)
            LoadBanners(player);
    }
    
    public static void UnloadBanners(Player player)
    {
        foreach (var banner in BannerList.Where(banner => banner.Value == player))
        {
            var display = RueDisplay.Get(banner.Value);
            display.Remove(banner.Key);
            display.Update();
        }
        BannerList.Clear();
    }
    
    public static void UnloadBanners()
    {
        foreach (var banner in BannerList)
        {
            var display = RueDisplay.Get(banner.Value);
            display.Remove(banner.Key);
            display.Update();
        }
        BannerList.Clear();
    }

    public void OnPlayerJoined(PlayerJoinedEventArgs ev)
    {
        if (!Core.Instance.GlobalBanners.ShowInLobby)
            return;
        
        LoadBanners(ev.Player);
    }

    public void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        var banners = Core.Instance.GlobalBanners;
        if (!banners.IsEnabled || banners.Banners.IsEmpty())
            return;
        
        UnloadBanners(ev.Player);
    }

    public void OnRoleChanged(PlayerChangedRoleEventArgs ev)
    {
        var banners = Core.Instance.GlobalBanners;
        
        if (!banners.IsEnabled || banners.Banners.IsEmpty())
            return;
        
        UnloadBanners(ev.Player);
        
        if (!banners.ExcludedRoles.Contains(ev.NewRole.RoleTypeId))
            LoadBanners(ev.Player);
    }
}