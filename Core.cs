using System;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using MEC;
using RueI.API;
using RueI.API.Elements;
using ServerADS.Configs;
using ServerADS.Handlers;
using UnityEngine;
using api = LabApi.Events.Handlers;
using Logger = LabApi.Features.Console.Logger;

namespace ServerADS
{
    public class Core : Plugin
    {
        public override string Name { get; } = "ServerADS";
        public override string Description { get; } = "ADS overlays on screens and more";
        public override string Author { get; } = "Morkamo";
        public override Version Version { get; } = new(1, 0, 0);
        public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);
        
        public static Core Instance { get; private set; }
        
        public WelcomeMessages WmConfig { get; set; }
        public AdvicesForSpectators AfsConfig { get; set; }

        public WmHandler WmHandler { get; private set; }
        public AfsHandler AfsHandler { get; private set; }
        
        public override void Enable()
        {
            Instance = this;
            
            WmConfig = this.LoadConfig<WelcomeMessages>("WelcomeMessages");
            AfsConfig = this.LoadConfig<AdvicesForSpectators>("AdvicesForSpectators");

            WmHandler = new WmHandler();
            AfsHandler = new AfsHandler();
            
            SubscribeEvents();
        }

        public override void Disable()
        {
            UnsubscribeEvents();
            
            WmConfig = null;
            AfsConfig = null;
            
            AfsHandler = null;
            WmHandler = null;
            
            Instance = null;
        }

        private void SubscribeEvents()
        {
            api.PlayerEvents.Joined += WmHandler.OnPlayerJoined;
            api.ServerEvents.RoundStarted += AfsHandler.OnRoundStarted;
            api.ServerEvents.WaitingForPlayers += AfsHandler.OnWaitingForPlayers;
        }
        
        private void UnsubscribeEvents()
        {
            api.PlayerEvents.Joined -= WmHandler.OnPlayerJoined;
            api.ServerEvents.RoundStarted -= AfsHandler.OnRoundStarted;
            api.ServerEvents.WaitingForPlayers -= AfsHandler.OnWaitingForPlayers;
        }
    }
}