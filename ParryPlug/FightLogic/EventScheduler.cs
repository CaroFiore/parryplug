using System;
using System.Collections.Generic;
using Dalamud.Interface.Internal.UiDebug.Browsing;
using Dalamud.Plugin.Services;
using ParryPlug;

public class EventScheduler : IDisposable
{
    uint seed;

    //TODO: quick eligible players list to test with here, needs to just work normally.
    List<uint> tempEligiblePlayers = new List<uint>{0,1,2,3};
    List<DurationFightEvent> durationFightEvents; 
    InCombatWatcher inCombatWatcher;
    public RandomNumberGenerator mainRNG;

    public EventScheduler(uint _seed)
    {
        seed = _seed;
        inCombatWatcher = new();
        mainRNG = new(seed);
        // Add events here. Im not sure about the syntax of this actually, this is a simplifcation suggested by vscode.
        durationFightEvents =
        [
            new TetherEvent(1000,4000,2,tempEligiblePlayers, mainRNG),
            new TetherEvent(5000,8000,2,tempEligiblePlayers, mainRNG),
        ];

        Plugin.Framework.Update += this.OnFrameWorkTick;
    }

    public void OnFrameWorkTick(IFramework framework)
    {
        foreach(DurationFightEvent e in durationFightEvents){
            //Plugin.Log.Information($"Is {inCombatWatcher.fightElapsedTime} bigger than {e.ActivationTime} and is e.isActive? {e.isActive}");
            if (inCombatWatcher.fightElapsedTime > e.ActivationTime && !e.isActive && !e.isDisposed)
            {
                e.isActive = true;
                Plugin.Log.Information($"New event activated: {e}");
                Plugin.Framework.Update += e.OnFrameWorkTick;
                Plugin.PluginInterface.UiBuilder.Draw += e.OnDraw;
            }

            if (inCombatWatcher.fightElapsedTime > e.ResolveTime && e.isActive && !e.isDisposed)
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