using Dalamud.Interface.Textures;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class MoonMissionInfo
    {
        private static string CraftingTableSearchText = "";
        private static string AttributeSearchText = "";
        private static uint RankSearch = 0;

        public static unsafe void Draw()
        {
            var itemSheet = ExcelHelper.ItemSheet;
            ImGui.SetNextItemWidth(250);
            ImGui.InputText("Search by Name", ref CraftingTableSearchText, 100);
            ImGui.SetNextItemWidth(250);
            ImGui.InputText("Search by Attribute", ref AttributeSearchText, 100);
            ImGui.SetNextItemWidth(250);
            ImGui.SliderUInt("Rank ID", ref RankSearch, 0, 6);

            if (ImGui.BeginTable("Moon Mission Information Table", 18, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.SizingFixedFit))
            {
                ImGui.TableSetupColumn("ID", ImGuiTableColumnFlags.WidthFixed, -1);
                ImGui.TableSetupColumn("Jobs", ImGuiTableColumnFlags.WidthFixed, -1);

                ImGui.TableSetupColumn("Mission Name", ImGuiTableColumnFlags.WidthFixed, -1);
                ImGui.TableSetupColumn("Job", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("2nd Job", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("Rank", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("ToDo ID", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("RecipeID", ImGuiTableColumnFlags.WidthFixed, 100);
                ImGui.TableSetupColumn("Silver", ImGuiTableColumnFlags.WidthFixed, -1);
                ImGui.TableSetupColumn("Gold", ImGuiTableColumnFlags.WidthFixed, -1);
                ImGui.TableSetupColumn("Attribute Flags", ImGuiTableColumnFlags.WidthFixed, -1);
                //ImGui.TableSetupColumn("Exp Type 1###MissionExpType1", ImGuiTableColumnFlags.WidthFixed, 100);
                //ImGui.TableSetupColumn("Exp Amount 1###MissionExpAmount1", ImGuiTableColumnFlags.WidthFixed, 100);
                //ImGui.TableSetupColumn("Exp Type 2###MissionExpType2", ImGuiTableColumnFlags.WidthFixed, 100);
                //ImGui.TableSetupColumn("Exp Amount 2###MissionExpAmount2", ImGuiTableColumnFlags.WidthFixed, 100);
                //ImGui.TableSetupColumn("Exp Type 3###MissionExpType3", ImGuiTableColumnFlags.WidthFixed, 100);
                //ImGui.TableSetupColumn("Exp Amount 3###MissionExpAmount3", ImGuiTableColumnFlags.WidthFixed, 100);

                IOrderedEnumerable<KeyValuePair<int, string>> orderedExp = CosmicHelper.ExpDictionary.ToList().OrderBy(exp => exp.Key);
                var agent = AgentMap.Instance();
                var wk = WKSManager.Instance();

                //_gatherCenter = new(marker.Unknown1 - 1024, marker.Unknown2 - 1024);
                //_gatherRadius = marker.Unknown3;

                foreach (var exp in orderedExp)
                {
                    ImGui.TableSetupColumn($"{exp.Value}", ImGuiTableColumnFlags.WidthFixed, -1);
                }

                ImGui.TableSetupColumn("Test Flag", ImGuiTableColumnFlags.WidthFixed, -1);

                ImGui.TableHeadersRow();

                var missionList = CosmicHelper.MissionInfoDict.Where(mission => mission.Value.Name.ToLower().Contains(CraftingTableSearchText.ToLower()));

                foreach (var entry in CosmicHelper.MissionInfoDict)
                {
                    // Case-insensitive name search
                    if (!string.IsNullOrEmpty(CraftingTableSearchText) &&
                        !entry.Value.Name.ToLower().Contains(CraftingTableSearchText.ToLower()))
                        continue;

                    // Case-insensitive attribute search
                    string attributedText = entry.Value.Attributes.ToString().ToLower();
                    if (!string.IsNullOrEmpty(AttributeSearchText) &&
                        !attributedText.Contains(AttributeSearchText.ToLower()))
                        continue;

                    if (RankSearch != 0 && entry.Value.Rank != RankSearch)
                        continue;

                    ImGui.TableNextRow();

                    // Mission ID
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text($"{entry.Key}");

                    // Job Icons (for quick reference)
                    ImGui.TableNextColumn();

                    Vector2 size = new Vector2(20, 20);

                    ISharedImmediateTexture? icon = CosmicHelper.JobIconDict[entry.Value.JobId];
                    ImGui.Image(icon.GetWrapOrEmpty().Handle, size);
                    if (entry.Value.JobId2 != 0)
                    {
                        ImGui.SameLine();
                        ISharedImmediateTexture? icon2 = CosmicHelper.JobIconDict[entry.Value.JobId2];
                        ImGui.Image(icon2.GetWrapOrEmpty().Handle, size);
                    }

                    // Mission Name
                    ImGui.TableNextColumn();
                    ImGui.Text(entry.Value.Name);

                    // JobId Attached to it
                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.JobId}");

                    // 2nd Job for quest
                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.JobId2}");

                    // Rank of the mission
                    ImGui.TableNextColumn();
                    string rank = "";
                    if (entry.Value.Rank == 1)
                        rank = "D";
                    else if (entry.Value.Rank == 2)
                        rank = "C";
                    else if (entry.Value.Rank == 3)
                        rank = "B";
                    else if (entry.Value.Rank == 4)
                        rank = "A";
                    else if (entry.Value.Rank == 5)
                        rank = "EX";
                    else if (entry.Value.Rank == 6)
                        rank = "EX+";
                    else
                    {
                        rank = entry.Value.Rank.ToString();
                    }
                    ImGui.Text($"{rank}");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.ToDoSlot}");

                    ImGui.TableNextColumn();
                    var RecipeSearch = entry.Value.RecipeId;
                    ImGui.Text($"{RecipeSearch}");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.SilverRequirement}");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.GoldRequirement}");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.Attributes}");

                    foreach (var expType in orderedExp)
                    {
                        var relicXp = entry.Value.ExperienceRewards.Where(exp => exp.Type == expType.Key).FirstOrDefault().Amount.ToString();
                        if (relicXp == "0")
                        {
                            relicXp = "-";
                        }

                        ImGui.TableNextColumn();
                        ImGui.Text($"{relicXp}");
                    }

                    ImGui.TableNextColumn();
                    if (entry.Value.MarkerId != 0)
                    {
                        if (ImGui.Button($"Flag###Flag-{entry.Key}"))
                        {
                            Utils.SetGatheringRing(entry.Value.TerritoryId, entry.Value.X, entry.Value.Y, entry.Value.Radius);
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text($"X: {entry.Value.X} | Y: {entry.Value.Y}");
                            ImGui.EndTooltip();
                        }
                    }
                }

                ImGui.EndTable();
            }
        }
    }
}
