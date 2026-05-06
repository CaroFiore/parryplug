using System;
using Dalamud.Plugin.Services;

namespace ParryPlug;

public class HealthWatcher : IDisposable
{
    public uint currentHealth {get; private set;}
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
        var nextHealth = player.CurrentHp;
        
        if (nextHealth == this.currentHealth) return;

        this.currentHealth = nextHealth;
        Plugin.Log.Information("The player's health has updated to {health}.", nextHealth);
    }
}