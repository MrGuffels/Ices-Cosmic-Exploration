using ECommons.ExcelServices.TerritoryEnumeration;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class IPCTesting
    {
        private static int Radius = 10;
        private static int XLoc = 0;
        private static int YLoc = 0;
        private static string importString = new string('\0', 2048); // Pre-allocate buffer
        private static string SwapToPreset = string.Empty;

        public static unsafe void Draw()
        {
            ImGui.Text($"Artisan Is Busy? {P.Artisan.IsBusy()}");
            ImGui.Text($"{EzThrottler.GetRemainingTime("[Main Item(s)] Starting Main Craft")}");
            if (ImGui.Button("Artisan, craft this"))
            {
                P.Artisan.CraftItem(36026, 1);
            }

            ImGui.SetNextItemWidth(125);
            ImGui.InputInt("Radius", ref Radius);
            ImGui.SetNextItemWidth(125);
            ImGui.InputInt("X Location", ref XLoc);
            ImGui.SetNextItemWidth(125);
            ImGui.InputInt("Y Location", ref YLoc);

            if (ImGui.Button($"Test Radius"))
            {
                var agent = AgentMap.Instance();

                Utils.SetGatheringRing(agent->CurrentTerritoryId, XLoc, YLoc, Radius);
            }

            ImGui.Separator();
            ImGui.Text("AutoHook");
            ImGui.SetNextItemWidth(150);
            ImGui.InputText("Preset String", ref importString, 2048);
            if (ImGui.Button("Import"))
            {
                P.AutoHook.ImportAndSelectPreset(importString);
                importString = string.Empty;
            }
            ImGui.SetNextItemWidth(150);
            ImGui.InputText("Swap to preset", ref SwapToPreset);
            if (ImGui.Button("Swap"))
            {
                P.AutoHook.SetPreset(SwapToPreset);
            }
        }
    }
}
