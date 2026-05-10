using System;
using System.Collections.Generic;
using Dalamud.Interface.Internal.UiDebug.Browsing;
using Dalamud.Plugin.Services;
using ParryPlug;

public class Nexus : IDisposable
{
    public readonly HealthWatcher healthWatcher = new();
    public readonly InCombatWatcher inCombatWatcher = new();
    public readonly PartyInfo partyInfo;
    public readonly RandomNumberGenerator rng;
    public readonly PlayerPicker playerPicker;
    public Nexus()
    {
        partyInfo = new PartyInfo();
        rng = new RandomNumberGenerator(0);
        playerPicker = new PlayerPicker(partyInfo, rng);
    }

    public void OnFrameWorkTick(IFramework framework)
    {
       

    }
    
    public void Dispose()
    {
        healthWatcher.Dispose();
        inCombatWatcher.Dispose();
    }
}