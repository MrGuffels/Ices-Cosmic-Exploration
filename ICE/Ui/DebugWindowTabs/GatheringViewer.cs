using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class GatheringViewer
    {
        private static Dictionary<Vector2, uint> SavedNodeDict => D.NodeDict;
        private static List<GatheringUtil.GathNodeInfo> TempNodeInfo => D.CustomNodeInfoList;

        private static Dictionary<Vector2, uint> selectedDict = new();
        private static List<GatheringUtil.GathNodeInfo> selectedList = new();

        private static Vector2 PositionEntry = new Vector2(0, 0);
        private static uint ListingEntry = 0;
        private static bool showBuiltIn = true;
        private static bool showCustom = false;

        public static unsafe void Draw()
        {
            if (ImGui.RadioButton("Show Built in", showBuiltIn))
            {
                showCustom = false;
                showBuiltIn = true;
            }
            ImGui.SameLine();
            if (ImGui.RadioButton("Show Custom", showCustom))
            {
                showCustom = true;
                showBuiltIn = false;
            }

            if (showBuiltIn)
            {
                
                // selectedList = GatheringUtil.MoonNodeInfoList;
            }
            else if (showCustom)
            {
                selectedDict = SavedNodeDict;
                selectedList = TempNodeInfo;
            }

            foreach (var kvp in selectedDict)
            {
                var key = kvp.Key;
                var value = kvp.Value;

                if (ImGui.CollapsingHeader($"Nodeset {value} | X: {key.X} Y: {key.Y}###{key} {value}"))
                {
                    if (ImGui.BeginTable($"Gathering Location Info###GatheringNodeInfo{key}_{value}", 3, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Borders))
                    {
                        ImGui.TableSetupColumn("Target Pos");
                        ImGui.TableSetupColumn("Land Zone");
                        ImGui.TableSetupColumn("Node Id");

                        ImGui.TableHeadersRow();

                        foreach (var entry in selectedList)
                        {
                            if (entry.NodeSet != value)
                                { continue; }

                            ImGui.TableNextRow();
                            ImGui.TableSetColumnIndex(0);
                            ImGui.Text($"{entry.Position.X:N2}, {entry.Position.Y:N2}, {entry.Position.Z:N2}");

                            ImGui.TableNextColumn();
                            ImGui.Text($"{entry.LandZone.X}, {entry.Position.Y:N2}, {entry.Position.Z:N2}");

                            ImGui.TableNextColumn();
                            ImGui.Text($"{entry.NodeId}");
                        }

                        ImGui.EndTable();
                    }
                }
            }
        }
    }
}
