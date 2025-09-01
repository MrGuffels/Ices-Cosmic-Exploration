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

        private static bool isEditMode = false;

        // Temporary edit fields
        private static uint tempAmount;
        private static Dictionary<int, int> tempXpTable = new();
        private static uint tempBronzeScore;
        private static uint tempSilverScore;
        private static uint tempGoldScore;

        // Additional temp fields for other properties
        private static uint tempAttributes;
        private static uint tempWeather;
        private static uint tempClassScore;
        private static uint tempCosmoCredit;
        private static uint tempLunarCredit;
        private static Vector2 tempMapPosition;
        private static float tempRadius;
        private static uint tempTerritoryId;
        private static HashSet<uint> tempJobs = new();
        private static HashSet<uint> tempPreviousMissions = new();

        // For adding new XP entries
        private static int newXpType = 1;
        private static int newXpAmount = 0;

        // For editing individual items in RequiredFish
        private static Dictionary<string, int> tempItemAmounts = new();
        private static string newFishItemName = "";
        private static uint newFishItemId = 0;

        private static string missionSearch = "";
        private static uint jobToAdd = 0;
        private static uint missionToAdd = 0;
        private static Dictionary<string, uint> newIds = new();

        public static void Draw()
        {
            if (ImGui.BeginChild("TopControls", new Vector2(0, 100), true))
            {
                ImGui.InputUInt("Add MissionID", ref addMissionId);

                if (ImGui.Button("Add mission"))
                {
                    CosmicHelper.Dict_CosmicMissions[addMissionId] = new();
                }
                ImGui.SameLine();

                if (ImGui.Button("Import from old"))
                {
                    ImportMission(addMissionId);
                }
                ImGui.SameLine();

                if (ImGui.Button("Delete Selected Mission") && selectedMission != 0)
                {
                    ImGui.OpenPopup("Confirm Delete");
                }
                ImGui.SameLine();

                if (ImGui.Button("Copy All Missions"))
                {
                    string formattedCode = CopyAllMissions();
                    ImGui.SetClipboardText(formattedCode);
                }

                if (ImGui.BeginPopupModal("Confirm Delete"))
                {
                    ImGui.Text($"Are you sure you want to delete mission {selectedMission}?");

                    if (ImGui.Button("Yes"))
                    {
                        CosmicHelper.Dict_CosmicMissions.Remove(selectedMission);
                        selectedMission = 0;
                        ImGui.CloseCurrentPopup();
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("No"))
                    {
                        ImGui.CloseCurrentPopup();
                    }

                    ImGui.EndPopup();
                }

                ImGui.EndChild();
            }

            Vector2 windowSize = ImGui.GetContentRegionAvail();
            float leftPanelWidth = windowSize.X * 0.5f;
            float rightPanelWidth = windowSize.X * 0.5f - ImGui.GetStyle().ItemSpacing.X;

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
            ImGui.InputText("Search Missions", ref missionSearch);
            ImGui.Separator();

            if (CosmicHelper.Dict_CosmicMissions.Count > 0)
            {
                foreach (var entry in CosmicHelper.Dict_CosmicMissions)
                {
                    var id = entry.Key;
                    string missionName = SheetMissionDict.ContainsKey(id)
                        ? SheetMissionDict[id].Name
                        : "Unknown Mission";

                    if (!string.IsNullOrEmpty(missionSearch) &&
                        !missionName.Contains(missionSearch, StringComparison.OrdinalIgnoreCase) &&
                        !id.ToString().Contains(missionSearch))
                    {
                        continue;
                    }

                    bool isSelected = selectedMission == id;
                    if (ImGui.Selectable($"[{id}] - {missionName}", isSelected))
                    {
                        selectedMission = id;
                        if (isEditMode)
                        {
                            isEditMode = false;
                        }
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
            if (!CosmicHelper.Dict_CosmicMissions.TryGetValue(selectedMission, out var missionInfo))
            {
                ImGui.Text("Select a mission to view details");
                return;
            }

            string missionName = CosmicHelper.SheetMissionDict.ContainsKey(selectedMission)
                ? CosmicHelper.SheetMissionDict[selectedMission].Name
                : "Unknown Mission";

            ImGui.Text($"Mission: [{selectedMission}] {missionName}");
            ImGui.Separator();

            if (ImGui.Button(isEditMode ? "Save" : "Edit", new Vector2(80, 0)))
            {
                if (isEditMode)
                {
                    SaveChanges(missionInfo);
                }
                else
                {
                    StartEditMode(missionInfo);
                }
                isEditMode = !isEditMode;
            }

            ImGui.SameLine();
            if (isEditMode && ImGui.Button("Cancel", new Vector2(80, 0)))
            {
                isEditMode = false;
            }

            ImGui.SameLine();
            if (ImGui.Button("Copy Mission to Clipboard", new Vector2(180, 0)))
            {
                string formattedCode = CopySpecificMission();
                ImGui.SetClipboardText(formattedCode);
            }

            ImGui.Separator();

            if (ImGui.BeginTabBar("MissionTabs"))
            {
                if (ImGui.BeginTabItem("Basic Info"))
                {
                    DrawBasicInfo(selectedMission);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Requirements"))
                {
                    DrawRequirements(selectedMission);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Rewards"))
                {
                    DrawRewards(selectedMission);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Items/Fish"))
                {
                    DrawItemsAndFish(selectedMission);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Crafting"))
                {
                    DrawCrafting(selectedMission);
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }

        private static void DrawBasicInfo(uint missionId)
        {
            var missionInfo = CosmicHelper.Dict_CosmicMissions[missionId];

            ImGui.Text("Basic Information");
            ImGui.Separator();

            if (isEditMode)
            {
                ImGui.InputUInt("Territory ID", ref tempTerritoryId);
                ImGui.InputFloat2("Map Position", ref tempMapPosition);
                ImGui.InputFloat("Radius", ref tempRadius);
                ImGui.InputUInt("Attributes", ref tempAttributes);
                ImGui.InputUInt("Weather", ref tempWeather);
                ImGui.InputUInt("Class Score", ref tempClassScore);

                ImGui.Text("Jobs:");
                ImGui.Indent();

                ImGui.InputUInt("Add Job ID", ref jobToAdd);
                ImGui.SameLine();
                if (ImGui.Button("Add Job") && jobToAdd != 0)
                {
                    tempJobs.Add(jobToAdd);
                    jobToAdd = 0;
                }

                List<uint> jobsToRemove = new();
                foreach (var job in tempJobs)
                {
                    ImGui.Text($"Job: {job}");
                    ImGui.SameLine();
                    if (ImGui.SmallButton($"Remove##job{job}"))
                    {
                        jobsToRemove.Add(job);
                    }
                }
                foreach (var job in jobsToRemove)
                {
                    tempJobs.Remove(job);
                }

                ImGui.Unindent();

                ImGui.Text("Previous Missions:");
                ImGui.Indent();

                ImGui.InputUInt("Add Previous Mission", ref missionToAdd);
                ImGui.SameLine();
                if (ImGui.Button("Add Mission") && missionToAdd != 0)
                {
                    tempPreviousMissions.Add(missionToAdd);
                    missionToAdd = 0;
                }

                List<uint> missionsToRemove = new();
                foreach (var mission in tempPreviousMissions)
                {
                    ImGui.Text($"Mission: {mission}");
                    ImGui.SameLine();
                    if (ImGui.SmallButton($"Remove##mission{mission}"))
                    {
                        missionsToRemove.Add(mission);
                    }
                }
                foreach (var mission in missionsToRemove)
                {
                    tempPreviousMissions.Remove(mission);
                }

                ImGui.Unindent();
            }
            else
            {
                ImGui.Text($"Territory ID: {missionInfo.TerritoryId}");
                ImGui.Text($"Map Position: {missionInfo.MapPosition}");
                ImGui.Text($"Radius: {missionInfo.Radius}");
                ImGui.Text($"Attributes: {missionInfo.Attributes}");
                ImGui.Text($"Weather: {missionInfo.Weather}");
                ImGui.Text($"Class Score: {missionInfo.ClassScore}");

                if (missionInfo.Jobs != null && missionInfo.Jobs.Count > 0)
                {
                    ImGui.Text($"Jobs: {string.Join(", ", missionInfo.Jobs)}");
                }

                if (missionInfo.PreviousMissions != null && missionInfo.PreviousMissions.Count > 0)
                {
                    ImGui.Text($"Previous Missions: {string.Join(", ", missionInfo.PreviousMissions)}");
                }
            }
        }

        private static void DrawRequirements(uint missionId)
        {
            var missionInfo = CosmicHelper.Dict_CosmicMissions[missionId];

            ImGui.Text("Requirements & Scores");
            ImGui.Separator();

            if (isEditMode)
            {
                ImGui.InputUInt("Amount to gather", ref tempAmount);
                ImGui.InputUInt("Bronze Score", ref tempBronzeScore);
                ImGui.InputUInt("Silver Score", ref tempSilverScore);
                ImGui.InputUInt("Gold Score", ref tempGoldScore);
            }
            else
            {
                ImGui.Text($"Amount to gather: {missionInfo.FishCountRequired}");
                ImGui.Text($"Bronze Score: {missionInfo.BronzeScore}");
                ImGui.Text($"Silver Score: {missionInfo.SilverScore}");
                ImGui.Text($"Gold Score: {missionInfo.GoldScore}");
            }
        }

        private static void DrawRewards(uint missionId)
        {
            var missionInfo = CosmicHelper.Dict_CosmicMissions[missionId];

            ImGui.Text("Rewards");
            ImGui.Separator();

            if (isEditMode)
            {
                ImGui.InputUInt("Cosmo Credit", ref tempCosmoCredit);
                ImGui.InputUInt("Lunar Credit", ref tempLunarCredit);

                ImGui.Separator();
                ImGui.Text("XP Rewards:");

                ImGui.PushID("AddXP");
                ImGui.SetNextItemWidth(100);
                ImGui.InputInt("Type", ref newXpType);
                ImGui.SameLine();
                ImGui.SetNextItemWidth(100);
                ImGui.InputInt("Amount", ref newXpAmount);
                ImGui.SameLine();
                if (ImGui.Button("Add XP Entry"))
                {
                    if (!tempXpTable.ContainsKey(newXpType))
                    {
                        tempXpTable[newXpType] = newXpAmount;
                    }
                }
                ImGui.PopID();

                if (ImGui.BeginTable("XP Edit Table", 3, ImGuiTableFlags.Borders))
                {
                    ImGui.TableSetupColumn("Type");
                    ImGui.TableSetupColumn("Amount");
                    ImGui.TableSetupColumn("Actions");
                    ImGui.TableHeadersRow();

                    List<int> keysToRemove = new();
                    Dictionary<int, int> updates = new();

                    foreach (var xp in tempXpTable)
                    {
                        ImGui.TableNextRow();
                        ImGui.TableSetColumnIndex(0);
                        ImGui.Text(XpKind(xp.Key));

                        ImGui.TableSetColumnIndex(1);
                        int amount = xp.Value;
                        ImGui.SetNextItemWidth(100);
                        if (ImGui.InputInt($"##xpamount{xp.Key}", ref amount))
                        {
                            updates[xp.Key] = amount;
                        }

                        ImGui.TableSetColumnIndex(2);
                        if (ImGui.SmallButton($"Remove##xp{xp.Key}"))
                        {
                            keysToRemove.Add(xp.Key);
                        }
                    }

                    foreach (var key in keysToRemove)
                    {
                        tempXpTable.Remove(key);
                    }

                    foreach (var update in updates)
                    {
                        tempXpTable[update.Key] = update.Value;
                    }

                    ImGui.EndTable();
                }
            }
            else
            {
                ImGui.Text($"Cosmo Credit: {missionInfo.CosmoCredit}");
                ImGui.Text($"Lunar Credit: {missionInfo.LunarCredit}");

                if (missionInfo.RelicXpInfo != null && missionInfo.RelicXpInfo.Count > 0)
                {
                    ImGui.Separator();
                    ImGui.Text("XP Rewards:");

                    if (ImGui.BeginTable("Mission XP Info", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.SizingFixedFit))
                    {
                        ImGui.TableSetupColumn("Type");
                        ImGui.TableSetupColumn("Amount");
                        ImGui.TableHeadersRow();

                        foreach (var xp in missionInfo.RelicXpInfo)
                        {
                            ImGui.TableNextRow();
                            ImGui.TableSetColumnIndex(0);
                            ImGui.Text(XpKind(xp.Key));
                            ImGui.TableSetColumnIndex(1);
                            ImGui.Text($"{xp.Value}");
                        }

                        ImGui.EndTable();
                    }
                }
            }
        }

        private static void DrawItemsAndFish(uint missionId)
        {
            var missionInfo = CosmicHelper.Dict_CosmicMissions[missionId];

            ImGui.Text("Required Items/Fish");
            ImGui.Separator();

            if (ImGui.Button("Add item to dictionary"))
            {
                ImGui.OpenPopup("Item Addition to mission");
            }

            if (isEditMode)
            {
                ImGui.SameLine();
                ImGui.Text("(Edit mode - You can modify item counts)");
            }

            // Gathering minimum
            if (missionInfo.Gathering_Min != null)
            {
                ImGui.Text($"Minimum Gathering Items: {missionInfo.Gathering_Min}");
                if (isEditMode)
                {
                    // TODO: Add Editing the Gathering info
                }
            }

            ImGui.Separator();

            if (missionInfo.RequiredFish != null && missionInfo.RequiredFish.Count > 0)
            {
                List<string> itemsToRemove = new();

                foreach (var item in missionInfo.RequiredFish)
                {
                    string itemName = item.Key;
                    var itemIds = item.Value;

                    ImGui.PushID(itemName);
                    ImGui.AlignTextToFramePadding();
                    ImGui.Text($"{itemName}");

                    if (isEditMode)
                    {
                        // Allow editing individual item IDs
                        ImGui.SameLine();
                        ImGui.Text("IDs:");

                        List<uint> idsToRemove = new();
                        List<uint> idsToAdd = new();

                        foreach (var id in itemIds)
                        {
                            ImGui.SameLine();
                            ImGui.Text($"{id}");
                            ImGui.SameLine();
                            if (ImGui.SmallButton($"X##remove{id}"))
                            {
                                idsToRemove.Add(id);
                            }
                        }

                        if (!newIds.ContainsKey(itemName))
                            newIds[itemName] = 0;

                        uint newId = newIds[itemName];
                        ImGui.SetNextItemWidth(100);
                        if (ImGui.InputUInt($"##newid{itemName}", ref newId))
                        {
                            newIds[itemName] = newId;
                        }
                        ImGui.SameLine();
                        if (ImGui.SmallButton($"Add ID##add{itemName}") && newId != 0)
                        {
                            itemIds.Add(newId);
                            newIds[itemName] = 0;
                        }

                        foreach (var id in idsToRemove)
                        {
                            itemIds.Remove(id);
                        }
                    }
                    else
                    {
                        ImGui.SameLine();
                        ImGui.Text($"| IDs: {string.Join(", ", itemIds)} | Count: {itemIds.Count}");
                    }

                    ImGui.SameLine();
                    if (ImGui.Button($"Remove##Remove_{itemName}"))
                    {
                        itemsToRemove.Add(itemName);
                    }

                    ImGui.PopID();
                }

                foreach (var item in itemsToRemove)
                {
                    missionInfo.RequiredFish.Remove(item);
                }
            }
            else
            {
                ImGui.Text("No required items/fish");
            }

            if (ImGui.BeginPopup("Item Addition to mission"))
            {
                ImGui.InputText("Search for item", ref searchItem);
                ImGui.Separator();

                var filteredItems = GatheringItems
                    .Where(kvp => string.IsNullOrEmpty(searchItem) ||
                                 kvp.Key.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                int totalItems = filteredItems.Count;
                int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

                if (currentPage >= totalPages && totalPages > 0)
                    currentPage = totalPages - 1;
                if (currentPage < 0)
                    currentPage = 0;

                var pageItems = filteredItems
                    .Skip(currentPage * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();

                ImGui.Text($"Page {currentPage + 1} of {Math.Max(1, totalPages)} ({totalItems} total items)");

                if (ImGui.Button("Previous") && currentPage > 0)
                    currentPage--;

                ImGui.SameLine();

                if (ImGui.Button("Next") && currentPage < totalPages - 1)
                    currentPage++;

                ImGui.Separator();

                foreach (var kvp in pageItems)
                {
                    bool isSelected = selectedItemKey == kvp.Key;
                    if (ImGui.Selectable(kvp.Key, isSelected))
                    {
                        selectedItemKey = kvp.Key;
                        if (GatheringItems.TryGetValue(selectedItemKey, out var gatherItem))
                        {
                            if (missionInfo.RequiredFish == null)
                                missionInfo.RequiredFish = new Dictionary<string, HashSet<uint>>();

                            missionInfo.RequiredFish[selectedItemKey] = new HashSet<uint>(gatherItem.itemIds);
                        }
                        ImGui.CloseCurrentPopup();
                    }
                }

                ImGui.EndPopup();
            }
        }

        private static void DrawCrafting(uint missionId)
        {
            var missionInfo = CosmicHelper.Dict_CosmicMissions[missionId];

            ImGui.Text("Crafting Information");
            ImGui.Separator();

            if (missionInfo.Crafts_Main != null && missionInfo.Crafts_Main.Count > 0)
            {
                ImGui.Text("Main Crafts:");
                ImGui.Indent();
                foreach (var craft in missionInfo.Crafts_Main)
                {
                    ImGui.Text($"Recipe {craft.Key}: Count = {craft.Value}");
                    if (isEditMode)
                    {
                        // TODO: Add Crafting Editor
                    }
                }
                ImGui.Unindent();
            }

            if (missionInfo.Crafts_Pre != null && missionInfo.Crafts_Pre.Count > 0)
            {
                ImGui.Text("Pre Crafts:");
                ImGui.Indent();
                foreach (var craft in missionInfo.Crafts_Pre)
                {
                    ImGui.Text($"Recipe {craft.Key}: Count = {craft.Value}");
                    if (isEditMode)
                    {
                        // TODO: Add Pre-Craft Editor
                    }
                }
                ImGui.Unindent();
            }

            if ((missionInfo.Crafts_Main == null || missionInfo.Crafts_Main.Count == 0) &&
                (missionInfo.Crafts_Pre == null || missionInfo.Crafts_Pre.Count == 0))
            {
                ImGui.Text("No crafting requirements");
            }
        }

        private static void StartEditMode(dynamic missionInfo)
        {
            tempAmount = missionInfo.FishCountRequired;
            tempBronzeScore = missionInfo.BronzeScore;
            tempSilverScore = missionInfo.SilverScore;
            tempGoldScore = missionInfo.GoldScore;
            tempXpTable = new Dictionary<int, int>(missionInfo.RelicXpInfo ?? new Dictionary<int, int>());

            tempAttributes = missionInfo.Attributes;
            tempWeather = missionInfo.Weather;
            tempClassScore = missionInfo.ClassScore;
            tempCosmoCredit = missionInfo.CosmoCredit;
            tempLunarCredit = missionInfo.LunarCredit;
            tempMapPosition = missionInfo.MapPosition;
            tempRadius = missionInfo.Radius;
            tempTerritoryId = missionInfo.TerritoryId;

            if (missionInfo.Jobs != null)
                tempJobs = new HashSet<uint>(missionInfo.Jobs);
            else
                tempJobs = new HashSet<uint>();

            if (missionInfo.PreviousMissions != null)
                tempPreviousMissions = new HashSet<uint>(missionInfo.PreviousMissions);
            else
                tempPreviousMissions = new HashSet<uint>();
        }

        private static void SaveChanges(dynamic missionInfo)
        {
            missionInfo.FishCountRequired = tempAmount;
            missionInfo.BronzeScore = tempBronzeScore;
            missionInfo.SilverScore = tempSilverScore;
            missionInfo.GoldScore = tempGoldScore;
            missionInfo.RelicXpInfo = new Dictionary<int, int>(tempXpTable);

            missionInfo.Attributes = tempAttributes;
            missionInfo.Weather = tempWeather;
            missionInfo.ClassScore = tempClassScore;
            missionInfo.CosmoCredit = tempCosmoCredit;
            missionInfo.LunarCredit = tempLunarCredit;
            missionInfo.MapPosition = tempMapPosition;
            missionInfo.Radius = tempRadius;
            missionInfo.TerritoryId = tempTerritoryId;
            missionInfo.Jobs = new HashSet<uint>(tempJobs);
            missionInfo.PreviousMissions = new HashSet<uint>(tempPreviousMissions);
        }

        private static string CopyAllMissions()
        {
            var sb = new StringBuilder();

            sb.AppendLine("public static Dictionary<uint, FishingInfo> FishingMission = new()");
            sb.AppendLine("{");

            foreach (var mission in CosmicHelper.Dict_CosmicMissions)
            {
                uint missionId = mission.Key;
                var fishingInfo = mission.Value;

                sb.AppendLine($"    [{missionId}] = new FishingInfo");
                sb.AppendLine("    {");

                // All properties
                sb.AppendLine($"        FishCountRequired = {fishingInfo.FishCountRequired},");
                sb.AppendLine($"        BronzeScore = {fishingInfo.BronzeScore},");
                sb.AppendLine($"        SilverScore = {fishingInfo.SilverScore},");
                sb.AppendLine($"        GoldScore = {fishingInfo.GoldScore},");
                sb.AppendLine($"        Attributes = {fishingInfo.Attributes},");
                sb.AppendLine($"        Weather = {fishingInfo.Weather},");
                sb.AppendLine($"        ClassScore = {fishingInfo.ClassScore},");
                sb.AppendLine($"        CosmoCredit = {fishingInfo.CosmoCredit},");
                sb.AppendLine($"        LunarCredit = {fishingInfo.LunarCredit},");
                sb.AppendLine($"        TerritoryId = {fishingInfo.TerritoryId},");
                sb.AppendLine($"        Radius = {fishingInfo.Radius}f,");
                sb.AppendLine($"        MapPosition = new Vector2({fishingInfo.MapPosition.X}f, {fishingInfo.MapPosition.Y}f),");

                // Jobs
                if (fishingInfo.Jobs != null && fishingInfo.Jobs.Count > 0)
                {
                    sb.AppendLine($"        Jobs = new HashSet<uint> {{ {string.Join(", ", fishingInfo.Jobs)} }},");
                }

                // Previous Missions
                if (fishingInfo.PreviousMissions != null && fishingInfo.PreviousMissions.Count > 0)
                {
                    sb.AppendLine($"        PreviousMissions = new HashSet<uint> {{ {string.Join(", ", fishingInfo.PreviousMissions)} }},");
                }

                // RelicXpInfo
                if (fishingInfo.RelicXpInfo != null && fishingInfo.RelicXpInfo.Count > 0)
                {
                    sb.AppendLine("        RelicXpInfo = new Dictionary<int, int>");
                    sb.AppendLine("        {");
                    foreach (var xp in fishingInfo.RelicXpInfo)
                    {
                        sb.AppendLine($"            [{xp.Key}] = {xp.Value},");
                    }
                    sb.AppendLine("        },");
                }

                // RequiredFish
                if (fishingInfo.RequiredFish != null && fishingInfo.RequiredFish.Any())
                {
                    sb.AppendLine("        RequiredFish = new Dictionary<string, HashSet<uint>>");
                    sb.AppendLine("        {");

                    foreach (var fish in fishingInfo.RequiredFish)
                    {
                        string fishName = fish.Key;
                        var itemIds = fish.Value;

                        sb.Append($"            [\"{fishName}\"] = new HashSet<uint> {{ ");
                        sb.Append(string.Join(", ", itemIds));
                        sb.AppendLine(" },");
                    }

                    sb.AppendLine("        },");
                }

                // Crafts_Main
                if (fishingInfo.Crafts_Main != null && fishingInfo.Crafts_Main.Count > 0)
                {
                    sb.AppendLine("        Crafts_Main = new Dictionary<uint, int>");
                    sb.AppendLine("        {");
                    foreach (var craft in fishingInfo.Crafts_Main)
                    {
                        sb.AppendLine($"            [{craft.Key}] = {craft.Value},");
                    }
                    sb.AppendLine("        },");
                }

                // Crafts_Pre
                if (fishingInfo.Crafts_Pre != null && fishingInfo.Crafts_Pre.Count > 0)
                {
                    sb.AppendLine("        Crafts_Pre = new Dictionary<uint, int>");
                    sb.AppendLine("        {");
                    foreach (var craft in fishingInfo.Crafts_Pre)
                    {
                        sb.AppendLine($"            [{craft.Key}] = {craft.Value},");
                    }
                    sb.AppendLine("        },");
                }

                sb.AppendLine("    },");
            }

            sb.AppendLine("};");

            return sb.ToString();
        }

        private static string CopySpecificMission()
        {
            if (!CosmicHelper.Dict_CosmicMissions.TryGetValue(selectedMission, out var missionInfo))
            {
                return "// No mission selected or mission not found";
            }

            var sb = new StringBuilder();

            sb.AppendLine($"[{selectedMission}] = new CosmicInfo");
            sb.AppendLine("{");

            // All properties
            sb.AppendLine($"    FishCountRequired = {missionInfo.FishCountRequired},");
            sb.AppendLine($"    BronzeScore = {missionInfo.BronzeScore},");
            sb.AppendLine($"    SilverScore = {missionInfo.SilverScore},");
            sb.AppendLine($"    GoldScore = {missionInfo.GoldScore},");
            sb.AppendLine($"    Attributes = {missionInfo.Attributes},");
            sb.AppendLine($"    Weather = {missionInfo.Weather},");
            sb.AppendLine($"    ClassScore = {missionInfo.ClassScore},");
            sb.AppendLine($"    CosmoCredit = {missionInfo.CosmoCredit},");
            sb.AppendLine($"    LunarCredit = {missionInfo.LunarCredit},");
            sb.AppendLine($"    TerritoryId = {missionInfo.TerritoryId},");
            sb.AppendLine($"    Radius = {missionInfo.Radius}f,");
            sb.AppendLine($"    MapPosition = new Vector2({missionInfo.MapPosition.X}f, {missionInfo.MapPosition.Y}f),");

            // Jobs
            if (missionInfo.Jobs != null && missionInfo.Jobs.Count > 0)
            {
                sb.AppendLine($"    Jobs = new HashSet<uint> {{ {string.Join(", ", missionInfo.Jobs)} }},");
            }

            // Previous Missions
            if (missionInfo.PreviousMissions != null && missionInfo.PreviousMissions.Count > 0)
            {
                sb.AppendLine($"    PreviousMissions = new HashSet<uint> {{ {string.Join(", ", missionInfo.PreviousMissions)} }},");
            }

            // RelicXpInfo
            if (missionInfo.RelicXpInfo != null && missionInfo.RelicXpInfo.Count > 0)
            {
                sb.AppendLine("    RelicXpInfo = new Dictionary<int, int>");
                sb.AppendLine("    {");
                foreach (var xp in missionInfo.RelicXpInfo)
                {
                    sb.AppendLine($"        [{xp.Key}] = {xp.Value},");
                }
                sb.AppendLine("    },");
            }

            // RequiredFish
            if (missionInfo.RequiredFish != null && missionInfo.RequiredFish.Any())
            {
                sb.AppendLine("    RequiredFish = new Dictionary<string, HashSet<uint>>");
                sb.AppendLine("    {");

                foreach (var fish in missionInfo.RequiredFish)
                {
                    string fishName = fish.Key;
                    var itemIds = fish.Value;

                    sb.Append($"        [\"{fishName}\"] = new HashSet<uint> {{ ");
                    sb.Append(string.Join(", ", itemIds));
                    sb.AppendLine(" },");
                }

                sb.AppendLine("    },");
            }

            // Crafts_Main
            if (missionInfo.Crafts_Main != null && missionInfo.Crafts_Main.Count > 0)
            {
                sb.AppendLine("    Crafts_Main = new Dictionary<uint, int>");
                sb.AppendLine("    {");
                foreach (var craft in missionInfo.Crafts_Main)
                {
                    sb.AppendLine($"        [{craft.Key}] = {craft.Value},");
                }
                sb.AppendLine("    },");
            }

            // Crafts_Pre
            if (missionInfo.Crafts_Pre != null && missionInfo.Crafts_Pre.Count > 0)
            {
                sb.AppendLine("    Crafts_Pre = new Dictionary<uint, int>");
                sb.AppendLine("    {");
                foreach (var craft in missionInfo.Crafts_Pre)
                {
                    sb.AppendLine($"        [{craft.Key}] = {craft.Value},");
                }
                sb.AppendLine("    },");
            }

            // Gathering_Min
            if (missionInfo.Gathering_Min != null)
            {
                sb.AppendLine($"    Gathering_Min = {missionInfo.Gathering_Min},");
            }

            sb.AppendLine("},");

            return sb.ToString();
        }

        private static void ImportMission(uint missionId)
        {
            if (CosmicHelper.SheetMissionDict.TryGetValue(missionId, out var mission))
            {
                if (!CosmicHelper.Dict_CosmicMissions.ContainsKey(missionId))
                {
                    CosmicHelper.Dict_CosmicMissions[missionId] = new()
                    {
                        Attributes = mission.Attributes,
                        Weather = mission.Weather,
                        ClassScore = mission.missionScore,
                        CosmoCredit = mission.CosmoCredit,
                        LunarCredit = mission.LunarCredit,
                        PreviousMissions = new() { mission.PreviousMissionID },
                        BronzeScore = mission.BronzeRequirement,
                        SilverScore = mission.SilverRequirement,
                        GoldScore = mission.GoldRequirement,
                        MapPosition = mission.MapPosition,
                        Radius = mission.Radius,
                        TerritoryId = mission.TerritoryId,
                    };

                    var newEntry = CosmicHelper.Dict_CosmicMissions[missionId];
                    newEntry.Jobs.Add(mission.JobId);
                    if (mission.JobId2 != 0) newEntry.Jobs.Add(mission.JobId2);
                    if (mission.JobId3 != 0) newEntry.Jobs.Add(mission.JobId3);

                    foreach (var xp in mission.ExperienceRewards)
                    {
                        newEntry.RelicXpInfo[xp.Type] = xp.Amount;
                    }

                    // General information is added. Time to check for crafting/gathering/fishing specifics
                    if (CosmicHelper.MoonRecipies.TryGetValue(missionId, out var craftingInfo))
                    {
                        if (craftingInfo.MainCraftsDict.Count > 0)
                            newEntry.Crafts_Main = craftingInfo.MainCraftsDict;
                        if (craftingInfo.PreCraftDict.Count > 0 && craftingInfo.PreCrafts)
                            newEntry.Crafts_Pre = craftingInfo.PreCraftDict;
                    }

                    if (CosmicHelper.GatheringItemDict.TryGetValue(missionId, out var gatheringInfo))
                    {
                        newEntry.Gathering_Min = gatheringInfo.MinGatherItems;
                    }
                }
            }
        }

        private static string XpKind(int type)
        {
            switch (type)
            {
                case 1:
                    return "I";
                case 2:
                    return "II";
                case 3:
                    return "III";
                case 4:
                    return "IV";
                case 5:
                    return "V";
                default:
                    return $"Type {type}";
            }
        }
    }
}