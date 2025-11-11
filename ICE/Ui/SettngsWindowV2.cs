using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ICE.Ui.SettingTabs;
using ICE.Ui.Waypoint_Manager;
using System.Collections.Generic;
using Action = System.Action;

namespace ICE.Ui;

internal class SettingsWindowV2 : Window
{
    public SettingsWindowV2() : 
        base($"I.C.E. Settings {P.GetType().Assembly.GetName().Version} ###ICESettingsWindowV2")
    {
        Flags = ImGuiWindowFlags.None;
        SizeConstraints = new()
        {
            MinimumSize = new Vector2(100, 100),
            MaximumSize = new Vector2(2000, 2000),
        };
        P.windowSystem.AddWindow(this);
        AllowPinning = true;
    }

    public void Dispose()
    {
        P.windowSystem.RemoveWindow(this);
    }

    public string SelectedSetting = "Safety";
    private string[] SettingOptions = ["Safety", "Gathering", "Misc", "Gamble Wheel", "Mission Settings", "Shopping List"];
    private string[] DebugOptions = ["Debug", "Path Creation"];

    public override void Draw()
    {
        float paddingX = 20f;
        float paddingY = 5f;
        Vector2 textSize = new Vector2(0.0f);

        foreach (var setting in SettingOptions)
        {
            Vector2 testText = ImGui.CalcTextSize(setting);
            
            if (testText.X > textSize.X)
            {
                textSize = testText;
            }
        }

#if DEBUG
        foreach (var setting in DebugOptions)
        {
            Vector2 testText = ImGui.CalcTextSize(setting);

            if (testText.X > textSize.X)
            {
                textSize = testText;
            }
        }
#endif

        Vector2 buttonSize = new Vector2(textSize.X + paddingX * 2, textSize.Y + paddingY * 2);

        // New child windows
        // LEFT PANEL
        if (ImGui.BeginChild("LeftPanel", new Vector2(buttonSize.X + 20, 0), true))
        {
            foreach (var setting in SettingOptions)
            {
                if (ImGui.Button(setting, buttonSize))
                {
                    SelectedSetting = setting;
                }
            }

#if DEBUG
            foreach (var setting in DebugOptions)
            {
                if (ImGui.Button(setting, buttonSize))
                {
                    SelectedSetting = setting;
                }
            }
#endif

            ImGui.EndChild();
        }

        // RIGHT PANEL (same line so it appears to the right)
        ImGui.SameLine();

        if (ImGui.BeginChild("RightPanel", new System.Numerics.Vector2(0, 0), true)) // 0 width = fill remaining
        {
            if (SelectedSetting == SettingOptions[0])
                SafetySettings.Draw();
            else if (SelectedSetting == SettingOptions[1])
                GatherSettings.Draw();
            else if (SelectedSetting == SettingOptions[2])
                MiscTab.Draw();
            else if (SelectedSetting == SettingOptions[3])
                GambaWheel.Draw();
            else if (SelectedSetting == SettingOptions[4])
                MissionSettings.Draw();
            else if (SelectedSetting == SettingOptions[5])
                ShoppingTab.Draw();
#if DEBUG
            else if (SelectedSetting == DebugOptions[0])
                DebugTab.Draw();
            else if (SelectedSetting == DebugOptions[1])
                WaypointUi.WPUi();
#endif
            else
            {
                ImGui.Text($"Hmm... Maybe should put a dad joke here. For now it's empty.");
            }

            ImGui.EndChild();
        }

    }

}
