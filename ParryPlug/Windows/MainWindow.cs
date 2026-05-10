using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Lumina.Excel.Sheets;
using Microsoft.VisualBasic;
namespace ParryPlug.Windows;



public class MainWindow : Window, IDisposable
{
    private readonly string goatImagePath;
    private readonly Plugin plugin;
    private readonly Nexus nexus;


    // We give this window a hidden ID using ##.
    // The user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, string goatImagePath, Nexus _nexus)
        : base("ParryPlug##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {

        nexus = _nexus;

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.goatImagePath = goatImagePath;
        this.plugin = plugin;
    }

    public void Dispose()
    {
        nexus.Dispose();
    }

    public override void Draw()
    {
        //ImGui.Text($"The random config bool is {plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

     
        if (ImGui.Button("Settings"))
        {
            plugin.ToggleConfigUi();
        }

        if (ImGui.Button("BuildFight"))
        {
            Plugin.Log.Information("BuildFight Pressed");
            nexus.partyInfo.Build();
        }

        ImGui.Spacing();

        ImGui.Text("Debug");
        // Normally a BeginChild() would have to be followed by an unconditional EndChild(),
        // ImRaii takes care of this after the scope ends.
        // This works for all ImGui functions that require specific handling, examples are BeginTable() or Indent().
        using (var child = ImRaii.Child("SomeChildWithAScrollbar", Vector2.Zero, true))
        {
            // Check if this child is drawing
            if (child.Success)
            {
                var playerState = Plugin.PlayerState;
                if (!playerState.IsLoaded)
                {
                    ImGui.Text("Our local player is currently not logged in.");
                    return;
                }
                
                if (!playerState.ClassJob.IsValid)
                {
                    ImGui.Text("Our current job is currently not valid.");
                    return;
                }
                
                ImGui.AlignTextToFramePadding();
                ImGui.Text($"Current job:");
                
                // Scaling hardcoded pixel values is important, as otherwise users with HUD scales above or below 100%
                // won't be able to see everything.
                ImGui.SameLine(120 * ImGuiHelpers.GlobalScale);
                
                // Get the icon id from a known offset + the class jobs id
                var jobIconId = 62100 + playerState.ClassJob.RowId;
                var iconTexture = Plugin.TextureProvider.GetFromGameIcon(new GameIconLookup(jobIconId)).GetWrapOrEmpty();
                ImGui.Image(iconTexture.Handle, new Vector2(28, 28) * ImGuiHelpers.GlobalScale);
                
                ImGui.SameLine();
                
                // If you want to see the Macro representation of this SeString use `.ToMacroString()`
                // More info about SeStrings: https://dalamud.dev/plugin-development/sestring/
                ImGui.Text(playerState.ClassJob.Value.Abbreviation.ToString());
                
            

                ImGui.SameLine();
                ImGui.Text($" [Level {playerState.Level}]");
                
                // Example for querying Lumina, getting the name of our current area.
                var territoryId = Plugin.ClientState.TerritoryType;
                if (Plugin.DataManager.GetExcelSheet<TerritoryType>().TryGetRow(territoryId, out var territoryRow))
                {
                    ImGui.Text($"Current location:");
                    ImGui.SameLine(120 * ImGuiHelpers.GlobalScale);
                    ImGui.Text(territoryRow.PlaceName.Value.Name.ToString());
                }
                else
                {
                    ImGui.Text("Invalid territory.");
                }

                ImGui.Text($"Local time: {DateTime.Now:T}");

                ImGui.Text($"InCombat: {nexus.inCombatWatcher.inCombatApp}");
                

                ImGui.Text($"Local Player HP: {nexus.healthWatcher.currentHealth}");
                ImGui.Text($"Fight Time: {nexus.inCombatWatcher.fightElapsedTime}");

                foreach(Player player in nexus.partyInfo.Get())
                {
                    ImGui.Text(player.PrintPlayerInfo());
                }

                ImGui.Text("Tanks: ");
                ImGui.SameLine();
                foreach(var player in nexus.playerPicker.Pick("AllTanks")){
                    if(player == null) return;
                    ImGui.Text(player.Name);
                    ImGui.SameLine();
                }
            }
        }
    }
}
