using Dalamud.Interface;
using ECommons;
using ICE.Enums;
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
        private static uint tempAmount = 0;
        private static Dictionary<int, int> tempXpTable = new();
        private static uint tempBronzeScore = 0;
        private static uint tempSilverScore = 0;
        private static uint tempGoldScore = 0;
        private static MissionAttributes tempAttributes = MissionAttributes.None;
        private static CosmicWeather tempWeather = CosmicWeather.FairSkies;
        private static uint tempStartTime = 0;
        private static uint tempEndTime = 0;
        private static uint tempClassScore = 0;
        private static uint tempCosmoCredit = 0;
        private static uint tempLunarCredit = 0;
        private static Vector2 tempMapPosition = Vector2.Zero;
        private static int tempRadius = 0;
        private static uint tempTerritoryId = 0;
        private static HashSet<uint> tempJobs = new();
        private static HashSet<uint> tempPreviousMissions = new();

        private static int newXpType = 1;
        private static int newXpAmount = 0;

        private static Dictionary<string, int> tempItemAmounts = new();
        private static string newFishItemName = "";
        private static uint newFishItemId = 0;

        private static string missionSearch = "";
        private static uint jobToAdd = 0;
        private static uint missionToAdd = 0;
        private static Dictionary<string, uint> newIds = new();

        // For selective import
        private static bool showImportPopup = false;
        private static bool importAttributes = true;
        private static bool importWeather = true;
        private static bool importStartEndTime = true;
        private static bool importClassScore = true;
        private static bool importCredits = true;
        private static bool importPreviousMissions = true;
        private static bool importScores = true;
        private static bool importLocation = true;
        private static bool importJobs = true;
        private static bool importXpRewards = true;
        private static bool importCrafting = true;
        private static bool importGathering = true;

        // For importing items
        private static bool ImportToFisher = true;
        private static bool ImportToGathering = false;

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

                ImGui.SameLine();

                if (ImGui.Button("Import all missing missions"))
                {
                    ImportAllMissions();
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
                // Easy way to just sort by key. Nice instead of having to 
                foreach (var entry in CosmicHelper.Dict_CosmicMissions.OrderBy(x => x.Key))
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
                            isEditMode = false; // Making sure to exit the editing mode... can't be in an edit mid mission selection.
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

            ImGui.SameLine();
            if (ImGui.Button("Selective Import", new Vector2(120, 0)))
            {
                showImportPopup = true;
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

            // Handle selective import popup
            if (showImportPopup)
            {
                ImGui.OpenPopup("Selective Import");
                showImportPopup = false;
            }

            if (ImGui.BeginPopupModal("Selective Import", ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.Text($"Select which properties to import for Mission {selectedMission}:");
                ImGui.Separator();

                // Check if the mission exists in the old dictionary
                bool canImport = CosmicHelper.SheetMissionDict.ContainsKey(selectedMission);

                if (!canImport)
                {
                    ImGui.TextColored(new Vector4(1, 0, 0, 1), $"Mission {selectedMission} not found in old dictionary!");
                }
                else
                {
                    ImGui.Text("Basic Properties:");
                    ImGui.Checkbox("Attributes", ref importAttributes);
                    ImGui.Checkbox("Weather", ref importWeather);
                    ImGui.Checkbox("Start | End time", ref importStartEndTime);
                    ImGui.Checkbox("Class Score", ref importClassScore);

                    ImGui.Separator();
                    ImGui.Text("Rewards:");
                    ImGui.Checkbox("Credits (Cosmo/Lunar)", ref importCredits);
                    ImGui.Checkbox("XP Rewards", ref importXpRewards);

                    ImGui.Separator();
                    ImGui.Text("Requirements:");
                    ImGui.Checkbox("Scores (Bronze/Silver/Gold)", ref importScores);
                    ImGui.Checkbox("Previous Missions", ref importPreviousMissions);
                    ImGui.Checkbox("Jobs", ref importJobs);

                    ImGui.Separator();
                    ImGui.Text("Location & Content:");
                    ImGui.Checkbox("Location (Territory/Position/Radius)", ref importLocation);
                    ImGui.Checkbox("Crafting Info", ref importCrafting);
                    ImGui.Checkbox("Gathering Info", ref importGathering);
                }

                ImGui.Separator();

                if (canImport && ImGui.Button("Import Selected", new Vector2(120, 0)))
                {
                    SelectiveImportMission(selectedMission, missionInfo);
                    if (isEditMode)
                    {
                        // Refreshes the edit mode with the new imported values
                        StartEditMode(missionInfo);
                    }
                    ImGui.CloseCurrentPopup();
                }

                ImGui.SameLine();
                if (ImGui.Button("Select All", new Vector2(80, 0)))
                {
                    importAttributes = importWeather 
                    = importStartEndTime = importClassScore 
                    = importCredits = importPreviousMissions 
                    = importScores = importLocation 
                    = importJobs = importXpRewards 
                    = importCrafting = importGathering 
                    = true;
                }

                ImGui.SameLine();
                if (ImGui.Button("Clear All", new Vector2(80, 0)))
                {
                    importAttributes = importWeather 
                    = importStartEndTime = importClassScore 
                    = importCredits = importPreviousMissions 
                    = importScores = importLocation 
                    = importJobs = importXpRewards 
                    = importCrafting = importGathering 
                    = false;
                }

                ImGui.SameLine();
                if (ImGui.Button("Cancel", new Vector2(80, 0)))
                {
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndPopup();
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
                ImGui.InputInt("Radius", ref tempRadius);
                ImGui.InputUInt("Start Time", ref tempStartTime);
                ImGui.InputUInt("End Time", ref tempEndTime);

                // Attributes as checkboxes
                ImGui.Text("Attributes:");
                ImGui.Indent();

                // Get all defined enum values
                var attributeValues = Enum.GetValues<MissionAttributes>()
                    .Where(attr => attr != MissionAttributes.None)
                    .ToArray();

                foreach (var attribute in attributeValues)
                {
                    bool hasAttribute = tempAttributes.HasFlag(attribute);
                    if (ImGui.Checkbox(attribute.ToString(), ref hasAttribute))
                    {
                        if (hasAttribute)
                            tempAttributes |= attribute;
                        else
                            tempAttributes &= ~attribute;
                    }
                }

                ImGui.Unindent();

                // Weather as dropdown
                ImGui.Text("Weather:");
                ImGui.Indent();
                var weatherValues = Enum.GetValues<CosmicWeather>();
                int currentWeatherIndex = (int)tempWeather;

                if (ImGui.Combo("##Weather", ref currentWeatherIndex, weatherValues.Select(w => w.ToString()).ToArray(), weatherValues.Length))
                {
                    tempWeather = weatherValues[currentWeatherIndex];
                }
                ImGui.Unindent();

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
                ImGui.Text($"Start Time: {missionInfo.StartTime}:00");
                ImGui.Text($"End Time: {missionInfo.EndTime}:00");

                // Display attributes in a readable format
                if (missionInfo.Attributes != MissionAttributes.None)
                {
                    var attributes = (MissionAttributes)missionInfo.Attributes;
                    var attributesList = Enum.GetValues<MissionAttributes>()
                        .Where(attr => attr != MissionAttributes.None && attributes.HasFlag(attr))
                        .Select(attr => attr.ToString())
                        .ToList();

                    ImGui.Text($"Attributes: {string.Join(", ", attributesList)}");
                }
                else
                {
                    ImGui.Text("Attributes: None");
                }

                var weather = missionInfo.Weather;
                ImGui.Text($"Weather: {weather}");

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

            // Gathering minimum TODO: Make the add items specify which one to add to
            if (missionInfo.Gathering_Min != null && missionInfo.Gathering_Min.Count > 0)
            {
                List<uint> itemsToRemove = new();
                foreach (var item in missionInfo.Gathering_Min)
                {
                    uint itemId = item.Key;
                    var itemCount = item.Value;
                    var itemName = Svc.Data.GetExcelSheet<Item>().GetRow(itemId).Name;

                    ImGui.PushID(itemId);
                    ImGui.AlignTextToFramePadding();
                    ImGui.Text($"{itemName}");

                    if (isEditMode)
                    {
                        ImGui.SameLine();

                    }
                    else
                    {
                        ImGui.SameLine();
                        ImGui.Text($" | Id: {itemId} | Count: {itemCount}");
                    }

                    if (ImGui.Button($"Remove##Remove_{itemName}"))
                    {
                        itemsToRemove.Add(itemId);
                    }

                    ImGui.PopID();
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
                    ImGui.Text($"{itemName} Ids:");

                    if (isEditMode)
                    {
                        List<uint> idsToRemove = new();
                        List<uint> idsToAdd = new();

                        foreach (var id in itemIds)
                        {
                            ImGui.Text($"{id}");
                            ImGui.SameLine();

                            ImGui.PushFont(UiBuilder.IconFont);
                            if (ImGui.SmallButton($"{FontAwesomeIcon.Trash.ToIconString()}##remove{id}"))
                            {
                                idsToRemove.Add(id);
                            }
                            ImGui.PopFont();
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
                        ImGui.Text($" {string.Join(", ", itemIds)} | Count: {itemIds.Count}");
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
            // Initialize temp values with proper null checks and default values
            tempAmount = missionInfo.FishCountRequired ?? 0;
            tempBronzeScore = missionInfo.BronzeScore ?? 0;
            tempSilverScore = missionInfo.SilverScore ?? 0;
            tempGoldScore = missionInfo.GoldScore ?? 0;

            // Initialize XP table with null check
            tempXpTable = missionInfo.RelicXpInfo != null
                ? new Dictionary<int, int>(missionInfo.RelicXpInfo)
                : new Dictionary<int, int>();

            // Handle MissionAttributes enum properly
            tempAttributes = missionInfo.Attributes != null
                ? (MissionAttributes)missionInfo.Attributes
                : MissionAttributes.None;

            // Handle CosmicWeather enum properly  
            tempWeather = missionInfo.Weather != null
                ? (CosmicWeather)missionInfo.Weather
                : CosmicWeather.FairSkies;
            tempClassScore = missionInfo.ClassScore ?? 0;
            tempCosmoCredit = missionInfo.CosmoCredit ?? 0;
            tempLunarCredit = missionInfo.LunarCredit ?? 0;
            tempMapPosition = missionInfo.MapPosition ?? Vector2.Zero;
            tempRadius = missionInfo.Radius ?? 0f;
            tempTerritoryId = missionInfo.TerritoryId ?? 0;

            // Initialize collections with null checks
            tempJobs = missionInfo.Jobs != null
                ? new HashSet<uint>(missionInfo.Jobs)
                : new HashSet<uint>();

            tempPreviousMissions = missionInfo.PreviousMissions != null
                ? new HashSet<uint>(missionInfo.PreviousMissions)
                : new HashSet<uint>();
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
            missionInfo.StartTime = tempStartTime;
            missionInfo.EndTime = tempEndTime;
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

            sb.AppendLine("public static Dictionary<uint, CosmicInfo> Dict_CosmicMissions = new()");
            sb.AppendLine("{");

            // Sort by key to ensure numerical order
            foreach (var mission in CosmicHelper.Dict_CosmicMissions.OrderBy(x => x.Key))
            {
                uint missionId = mission.Key;
                var missionInfo = mission.Value;

                sb.AppendLine($"    [{missionId}] = new CosmicInfo");
                sb.AppendLine("    {");

                // All properties (rest of your existing code stays the same)
                sb.AppendLine($"        FishCountRequired = {missionInfo.FishCountRequired},");
                sb.AppendLine($"        BronzeScore = {missionInfo.BronzeScore},");
                sb.AppendLine($"        SilverScore = {missionInfo.SilverScore},");
                sb.AppendLine($"        GoldScore = {missionInfo.GoldScore},");
                string attributesString = FormatMissionAttributes(missionInfo.Attributes);
                sb.AppendLine($"        Attributes = {attributesString},");
                sb.AppendLine($"        Weather = CosmicWeather.{missionInfo.Weather},");
                sb.AppendLine($"        StartTime = {missionInfo.StartTime},");
                sb.AppendLine($"        EndTime = {missionInfo.EndTime},");
                sb.AppendLine($"        ClassScore = {missionInfo.ClassScore},");
                sb.AppendLine($"        CosmoCredit = {missionInfo.CosmoCredit},");
                sb.AppendLine($"        LunarCredit = {missionInfo.LunarCredit},");
                sb.AppendLine($"        TerritoryId = {missionInfo.TerritoryId},");
                sb.AppendLine($"        Radius = {missionInfo.Radius},");
                sb.AppendLine($"        MapPosition = new Vector2({missionInfo.MapPosition.X}f, {missionInfo.MapPosition.Y}f),");

                // Jobs
                if (missionInfo.Jobs != null && missionInfo.Jobs.Count > 0)
                {
                    sb.AppendLine($"        Jobs = new HashSet<uint> {{ {string.Join(", ", missionInfo.Jobs)} }},");
                }

                // Previous Missions
                if (missionInfo.PreviousMissions != null && missionInfo.PreviousMissions.Count > 0)
                {
                    sb.AppendLine($"        PreviousMissions = new HashSet<uint> {{ {string.Join(", ", missionInfo.PreviousMissions)} }},");
                }

                // RelicXpInfo
                if (missionInfo.RelicXpInfo != null && missionInfo.RelicXpInfo.Count > 0)
                {
                    sb.AppendLine("        RelicXpInfo = new Dictionary<int, int>");
                    sb.AppendLine("        {");
                    foreach (var xp in missionInfo.RelicXpInfo)
                    {
                        sb.AppendLine($"            [{xp.Key}] = {xp.Value},");
                    }
                    sb.AppendLine("        },");
                }

                // RequiredFish
                if (missionInfo.RequiredFish != null && missionInfo.RequiredFish.Any())
                {
                    sb.AppendLine("        RequiredFish = new Dictionary<string, HashSet<uint>>");
                    sb.AppendLine("        {");

                    foreach (var fish in missionInfo.RequiredFish)
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
                if (missionInfo.Crafts_Main != null && missionInfo.Crafts_Main.Count > 0)
                {
                    sb.AppendLine("        Crafts_Main = new Dictionary<ushort, int>");
                    sb.AppendLine("        {");
                    foreach (var craft in missionInfo.Crafts_Main)
                    {
                        sb.AppendLine($"            [{craft.Key}] = {craft.Value},");
                    }
                    sb.AppendLine("        },");
                }

                // Crafts_Pre
                if (missionInfo.Crafts_Pre != null && missionInfo.Crafts_Pre.Count > 0)
                {
                    sb.AppendLine("        Crafts_Pre = new Dictionary<ushort, int>");
                    sb.AppendLine("        {");
                    foreach (var craft in missionInfo.Crafts_Pre)
                    {
                        sb.AppendLine($"            [{craft.Key}] = {craft.Value},");
                    }
                    sb.AppendLine("        },");
                }

                // Gathering_Min
                if (missionInfo.Gathering_Min != null && missionInfo.Gathering_Min.Count > 0)
                {
                    sb.AppendLine("        Gathering_Min = new Dictionary<uint, int>");
                    sb.AppendLine("        {");
                    foreach (var gatherItem in missionInfo.Gathering_Min)
                    {
                        var itemId = gatherItem.Key;
                        var amount = gatherItem.Value;

                        sb.AppendLine($"            [{itemId}] = {amount},");
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
            string attributesString = FormatMissionAttributes(missionInfo.Attributes);
            sb.AppendLine($"    Attributes = {attributesString},");
            sb.AppendLine($"    Weather = CosmicWeather.{missionInfo.Weather},");
            sb.AppendLine($"    StartTime = {missionInfo.StartTime},");
            sb.AppendLine($"    EndTime = {missionInfo.EndTime},");
            sb.AppendLine($"    ClassScore = {missionInfo.ClassScore},");
            sb.AppendLine($"    CosmoCredit = {missionInfo.CosmoCredit},");
            sb.AppendLine($"    LunarCredit = {missionInfo.LunarCredit},");
            sb.AppendLine($"    TerritoryId = {missionInfo.TerritoryId},");
            sb.AppendLine($"    Radius = {missionInfo.Radius},");
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
                sb.AppendLine("    Crafts_Main = new Dictionary<ushort, int>");
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
                sb.AppendLine("    Crafts_Pre = new Dictionary<ushort, int>");
                sb.AppendLine("    {");
                foreach (var craft in missionInfo.Crafts_Pre)
                {
                    sb.AppendLine($"        [{craft.Key}] = {craft.Value},");
                }
                sb.AppendLine("    },");
            }

            // Gathering_Min
            if (missionInfo.Gathering_Min != null && missionInfo.Gathering_Min.Count > 0)
            {
                sb.AppendLine($"    Gathering_Min = new Dictionary<uint, int>");
                sb.AppendLine("     {");
                foreach (var gatherItem in missionInfo.Gathering_Min)
                {
                    var itemId = gatherItem.Key;
                    var amount = gatherItem.Value;

                    sb.AppendLine($"        [{itemId}] = {amount}, ");
                }
                sb.AppendLine($"    }}");
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
                        ClassScore = mission.ClassScore,
                        CosmoCredit = mission.CosmoCredit,
                        LunarCredit = mission.LunarCredit,
                        PreviousMissions = mission.PreviousMissions,
                        BronzeScore = mission.BronzeScore,
                        SilverScore = mission.SilverScore,
                        GoldScore = mission.GoldScore,
                        Radius = mission.Radius,
                        TerritoryId = mission.TerritoryId,
                    };

                    var newEntry = CosmicHelper.Dict_CosmicMissions[missionId];
                    if (mission.MapPosition == new Vector2(-1024, -1024))
                        newEntry.MapPosition = new Vector2(0, 0);
                    else
                        newEntry.MapPosition = mission.MapPosition;

                    newEntry.StartTime = mission.StartTime;
                    newEntry.EndTime = mission.EndTime;

                    newEntry.Jobs = mission.Jobs;

                    foreach (var xp in mission.RelicXpInfo.OrderBy(x => x.Key))
                    {
                        newEntry.RelicXpInfo[xp.Key] = xp.Value;
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

        private static void ImportAllMissions()
        {
            foreach (var mission in CosmicHelper.SheetMissionDict)
            {
                var missionId = mission.Key;
                if (!CosmicHelper.Dict_CosmicMissions.ContainsKey(missionId))
                {
                    CosmicHelper.Dict_CosmicMissions[missionId] = new()
                    {
                        Attributes = mission.Value.Attributes,
                        Weather = mission.Value.Weather,
                        ClassScore = mission.Value.ClassScore,
                        CosmoCredit = mission.Value.CosmoCredit,
                        LunarCredit = mission.Value.LunarCredit,
                        PreviousMissions = mission.Value.PreviousMissions,
                        BronzeScore = mission.Value.BronzeScore,
                        SilverScore = mission.Value.SilverScore,
                        GoldScore = mission.Value.GoldScore,
                        Radius = mission.Value.Radius,
                        TerritoryId = mission.Value.TerritoryId,
                    };

                    var newEntry = CosmicHelper.Dict_CosmicMissions[missionId];
                    if (mission.Value.MapPosition == new Vector2(-1024, -1024))
                        newEntry.MapPosition = new Vector2(0, 0);
                    else
                        newEntry.MapPosition = mission.Value.MapPosition;

                    newEntry.StartTime = mission.Value.StartTime;
                    newEntry.EndTime = mission.Value.EndTime;


                    newEntry.Jobs = mission.Value.Jobs;

                    foreach (var xp in mission.Value.RelicXpInfo.OrderBy(x => x.Key))
                    {
                        newEntry.RelicXpInfo[xp.Key] = xp.Value;
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

        private static void SelectiveImportMission(uint missionId, dynamic missionInfo)
        {
            if (!CosmicHelper.SheetMissionDict.TryGetValue(missionId, out var mission))
                return;

            // Basic Properties
            if (importAttributes)
                missionInfo.Attributes = mission.Attributes;

            if (importWeather)
                missionInfo.Weather = mission.Weather;

            if (importClassScore)
                missionInfo.ClassScore = mission.ClassScore;

            if (importStartEndTime)
            {
                missionInfo.StartTime = mission.StartTime;
                missionInfo.EndTime = mission.EndTime;
            }

            // Rewards
            if (importCredits)
            {
                missionInfo.CosmoCredit = mission.CosmoCredit;
                missionInfo.LunarCredit = mission.LunarCredit;
            }

            if (importXpRewards)
            {
                if (missionInfo.RelicXpInfo == null)
                    missionInfo.RelicXpInfo = new Dictionary<int, int>();

                foreach (var xp in mission.RelicXpInfo.OrderBy(x => x.Key))
                {
                    missionInfo.RelicXpInfo[xp.Key] = xp.Value;
                }
            }

            // Requirements
            if (importScores)
            {
                missionInfo.BronzeScore = mission.BronzeScore;
                missionInfo.SilverScore = mission.SilverScore;
                missionInfo.GoldScore = mission.GoldScore;
            }

            if (importPreviousMissions)
            {
                if (missionInfo.PreviousMissions == null)
                    missionInfo.PreviousMissions = new HashSet<uint>();

                if (!mission.PreviousMissions.Contains(0))
                    missionInfo.PreviousMissions = mission.PreviousMissions;
            }

            if (importJobs)
            {
                if (missionInfo.Jobs == null)
                    missionInfo.Jobs = new HashSet<uint>();

                missionInfo.Jobs = mission.Jobs;
            }

            // Location & Content
            if (importLocation)
            {
                missionInfo.TerritoryId = mission.TerritoryId;
                missionInfo.MapPosition = mission.MapPosition;
                missionInfo.Radius = mission.Radius;
            }

            if (importCrafting)
            {
                if (CosmicHelper.MoonRecipies.TryGetValue(missionId, out var craftingInfo))
                {
                    if (craftingInfo.MainCraftsDict.Count > 0)
                        missionInfo.Crafts_Main = craftingInfo.MainCraftsDict;
                    if (craftingInfo.PreCraftDict.Count > 0 && craftingInfo.PreCrafts)
                        missionInfo.Crafts_Pre = craftingInfo.PreCraftDict;
                }
            }

            if (importGathering)
            {
                if (CosmicHelper.GatheringItemDict.TryGetValue(missionId, out var gatheringInfo))
                {
                    missionInfo.Gathering_Min = gatheringInfo.MinGatherItems;
                }
            }
        }

        private static string FormatMissionAttributes(MissionAttributes attributes)
        {
            if (attributes == MissionAttributes.None)
                return "MissionAttributes.None";

            // Get all the individual flags that are set
            var setFlags = Enum.GetValues<MissionAttributes>()
                .Where(flag => flag != MissionAttributes.None && attributes.HasFlag(flag))
                .Select(flag => $"MissionAttributes.{flag}")
                .ToList();

            // If only one flag is set, return it directly
            if (setFlags.Count == 1)
                return setFlags[0];

            // If multiple flags are set, join them with " | "
            return string.Join(" | ", setFlags);
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