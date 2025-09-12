using Dalamud.Interface.Textures;
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
                            string label = isSelected ? $"→ {flag.Key}" : $"{flag.Key}";
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
