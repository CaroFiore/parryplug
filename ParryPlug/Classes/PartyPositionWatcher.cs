using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Arrays;
using Lumina.Excel.Sheets;

namespace ParryPlug;

public class PartyPositionWatcher : IDisposable
{

    public Vector3?[] partyCurrentPositions {get; private set;} = new Vector3?[8];

    public PartyPositionWatcher()
    {
        Plugin.Log.Information("Constructor: PartyPositionWatcher");

        // Fill party healths with Null, so later we can skip printing player healths if they dont exist.
        for (int i = 0; i < partyCurrentPositions.Length; i++) {partyCurrentPositions[i] = null;}

        Plugin.Framework.Update += this.OnFrameWorkTick;
    }
    
    public void Dispose()
    {
        Plugin.Framework.Update -= this.OnFrameWorkTick;
    }

    private void OnFrameWorkTick(IFramework framework)
    {
        var partyList = Plugin.PartyList;
        var i = 0;

        foreach (var player in partyList)
        {
            partyCurrentPositions[i] = player.Position;
        }
    }
}