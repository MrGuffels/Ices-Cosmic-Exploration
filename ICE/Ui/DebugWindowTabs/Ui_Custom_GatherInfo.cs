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
    internal class Ui_Custom_GatherInfo
    {
        private static uint addMissionId = 0;
        private static uint selectedMission = 0;

        private static string searchItem = "";
        private static int currentPage = 0;
        private static int itemsPerPage = 10;
        private static string selectedItemKey = "";

        private static bool isEditMode = false;

        // Pagination and filtering for mission list
        private static int missionCurrentPage = 0;
        private static int missionsPerPage = 20;
        private static string missionSearch = "";
        private static int selectedJobFilter = -1; // -1 = All, 8-18 = specific job
        private static int sortMode = 0; // 0 = ID ascending, 1 = ID descending, 2 = Name ascending, 3 = Name descending

        // Mass edit functionality
        private static bool showMassEditPopup = false;
        private static HashSet<uint> selectedMissionsForMassEdit = new();
        private static bool selectAll = false;

        // Mass edit import flags (same as selective import)
        private static bool massImportAttributes = true;
        private static bool massImportWeather = true;
        private static bool massImportStartEndTime = true;
        private static bool massImportClassScore = true;
        private static bool massImportCredits = true;
        private static bool massImportPreviousMissions = true;
        private static bool massImportScores = true;
        private static bool massImportLocation = true;
        private static bool massImportJobs = true;
        private static bool massImportXpRewards = true;
        private static bool massImportCrafting = true;
        private static bool massImportGathering = true;

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

                // New mass edit button
                ImGui.SameLine();
                if (ImGui.Button("Mass Edit Existing"))
                {
                    showMassEditPopup = true;
                    selectedMissionsForMassEdit.Clear();
                    selectAll = false;
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
            float leftPanelWidth = 600f; // Fixed width for mission list
            float rightPanelWidth = windowSize.X - leftPanelWidth - ImGui.GetStyle().ItemSpacing.X;

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

            // Handle mass edit popup
            HandleMassEditPopup();
        }

        private static void MissionSelect()
        {
            // Search and filtering controls
            ImGui.SetNextItemWidth(100);
            ImGui.InputText("Search Missions", ref missionSearch);

            // Job filter dropdown
            ImGui.SetNextItemWidth(100);
            string[] jobFilterOptions = new string[] { "All Jobs" }.Concat(
                Enumerable.Range(8, 11).Select(i => $"Job {i}")
            ).ToArray();

            int jobFilterDisplay = selectedJobFilter + 1; // Convert -1 to 0 for display
            if (ImGui.Combo("Job Filter", ref jobFilterDisplay, jobFilterOptions, jobFilterOptions.Length))
            {
                selectedJobFilter = jobFilterDisplay - 1; // Convert back to -1 for "All"
                missionCurrentPage = 0; // Reset to first page when filter changes
            }

            // Sort mode dropdown
            ImGui.SameLine();
            ImGui.SetNextItemWidth(120);
            string[] sortOptions = { "ID Ascending", "ID Descending", "Name Ascending", "Name Descending" };
            if (ImGui.Combo("Sort", ref sortMode, sortOptions, sortOptions.Length))
            {
                missionCurrentPage = 0; // Reset to first page when sort changes
            }

            ImGui.Separator();

            if (CosmicHelper.Dict_CosmicMissions.Count > 0)
            {
                // Get filtered and sorted missions
                var filteredMissions = GetFilteredAndSortedMissions();

                // Pagination calculations
                int totalMissions = filteredMissions.Count;
                int totalPages = (int)Math.Ceiling((double)totalMissions / missionsPerPage);

                if (missionCurrentPage >= totalPages && totalPages > 0)
                    missionCurrentPage = totalPages - 1;
                if (missionCurrentPage < 0)
                    missionCurrentPage = 0;

                // Pagination controls
                ImGui.Text($"Page {missionCurrentPage + 1} of {Math.Max(1, totalPages)} ({totalMissions} missions)");

                if (ImGui.Button("First") && missionCurrentPage > 0)
                    missionCurrentPage = 0;
                ImGui.SameLine();

                if (ImGui.Button("Previous") && missionCurrentPage > 0)
                    missionCurrentPage--;
                ImGui.SameLine();

                if (ImGui.Button("Next") && missionCurrentPage < totalPages - 1)
                    missionCurrentPage++;
                ImGui.SameLine();

                if (ImGui.Button("Last") && missionCurrentPage < totalPages - 1)
                    missionCurrentPage = totalPages - 1;

                ImGui.SameLine();
                ImGui.SetNextItemWidth(80);
                int displayPage = missionCurrentPage + 1;
                if (ImGui.InputInt("##PageInput", ref displayPage))
                {
                    displayPage = Math.Clamp(displayPage, 1, Math.Max(1, totalPages));
                    missionCurrentPage = displayPage - 1;
                }

                ImGui.Separator();

                // Display missions for current page
                var pageMissions = filteredMissions
                    .Skip(missionCurrentPage * missionsPerPage)
                    .Take(missionsPerPage)
                    .ToList();

                foreach (var mission in pageMissions)
                {
                    var id = mission.Key;
                    string missionName = SheetMissionDict.ContainsKey(id)
                        ? SheetMissionDict[id].Name
                        : "Unknown Mission";

                    // Get job info for display
                    string jobInfo = "";
                    if (mission.Value.Jobs != null && mission.Value.Jobs.Count > 0)
                    {
                        jobInfo = $" [Jobs: {string.Join(",", mission.Value.Jobs)}]";
                    }

                    bool isSelected = selectedMission == id;
                    if (ImGui.Selectable($"[{id}] - {missionName}{jobInfo}", isSelected))
                    {
                        selectedMission = id;
                        if (isEditMode)
                        {
                            isEditMode = false; // Exit edit mode when switching missions
                        }
                    }
                }
            }
            else
            {
                ImGui.Text("No current entries exist");
            }
        }

        private static List<KeyValuePair<uint, CosmicInfo>> GetFilteredAndSortedMissions()
        {
            var missions = CosmicHelper.Dict_CosmicMissions.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(missionSearch))
            {
                missions = missions.Where(m =>
                {
                    string missionName = SheetMissionDict.ContainsKey(m.Key)
                        ? SheetMissionDict[m.Key].Name
                        : "Unknown Mission";

                    return missionName.Contains(missionSearch, StringComparison.OrdinalIgnoreCase) ||
                           m.Key.ToString().Contains(missionSearch);
                });
            }

            // Apply job filter
            if (selectedJobFilter >= 8 && selectedJobFilter <= 18)
            {
                uint jobFilter = (uint)selectedJobFilter;
                missions = missions.Where(m =>
                    m.Value.Jobs != null && m.Value.Jobs.Contains(jobFilter));
            }

            // Apply sorting
            missions = sortMode switch
            {
                0 => missions.OrderBy(m => m.Key), // ID Ascending
                1 => missions.OrderByDescending(m => m.Key), // ID Descending
                2 => missions.OrderBy(m => SheetMissionDict.ContainsKey(m.Key) ? SheetMissionDict[m.Key].Name : "Unknown Mission"), // Name Ascending
                3 => missions.OrderByDescending(m => SheetMissionDict.ContainsKey(m.Key) ? SheetMissionDict[m.Key].Name : "Unknown Mission"), // Name Descending
                _ => missions.OrderBy(m => m.Key)
            };

            return missions.ToList();
        }

        private static void HandleMassEditPopup()
        {
            if (showMassEditPopup)
            {
                ImGui.OpenPopup("Mass Edit Missions");
                showMassEditPopup = false;
            }

            if (ImGui.BeginPopupModal("Mass Edit Missions", ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.Text("Select missions to update from old dictionary:");
                ImGui.Separator();

                // Selection controls
                if (ImGui.Checkbox("Select All Visible", ref selectAll))
                {
                    var filteredMissions = GetFilteredAndSortedMissions();
                    if (selectAll)
                    {
                        foreach (var mission in filteredMissions)
                        {
                            selectedMissionsForMassEdit.Add(mission.Key);
                        }
                    }
                    else
                    {
                        selectedMissionsForMassEdit.Clear();
                    }
                }

                ImGui.SameLine();
                ImGui.Text($"Selected: {selectedMissionsForMassEdit.Count}");

                if (ImGui.Button("Clear Selection"))
                {
                    selectedMissionsForMassEdit.Clear();
                    selectAll = false;
                }

                ImGui.Separator();

                // Mission selection list (with same filtering as main view)
                if (ImGui.BeginChild("MissionSelection", new Vector2(400, 300), true))
                {
                    var filteredMissions = GetFilteredAndSortedMissions();

                    foreach (var mission in filteredMissions)
                    {
                        var id = mission.Key;
                        string missionName = SheetMissionDict.ContainsKey(id)
                            ? SheetMissionDict[id].Name
                            : "Unknown Mission";

                        bool isSelected = selectedMissionsForMassEdit.Contains(id);
                        if (ImGui.Checkbox($"[{id}] - {missionName}", ref isSelected))
                        {
                            if (isSelected)
                                selectedMissionsForMassEdit.Add(id);
                            else
                                selectedMissionsForMassEdit.Remove(id);
                        }
                    }

                    ImGui.EndChild();
                }

                ImGui.Separator();

                // Import options (same as selective import)
                ImGui.Text("Select properties to import from old dictionary:");

                ImGui.Text("Basic Properties:");
                ImGui.Checkbox("Attributes##mass", ref massImportAttributes);
                ImGui.Checkbox("Weather##mass", ref massImportWeather);
                ImGui.Checkbox("Start | End time##mass", ref massImportStartEndTime);
                ImGui.Checkbox("Class Score##mass", ref massImportClassScore);

                ImGui.Separator();
                ImGui.Text("Rewards:");
                ImGui.Checkbox("Credits (Cosmo/Lunar)##mass", ref massImportCredits);
                ImGui.Checkbox("XP Rewards##mass", ref massImportXpRewards);

                ImGui.Separator();
                ImGui.Text("Requirements:");
                ImGui.Checkbox("Scores (Bronze/Silver/Gold)##mass", ref massImportScores);
                ImGui.Checkbox("Previous Missions##mass", ref massImportPreviousMissions);
                ImGui.Checkbox("Jobs##mass", ref massImportJobs);

                ImGui.Separator();
                ImGui.Text("Location & Content:");
                ImGui.Checkbox("Location (Territory/Position/Radius)##mass", ref massImportLocation);
                ImGui.Checkbox("Crafting Info##mass", ref massImportCrafting);
                ImGui.Checkbox("Gathering Info##mass", ref massImportGathering);

                ImGui.Separator();

                // Action buttons
                if (ImGui.Button("Execute Mass Import", new Vector2(150, 0)) && selectedMissionsForMassEdit.Count > 0)
                {
                    ExecuteMassImport();
                    ImGui.CloseCurrentPopup();
                }

                ImGui.SameLine();
                if (ImGui.Button("Select All Properties", new Vector2(140, 0)))
                {
                    massImportAttributes = massImportWeather = massImportStartEndTime = massImportClassScore
                    = massImportCredits = massImportPreviousMissions = massImportScores = massImportLocation
                    = massImportJobs = massImportXpRewards = massImportCrafting = massImportGathering = true;
                }

                ImGui.SameLine();
                if (ImGui.Button("Clear All Properties", new Vector2(130, 0)))
                {
                    massImportAttributes = massImportWeather = massImportStartEndTime = massImportClassScore
                    = massImportCredits = massImportPreviousMissions = massImportScores = massImportLocation
                    = massImportJobs = massImportXpRewards = massImportCrafting = massImportGathering = false;
                }

                ImGui.SameLine();
                if (ImGui.Button("Cancel", new Vector2(80, 0)))
                {
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndPopup();
            }
        }

        private static void ExecuteMassImport()
        {
            int successCount = 0;
            int failCount = 0;

            foreach (uint missionId in selectedMissionsForMassEdit)
            {
                if (CosmicHelper.Dict_CosmicMissions.TryGetValue(missionId, out var missionInfo))
                {
                    if (SelectiveImportMission(missionId, missionInfo,
                        massImportAttributes, massImportWeather, massImportStartEndTime, massImportClassScore,
                        massImportCredits, massImportXpRewards, massImportScores, massImportPreviousMissions,
                        massImportJobs, massImportLocation, massImportCrafting, massImportGathering))
                    {
                        successCount++;
                    }
                    else
                    {
                        failCount++;
                    }
                }
            }

            // You might want to show a message about the results
            // ImGui notification or console output could go here
            Console.WriteLine($"Mass import completed: {successCount} successful, {failCount} failed");
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

            // Handle selective import popup (existing code)
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
                    SelectiveImportMission(selectedMission, missionInfo,
                        importAttributes, importWeather, importStartEndTime, importClassScore,
                        importCredits, importXpRewards, importScores, importPreviousMissions,
                        importJobs, importLocation, importCrafting, importGathering);
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

        // Rest of the existing methods remain the same...
        // I'll include the modified SelectiveImportMission method and keep the rest unchanged

        private static bool SelectiveImportMission(uint missionId, CosmicInfo missionInfo,
            bool importAttributes, bool importWeather, bool importStartEndTime, bool importClassScore,
            bool importCredits, bool importXpRewards, bool importScores, bool importPreviousMissions,
            bool importJobs, bool importLocation, bool importCrafting, bool importGathering)
        {
            if (!CosmicHelper.SheetMissionDict.TryGetValue(missionId, out var mission))
                return false;

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

                missionInfo.RelicXpInfo.Clear();
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
                    missionInfo.PreviousMissions = new HashSet<uint>(mission.PreviousMissions);
            }

            if (importJobs)
            {
                if (missionInfo.Jobs == null)
                    missionInfo.Jobs = new HashSet<uint>();

                missionInfo.Jobs = new HashSet<uint>(mission.Jobs);
            }

            // Location & Content
            if (importLocation)
            {
                missionInfo.TerritoryId = mission.TerritoryId;
                missionInfo.MapPosition = mission.MapPosition == new Vector2(-1024, -1024) ? new Vector2(0, 0) : mission.MapPosition;
                missionInfo.Radius = mission.Radius;
            }

            if (importCrafting)
            {
                if (CosmicHelper.MoonRecipies.TryGetValue(missionId, out var craftingInfo))
                {
                    if (craftingInfo.MainCraftsDict.Count > 0)
                        missionInfo.Crafts_Main = new Dictionary<ushort, CosmicHelper.CraftingInfo>(craftingInfo.MainCraftsDict);
                    if (craftingInfo.PreCraftDict.Count > 0)
                        missionInfo.Crafts_Pre = new Dictionary<ushort, CosmicHelper.CraftingInfo>(craftingInfo.PreCraftDict);
                }
            }

            if (importGathering)
            {
                if (CosmicHelper.GatheringItemDict.TryGetValue(missionId, out var gatheringInfo))
                {
                    missionInfo.Gathering_Min = new Dictionary<uint, int>(gatheringInfo.MinGatherItems);
                }
            }

            return true;
        }

        // Keep all other existing methods unchanged...
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

            // Gathering minimum 
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
                        // Add edit functionality here if needed
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

            tempStartTime = missionInfo.StartTime ?? 0;
            tempEndTime = missionInfo.EndTime ?? 0;
            tempClassScore = missionInfo.ClassScore ?? 0;
            tempCosmoCredit = missionInfo.CosmoCredit ?? 0;
            tempLunarCredit = missionInfo.LunarCredit ?? 0;
            tempMapPosition = missionInfo.MapPosition ?? Vector2.Zero;
            tempRadius = missionInfo.Radius ?? 0;
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

                // All properties
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
                sb.AppendLine("    Gathering_Min = new Dictionary<uint, int>");
                sb.AppendLine("    {");
                foreach (var gatherItem in missionInfo.Gathering_Min)
                {
                    var itemId = gatherItem.Key;
                    var amount = gatherItem.Value;

                    sb.AppendLine($"        [{itemId}] = {amount},");
                }
                sb.AppendLine("    },");
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
                        if (craftingInfo.PreCraftDict.Count > 0)
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
                        if (craftingInfo.PreCraftDict.Count > 0)
                            newEntry.Crafts_Pre = craftingInfo.PreCraftDict;
                    }

                    if (CosmicHelper.GatheringItemDict.TryGetValue(missionId, out var gatheringInfo))
                    {
                        newEntry.Gathering_Min = gatheringInfo.MinGatherItems;
                    }
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