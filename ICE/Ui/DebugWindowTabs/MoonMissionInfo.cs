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
        private static uint jobSearch = 7;

        public static unsafe void Draw()
        {
            var itemSheet = ExcelHelper.ItemSheet;
            ImGui.SetNextItemWidth(250);
            ImGui.InputText("Search by Name", ref CraftingTableSearchText, 100);
            ImGui.SetNextItemWidth(250);
            ImGui.InputText("Search by Attribute", ref AttributeSearchText, 100);
            ImGui.SetNextItemWidth(250);
            ImGui.SliderUInt("Rank ID", ref RankSearch, 0, 6);
            ImGui.SetNextItemWidth(250);
            ImGui.SliderUInt("Class Selection", ref jobSearch, 7, 18);
            if (ImGui.Button("Copy Scores"))
            {
                ImGui.SetClipboardText(GenerateMissionScoreDictionaryCode());
            }

            ImGuiTableFlags tableFlags = ImGuiTableFlags.RowBg |
                            ImGuiTableFlags.Borders |
                            ImGuiTableFlags.SizingFixedFit |
                            ImGuiTableFlags.Resizable |           // Allow column resizing
                            ImGuiTableFlags.Reorderable |         // Allow column reordering
                            ImGuiTableFlags.Hideable;             // Allow hiding columns via right-click

            if (ImGui.BeginTable("Moon Mission Information Table", 18, tableFlags))
            {
                ImGui.TableSetupColumn("ID");
                ImGui.TableSetupColumn("Jobs");

                ImGui.TableSetupColumn("Mission Name");
                ImGui.TableSetupColumn("Job");
                ImGui.TableSetupColumn("2nd Job");
                ImGui.TableSetupColumn("Rank");
                ImGui.TableSetupColumn("ToDo ID");
                ImGui.TableSetupColumn("RecipeID");
                ImGui.TableSetupColumn("Silver");
                ImGui.TableSetupColumn("Gold");
                ImGui.TableSetupColumn("Attribute Flags");

                IOrderedEnumerable<KeyValuePair<int, string>> orderedExp = CosmicHelper.ExpDictionary.ToList().OrderBy(exp => exp.Key);
                var agent = AgentMap.Instance();
                var wk = WKSManager.Instance();

                foreach (var exp in orderedExp)
                {
                    ImGui.TableSetupColumn($"{exp.Value}", ImGuiTableColumnFlags.WidthFixed, -1);
                }

                ImGui.TableSetupColumn("Test Flag");

                ImGui.TableSetupColumn("Score");

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

                    if (jobSearch != 7)
                    {
                        var jobs = new[] { entry.Value.JobId, entry.Value.JobId2 }
                                         .Where(id => id != 0)
                                         .ToHashSet();

                        if (!jobs.Contains(jobSearch))
                            continue;
                    }

                    ImGui.TableNextRow();

                    ImGui.PushID(entry.Key);

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
                    if (ImGui.IsItemClicked())
                    {
                        ImGui.SetClipboardText(RemovePrivateUseChars(entry.Value.Name));
                    }

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
                        ImGui.SameLine();
                        if (ImGui.Button($"Copy Flag##Flag-{entry.Key}"))
                        {
                            ImGui.SetClipboardText($"{entry.Value.X}, {entry.Value.Y}");
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text($"X: {entry.Value.X} | Y: {entry.Value.Y}");
                            ImGui.EndTooltip();
                        }
                    }

                    ImGui.TableNextColumn();
                    uint missionScore = entry.Value.missionScore;
                    ImGui.SetNextItemWidth(100);
                    if (ImGui.InputUInt($"##MissionScore", ref missionScore))
                    {
                        entry.Value.missionScore = missionScore;
                    }
                    ImGui.PopID();
                }

                ImGui.EndTable();
            }
        }

        public static string GenerateMissionScoreDictionaryCode()
        {
            var sb = new StringBuilder();
            sb.AppendLine("public static Dictionary<uint, uint> MissionScoreDict = new Dictionary<uint, uint>");
            sb.AppendLine("{");

            foreach (var kvp in CosmicHelper.MissionInfoDict)
            {
                sb.AppendLine($"    [{kvp.Key}] = {kvp.Value.missionScore},");
            }

            sb.AppendLine("};");

            return sb.ToString();
        }

        public static string RemovePrivateUseChars(string input)
        {
            var result = new StringBuilder();
            foreach (char c in input)
            {
                int code = (int)c;
                // Skip Private Use Areas where games often store custom symbols
                if (!(code >= 0xE000 && code <= 0xF8FF) &&    // Private Use Area
                    !(code >= 0xF0000 && code <= 0xFFFFD) &&  // Supplementary Private Use Area A
                    !(code >= 0x100000 && code <= 0x10FFFD))  // Supplementary Private Use Area B
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}
