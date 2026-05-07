using System;
using System.Collections.Generic;
using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Arrays;
using Lumina.Excel.Sheets;

namespace ParryPlug;

public class PartyHealthWatcher : IDisposable
{

    public uint?[] partyCurrentHealths {get; private set;} = new uint?[8];

    public PartyHealthWatcher()
    {
        Console.WriteLine("Constructor: PartyHealthWatcher");

        // Fill party healths with Null, so later we can skip printing player healths if they dont exist.
        for (int i = 0; i < partyCurrentHealths.Length; i++){partyCurrentHealths[i] = null;}

        

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
            partyCurrentHealths[i] = player.CurrentHP;
        }
    }
}