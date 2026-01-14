using System;
using System.Collections.Generic;
using System.Text;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Table_LevelingMissions
    {
        public static void Draw()
        {
            if (ImGui.BeginTable("Leveling Table", 5, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit))
            {
                ImGui.TableSetupColumn("ID");
                ImGui.TableSetupColumn("Job Icon");
                ImGui.TableSetupColumn("Level");
                ImGui.TableSetupColumn("Enabled");
                ImGui.TableSetupColumn("Name");

                ImGui.TableHeadersRow();

                for (int i = 0; i < CosmicHelper.QuickLevelList.Count; i++)
                {
                    var id = CosmicHelper.QuickLevelList[i];
                    if (CosmicHelper.SheetMissionDict.TryGetValue(id, out var missionInfo))
                    {
                        ImGui.TableNextRow();
                        ImGui.TableSetColumnIndex(0);
                        ImGui.Text($"[{id}]");

                        ImGui.TableNextColumn();
                        var jobIcon = CosmicHelper.JobIconDict[missionInfo.Jobs.First()];
                        Vector2 imageSize = new Vector2(23, 23);
                        ImGui.Image(jobIcon.GetWrapOrEmpty().Handle, imageSize);

                        ImGui.TableNextColumn();
                        ImGui.Text($"{missionInfo.Level}");

                        ImGui.TableNextColumn();
                        if (C.MissionConfig.TryGetValue(id, out var configInfo))
                        {
                            bool enabled = configInfo.Enabled;
                            if (ImGui.Checkbox($"##Enabed_{id}", ref enabled))
                            {
                                configInfo.Enabled = enabled;
                                C.Save();
                            }
                        }

                        ImGui.TableNextColumn();
                        ImGui.Text($"{missionInfo.Name}");
                    }
                }

                ImGui.EndTable();
            }
        }
    }
}
