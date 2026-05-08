using System.Collections.Generic;
using Dalamud.Interface.Internal.UiDebug.Browsing;
using ParryPlug;

public class EventScheduler
{
    uint seed;

    //TODO: quick eligible players list to test with here, needs to just work normally.
    List<uint> tempEligiblePlayers = new List<uint>{0,1};
    List<DurationFightEvent> durationFightEvents; 
    public EventScheduler(uint _seed)
    {
        seed = _seed;

        // Add events here. Im not sure about the syntax of this actually, this is a simplifcation suggested by vscode.
        durationFightEvents =
        [
            new TetherEvent(6000,10000,seed,1,tempEligiblePlayers),
            new TetherEvent(12000,14000,seed,1,tempEligiblePlayers),
        ];
    }

}