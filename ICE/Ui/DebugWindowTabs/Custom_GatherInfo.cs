using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICE.Utilities.CosmicHelper;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Custom_GatherInfo
    {
        private static uint addMissionId = 0;
        private static uint selectedMission = 0;

        private static string searchItem = "";
        private static int currentPage = 0;
        private static int itemsPerPage = 10;
        private static string selectedItemKey = "";

        public static void Draw()
        {
            ImGui.InputUInt("Add MissionID", ref addMissionId);
            if (ImGui.Button("Add mission"))
            {
                CosmicHelper.CustomFishDict[addMissionId] = new();
            }
            Vector2 windowSize = ImGui.GetContentRegionAvail();
            float leftPanelWidth = windowSize.X * 0.3f; // 60% for mission list
            float rightPanelWidth = windowSize.X * 0.7f - ImGui.GetStyle().ItemSpacing.X; // 40% for details

            if (ImGui.BeginChild("Mission Entries Window", new Vector2(leftPanelWidth, 0), true))
            {
                MissionSelect();

                ImGui.EndChild();
            }
            ImGui.SameLine();
            if (ImGui.BeginChild("Mission Details Window", new Vector2(rightPanelWidth, 0), true))
            {
                MissionDetails();

                ImGui.EndChild();
            }
        }

        private static void MissionSelect()
        {
            if (CosmicHelper.CustomFishDict.Count > 0)
            {
                foreach (var entry in CosmicHelper.CustomFishDict)
                {
                    var id = entry.Key;
                    string missionName = MissionInfoDict[id].Name;

                    // Pass true for selected parameter when this item matches selectedMission
                    bool isSelected = selectedMission == id;

                    if (ImGui.Selectable($"[{id}] - {missionName}", isSelected))
                    {
                        selectedMission = id;
                    }
                }
            }
            else
            {
                ImGui.Text("No current entries exist");
            }
        }

        private static void MissionDetails()
        {
            if (CosmicHelper.CustomFishDict.TryGetValue(selectedMission, out var fishDict))
            {
                if (ImGui.Button("Add item to dictionary"))
                {
                    ImGui.OpenPopup("Item Addition to mission");
                }

                ImGui.SameLine();

                if (ImGui.Button("Copy Mission to Clipboard"))
                {
                    string formattedCode = FormatFishingMissionForClipboard(selectedMission, fishDict);
                    ImGui.SetClipboardText(formattedCode);
                }

                if (ImGui.BeginPopup("Item Addition to mission"))
                {
                    ImGui.InputText("Search for item", ref searchItem);
                    ImGui.Separator();

                    // Filter items based on search
                    var filteredItems = GatheringItems
                        .Where(kvp => string.IsNullOrEmpty(searchItem) ||
                                     kvp.Key.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    // Calculate pagination
                    int totalItems = filteredItems.Count;
                    int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

                    // Ensure current page is valid
                    if (currentPage >= totalPages && totalPages > 0)
                        currentPage = totalPages - 1;
                    if (currentPage < 0)
                        currentPage = 0;

                    // Get items for current page
                    var pageItems = filteredItems
                        .Skip(currentPage * itemsPerPage)
                        .Take(itemsPerPage)
                        .ToList();

                    // Display pagination info
                    ImGui.Text($"Page {currentPage + 1} of {Math.Max(1, totalPages)} ({totalItems} total items)");

                    // Pagination controls
                    if (ImGui.Button("Previous") && currentPage > 0)
                        currentPage--;

                    ImGui.SameLine();

                    if (ImGui.Button("Next") && currentPage < totalPages - 1)
                        currentPage++;

                    ImGui.Separator();

                    // Display selectable items
                    foreach (var kvp in pageItems)
                    {
                        bool isSelected = selectedItemKey == kvp.Key;
                        if (ImGui.Selectable(kvp.Key, isSelected))
                        {
                            selectedItemKey = kvp.Key;
                            if (GatheringItems.TryGetValue(selectedItemKey, out var gatherItem))
                            {
                                fishDict.fishingItems[selectedItemKey] = gatherItem.itemIds;
                            }
                        }
                    }

                    ImGui.EndPopup();
                }

                int amount = fishDict.amount;
                ImGui.SetNextItemWidth(200);
                if (ImGui.InputInt("Amount to gather", ref amount))
                {
                    fishDict.amount = amount;
                }

                foreach (var item in fishDict.fishingItems)
                {
                    string itemName = item.Key;

                    ImGui.AlignTextToFramePadding();
                    ImGui.Text($"{itemName} | Amount: {item.Value.Count}");

                    ImGui.SameLine();
                    if (ImGui.Button($"Remove##Remove_{item}"))
                    {
                        fishDict.fishingItems.Remove(item);
                    }
                }
            }
        }

        private static string FormatFishingMissionForClipboard(uint missionId, FishingMissionInfo fishingMission)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"[{missionId}] = new FishingMissionInfo");
            sb.AppendLine("{");
            sb.AppendLine("    fishingItems = new Dictionary<string, HashSet<uint>>");
            sb.AppendLine("    {");

            foreach (var item in fishingMission.fishingItems)
            {
                string itemIds = "{ " + string.Join(", ", item.Value) + " }";
                sb.AppendLine($"        [\"{item.Key}\"] = new HashSet<uint> {itemIds},");
            }

            sb.AppendLine("    },");
            sb.AppendLine($"    amount = {fishingMission.amount}");
            sb.AppendLine("},");

            return sb.ToString();
        }
    }
}