using System;
using System.Numerics;
using System.Runtime.Serialization;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Lumina.Excel.Sheets;
using Microsoft.VisualBasic;
using Dalamud.Game.Player;
using FFXIVClientStructs.STD;
using Dalamud.Utility;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.System.Framework;


namespace ParryPlug;


public class HealthWatcher : IDisposable
{
    private uint _lastHealth;
    public HealthWatcher()
    {
        Console.WriteLine("Constructor: HealthWatcher");
        Plugin.Framework.Update += this.OnFrameWorkTick;
    }

    public void Dispose()
    {
        Plugin.Framework.Update -= this.OnFrameWorkTick;
    }

    private void OnFrameWorkTick(IFramework framework)
    {
        var player = Plugin.ObjectTable.LocalPlayer;

        if (player == null) return;
        var currentHealth = player.CurrentHp;
        
        if (currentHealth == this._lastHealth) return;

        this._lastHealth = currentHealth;
        Plugin.Log.Information("The player's health has updated to {health}.", currentHealth);
    }
}