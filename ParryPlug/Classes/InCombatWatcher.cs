using System;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Dalamud.Game.ClientState.Conditions;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace ParryPlug;

public class InCombatWatcher : IDisposable
{
    public bool inCombatApp {get; private set;}
    private bool inCombatGame;
    public long fightElapsedTime {get; private set;}
    Stopwatch stopwatch = new();
    public InCombatWatcher()
    {
        Plugin.Log.Information("Constructor: InCombatWatcher");

        
        inCombatApp = false;
        inCombatGame = inCombatGame =  Plugin.Condition[ConditionFlag.InCombat];


        Plugin.Framework.Update += this.OnFrameWorkTick;
    }
    
    public void Dispose()
    {
        Plugin.Framework.Update -= this.OnFrameWorkTick;
    }

    private void OnFrameWorkTick(IFramework framework)
    {
        inCombatGame =  Plugin.Condition[ConditionFlag.InCombat]; //update the combat state
        if (inCombatApp == false && inCombatGame == true)
        {
            stopwatch.Start();
            inCombatApp = true;
        }
        if (inCombatApp == true && inCombatGame == false)
        {
            stopwatch.Reset();
        }
        
        if (inCombatApp == true) this.fightElapsedTime = stopwatch.ElapsedMilliseconds;
    }    
}