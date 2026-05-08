using System;
using System.Collections.Generic;
using Dalamud.Interface.Internal.UiDebug.Browsing;
using Dalamud.Plugin.Services;
using ParryPlug;

public class EventScheduler : IDisposable
{
    uint seed;

    //TODO: quick eligible players list to test with here, needs to just work normally.
    List<uint> tempEligiblePlayers = new List<uint>{0,1};
    List<DurationFightEvent> durationFightEvents; 
    InCombatWatcher inCombatWatcher;
    public EventScheduler(uint _seed)
    {
        seed = _seed;
        inCombatWatcher = new();
        // Add events here. Im not sure about the syntax of this actually, this is a simplifcation suggested by vscode.
        durationFightEvents =
        [
            new TetherEvent(6000,10000,seed,1,tempEligiblePlayers),
            new TetherEvent(12000,14000,seed,1,tempEligiblePlayers),
        ];

        Plugin.Framework.Update += this.OnFrameWorkTick;
    }

    public void OnFrameWorkTick(IFramework framework)
    {
        foreach(DurationFightEvent e in durationFightEvents){
            if (inCombatWatcher.fightElapsedTime > e.ActivationTime && !e.isActive)
            {
                e.isActive = true;
                Plugin.Framework.Update += e.OnFrameWorkTick;
            }

            if (inCombatWatcher.fightElapsedTime > e.ResolveTime && e.isActive)
            {
                e.isActive = false;
                e.Dispose();
            }
        }
    }
    
    public void Dispose()
    {
        // get rid of any events that are still being processed at this time. 
        // events only get added to the frameworktick if they are activated so this should work.
        foreach(DurationFightEvent e in durationFightEvents)
        {
            if (e.isActive) e.Dispose();
        }
        Plugin.Framework.Update -= this.OnFrameWorkTick;
    }

}