using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;





namespace ParryPlug;

public class TetherDrawer : IDisposable
{
    
    private readonly PartyPositionWatcher partyPositionWatcher = new();
    private readonly InCombatWatcher inCombatWatcher = new();
   
    private byte playerA;
    private byte playerB;

    private uint activationTime;
    private uint resolveTime;


    private Vector3? posPlayerA;
    private Vector3? posPlayerB;
    
    public TetherDrawer(uint activationTime, uint resolveTime, byte _playerA, byte _playerB)
    {
        Plugin.Log.Information("Constructor: TetherDrawer");

        this.playerA = _playerA;
        this.playerB = _playerB;

        Plugin.Framework.Update += this.OnFrameWorkTick;
        Plugin.PluginInterface.UiBuilder.Draw += this.OnDraw;
    }
    
    public void Dispose()
    {
        Plugin.Framework.Update -= this.OnFrameWorkTick;
        Plugin.PluginInterface.UiBuilder.Draw -= this.OnDraw;
    }

    private void OnFrameWorkTick(IFramework framework)
    {
        this.posPlayerA = partyPositionWatcher.partyCurrentPositions[this.playerA];
        this.posPlayerB = partyPositionWatcher.partyCurrentPositions[this.playerB];

        if(inCombatWatcher.fightElapsedTime > this.resolveTime) this.Dispose();
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

    private void OnDraw()
    {
        if (inCombatWatcher.fightElapsedTime > this.activationTime{
            this.DrawTether(posPlayerA,posPlayerB);  
        }
    }
}