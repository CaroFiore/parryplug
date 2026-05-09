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
using Lumina.Excel.Sheets;
using ParryPlug.Windows;
using System.Runtime.CompilerServices;
namespace ParryPlug;


public class TetherEvent : DurationFightEvent
{
    private uint amount;
    List<uint> eligiblePlayers;

    PartyPositionWatcher partyPositionWatcher = new();
    List<uint> chosenPairs = new();

    Vector3? posA;
    Vector3? posB;

    RandomNumberGenerator mainRNG;
    private float tetherThickness = 1f;
    private float sineX = 0;

    public TetherEvent(uint activationTime, uint resolveTime, uint amount, List<uint> _eligiblePlayers, RandomNumberGenerator _mainRNG) 
    : base(activationTime,resolveTime)
    {
        mainRNG = _mainRNG;
        this.amount = amount;
        this.eligiblePlayers = new List<uint>(_eligiblePlayers);
        Plugin.Log.Information($"in tetherevent constructor: eligiblePLayers.Count: {eligiblePlayers.Count}");
        

        //Step 1: check if the input is valid. Eligibleplayers / 2*amount can never be smaller than 1 (and amount can not be 0 thats stupid).
        // If it IS smaller, that means theres not enough eligible players for the amount of tethers. We dont like that.
        Plugin.Log.Information($"amount: {this.amount} and eligibleplayers: {eligiblePlayers.Count}");
        
        if (this.amount == 0) throw new ArgumentOutOfRangeException(nameof(amount), "Tether Event: Amount must be bigger than 0");
        if (this.eligiblePlayers.Count / this.amount < 1) 
            throw new ArgumentNullException(nameof(this.eligiblePlayers), 
            message: "Tether Event: The amount of eligible players is not enough for the amount of tethers being drawn..");
        if (this.eligiblePlayers.Any(p => p > 7))
            throw new ArgumentOutOfRangeException(nameof(eligiblePlayers), 
            message: "Tether Event: Eligible player indices must be valid party slots (0-7)");

        //Step 2: create random player pairs
        //Pick a random index from the eligiblePlayers list and move it from eligiblePlayers to the chosenPairs list.
        int nextNumber;
        while (amount != 0)
        {   
            for (int i = 0; i < 2; i++) // put 2 people into the list, then reduce amount by 1
            {
                Plugin.Log.Information($"eligiblePLayers.Count: {eligiblePlayers.Count}");
                nextNumber = mainRNG.Next((uint)eligiblePlayers.Count);
                chosenPairs.Add(eligiblePlayers[nextNumber]);
                eligiblePlayers.RemoveAt(nextNumber);
            }
            amount--;
        }
    }

        // Step 3: we should now have a list of pairs. So index 0 and 1 are together.. 2 and 3 etc.
        // On each framework tick, lets draw every tether.


    

    public override void OnFrameWorkTick(IFramework framework)
    {
    }

    public override void OnDraw()
    {
        for (int i = 0; i < chosenPairs.Count; i+=2){
            this.posA = partyPositionWatcher.partyCurrentPositions[chosenPairs[i+0]];
            this.posB = partyPositionWatcher.partyCurrentPositions[chosenPairs[i+1]];
           

            Vector3 adjustedPosA;
            Vector3 adjustedPosB;
            if (this.posA != null && this.posB != null)
            {
                adjustedPosA = posA.Value;
                adjustedPosA.Y += 0.7f;
                adjustedPosB = posB.Value;
                adjustedPosB.Y += 0.7f;

                tetherThickness = ComputeSine();
                Plugin.Log.Information($"tetherThickness: {tetherThickness}:");
            
                this.DrawTether(adjustedPosA, adjustedPosB, tetherThickness);
            }
        }
        //Plugin.Log.Information($"Attempting to draw {posA} and {posB}");
    }

    private void DrawTether(Vector3 posA, Vector3 posB, float thickness = 1.0f)
    {
        var drawList = ImGui.GetBackgroundDrawList();

        if (Plugin.GameGui.WorldToScreen(posA, out var screenA) |
            Plugin.GameGui.WorldToScreen(posB, out var screenB))
        {
            drawList.AddLine(screenA, screenB, ImGui.ColorConvertFloat4ToU32(new Vector4(0,1,0,0.8f)), thickness);
        }
    }

    private float ComputeSine()
    {
        sineX += 0.05f;
        return (MathF.Sin(sineX) + 3f) * 4f;
    }
}