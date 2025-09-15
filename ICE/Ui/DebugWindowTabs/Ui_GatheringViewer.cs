using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility.Raii;
using ECommons.GameHelpers;
using InteropGenerator.Runtime.Attributes;
using Pictomancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Ui_GatheringViewer
    {
        private static uint selectedZone = 0;
        private static Vector2 selectedFlag = Vector2.Zero;

        // Gathering Node Settings
        private static bool ViewOnlyTargeable = true;
        private static bool AddPictoCircle = true;
        private static float maxDistance = 25;

        // Node Viewier
        private static uint nodeId = 0;

        // Selected Node Entry
        private static int selectedNodeIndex = 0;

        private static bool PictoCircle = false;
        private static bool PictoDot = false;

        private static List<uint> MissionIds = new(); 


        public static unsafe void Draw()
        {
            if (ImGui.Button("Add Missing Flags"))
            {
                UpdateMissions();
            }
            ImGui.SameLine();
            if (ImGui.Button("Export All Data"))
            {
                var exportData = ExportAllGatheringData();
                ImGui.SetClipboardText(exportData);
                Svc.Chat.Print("All gathering data exported to clipboard!");
            }
            ImGui.SameLine();
            using (ImRaii.Disabled(selectedFlag == Vector2.Zero))
            {
                if (ImGui.Button("Export Selected Flag"))
                {
                    var exportData = ExportSingleFlag(selectedZone, selectedFlag);
                    ImGui.SetClipboardText(exportData);
                    Svc.Chat.Print($"Flag data for Zone {selectedZone} at ({selectedFlag.X}, {selectedFlag.Y}) exported to clipboard!");
                }
            }

            ImGui.Separator();

            if (ImGui.BeginTable("Gathering_RouteEditor", 2, ImGuiTableFlags.Resizable))
            {
                ImGui.TableNextRow();

                // First Column, Gathering Section Selector
                ImGui.TableSetColumnIndex(0);
                if (ImGui.BeginChild("Route_GatheringSelector", new Vector2(0, 0), true))
                {
                    foreach (var moon in GatheringUtil.MoonGatherLocations)
                    {
                        ImGui.Text($"ZoneId: {moon.Key}");
                        foreach (var flag in moon.Value)
                        {
                            var isSelected = selectedZone == moon.Key && selectedFlag == flag.Key;

                            uint jobId = GetJobIdForFlag(moon.Key, flag.Key);
                            ISharedImmediateTexture? jobIcon = CosmicHelper.JobIconDict[jobId];

                            ImGui.PushID($"{moon.Key}_{flag.Key.X}_{flag.Key.Y}");

                            // Draw icon first
                            if (jobIcon != null)
                            {
                                var iconSize = new Vector2(ImGui.GetTextLineHeight(), ImGui.GetTextLineHeight());
                                ImGui.Image(jobIcon.GetWrapOrDefault().Handle, iconSize);
                                ImGui.SameLine();
                            }

                            // Then the selectable text
                            string mapPos = $"X: {flag.Key.X} Z: {flag.Key.Y} [{flag.Value.Count}]";
                            string label = isSelected ? $"→ {mapPos}" : $"{mapPos}";
                            if (ImGui.Selectable(label, isSelected))
                            {
                                selectedZone = moon.Key;
                                selectedFlag = flag.Key;
                                selectedNodeIndex = 0;

                                MissionIds.Clear();
                                foreach (var mission in CosmicHelper.SheetMissionDict)
                                {
                                    var id = mission.Key;
                                    var mapFlag = mission.Value.MapPosition;
                                    if (mapFlag == selectedFlag)
                                        MissionIds.Add(id);
                                }
                            }

                            ImGui.PopID();
                        }
                    }

                    ImGui.EndChild();
                }

                // Second Column, route viewer
                ImGui.TableSetColumnIndex(1);
                var textLineHeight = ImGui.GetTextLineHeightWithSpacing();
                var childHeight = textLineHeight * 8 + 20;
                if (ImGui.BeginChild("Editor_GatheringSelector", new Vector2(0, childHeight + 150), false))
                {
                    string missions = "";
                    foreach (var id in MissionIds)
                    {
                        missions += $"{id}, ";
                    }
                    ImGui.Text($"Missions: {missions}");

                    if (ImGui.Button("Open map position"))
                    {
                        var missionEntry = CosmicHelper.SheetMissionDict.Where(x => x.Value.MapPosition == selectedFlag
                                                                             && x.Value.TerritoryId == selectedZone).FirstOrDefault();
                        if (missionEntry.Key != 0)
                        {
                            var mission = missionEntry.Value;
                            Utils.SetGatheringRing(mission.TerritoryId, (int)mission.MapPosition.X, (int)mission.MapPosition.Y, mission.Radius, mission.Name);
                        }
                    }
                    if (ImGui.Button("Copy Map Info"))
                    {
                        if (selectedFlag != Vector2.Zero)
                        {
                            var exportData = ExportSingleFlag(selectedZone, selectedFlag);
                            ImGui.SetClipboardText(exportData);
                            Svc.Chat.Print("Map info copied to clipboard!");
                        }
                    }

                    ImGui.Separator();

                    if (ImGui.BeginTable("NodeEditTable", 3, ImGuiTableFlags.Resizable))
                    {
                        // Set up column widths
                        ImGui.TableSetupColumn("NodeSelector", ImGuiTableColumnFlags.WidthStretch, 0.4f);
                        ImGui.TableSetupColumn("NodeViewer", ImGuiTableColumnFlags.WidthStretch, 0.4f);
                        ImGui.TableSetupColumn("Buttons", ImGuiTableColumnFlags.WidthFixed, 100f);

                        ImGui.TableNextRow();

                        // Node Selector
                        ImGui.TableSetColumnIndex(0);
                        ImGui.Text("Node Selector");
                        if (ImGui.BeginChild("NodeSelector", new Vector2(0, childHeight), true))
                        {
                            if (selectedFlag != Vector2.Zero)
                            {
                                for (int i = 0; i < GatheringUtil.MoonGatherLocations[selectedZone][selectedFlag].Count; i++)
                                {
                                    var nodeInfo = GatheringUtil.MoonGatherLocations[selectedZone][selectedFlag][i];

                                    // Create a unique label for the selectable (you can customize this)
                                    string label = $"Node: {nodeInfo.NodeId}";

                                    if (ImGui.Selectable(label, selectedNodeIndex == i))
                                    {
                                        selectedNodeIndex = i; // Track which node is selected
                                                               // Add any additional logic when a node is selected
                                    }
                                }
                            }

                            ImGui.EndChild();
                        }

                        // Node Viewer
                        ImGui.TableSetColumnIndex(1);
                        ImGui.Text("Gathering Node Viewer");
                        if (ImGui.BeginChild("Node Viewer", new Vector2(0, childHeight), true))
                        {
                            foreach (var x in Svc.Objects
                                .Where(x => x.ObjectKind == ObjectKind.GatheringPoint & Player.DistanceTo(x.Position) <= maxDistance)
                                    .OrderBy(x => Player.DistanceTo(x.Position))
                            .ToList())
                            {
                                if (ImGui.Selectable($"Id: {x.DataId} | Distance: {Player.DistanceTo(x.Position):N2}"))
                                {
                                    nodeId = x.DataId;
                                }
                            }
                            ImGui.EndChild();
                        }

                        // Side Buttons
                        ImGui.TableSetColumnIndex(2);
                        ImGui.Text(" ");
                        if (ImGui.BeginChild("SideButtons", new Vector2(0, childHeight), true))
                        {
                            using (ImRaii.Disabled(nodeId == 0))
                            {
                                if (ImGui.Button("Add Node", new Vector2(-1, 0)))
                                {
                                    var x = Svc.Objects.Where(x => x.DataId == nodeId).FirstOrDefault();
                                    if (x != null)
                                    {
                                        var gatheringZone = GatheringUtil.MoonGatherLocations[selectedZone][selectedFlag];
                                        var playerPos = Player.Position;

                                        var addNode = new GatheringUtil.GathNodeInfo()
                                        {
                                            LandZone = playerPos,
                                            Position = x.Position,
                                            NodeId = x.DataId
                                        };
                                        gatheringZone.Add(addNode);
                                    }

                                    nodeId = 0;
                                }
                            }

                            using (ImRaii.Disabled(!ImGui.GetIO().KeyShift))
                            {
                                if (ImGui.Button("Remove", new Vector2(-1, 0)))
                                {
                                    var nodeInfo = GatheringUtil.MoonGatherLocations[selectedZone][selectedFlag][selectedNodeIndex];
                                    GatheringUtil.MoonGatherLocations[selectedZone][selectedFlag].Remove(nodeInfo);
                                    selectedNodeIndex -= 1;
                                }
                            }
                            ImGui.EndChild();
                        }

                        ImGui.EndTable();
                    }

                    ImGui.EndChild();
                }

                if (selectedFlag != Vector2.Zero)
                {
                    if (GatheringUtil.MoonGatherLocations[selectedZone][selectedFlag].Count > 0)
                    {
                        ImGui.Checkbox("Draw Node Position", ref PictoCircle);
                        ImGui.SameLine();
                        Vector4 circleColor = Utils.FromUintABGR(C.PictoColor_Circle);
                        ImGui.SetNextItemWidth(200);
                        if (ImGui.ColorEdit4("##CircleColorEditor", ref circleColor))
                        {
                            C.PictoColor_Circle = Utils.ToUintABGR(circleColor);
                            C.Save();
                        }

                        ImGui.Checkbox("Draw Land Position", ref PictoDot);
                        ImGui.SameLine();
                        Vector4 dotColor = Utils.FromUintABGR(C.PictoColor_Dot);
                        ImGui.SetNextItemWidth(200);
                        if (ImGui.ColorEdit4("##DotColorEditor", ref dotColor))
                        {
                            C.PictoColor_Dot = Utils.ToUintABGR(dotColor);
                            C.Save();
                        }

                        var nodeInfo = GatheringUtil.MoonGatherLocations[selectedZone][selectedFlag][selectedNodeIndex];

                        ImGui.Text($"Position -> X: {nodeInfo.Position.X:N2}, Y: {nodeInfo.Position.Y:N2}, Z: {nodeInfo.Position.Z:N2}");
                        ImGui.Text($"NodeId: {nodeInfo.NodeId}");

                        ImGui.Text($"Player Position");
                        Vector3 nodePlayerPos = nodeInfo.LandZone;
                        ImGui.SetNextItemWidth(200);
                        if (ImGui.DragFloat3($"##Player_Position: {nodeInfo.NodeId}", ref nodePlayerPos))
                        {
                            nodeInfo.LandZone = nodePlayerPos;
                        }
                        ImGui.SameLine();
                        if (ImGui.Button("Set to current POS"))
                        {
                            Vector3 currentPos = Player.Position;
                            nodeInfo.LandZone = currentPos;
                        }

                        using (var drawList = PictoService.Draw())
                        {
                            if (drawList == null)
                                return;

                            if (PictoCircle)
                            {
                                PictoService.VfxRenderer.AddCircle("Mount_Radius Circle", nodeInfo.Position, 3f, Utils.FromUintABGR(2616716297));
                            }
                            if (PictoDot)
                            {
                                drawList.AddDot(nodeInfo.LandZone, 5f, 2616716297);
                            }
                        }
                    }
                }

                ImGui.EndTable();
            }
        }

        private static void UpdateMissions()
        {
            foreach (var mission in CosmicHelper.SheetMissionDict)
            {
                if (mission.Value.MapPosition != Vector2.Zero 
                && (mission.Value.Jobs.Contains(16) || mission.Value.Jobs.Contains(17)))
                {
                    uint zoneId = mission.Value.TerritoryId;
                    Vector2 flag = mission.Value.MapPosition;

                    if (GatheringUtil.MoonGatherLocations.TryGetValue(zoneId, out var moon))
                    {
                        if (!moon.ContainsKey(flag))
                        {
                            // Location doesn't exist, time to input it in.
                            moon[flag] = new();
                        }
                    }
                    else
                    {
                        GatheringUtil.MoonGatherLocations[zoneId] = new()
                        {
                            [flag] = new()
                        };
                    }
                }
            }
        }

        private static uint GetJobIdForFlag(uint territoryId, Vector2 map)
        {
            var missionEntry = CosmicHelper.SheetMissionDict.Where(x => x.Value.MapPosition == map
                                                                && x.Value.TerritoryId == territoryId).FirstOrDefault();

            if (missionEntry.Key != 0)
            {
                if (missionEntry.Value.Jobs.Contains(17))
                    return 17;
                else if (missionEntry.Value.Jobs.Contains(16))
                    return 16;
                else
                    return 8;
            }
            else
                return 8;
        }

        private static string ExportAllGatheringData()
        {
            var sb = new StringBuilder();
            sb.AppendLine("public static Dictionary<uint, Dictionary<Vector2, List<GathNodeInfo>>> MoonGatherLocations = new()");
            sb.AppendLine("{");

            foreach (var zone in GatheringUtil.MoonGatherLocations)
            {
                sb.AppendLine($"    [{zone.Key}] = new()");
                sb.AppendLine("    {");

                foreach (var flag in zone.Value)
                {
                    sb.AppendLine($"        [new Vector2({flag.Key.X}f, {flag.Key.Y}f)] = new()");
                    sb.AppendLine("        {");

                    foreach (var node in flag.Value)
                    {
                        sb.AppendLine("            new GathNodeInfo()");
                        sb.AppendLine("            {");
                        sb.AppendLine($"                Position = new Vector3({node.Position.X:N2}f, {node.Position.Y:N2}f, {node.Position.Z:N2}f),");
                        sb.AppendLine($"                LandZone = new Vector3({node.LandZone.X:N2}f, {node.LandZone.Y:N2}f, {node.LandZone.Z:N2}f),");
                        sb.AppendLine($"                NodeId = {node.NodeId},");
                        sb.AppendLine("            },");
                    }

                    sb.AppendLine("        },");
                }

                sb.AppendLine("    },");
            }

            sb.AppendLine("};");
            return sb.ToString();
        }

        private static string ExportSingleFlag(uint zoneId, Vector2 flag)
        {
            if (!GatheringUtil.MoonGatherLocations.TryGetValue(zoneId, out var zone) ||
                !zone.TryGetValue(flag, out var nodes))
            {
                return "// No data found for the selected flag";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"// Export for Zone {zoneId}, Flag ({flag.X}, {flag.Y})");
            sb.AppendLine($"[new Vector2({flag.X}f, {flag.Y}f)] = new()");
            sb.AppendLine("{");

            foreach (var node in nodes)
            {
                sb.AppendLine("    new GathNodeInfo()");
                sb.AppendLine("    {");
                sb.AppendLine($"        Position = new Vector3({node.Position.X:N2}f, {node.Position.Y:N2}f, {node.Position.Z:N2}f),");
                sb.AppendLine($"        LandZone = new Vector3({node.LandZone.X:N2}f, {node.LandZone.Y:N2}f, {node.LandZone.Z:N2}f),");
                sb.AppendLine($"        NodeId = {node.NodeId},");
                sb.AppendLine("    },");
            }

            sb.AppendLine("},");
            return sb.ToString();
        }
    }
}
