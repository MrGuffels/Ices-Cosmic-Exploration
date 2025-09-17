using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Hud_CollectableGathering
    {
        public static unsafe void Draw()
        {
            if (GenericHelpers.TryGetAddonMaster<GatheringMasterpiece>("GatheringMasterpiece", out var gather) && gather.IsAddonReady)
            {
                ImGuiTableFlags tableFlags = ImGuiTableFlags.RowBg |
                             ImGuiTableFlags.Borders |
                             ImGuiTableFlags.SizingFixedFit |
                             ImGuiTableFlags.Resizable |           // Allow column resizing
                             ImGuiTableFlags.Reorderable |         // Allow column reordering
                             ImGuiTableFlags.Hideable;             // Allow hiding columns via right-click

                if (ImGui.BeginTable("Gathering_Collectable", 2, tableFlags))
                {
                    ImGui.TableSetupColumn("##AddonInfo");
                    ImGui.TableSetupColumn("##AddonValue");

                    // Row 1
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Item Name: ");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{gather.ItemName}");

                    // Row 2
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Item ID: ");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{gather.ItemID}");

                    // Row 3
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Current Collectability: ");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{gather.CurrentCollectability}");

                    // Row 4
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Item Integrity: ");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{gather.CurrentIntegrity} / {gather.TotalIntegrity}");

                    // Row 5
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Min Collectibility: ");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{gather.MinCollectability}");

                    // Row 6
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Mid Collectibility: ");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{gather.MidCollectability}");

                    // Row 7
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("High Collectibility: ");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{gather.HighCollectability}");

                    // Row 8
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text("Max Collectibility: ");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{gather.MaxCollectability}");

                    ImGui.EndTable();
                }
            }
            else
            {
                ImGui.Text("Waiting for Gather Collectable window to be visible");
            }
        }
    }
}
