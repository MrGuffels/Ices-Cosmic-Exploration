using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Hud_MissionInfo
    {
        public static void Draw()
        {
            uint currentScore = 0;
            uint silverScore = 0;
            uint goldScore = 0;

            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var x) && x.IsAddonReady)
            {
                currentScore = x.CurrentScore;
                silverScore = x.SilverScore;
                goldScore = x.GoldScore;

                var isAddonReady = AddonHelper.IsAddonActive("WKSMissionInfomation");
                ImGui.Text($"Addon Ready: {isAddonReady}");
                if (isAddonReady)
                {
                    ImGui.Text($"Node Text: {AddonHelper.GetNodeText("WKSMissionInfomation", 27)}");
                }

                ImGuiTableFlags tableFlags = ImGuiTableFlags.RowBg |
                                             ImGuiTableFlags.Borders |
                                             ImGuiTableFlags.SizingFixedFit |
                                             ImGuiTableFlags.Resizable |           // Allow column resizing
                                             ImGuiTableFlags.Reorderable |         // Allow column reordering
                                             ImGuiTableFlags.Hideable;             // Allow hiding columns via right-click

                if (ImGui.BeginTable("WKSMissionInfomationAddon_Table", 2, tableFlags))
                {
                    ImGui.TableSetupColumn("###Info", ImGuiTableColumnFlags.WidthFixed, 150);
                    ImGui.TableSetupColumn("###UiInfo", ImGuiTableColumnFlags.WidthFixed, 100);

                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Current Score:");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{currentScore}");

                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Silver Score:");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{silverScore}");

                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Gold Score:");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{goldScore}");

                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    if (ImGui.Button("Cosmo Pouch"))
                    {
                        x.CosmoPouch();
                    }

                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    if (ImGui.Button("Cosmo Crafting Log"))
                    {
                        x.CosmoCraftingLog();
                    }

                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    if (ImGui.Button("Steller Reduction"))
                    {
                        x.StellerReduction();
                    }

                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    if (ImGui.Button("Report"))
                    {
                        x.Report();
                    }

                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    if (ImGui.Button("Abandon"))
                    {
                        x.Abandon();
                    }


                    ImGui.EndTable();
                }
            }
            else
            {
                ImGui.Text("Waiting for \"WKSMissionInfomation\" to be visible");
            }
        }
    }
}
