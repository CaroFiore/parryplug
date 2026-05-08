using FFXIVClientStructs.FFXIV.Client.Game.Character;
using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Dalamud.Game.Player;
using System.Diagnostics.Metrics;
using System.Linq;
namespace ParryPlug;


public class TetherEvent : DurationFightEvent
{
    private uint amount;
    List<uint> eligiblePlayers;

    PartyPositionWatcher partyPositionWatcher = new();
    List<uint> chosenPairs = new();
    RandomNumberGenerator rng = new();

    Vector3? posA;
    Vector3? posB;

    public TetherEvent(uint activationTime, uint resolveTime, uint seed, uint amount, List<uint> eligiblePlayers) 
    : base(activationTime,resolveTime,seed)
    {
        this.amount = amount;
        this.eligiblePlayers = eligiblePlayers;

        //Step 1: check if the input is valid. Eligibleplayers / 2*amount can never be smaller than 1 (and amount can not be 0 thats stupid).
        if (this.amount == 0) throw new ArgumentOutOfRangeException(nameof(amount), "Tether Event: Amount must be bigger than 0");
        if (this.eligiblePlayers.Count / this.amount < 1) 
            throw new ArgumentNullException(nameof(this.eligiblePlayers), 
            message: "Tether Event: The amount of eligible players is not enough for the amount of tethers being drawn..");

        //Step 2: create random player pairs
        //Pick a random index from the eligiblePlayers list and move it from eligiblePlayers to the chosenPairs list.
        rng = new(seed, eligiblePlayers.Count); // create a number generator with the seed 
        int nextNumber;
        while (amount != 0)
        {   
            for (int i = 0; i < 2; i++) // put 2 people into the list, then reduce amount by 1
            {
                nextNumber = rng.Next();
                chosenPairs.Add(eligiblePlayers[nextNumber]);
                eligiblePlayers.Remove(eligiblePlayers[nextNumber]);
                rng.UpdateMax(eligiblePlayers.Count);    
            }
        }
        amount--;
    }

        // Step 3: we should now have a list of pairs. So index 0 and 1 are together.. 2 and 3 etc.
        // On each framework tick, lets draw every tether.


    

    public override void OnFrameWorkTick(IFramework framework)
    {
        for (int i = 0; i < chosenPairs.Count; i+=2){
            this.posA = partyPositionWatcher.partyCurrentPositions[chosenPairs[i]];
            this.posB = partyPositionWatcher.partyCurrentPositions[chosenPairs[i+1]];
        }

        DrawTether(posA, posB);
    }

    private void DrawTether(Vector3? posA, Vector3? posB)
    {
        if(posA == null || posB == null) return;
        
        var drawList = ImGui.GetBackgroundDrawList();

        if (Plugin.GameGui.WorldToScreen(posA.Value, out var screenA) |
            Plugin.GameGui.WorldToScreen(posB.Value, out var screenB))
        {
            drawList.AddLine(screenA, screenB, ImGui.ColorConvertFloat4ToU32(new Vector4(1,0,0,1)), 2f);
        }
    }
}