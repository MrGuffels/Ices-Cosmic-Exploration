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
    internal class Table_MissionInfo
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
            if (ImGui.Button("Export Fishing Missions"))
            {
                var fishingMissions = CosmicHelper.SheetMissionDict
                    .Where(kvp => kvp.Value.Attributes.HasFlag(MissionAttributes.Fish)) // Adjust flag name as needed
                    .OrderBy(kvp => kvp.Key) // Optional: sort by mission ID
                    .Select(kvp => $"    [{kvp.Key}] = \"\",")
                    .ToArray();

                if (fishingMissions.Length > 0)
                {
                    var result = string.Join(Environment.NewLine, fishingMissions);

                    // Copy to clipboard
                    ImGui.SetClipboardText(result);

                    // Optional: Show a tooltip or notification
                    // You could also use a popup or status message here
                    ImGui.SetTooltip($"Copied {fishingMissions.Length} fishing missions to clipboard!");
                }
                else
                {
                    ImGui.SetTooltip("No fishing missions found!");
                }
            }

            ImGuiTableFlags tableFlags = ImGuiTableFlags.RowBg |
                            ImGuiTableFlags.Borders |
                            ImGuiTableFlags.SizingFixedFit |
                            ImGuiTableFlags.Resizable |           // Allow column resizing
                            ImGuiTableFlags.Reorderable |         // Allow column reordering
                            ImGuiTableFlags.Hideable;             // Allow hiding columns via right-click

            if (ImGui.BeginTable("Moon Mission Information Table", 27, tableFlags)) // Increased column count by 1
            {
                ImGui.TableSetupColumn("ID");
                ImGui.TableSetupColumn("Jobs");

                ImGui.TableSetupColumn("Mission Name");
                ImGui.TableSetupColumn("Job");
                ImGui.TableSetupColumn("2nd Job");
                ImGui.TableSetupColumn("Rank");
                ImGui.TableSetupColumn("ToDo ID");
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

                ImGui.TableSetupColumn("Main Item 1");
                ImGui.TableSetupColumn("Amount #1");
                ImGui.TableSetupColumn("Main Item 2");
                ImGui.TableSetupColumn("Amount #2");
                ImGui.TableSetupColumn("Main Item 3");
                ImGui.TableSetupColumn("Amount #3");
                ImGui.TableSetupColumn("Pre-Craft Item");
                ImGui.TableSetupColumn("Pre-Craft Amount");
                ImGui.TableSetupColumn("Export"); // New column for export button

                ImGui.TableHeadersRow();

                foreach (var entry in CosmicHelper.SheetMissionDict)
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
                        var jobs = entry.Value.Jobs;

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

                    int index = 0;
                    foreach (var jobId in entry.Value.Jobs)
                    {
                        ISharedImmediateTexture? icon = CosmicHelper.JobIconDict[jobId];
                        ImGui.Image(icon.GetWrapOrEmpty().Handle, size);

                        if (index < entry.Value.Jobs.Count - 1) // Not the only job, adding a sameline for it
                            ImGui.SameLine();

                        index++;
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
                    ImGui.Text($"{entry.Value.Jobs.First()}");

                    // 2nd Job for quest
                    ImGui.TableNextColumn();
                    if (entry.Value.Jobs.Count > 1)
                    {
                        ImGui.Text($"{entry.Value.Jobs.Last()}");
                    }
                    else
                    {
                        ImGui.Text($"0");
                    }

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
                    ImGui.Text($"{entry.Value.ToDoId}");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.SilverScore}");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.GoldScore}");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{entry.Value.Attributes}");

                    foreach (var expType in orderedExp)
                    {
                        var type = expType.Key;
                        ImGui.TableNextColumn();
                        if (entry.Value.RelicXpInfo.TryGetValue(type, out var amount))
                        {
                            ImGui.Text($"{amount}");
                        }
                        else
                        {
                            ImGui.Text($"-");
                        }
                    }

                    ImGui.TableNextColumn();
                    if (entry.Value.MarkerId != 0)
                    {
                        if (ImGui.Button($"Flag###Flag-{entry.Key}"))
                        {
                            Utils.SetGatheringRing(entry.Value.TerritoryId, (int)entry.Value.MapPosition.X, (int)entry.Value.MapPosition.Y, entry.Value.Radius);
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text($"X: {entry.Value.MapPosition.X} | Y: {entry.Value.MapPosition.Y}");
                            ImGui.EndTooltip();
                        }
                        ImGui.SameLine();
                        if (ImGui.Button($"Copy Flag##Flag-{entry.Key}"))
                        {
                            ImGui.SetClipboardText($"{entry.Value.MapPosition.X}, {entry.Value.MapPosition.Y}");
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text($"X: {entry.Value.MapPosition.X} | Y: {entry.Value.MapPosition.Y}");
                            ImGui.EndTooltip();
                        }
                    }

                    ImGui.TableNextColumn();
                    uint missionScore = entry.Value.ClassScore;
                    ImGui.SetNextItemWidth(100);
                    if (ImGui.InputUInt($"##MissionScore", ref missionScore))
                    {
                        entry.Value.ClassScore = missionScore;
                    }

                    if (CosmicHelper.GatheringItemDict.TryGetValue(entry.Key, out var gatherInfo))
                    {
                        foreach (var gatherEntry in gatherInfo.MinGatherItems)
                        {
                            ImGui.TableNextColumn();
                            string name = itemSheet.GetRow(gatherEntry.Key).Name.ToString();
                            ImGui.Text(name);
                            if (ImGui.IsItemHovered())
                            {
                                ImGui.BeginTooltip();
                                ImGui.Text($"{gatherEntry.Key}");
                                ImGui.EndTooltip();
                            }

                            ImGui.TableNextColumn();
                            ImGui.Text($"{gatherEntry.Value}");
                        }
                    }
                    else if (CosmicHelper.MoonRecipies.TryGetValue(entry.Key, out var recipeInfo))
                    {
                        foreach (var recipe in recipeInfo.MainCraftsDict)
                        {
                            ImGui.TableNextColumn();
                            ImGui.Text($"{recipe.Value.ItemId}");

                            ImGui.TableNextColumn();
                            ImGui.Text($"{recipe.Value.RequiredAmount}");
                        }

                        ImGui.TableSetColumnIndex(23);
                        foreach (var precraft in recipeInfo.PreCraftDict)
                        {
                            ImGui.Text($"{precraft.Value.ItemId}");

                            ImGui.TableNextColumn();
                            ImGui.Text($"{precraft.Value.RequiredAmount}");
                        }
                    }


                    ImGui.TableSetColumnIndex(25);
                    if (entry.Value.Jobs.Contains(18)) // Check if it's a fishing mission
                    {
                        if (ImGui.Button($"Export##Export-{entry.Key}"))
                        {
                            var fishingEntryCode = GenerateFishingEntryCode(entry.Key, entry.Value);
                            ImGui.SetClipboardText(fishingEntryCode);
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text("Export this fishing mission entry");
                            ImGui.EndTooltip();
                        }
                    }

                    ImGui.PopID();
                }

                ImGui.EndTable();
            }
        }

        public static string GenerateFishingEntryCode(uint missionId, dynamic missionInfo)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"        [{missionId}] = new FishingInfo");
            sb.AppendLine("        {");

            // Add RequiredFish if gathering items exist
            if (CosmicHelper.GatheringItemDict.TryGetValue(missionId, out var gatherInfo) && gatherInfo.MinGatherItems.Any())
            {
                sb.AppendLine("            RequiredFish = new Dictionary<string, HashSet<uint>>");
                sb.AppendLine("            {");
                sb.AppendLine("                // TODO: Define fish categories and their IDs");
                sb.AppendLine("                // Example: [\"CategoryName\"] = new HashSet<uint> { fish_id_1, fish_id_2 },");
                foreach (var item in gatherInfo.MinGatherItems)
                {
                    var itemSheet = ExcelHelper.ItemSheet;
                    string itemName = itemSheet.GetRow((uint)item.Key).Name.ToString();
                    sb.AppendLine($"                // {RemovePrivateUseChars(itemName)} (ID: {item.Key}) - Quantity: {item.Value}");
                }
                sb.AppendLine("            },");
            }
            else
            {
                sb.AppendLine("            // RequiredFish = new Dictionary<string, HashSet<uint>>(),");
            }

            // Add FishCountRequired - you might need to determine this from mission data
            sb.AppendLine("            FishCountRequired = 0, // TODO: Set based on mission requirements");

            // Add Bronze, Silver, Gold scores
            sb.AppendLine($"            BronzeScore = 0, // TODO: Determine bronze requirement");

            // Use existing Silver and Gold requirements if available
            if (HasProperty(missionInfo, "SilverRequirement"))
            {
                sb.AppendLine($"            SilverScore = {missionInfo.SilverRequirement},");
            }
            else
            {
                sb.AppendLine("            SilverScore = 0,");
            }

            if (HasProperty(missionInfo, "GoldRequirement"))
            {
                sb.AppendLine($"            GoldScore = {missionInfo.GoldRequirement},");
            }
            else
            {
                sb.AppendLine("            GoldScore = 0,");
            }

            sb.AppendLine("        },");

            return sb.ToString();
        }

        // Helper method to check if a dynamic object has a property
        private static bool HasProperty(dynamic obj, string propertyName)
        {
            try
            {
                var value = obj.GetType().GetProperty(propertyName)?.GetValue(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GenerateMissionScoreDictionaryCode()
        {
            var sb = new StringBuilder();
            sb.AppendLine("public static Dictionary<uint, uint> MissionScoreDict = new Dictionary<uint, uint>");
            sb.AppendLine("{");

            foreach (var kvp in CosmicHelper.SheetMissionDict)
            {
                sb.AppendLine($"    [{kvp.Key}] = {kvp.Value.ClassScore},");
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