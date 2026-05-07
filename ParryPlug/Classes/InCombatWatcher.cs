using System;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Dalamud.Game.ClientState.Conditions;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace ParryPlug;

public class InCombatWatcher : IDisposable
{
    public bool inCombat {get; private set;}
    public long fightElapsedTime {get; private set;}
    Stopwatch stopwatch = new();
    public InCombatWatcher()
    {
        Console.WriteLine("Constructor: InCombatWatcher");

        inCombat = Plugin.Condition[ConditionFlag.InCombat];
        this.fightElapsedTime = stopwatch.ElapsedMilliseconds;

        Plugin.Framework.Update += this.OnFrameWorkTick;
    }
    
    public void Dispose()
    {
        Plugin.Framework.Update -= this.OnFrameWorkTick;
    }

    private void OnFrameWorkTick(IFramework framework)
    {
        var _previousCombatState = inCombat; //check what the previous combat state was
        inCombat =  Plugin.Condition[ConditionFlag.InCombat]; //update the combat state
        if (_previousCombatState != inCombat) //do actions if changed
        {
            if (inCombat == true) this.stopwatch.Restart();
            else this.stopwatch.Stop();
        }
    }
}