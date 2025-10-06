using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using ICE.Ui.DebugWindowTabs;
using ICE.Ui.Waypoint_Manager;
using Lumina.Excel.Sheets;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;
using static ICE.Utilities.CosmicHelper;

namespace ICE.Ui;

internal class DebugWindow : Window
{
    public DebugWindow() :
        base($"ICE {P.GetType().Assembly.GetName().Version} Debugger ###IceCosmicDebug1")
    {
        Flags = ImGuiWindowFlags.None;
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(100, 100),
            MaximumSize = new Vector2(3000, 3000)
        };
        P.windowSystem.AddWindow(this);
    }

    public void Dispose()
    {
        P.windowSystem.RemoveWindow(this);
    }

    private string[] DebugTypes = 
    [
        // HUD Elements
        "Hud: Moon Main",
        "Hud: Mission",
        "Hud: Mission Info",
        "Hud: Wheel of fortune!",
        "Hud: Moon Recipe",
        "Hud: Gather Collectable",
    
        // Table Elements
        "Table: Mission Info",
        "Table: Item List",
        "Table: Gathering Missions",
        "Table: Special Missions",
        "Table: Mission Text",
        "Table: Recipies",
    
        // UI Elements
        "Ui: Fishing Hole Editor",
        "Ui: Fishing Preset Editor",
        "Ui: Gather Editor",
    
        // Non-labeled Elements
        "Player Info",
        "Test Buttons",
        "IPC Testing",
        "Map Test",
        "Navmesh Testing",
        "Relic Info",
        "TaskManager Testing"
    ];

    int selectedDebugIndex = 0; // Keeping which tab I'm selecting here. Just persistant stuff.

    public override unsafe void Draw()
    {
        float spacing = 10f;
        float leftPanelWidth = 200f;
        float rightPanelWidth = ImGui.GetContentRegionAvail().X - leftPanelWidth - spacing;
        float childHeight = ImGui.GetContentRegionAvail().Y;

        if (ImGui.BeginChild("DebugSelector", new System.Numerics.Vector2(leftPanelWidth, childHeight), true))
        {
            for (int i = 0; i < DebugTypes.Length; i++)
            {
                bool isSelected = (selectedDebugIndex == i);
                string label = isSelected ? $"→ {DebugTypes[i]}" : $"   {DebugTypes[i]}";

                if (ImGui.Selectable(label, isSelected))
                {
                    selectedDebugIndex = i;
                }
            }
            ImGui.EndChild();
        }

        ImGui.SameLine(0, spacing);

        if (ImGui.BeginChild("DebugContent", new System.Numerics.Vector2(rightPanelWidth, childHeight), true))
        {
            switch (selectedDebugIndex)
            {
                // HUD Elements (0-5)
                case 0: Hud_MainMoon.Draw(); break;
                case 1: Hud_Mission.Draw(); break;
                case 2: Hud_MissionInfo.Draw(); break;
                case 3: Hud_WheelofFortune.Draw(); break;
                case 4: Hud_MoonRecipe.Draw(); break;
                case 5: Hud_CollectableGathering.Draw(); break;

                // Table Elements (6-11)
                case 6: Table_MissionInfo.Draw(); break;
                case 7: Table_CustomItems.Draw(); break;
                case 8: Table_GatheringInfo.Draw(); break;
                case 9: Table_TimeWeather.Draw(); break;
                case 10: Table_MissionText.Draw(); break;
                case 11: Table_MoonRecipies.Draw(); break;

                // UI Elements (12-13)
                case 12: Ui_FishingEditor.Draw(); break;
                case 13: Ui_FishingMissionEditor.Draw(); break;
                case 14: Ui_GatherRoute_Editor.Draw(); break;

                // Non-labeled Elements (14-21)
                case 15: Ui_PlayerInfo.Draw(); break;
                case 16: Ui_TestButtons.Draw(); break;
                case 17: Ui_IPCTesting.Draw(); break;
                case 18: Ui_MapTesting.Draw(); break;
                case 19: Ui_NavmeshTesting.Draw(); break;
                case 20: Ui_RelicInfo.Draw(); break;
                case 21: Ui_TaskManagerInfo.Draw(); break;

                default: ImGui.Text("Unknown Debug View"); break;
            }
        }
        ImGui.EndChild();
    }
}
