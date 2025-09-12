using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Interface.Textures;
using ECommons.GameHelpers;
using InteropGenerator.Runtime.Attributes;
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

        public static unsafe void Draw()
        {
            if (ImGui.Button("Add Missing Flags"))
            {
                UpdateMissions();
            }

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
                            string mapPos = $"X: {flag.Key.X} Z: {flag.Key.Y}";
                            string label = isSelected ? $"→ {mapPos}" : $"{mapPos}";
                            if (ImGui.Selectable(label, isSelected))
                            {
                                selectedZone = moon.Key;
                                selectedFlag = flag.Key;
                            }

                            ImGui.PopID();
                        }
                    }

                    ImGui.EndChild();
                }

                // Second Column, route viewer
                ImGui.TableSetColumnIndex(1);
                if (ImGui.BeginChild("Editor_GatheringSelector", new Vector2(0, 0), true))
                {
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
                        // TODO: Actually put the code here to copy the info for this particular map.
                    }

                    ImGui.Separator();

                    var textLineHeight = ImGui.GetTextLineHeightWithSpacing();
                    var childHeight = textLineHeight * 8 + 20;

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
                            for (int i = 0; i < 8; i++)
                            {
                                ImGui.Text($"Node entry {i + 1}");
                            }
                            ImGui.EndChild();
                        }

                        // Node Viewer
                        ImGui.TableSetColumnIndex(1);
                        ImGui.Text("Gathering Node Viewer");
                        if (ImGui.BeginChild("Node Viewer", new Vector2(0, childHeight), true))
                        {
                            foreach (var x in Svc.Objects)
                            {
                                if (x.ObjectKind == ObjectKind.GatheringPoint)
                                {
                                    if (Player.DistanceTo(x.Position) <= maxDistance)
                                    {
                                        ImGui.Text($"Id: {x.DataId} | Distance: {Player.DistanceTo(x.Position):N2}");
                                    }
                                }
                            }
                            ImGui.EndChild();
                        }

                        // Side Buttons
                        ImGui.TableSetColumnIndex(2);
                        ImGui.Text(" ");
                        if (ImGui.BeginChild("SideButtons", new Vector2(0, childHeight), true))
                        {
                            if (ImGui.Button("Add Node", new Vector2(-1, 0)))
                            {
                                // Add node logic
                            }
                            if (ImGui.Button("Remove", new Vector2(-1, 0)))
                            {
                                // Remove node logic
                            }
                            if (ImGui.Button("Edit", new Vector2(-1, 0)))
                            {
                                // Edit node logic
                            }
                            ImGui.EndChild();
                        }

                        ImGui.EndTable();
                    }

                    ImGui.EndChild();
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
    }
}
