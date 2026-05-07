using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Arrays;
using Lumina.Excel.Sheets;

namespace ParryPlug;

public class PartyNameAndJobWatcher : IDisposable
{

    public String?[] partyNamesAndJobs {get; private set;} = new String?[8];

    public PartyNameAndJobWatcher()
    {
        Console.WriteLine("Constructor: PartyNameAndJobWatcher");

        // Fill party healths with Null, so later we can skip printing player healths if they dont exist.
        for (int i = 0; i < partyNamesAndJobs.Length; i++) {partyNamesAndJobs[i] = null;}

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
            partyNamesAndJobs[i] = $"{player.ClassJob} {player.Name}";
        }
    }
}