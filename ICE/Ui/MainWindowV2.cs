using Dalamud.Game.Text;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Utility.Table;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using ICE.Enums;
using ICE.Utilities.Cosmic;
using Lumina.Excel.Sheets;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using System.Xml.Schema;
using static ICE.Utilities.CosmicHelper;

namespace ICE.Ui
{
    internal class MainWindowV2 : Window
    {
        public MainWindowV2() :
#if DEBUG
            base($"Ice's Cosmic Exploration {P.GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion} Debug build ###ICEMainWindowV2")
#else
            base($"Ice's Cosmic Exploration {P.GetType().Assembly.GetName().Version} ###ICEMainWindow")
#endif
        {
            Flags = ImGuiWindowFlags.None;

            // Set up size constraints to ensure window cannot be too small or too large.
            // Increased minimum size to better accommodate larger font sizes.
            SizeConstraints = new()
            {
                MinimumSize = new Vector2(100, 100),
                MaximumSize = new Vector2(2000, 3000)
            };

            P.windowSystem.AddWindow(this);

            AllowPinning = true;
            AllowClickthrough = true;
        }

        public void Dispose()
        {
            P.windowSystem.RemoveWindow(this);
        }

        // Available jobs and their IDs.
        // Matching up to the sheet `ClassJob` vs `ClassJobCategory` for future use (idk why that sheet even exist...)
        public static List<(string Name, uint Id)> jobOptions = new()
        {
            ("CRP", 8),
            ("BSM", 9),
            ("ARM", 10),
            ("GSM", 11),
            ("LTW", 12),
            ("WVR", 13),
            ("ALC", 14),
            ("CUL", 15),
            ("MIN", 16),
            ("BTN", 17),
            ("FSH", 18),
        };

        // Available mission ranks and their identifiers.
        private static List<(uint RankId, string RankName)> rankOptions = new()
        {
            (1, "D"),
            (2, "C"),
            (3, "B"),
            (4, "A")
        };

        private uint? currentJobId => PlayerHelper.GetClassJobId();
        private bool usingSupportedJob => jobOptions.Any(job => job.Id == currentJobId);
        private uint selectedJob = C.SelectedJob;

        // Left Column Settings
        private bool onlyGrabMission = C.OnlyGrabMission;
        private bool stopCosmic = C.StopOnceHitCosmoCredits;
        private int cosmicCap = C.CosmoCreditsCap;
        private bool stopLunar = C.StopOnceHitLunarCredits;
        private int lunarCap = C.LunarCreditsCap;
        private bool stopScore = C.StopOnceHitCosmicScore;
        private int scoreCap = C.CosmicScoreCap;
        private bool autoPickCurrentJob = C.AutoPickCurrentJob;
        private bool stopWhenLevel = C.StopWhenLevel;
        private int targetLevel = C.TargetLevel;

        private bool showCritical = C.ShowCritical;
        private bool showSequential = C.ShowSequential;
        private bool showWeather = C.ShowWeather;
        private bool showTimeRestricted = C.ShowTimeRestricted;
        private bool showClassA = C.ShowClassA;
        private bool showClassB = C.ShowClassB;
        private bool showClassC = C.ShowClassC;
        private bool showClassD = C.ShowClassD;

        // Middle Column stuff
        private Dictionary<string, bool> headerStates = new();
        private bool hideUnsupported = C.HideUnsupportedMissions;

        private bool showTableSetting = false;

        private static Dictionary<string, List<(uint id, bool gather, bool enabled)>> missionList = new()
        {
            ["Critical"] = new List<(uint id, bool gather, bool enabled)>(),
            ["Weather"] = new List<(uint id, bool gather, bool enabled)>(),
            ["Timed"] = new List<(uint id, bool gather, bool enabled)> (),
            ["Sequence"] = new List<(uint id, bool gather, bool enabled)> (),
            ["ARank"] = new List<(uint id, bool gather, bool enabled)> (),
            ["BRank"] = new List<(uint id, bool gather, bool enabled)> (),
            ["CRank"] = new List<(uint id, bool gather, bool enabled)> (),
            ["DRank"] = new List<(uint id, bool gather, bool enabled)> ()
        };

        private string[] missionSortOptions = ["Id", "Name", "Cosmo Credits", "Lunar Credits", "Exp I", "Exp II", "Exp III", "Exp IV", "Map Location"];
        private int missionSelectedOption = C.TableSortOption;
        private List<(uint id, bool gather, bool enabled)> SortMissionList(List<(uint id, bool gather, bool enabled)> missions)
        {
            int sortOption = missionSelectedOption;
            var missionInfo = CosmicHelper.MissionInfoDict;

            switch (sortOption)
            {
                case 0: // Sorting by Id
                    return missions.ToList();
                case 1: // Name 
                    return missions.OrderBy(m => missionInfo[m.id].Name).ToList();
                case 2: // Cosmo Credits
                    return missions.OrderByDescending(m => missionInfo[m.id].CosmoCredit).ToList();
                case 3: // Lunar Credits
                    return missions.OrderByDescending(m => missionInfo[m.id].LunarCredit).ToList();
                case 4: // Exp Type 0:
                    return missions.OrderByDescending(m => missionInfo[m.id].ExperienceRewards
                                                     .Where(exp => exp.Type == 1)
                                                     .Sum(exp => exp.Amount)).ToList();
                case 5: // Exp Type 1:
                    return missions.OrderByDescending(m => missionInfo[m.id].ExperienceRewards
                                                     .Where(exp => exp.Type == 2)
                                                     .Sum(exp => exp.Amount)).ToList();
                case 6: // Exp Type 2:
                    return missions.OrderByDescending(m => missionInfo[m.id].ExperienceRewards
                                                     .Where(exp => exp.Type == 3)
                                                     .Sum(exp => exp.Amount)).ToList();
                case 7: // Exp Type 3:
                    return missions.OrderByDescending(m => missionInfo[m.id].ExperienceRewards
                                                     .Where(exp => exp.Type == 4)
                                                     .Sum(exp => exp.Amount)).ToList();
                case 8: // Map Location
                    return missions.OrderBy(m => missionInfo[m.id].MarkerId).ToList();
                default:
                    return missions.ToList();
            }
        }

        private string[] missionOptions = ["Current Class", "All Missions", "Currently Enabled"];
        private string selectedOption = "Current Class";

        // Right Column stuff
        private uint selectedMission = 0;

        public override void Draw()
        {
            // Calculate scaling factors based on current font size
            float fontScale = ImGui.GetIO().FontGlobalScale;
            float textLineHeight = ImGui.GetTextLineHeight();
            float scaledSpacing = ImGui.GetStyle().ItemSpacing.Y * fontScale;
            float headerPadding = textLineHeight * 1.2f;

            float headerHeight = textLineHeight + headerPadding * 2;
            float contentAreaHeight = ImGui.GetWindowHeight() - headerHeight - 4;
            float labelHeight = ImGui.GetTextLineHeightWithSpacing();
            float childHeight = ImGui.GetContentRegionAvail().Y;

            // Setting up the columns to be 3 right here. 
            float leftPanelWidth = Math.Max(220, textLineHeight * 14);
            float middlePanelWidth = Math.Max(0, textLineHeight * 22);

            ImGui.Columns(3, "Main Window", false);
            // ----------------------------
            //  LEFT PANEL (Start/Stop, Class Selection, Filter Ui)
            // ----------------------------

            ImGui.SetColumnWidth(0, leftPanelWidth);

            if (ImGui.BeginChild("Filter Panel##Filter Panel", new Vector2(0, childHeight), true))
            {
                ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 4.0f);

                using (ImRaii.Disabled(SchedulerMain.State != IceState.Idle || !usingSupportedJob))
                {
                    if (ImGui.Button("Start", new Vector2(ImGui.GetContentRegionAvail().X, textLineHeight * 1.5f)))
                    {
                        SchedulerMain.EnablePlugin();
                    }
                }

                using (ImRaii.Disabled(SchedulerMain.State == IceState.Idle))
                {
                    if (ImGui.Button("Stop", new Vector2(ImGui.GetContentRegionAvail().X, textLineHeight * 1.5f)))
                    {
                        SchedulerMain.DisablePlugin();
                    }
                }
                if (ImGui.Button("Settings", new Vector2(ImGui.GetContentRegionAvail().X, textLineHeight * 1.5f)))
                {
                    P.settingsWindowV2.IsOpen = !P.settingsWindowV2.IsOpen;
                }

                if (ImGui.Checkbox($"Only grab mission", ref onlyGrabMission))
                {
                    C.OnlyGrabMission = onlyGrabMission;
                    C.Save();
                }

                ImGui.PopStyleVar();

                ImGui.Spacing();

                ImGui.Separator();

                ImGui.Spacing();

                ImGui.Checkbox("Stop after current mission", ref Mission_Settings.StopBeforeGrab);
                if (ImGui.Checkbox($"Stop at Cosmic Credits", ref stopCosmic))
                {
                    C.StopOnceHitCosmoCredits = stopCosmic;
                    C.Save();
                }
                if (stopCosmic)
                {
                    ImGui.Indent(15);
                    ImGui.SetNextItemWidth(-1);
                    if (ImGui.SliderInt("##CosmicStop", ref cosmicCap, 0, 30000))
                    {
                        C.CosmoCreditsCap = cosmicCap;
                        C.Save();
                    }
                    ImGui.Unindent(15);
                }

                if (ImGui.Checkbox($"Stop at Lunar Credits", ref stopLunar))
                {
                    C.StopOnceHitLunarCredits = stopLunar;
                    C.Save();
                }
                if (stopLunar)
                {
                    ImGui.Indent(15);
                    ImGui.SetNextItemWidth(-1);
                    if (ImGui.SliderInt("##LunarStop", ref lunarCap, 0, 10000))
                    {
                        C.LunarCreditsCap = lunarCap;
                        C.Save();
                    }
                    ImGui.Unindent(15);
                }

                if (ImGui.Checkbox($"Stop at Cosmic Score", ref stopScore))
                {
                    C.StopOnceHitCosmicScore = stopScore;
                    C.Save();
                }
                if (stopScore)
                {
                    ImGui.Indent(15);
                    ImGui.SetNextItemWidth(-1);
                    if (ImGui.InputInt("###ScoreStop", ref scoreCap, 10000, 50000))
                    {
                        C.CosmicScoreCap = scoreCap >= 0 ? scoreCap : 0;
                        C.Save();
                    }
                    ImGui.Unindent(15);
                }

                if (ImGui.Checkbox($"Stop at Level", ref stopWhenLevel))
                {
                    C.StopWhenLevel = stopWhenLevel;
                    C.Save();
                }
                if (stopWhenLevel)
                {
                    ImGui.Indent(15);
                    ImGui.SetNextItemWidth(-1);
                    if (ImGui.SliderInt("##Level", ref targetLevel, 10, 100))
                    {
                        C.TargetLevel = targetLevel;
                        C.Save();
                    }
                    ImGui.Unindent(15);
                }

                ImGui.Spacing();

                ImGui.Separator();

                ImGui.Dummy(new(0, 10));

                bool EnableRelicXp = C.XPRelicGrind;
                if (ImGui.Checkbox("Auto-Pick For Relic XP", ref EnableRelicXp))
                {
                    C.XPRelicGrind = EnableRelicXp;
                    C.Save();
                }
                if (EnableRelicXp)
                {
                    bool IgnoreManual = C.XPRelicIgnoreManual;
                    if (ImGui.Checkbox("Ignore Manual Mode Missions", ref IgnoreManual))
                    {
                        C.XPRelicIgnoreManual = IgnoreManual;
                        C.Save();
                    }

                    bool OnlySelected = C.XPRelicOnlyEnabled;
                    if (ImGui.Checkbox("Only selected missions", ref OnlySelected))
                    {
                        C.XPRelicOnlyEnabled = OnlySelected;
                        C.Save();
                    }
                }

                ImGui.Spacing();

                ImGui.Separator();

                ImGui.Dummy(new(0, 10));
                if (ImGui.Checkbox("Auto Pick Current Job", ref autoPickCurrentJob))
                {
                    C.AutoPickCurrentJob = autoPickCurrentJob;
                    C.Save();
                }

                if (autoPickCurrentJob && usingSupportedJob)
                {
                    if (currentJobId != selectedJob)
                    {
                        selectedJob = currentJobId.Value;
                        C.SelectedJob = selectedJob;
                        C.Save();
                    }
                }

                ImGui.Dummy(new(0, 5));

                float iconSize = 32;
                float iconSpacing = 8;
                float availWidth = ImGui.GetContentRegionAvail().X;
                float startX = (availWidth - (iconSize + iconSpacing) * 4 + iconSpacing) * 0.5f;
                ImGui.SetCursorPosX(startX);

                // Row 1: CRP, BSM, ARM, GSM
                DrawJobSelection(8, "CRP");
                ImGui.SameLine(0, iconSpacing);
                DrawJobSelection(9, "BSM");
                ImGui.SameLine(0, iconSpacing);
                DrawJobSelection(10, "ARM");
                ImGui.SameLine(0, iconSpacing);
                DrawJobSelection(11, "GSM");

                // Row 2: LTW, WVR, ALC, CUL
                ImGui.SetCursorPosX(startX);

                DrawJobSelection(12, "LTW");
                ImGui.SameLine(0, iconSpacing);
                DrawJobSelection(13, "WVR");
                ImGui.SameLine(0, iconSpacing);
                DrawJobSelection(14, "ALC");
                ImGui.SameLine(0, iconSpacing);
                DrawJobSelection(15, "CUL");

                // Row 3: MIN, BTN, FSH
                ImGui.SetCursorPosX(startX);
                DrawJobSelection(16, "MIN");
                ImGui.SameLine(0, iconSpacing);
                DrawJobSelection(17, "BTN");
                ImGui.SameLine(0, iconSpacing);
                DrawJobSelection(18, "FSH");

                ImGui.Dummy(new Vector2(0, 5));

                ImGui.Separator();

                ImGui.Dummy(new Vector2(0, 5));

                ImGui.Text("Quick Mission Apply");

                ImGui.Dummy(new Vector2(0, 5));
                UpdateMissions();

                ImGui.Dummy(new Vector2(0, 5));

                ImGui.Separator();

                ImGui.Dummy(new Vector2(0, 5));

                RelicXP();
            }

            ImGui.EndChild();

            // ------------------------------------------ 
            //  MIDDLE PANEL: MISSION LISTING
            // ------------------------------------------
            ImGui.NextColumn();

            // Buffer room for the scrollbar
            ImGui.SetColumnWidth(1, C.MiddleColumnWidth);

            if (ImGui.BeginChild("##MissionList", new Vector2(0, childHeight), true))
            {

                if (ImGui.Checkbox("Hide Unsupported Missions", ref hideUnsupported))
                {
                    C.HideUnsupportedMissions = hideUnsupported;
                    C.Save();
                }

                ImGui.SameLine();
                ImGui.SetNextItemWidth(150);
                if (ImGui.BeginCombo("Sort By", missionSortOptions[missionSelectedOption]))
                {
                    for (int i = 0; i < missionSortOptions.Length; i++)
                    {
                        bool isSelected = (i == missionSelectedOption);
                        if (ImGui.Selectable(missionSortOptions[i], isSelected))
                        {
                            missionSelectedOption = i;
                        }
                        if (isSelected)
                        {
                            ImGui.SetItemDefaultFocus();
                        }
                        if (missionSelectedOption != C.TableSortOption)
                        {
                            C.TableSortOption = missionSelectedOption;
                            C.Save();
                        }
                    }
                    ImGui.EndCombo();
                }
                ImGui.SameLine();
                float middleColumnWidth = C.MiddleColumnWidth;
                ImGui.SetNextItemWidth(75);
                if (ImGui.DragFloat("Middle Column Width Adjuster", ref middleColumnWidth))
                {
                    C.MiddleColumnWidth = middleColumnWidth;
                    C.Save();
                }

                ImGui.Dummy(new Vector2(0, 5));

                ImGui.Separator();

                ImGui.Dummy(new Vector2(0, 5));

                // Mission Dropdown Sorting + Dropdowns themselves

                #region Mission Dropdowns

                foreach (var missionType in missionList)
                {
                    missionType.Value.Clear();
                }

                foreach (var mission in CosmicHelper.MissionInfoDict)
                {
                    HashSet<uint> Jobs = new HashSet<uint>();
                    Jobs.Add(mission.Value.JobId, mission.Value.JobId2);

                    if (!Jobs.Contains(selectedJob))
                        continue;

                    bool isGatherMission = CosmicHelper.GatheringJobList.Contains((int)mission.Value.JobId) || CosmicHelper.GatheringJobList.Contains((int)mission.Value.JobId2);
                    if (mission.Value.Attributes.HasFlag(MissionAttributes.Critical))
                        missionList["Critical"].Add((mission.Key, isGatherMission, C.MissionConfig[mission.Key].Enabled));
                    else if (mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalWeather))
                        missionList["Weather"].Add((mission.Key, isGatherMission, C.MissionConfig[mission.Key].Enabled));
                    else if (mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalTimed))
                        missionList["Timed"].Add((mission.Key, isGatherMission, C.MissionConfig[mission.Key].Enabled));
                    else if (mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalSequential))
                        missionList["Sequence"].Add((mission.Key, isGatherMission, C.MissionConfig[mission.Key].Enabled));
                    else if (mission.Value.Rank > 3)
                        missionList["ARank"].Add((mission.Key, isGatherMission, C.MissionConfig[mission.Key].Enabled));
                    else if (mission.Value.Rank == 3)
                        missionList["BRank"].Add((mission.Key, isGatherMission, C.MissionConfig[mission.Key].Enabled));
                    else if (mission.Value.Rank == 2)
                        missionList["CRank"].Add((mission.Key, isGatherMission, C.MissionConfig[mission.Key].Enabled));
                    else if (mission.Value.Rank == 1)
                        missionList["DRank"].Add((mission.Key, isGatherMission, C.MissionConfig[mission.Key].Enabled));
                }


                if (showCritical)
                {
                    int amountEnabled = missionList.ContainsKey("Critical") ? missionList["Critical"].Count(mission => mission.enabled) : 0;
                    DrawCollapsibleHeader($"Critical Missions", $"Critical Missions | Enabled: {amountEnabled}");
                    if (headerStates.TryGetValue("Critical Missions", out var isOpen) && isOpen)
                    {
                        MissionInfoV2("Critical Missions", SortMissionList(missionList["Critical"]));
                    }
                }

                if (showSequential)
                {
                    int amountEnabled = missionList.ContainsKey("Sequence") ? missionList["Sequence"].Count(mission => mission.enabled) : 0;
                    DrawCollapsibleHeader($"Sequential Missions", $"Sequential Missions | Enabled: {amountEnabled}");
                    if (headerStates.TryGetValue("Sequential Missions", out var isOpen) && isOpen)
                    {
                        MissionInfoV2("Sequence Missions", SortMissionList(missionList["Sequence"]));
                    }
                }

                if (showWeather)
                {
                    int amountEnabled = missionList.ContainsKey("Weather") ? missionList["Weather"].Count(mission => mission.enabled) : 0;

                    DrawCollapsibleHeader($"Weather Missions", $"Weather Missions | Enabled: {amountEnabled}");
                    if (headerStates.TryGetValue("Weather Missions", out var isOpen) && isOpen)
                    {
                        MissionInfoV2("Weather Missions", SortMissionList(missionList["Weather"]));
                    }
                }

                if (showTimeRestricted)
                {
                    int amountEnabled = missionList.ContainsKey("Timed") ? missionList["Timed"].Count(mission => mission.enabled) : 0;

                    DrawCollapsibleHeader($"Time-Restricted Missions", $"Time-Restricted Missions | {amountEnabled}");
                    if (headerStates.TryGetValue("Time-Restricted Missions", out var isOpen) && isOpen)
                    {
                        MissionInfoV2("Timed Missions", SortMissionList(missionList["Timed"]));
                    }
                }

                if (showClassA)
                {
                    int amountEnabled = missionList.ContainsKey("ARank") ? missionList["ARank"].Count(mission => mission.enabled) : 0;

                    DrawCollapsibleHeader($"A Rank Missions", $"A Rank Mission | Enabled: {amountEnabled}");
                    if (headerStates.TryGetValue("A Rank Missions", out var isOpen) && isOpen)
                    {
                        MissionInfoV2("A Rank Missions", SortMissionList(missionList["ARank"]));
                    }
                }

                if (showClassB)
                {
                    int amountEnabled = missionList.ContainsKey("BRank") ? missionList["BRank"].Count(mission => mission.enabled) : 0;
                    DrawCollapsibleHeader($"B Rank Missions", $"B Rank Mission | Enabled: {amountEnabled}");
                    if (headerStates.TryGetValue("B Rank Missions", out var isOpen) && isOpen)
                    {
                        MissionInfoV2("B Rank Missions", SortMissionList(missionList["BRank"]));
                    }
                }

                if (showClassC)
                {
                    int amountEnabled = missionList.ContainsKey("CRank") ? missionList["CRank"].Count(mission => mission.enabled) : 0;
                    DrawCollapsibleHeader($"C Rank Missions", $"C Rank Mission | Enabled: {amountEnabled}");
                    if (headerStates.TryGetValue("C Rank Missions", out var isOpen) && isOpen)
                    {
                        MissionInfoV2("C Rank Missions", SortMissionList(missionList["CRank"]));
                    }
                }

                if (showClassD)
                {
                    int amountEnabled = missionList.ContainsKey("DRank") ? missionList["DRank"].Count(mission => mission.enabled) : 0;
                    DrawCollapsibleHeader($"D Rank Missions", $"D Rank Mission | Enabled: {amountEnabled}");
                    if (headerStates.TryGetValue("D Rank Missions", out var isOpen) && isOpen)
                    {
                        MissionInfoV2("D Rank Missions", SortMissionList(missionList["DRank"]));
                    }
                }

                #endregion
            }

            ImGui.EndChild();

            // ------------------------------------------
            // RIGHT PANEL: MISSION INFO
            // ------------------------------------------
            ImGui.NextColumn();
            float maxWidthAvail = ImGui.GetContentRegionMax().X;
            ImGui.SetColumnWidth(2, maxWidthAvail);

            if (ImGui.BeginChild("###MissionDetailPanel", new Vector2(0, childHeight), true))
            {
                Kofi.DrawRight();
                ImGui.NewLine();

                if (selectedMission != 0)
                {
                    ImGui.Text($"Mission Info (More Detailed)");
                    ImGui.Separator();

                    var mission = MissionInfoDict[selectedMission];

                    var MissionInfo = new List<(string Label, string Value)>
                    {
                        ("ID:", $"{selectedMission}"),
                        ("Mission Name:", mission.Name),
                        ("Cosmocredits:", mission.CosmoCredit.ToString()),
                        ("Lunar Credits", mission.LunarCredit.ToString()),
                        ("Silver Requirements:", mission.SilverRequirement.ToString()),
                        ("Gold Requirements:", mission.GoldRequirement.ToString())
                    };

                    float infoSize1 = MissionInfo.Max(row => ImGui.CalcTextSize(row.Label).X) + 10;
                    float infoSize2 = MissionInfo.Max(row => ImGui.CalcTextSize(row.Value).X) + 10;

                    if (ImGui.BeginTable("Detail##DetailPanelTable", 2, ImGuiTableFlags.SizingFixedFit))
                    {
                        ImGui.TableSetupColumn("##Label");
                        ImGui.TableSetupColumn("##Value");

                        foreach (var row in MissionInfo)
                        {
                            ImGui.TableNextRow();

                            ImGui.TableSetColumnIndex(0);
                            ImGui.Text(row.Label);

                            ImGui.TableSetColumnIndex(1);
                            ImGui.Text(row.Value);
                        }

                        // used as a dummy spacer because don't wanna make a whole new table / CBA
                        ImGui.TableNextRow();

                        ImGui.TableNextRow();
                        ImGui.TableSetColumnIndex(0);
                        ImGui.Text($"Tool XP Reward");

                        for (int i = mission.ExperienceRewards.Count - 1; i >= 0; i--)
                        {
                            var row = mission.ExperienceRewards[i];
                            if (row.Amount != 0)
                            {
                                ImGui.TableNextRow();

                                ImGui.TableSetColumnIndex(0);
                                string type = "";
                                if (row.Type == 1)
                                    type = "I";
                                else if (row.Type == 2)
                                    type = "II";
                                else if (row.Type == 3)
                                    type = "III";
                                else if (row.Type == 4)
                                    type = "IV";
                                ImGui.Text($"Lv {type}:");

                                ImGui.TableSetColumnIndex(1);
                                ImGui.Text($"{row.Amount}");
                            }
                        }

                        ImGui.EndTable();

                        ImGui.Dummy(new Vector2(0, 5));

                        ImGui.Separator();

                        ImGui.Dummy(new Vector2(0, 5));

                        MissionAttributes flags = mission.Attributes;
                        var activeFlags = Enum.GetValues(typeof(MissionAttributes))
                                              .Cast<MissionAttributes>()
                                              .Where(f => f != MissionAttributes.None && flags.HasFlag(f))
                                              .ToList();

                        var entry = C.MissionConfig.Where(e => e.Key == selectedMission);

                        ImGui.Text("Notes:");
                        bool hasPreviousNotes = false;
                        if (mission.Weather != CosmicWeather.FairSkies)
                        {
                            hasPreviousNotes = true;

                            ImGui.TextWrapped(mission.Weather.ToString());
                        }
                        else if (mission.Time != 0)
                        {
                            hasPreviousNotes = true;

                            ImGui.TextWrapped($"{2 * (mission.Time - 1)}:00 - {2 * (mission.Time)}:00");
                        }
                        else if (mission.PreviousMissionID != 0)
                        {
                            hasPreviousNotes = true;

                            var (Id, Name) = MissionInfoDict.Where(m => m.Key == mission.PreviousMissionID).Select(m => (Id: m.Key, Name: m.Value.Name)).FirstOrDefault();
                            ImGui.TextWrapped($"[{Id}] {Name}");
                        }
                        if (mission.JobId2 != 0)
                        {
                            if (hasPreviousNotes) ImGui.SameLine();
                            ImGui.TextWrapped($"{jobOptions.Find(job => job.Id == mission.JobId).Name}/{jobOptions.Find(job => job.Id == mission.JobId2).Name}");
                        }

                        if (mission.Attributes.HasFlag(MissionAttributes.Gather))
                        {
                            ImGui.Dummy(new Vector2(0, 5));

                            ImGui.Separator();

                            ImGui.Dummy(new Vector2(0, 5));

                            bool craftMission = mission.Attributes.HasFlag(MissionAttributes.Craft);

                            bool LimitedQuant = mission.Attributes.HasFlag(MissionAttributes.Limited);
                            // Gather X Amount is just "Gather" 
                            bool TimedMission = mission.Attributes.HasFlag(MissionAttributes.ScoreTimeRemaining);
                            bool ChainedMission = mission.Attributes.HasFlag(MissionAttributes.ScoreChains);
                            bool BoonMission = mission.Attributes.HasFlag(MissionAttributes.ScoreGatherersBoon);
                            bool collectableMission = mission.Attributes.HasFlag(MissionAttributes.Collectables);
                            bool stellerReductionMission = mission.Attributes.HasFlag(MissionAttributes.ReducedItems);

                            bool GatherX = !stellerReductionMission && !collectableMission && !BoonMission && !ChainedMission && !TimedMission && !LimitedQuant;

                            string MissionType = "";
                            if (craftMission)
                            {
                                MissionType = "Dual Class Mission";
                            }
                            else if (LimitedQuant)
                            {
                                MissionType = "Limited Quantity/Nodes";
                            }
                            else if (TimedMission)
                                MissionType = "Timed Scoring/Time Attack";
                            else if (ChainedMission && !BoonMission)
                                MissionType = "Chained Gather Scoring";
                            else if (BoonMission && !ChainedMission)
                                MissionType = "Gatherer's Boon Scoring";
                            else if (BoonMission && ChainedMission)
                                MissionType = "Chained + Gatherer's Boon Scoring";
                            else if (collectableMission && !stellerReductionMission)
                                MissionType = "Collectable Scoring";
                            else if (stellerReductionMission)
                                MissionType = "Steller Reduction/Collectables";
                            else if (GatherX)
                                MissionType = "Gather X Amount of Items";

                            ImGui.Text("Mission Type: " + MissionType);
                        }
#if DEBUG
                        ImGui.Dummy(new(0, 10));
                        ImGui.Text($"Debug Section");
                        ImGui.Spacing();

                        ImGui.Text($"[Debug] Nodeset: {mission.NodeSet}");

                        ImGui.Text($"[Debug] Active Mission Flags:");
                        foreach (var flag in activeFlags)
                        {
                            ImGui.Text($"{flag}");
                        }
#endif
                    }
                }
            }
            ImGui.EndChild();
        }
        public void DrawJobSelection(uint jobId, string tooltip)
        {
            bool state = selectedJob == jobId;
            ISharedImmediateTexture? icon = state ? CosmicHelper.JobIconDict[jobId] : CosmicHelper.GreyTexture[jobId];

            // Slight padding around the button
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(2, 2));

            int styleCount = 1;
            int colorCount = 0;

            if (state)
            {
                // Dalamud theme
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.3f, 0.3f, 0.35f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.Border, ImGuiColors.ParsedGold);
                colorCount = 2;

                ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1.0f);
                styleCount++;
            }
            else
            {
                // Disabled job with Dalamud theme
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0.2f, 0.2f, 0.1f));
                ImGui.PushStyleColor(ImGuiCol.Border, new Vector4(0.4f, 0.4f, 0.4f, 0.5f));
                colorCount = 2;
                ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 0.5f);
                styleCount++;
            }

            // Rounded corners
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 2.0f);
            styleCount++;

            Vector2 size = new Vector2(26, 26);
            float zoomFactor = 0.25f; // 25% zoom-in
            float cropAmount = zoomFactor / 2; // Crop equally from all sides

            Vector2 uv0 = state ? new Vector2(0, 0) : new Vector2(cropAmount, cropAmount);
            Vector2 uv1 = state ? new Vector2(1, 1) : new Vector2(1 - cropAmount, 1 - cropAmount);


            if (ImGui.ImageButton(icon.GetWrapOrEmpty().Handle, size, uv0, uv1))
            {
                if (autoPickCurrentJob)
                {
                    autoPickCurrentJob = false;
                    C.AutoPickCurrentJob = autoPickCurrentJob;
                }
                C.SelectedJob = jobId;
                selectedJob = jobId;
                C.Save();
            }

            // Pop style variables and colors
            ImGui.PopStyleVar(styleCount);
            if (colorCount > 0)
            {
                ImGui.PopStyleColor(colorCount);
            }

            // Show tooltip on hover
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text($"{tooltip}");
                ImGui.EndTooltip();
            }
        }

        private void DrawCollapsibleHeader(string id, string label, float spacing = 4f)
        {
            var drawList = ImGui.GetWindowDrawList();
            var cursorPos = ImGui.GetCursorScreenPos();
            var windowWidth = ImGui.GetContentRegionAvail().X;

            var padding = 6.0f;
            var textSize = ImGui.CalcTextSize(label);
            var bgHeight = textSize.Y + padding * 2;

            if (!headerStates.ContainsKey(id))
                headerStates[id] = false;

            var headerRectMin = cursorPos;
            var headerRectMax = new Vector2(cursorPos.X + windowWidth, cursorPos.Y + bgHeight);

            // Draw background
            drawList.AddRectFilled(headerRectMin, headerRectMax, ImGui.GetColorU32(new Vector4(0.2f, 0.2f, 0.2f, 1f)), 2f);
            drawList.AddRect(headerRectMin, headerRectMax, ImGui.GetColorU32(ImGuiColors.ParsedGold), 2f);

            // Draw centered label text
            var textPos = new Vector2(
                cursorPos.X + (windowWidth - textSize.X) * 0.5f,
                cursorPos.Y + padding
            );
            drawList.AddText(textPos, ImGui.GetColorU32(new Vector4(1f, 1f, 1f, 1f)), label);

            // Register invisible button for interaction using a unique ID
            ImGui.SetCursorScreenPos(cursorPos);
            ImGui.PushID(id); // Use internal ID
            ImGui.InvisibleButton("##header", new Vector2(windowWidth, bgHeight));
            if (ImGui.IsItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                headerStates[id] = !headerStates[id];
            ImGui.PopID();

            ImGui.SetCursorScreenPos(new Vector2(cursorPos.X, cursorPos.Y + bgHeight + spacing));
        }
        private void MissionInfoV2(string tableName, List<(uint id, bool ShowGather, bool enabled)> missions)
        {
            // Fixed column count - include ALL possible columns
            int totalColumns = 14; // Enabled, Manual, ID, Mission Name, Cosmo, Lunar, I, II, III, IV, Turnin, Gather, Notes

            ImGuiTableFlags tableFlags = ImGuiTableFlags.RowBg |
                                        ImGuiTableFlags.Borders |
                                        ImGuiTableFlags.SizingFixedFit |
                                        ImGuiTableFlags.Resizable |           // Allow column resizing
                                        ImGuiTableFlags.Reorderable |         // Allow column reordering
                                        ImGuiTableFlags.Hideable;             // Allow hiding columns via right-click

            if (ImGui.BeginTable($"MissionList###{tableName}_{selectedJob}", totalColumns, tableFlags))
            {
                float padding = 10f;

                // Setup ALL columns - all visible by default, users can hide what they don't want via right-click
                ImGui.TableSetupColumn("Enabled", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("Enabled").X + padding);
                ImGui.TableSetupColumn("Manual", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("Manual").X + padding);
                ImGui.TableSetupColumn("ID", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("99999").X + padding);
                ImGui.TableSetupColumn("Mission Name", ImGuiTableColumnFlags.WidthFixed, 250f);
                ImGui.TableSetupColumn("Cosmo", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("Cosmo").X + padding);
                ImGui.TableSetupColumn("Lunar", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("Lunar").X + padding);
                ImGui.TableSetupColumn("Score", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("Score").X + padding);

                // XP columns
                float xpWidth = ImGui.CalcTextSize("III").X + padding;
                ImGui.TableSetupColumn("I", ImGuiTableColumnFlags.WidthFixed, xpWidth);
                ImGui.TableSetupColumn("II", ImGuiTableColumnFlags.WidthFixed, xpWidth);
                ImGui.TableSetupColumn("III", ImGuiTableColumnFlags.WidthFixed, xpWidth);
                ImGui.TableSetupColumn("IV", ImGuiTableColumnFlags.WidthFixed, xpWidth);

                ImGui.TableSetupColumn("Turnin Mode", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("Select Turnin").X + padding + 15);
                ImGui.TableSetupColumn("Gathering Profile", ImGuiTableColumnFlags.WidthFixed, ImGui.CalcTextSize("Gathering Profile").X + padding + 15);
                ImGui.TableSetupColumn("Mission Notes", ImGuiTableColumnFlags.WidthFixed, Math.Max(ImGui.CalcTextSize("Mission Notes").X + padding, 75));

                // Render headers with right-click menu
                ImGui.TableHeadersRow();

                foreach (var entry in missions)
                {
                    var Id = entry.id;
                    var missionConfig = C.MissionConfig[Id];
                    var missionInfo = CosmicHelper.MissionInfoDict[Id];

                    bool unsupported = UnsupportedMissions.Ids.Contains(Id);

                    bool craftMission = missionInfo.Attributes.HasFlag(MissionAttributes.Craft);
                    bool gatherMission = missionInfo.Attributes.HasFlag(MissionAttributes.Gather);
                    bool fishMission = missionInfo.Attributes.HasFlag(MissionAttributes.Fish);
                    bool collectableMission = missionInfo.Attributes.HasFlag(MissionAttributes.Collectables);
                    bool stellerReductionMission = missionInfo.Attributes.HasFlag(MissionAttributes.ReducedItems);

                    bool dualclass = craftMission && (gatherMission || fishMission);

                    if (unsupported && hideUnsupported)
                        continue;

                    ImGui.TableNextRow();

                    // Mission Enable/Disable Checkbox
                    ImGui.PushID(Id);

                    // Enable | Disable Mission Selection
                    ImGui.TableSetColumnIndex(0);
                    bool enabled = missionConfig.Enabled;
                    if (CenterCheckbox("##EnableMission", ref enabled))
                    {
                        missionConfig.Enabled = enabled;
                        C.Save();
                    }
                    if (ImGui.IsItemClicked())
                    {
                        selectedMission = Id;
                    }


                    // Manual mode checkbox
                    ImGui.TableNextColumn();
                    bool manualMode = missionConfig.ManualMode;
                    if (CenterCheckbox("##Manual Mode", ref manualMode))
                    {
                        missionConfig.ManualMode = manualMode;
                        C.Save();
                    }
                    if (ImGui.IsItemClicked())
                    {
                        selectedMission = Id;
                    }

                    // Mission ID
                    ImGui.TableNextColumn();
                    CenterTextInTableCell(Id.ToString());

                    // Mission Name
                    ImGui.TableNextColumn();
                    ImGui.Text(missionInfo.Name);
                    if (ImGui.IsItemClicked())
                    {
                        selectedMission = Id;
                    }
                    if (missionInfo.MarkerId != 0)
                    {
                        ImGui.SameLine();
                        ImGui.PushFont(UiBuilder.IconFont);
                        ImGui.Text(FontAwesomeIcon.Flag.ToIconString());
                        ImGui.PopFont();
                        if (ImGui.IsItemClicked())
                        {
                            selectedMission = Id;
                            Utils.SetGatheringRing(missionInfo.TerritoryId, (int)missionInfo.MapPosition.X, (int)missionInfo.MapPosition.Y, missionInfo.Radius, missionInfo.Name);
                        }
                    }

                    // Cosmo/Lunar Credits
                    ImGui.TableNextColumn();
                    CenterTextInTableCell(missionInfo.CosmoCredit.ToString());

                    ImGui.TableNextColumn();
                    CenterTextInTableCell(missionInfo.LunarCredit.ToString());

                    ImGui.TableNextColumn();
                    CenterTextInTableCell(missionInfo.missionScore.ToString());

                    // XP Columns
                    for (int i = 1; i < 5; i++)
                    {
                        ImGui.TableNextColumn();
                        var expReward = missionInfo.ExperienceRewards.Where(exp => exp.Type == i).FirstOrDefault();
                        var relicXp = expReward.Amount.ToString();

                        if (relicXp == "0")
                        {
                            relicXp = "-";
                        }

                        CenterTextInTableCell(relicXp);
                    }

                    // Mission Turnin Settings
                    ImGui.TableNextColumn();
                    if (missionInfo.Attributes.HasFlag(MissionAttributes.ScoreTimeRemaining))
                    {
                        CenterTextInTableCell("Auto");
                        if (missionConfig.AutoTurnin == false)
                        {
                            missionConfig.AutoTurnin = true;
                            missionConfig.TurninGold = false;
                            missionConfig.TurninSilver = false;
                            missionConfig.TurninBronze = false;

                            C.Save();
                        }
                    }
                    else
                    {
                        if (CenterButton("Select Turnin"))
                        {
                            ImGui.OpenPopup("Mission Turnin Settings");
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            if (missionConfig.AutoTurnin)
                                ImGui.Text($"Auto Turnin - True");
                            else
                            {
                                if (missionConfig.TurninGold)
                                    ImGui.Text($"Gold Enabled");
                                if (missionConfig.TurninSilver)
                                    ImGui.Text($"Silver Enabled");
                                if (missionConfig.TurninBronze)
                                    ImGui.Text($"Bronze Enabled");
                            }

                            ImGui.EndTooltip();
                        }

                        if (ImGui.BeginPopup("Mission Turnin Settings"))
                        {
                            bool anyTurnin = missionConfig.AutoTurnin;
                            bool goldTurnin = missionConfig.TurninGold;
                            bool silverTurnin = missionConfig.TurninSilver;
                            bool bronzeTurnin = missionConfig.TurninBronze;

                            ImGui.Text("Select Turnin Options");
                            ImGui.Dummy(new Vector2(0, 2));

                            if (ImGui.Checkbox("Auto", ref anyTurnin))
                            {
                                if (anyTurnin)
                                {
                                    missionConfig.TurninGold = false;
                                    missionConfig.TurninSilver = false;
                                    missionConfig.TurninBronze = false;

                                    missionConfig.AutoTurnin = anyTurnin;
                                }
                                else
                                {
                                    if (!(bronzeTurnin && silverTurnin && goldTurnin))
                                    {
                                        missionConfig.AutoTurnin = true;
                                    }
                                }

                                C.Save();
                            }
                            ImGuiEx.HelpMarker("This option will strive to get the best result, but will turn in any result if necessary without stopping.");

                            ImGui.Separator();

                            if (ImGui.Checkbox("Gold", ref goldTurnin))
                            {
                                if (anyTurnin && goldTurnin)
                                    missionConfig.AutoTurnin = false;

                                missionConfig.TurninGold = goldTurnin;
                                C.Save();
                            }
                            if (ImGui.Checkbox("Silver", ref silverTurnin))
                            {
                                if (anyTurnin && silverTurnin)
                                    missionConfig.AutoTurnin = false;

                                missionConfig.TurninSilver = silverTurnin;
                                C.Save();
                            }
                            if (ImGui.Checkbox("Bronze", ref bronzeTurnin))
                            {
                                if (anyTurnin && bronzeTurnin)
                                    missionConfig.AutoTurnin = false;

                                missionConfig.TurninBronze = bronzeTurnin;
                                C.Save();
                            }

                            ImGui.EndPopup();
                        }
                    }

                    // Gather Mission Profile Settings
                    ImGui.TableNextColumn();
                    if (missionInfo.Attributes.HasFlag(MissionAttributes.Gather))
                    {
                        string profileName = C.GatherSettings[missionConfig.GatherProfileId].Name;

                        if (CenterButton($"{profileName}##GatherProfile_{profileName}"))
                        {
                            ImGui.OpenPopup("Selecting Gathering Profile");
                        }
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text("Select profile to use");
                            ImGui.EndTooltip();
                        }
                        if (ImGui.BeginPopup("Selecting Gathering Profile"))
                        {
                            ImGui.Text($"Currently Selected: {profileName}");
                            ImGui.Separator();
                            for (int i = 0; i < C.GatherSettings.Count; i++)
                            {
                                var GProfile = C.GatherSettings[i];
                                bool GProfileSelected = missionConfig.GatherProfileId == i;

                                if (ImGui.RadioButton(GProfile.Name, GProfileSelected))
                                {
                                    missionConfig.GatherProfileId = i;
                                    C.Save();
                                }
                            }

                            ImGui.EndPopup();
                        }
                    }
                    else if (missionInfo.Attributes.HasFlag(MissionAttributes.Fish))
                    {
                        if (CenterButton($"Select Profile##Select_Fishing_Profile"))
                        {
                            ImGui.OpenPopup("Select Fishing Profile");
                        }
                        if (ImGui.BeginPopup("Select Fishing Profile"))
                        {
                            ImGui.Text($"Fishing profile: {missionInfo.Name}");
                            ImGui.Separator();
                            bool builtInPreset = missionConfig.Use_BuildinPreset;
                            if (ImGui.Checkbox("Use Built In Preset", ref builtInPreset))
                            {
                                missionConfig.Use_BuildinPreset = builtInPreset;
                                C.Save();
                            }
                            ImGuiEx.HelpMarker("Having this enabled means it will use the default preset that is included with the plugin for autohook. \n" +
                                               "If you would like to use one that you already have in autohook, you can un-checkmark this and type the name of it below");
                            using (ImRaii.Disabled(builtInPreset))
                            {
                                string presetName = missionConfig.AutoHookPresetName;
                                ImGui.SetNextItemWidth(200);
                                if (ImGui.InputText("Preset Name", ref presetName))
                                {
                                    missionConfig.AutoHookPresetName = presetName;
                                    C.Save();
                                }
                            }

                            ImGui.EndPopup();
                        }
                    }

                        ImGui.TableNextColumn();
                    int notesCount = 0;

                    ImGui.Dummy(new(2, 0));
                    if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalSequential))
                    {
                        ImGui.PushFont(UiBuilder.IconFont);
                        ImGui.Text(FontAwesomeIcon.ListOl.ToIconString());
                        ImGui.PopFont();
                        if (ImGui.IsItemHovered())
                        {
                            var prevMissions = GetOnlyPreviousMissionsRecursive(Id);

                            ImGui.BeginTooltip();
                            ImGui.Text("Sequence Missions");
                            ImGui.Separator();
                            for (int i = 0; i < prevMissions.Count; i++)
                            {
                                var prevMission = prevMissions[i];
                                ImGui.Text($"{i + 1}: [{prevMission}] - {CosmicHelper.MissionInfoDict[prevMission].Name}");
                            }
                            ImGui.EndTooltip();
                        }
                        notesCount++;
                    }
                    if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalWeather))
                    {
                        if (notesCount > 0)
                            ImGui.SameLine();

                        ImGui.PushFont(UiBuilder.IconFont);
                        ImGui.Text(FontAwesomeIcon.Cloud.ToIconString());
                        ImGui.PopFont();
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text($"Weather: {missionInfo.Weather}");
                            ImGui.EndTooltip();
                        }
                        notesCount++;
                    }
                    if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalTimed))
                    {
                        if (notesCount > 0)
                            ImGui.SameLine();
                        ImGui.PushFont(UiBuilder.IconFont);
                        ImGui.Text(FontAwesomeIcon.Clock.ToIconString());
                        ImGui.PopFont();
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text($"{2 * (missionInfo.Time - 1)}:00 - {2 * (missionInfo.Time)}:00");
                            ImGui.EndTooltip();
                        }
                        notesCount++;
                    }
                    if (missionInfo.JobId2 != 0)
                    {
                        if (notesCount > 0)
                            ImGui.SameLine();

                        ISharedImmediateTexture? job1Icon = CosmicHelper.JobIconDict[missionInfo.JobId];
                        ISharedImmediateTexture? job2Icon = CosmicHelper.JobIconDict[missionInfo.JobId2];
                        Vector2 imageSize = new Vector2(23, 23);

                        ImGui.Image(job1Icon.GetWrapOrEmpty().Handle, imageSize);
                        ImGui.SameLine();
                        ImGui.Image(job2Icon.GetWrapOrEmpty().Handle, imageSize);
                    }

                    ImGui.PopID();
                }

                ImGui.EndTable();
            }
        }

        private Dictionary<string, bool> QuickSetModes = new()
        {
            ["Any"] = true,
            ["Gold"] = false,
            ["Silver"] = false,
            ["Bronze"] = false,
            ["Manual"] = false,
        };

        private List<string> QuickApplyOptions = new() { "Current Class", "All Enabled", "All Missions" };
        private int QuickSelectedOption = 0;

        private void UpdateMissions()
        {
            ImGui.SetNextItemWidth(100);
            if (ImGui.Button("Select Modes"))
            {
                ImGui.OpenPopup("Select Mission Profiles");
            }

            if (ImGui.BeginPopup("Select Mission Profiles"))
            {
                ImGui.Text("QuickSet Modes");

                // Any checkbox
                bool anyValue = QuickSetModes["Any"];
                bool goldValue = QuickSetModes["Gold"];
                bool silverValue = QuickSetModes["Silver"];
                bool bronzeValue = QuickSetModes["Bronze"];
                if (!(anyValue ||  goldValue || silverValue || bronzeValue))
                {
                    QuickSetModes["Any"] = true;
                }

                if (ImGui.Checkbox("Any", ref anyValue))
                {
                    QuickSetModes["Any"] = anyValue;
                    if (anyValue && (goldValue ||  silverValue || bronzeValue))
                    {
                        QuickSetModes["Gold"] = false;
                        QuickSetModes["Silver"] = false;
                        QuickSetModes["Bronze"] = false;
                    }
                }

                // Separator between Any and medal types
                ImGui.Separator();

                // Medal checkboxes
                if (ImGui.Checkbox("Gold", ref goldValue))
                {
                    QuickSetModes["Gold"] = goldValue;
                    QuickSetModes["Any"] = false;
                }

                if (ImGui.Checkbox("Silver", ref silverValue))
                {
                    QuickSetModes["Silver"] = silverValue;
                    QuickSetModes["Any"] = false;
                }

                if (ImGui.Checkbox("Bronze", ref bronzeValue))
                {
                    QuickSetModes["Bronze"] = bronzeValue;
                    QuickSetModes["Any"] = false;
                }

                // Separator between medals and Manual
                ImGui.Separator();

                // Manual checkbox
                bool manualValue = QuickSetModes["Manual"];
                if (ImGui.Checkbox("Manual", ref manualValue))
                {
                    QuickSetModes["Manual"] = manualValue;
                }

                ImGui.EndPopup();
            }

            ImGui.SetNextItemWidth(100);
            if (ImGui.BeginCombo("Quick Apply", QuickApplyOptions[QuickSelectedOption]))
            {
                for (int i = 0; i < QuickApplyOptions.Count; i++)
                {
                    bool isSelected = (QuickSelectedOption == i);
                    if (ImGui.Selectable(QuickApplyOptions[i], isSelected))
                    {
                        QuickSelectedOption = i;
                        string selectedOption = QuickApplyOptions[QuickSelectedOption];
                    }

                    // Set the initial focus when opening the combo
                    if (isSelected)
                        ImGui.SetItemDefaultFocus();
                }
                ImGui.EndCombo();
            }

            ImGui.Text($"Option: {QuickSelectedOption}");
            ImGui.Text($"Selected Job: {C.SelectedJob}");
            if (ImGui.Button("Apply to selected profiles"))
            {
                var currentJob = PlayerHelper.GetClassJobId();

                foreach (var mission in C.MissionConfig)
                {
                    var id = mission.Key;
                    var missionDict = CosmicHelper.MissionInfoDict[id];
                    bool selectedJob = (C.SelectedJob == missionDict.JobId || C.SelectedJob == missionDict.JobId2);
                    bool TimedMission = missionDict.Attributes.HasFlag(MissionAttributes.ScoreTimeRemaining);

                    PluginLog.Information($"ID: {id} | Enabled: {mission.Value.Enabled}");

                    if (QuickSelectedOption == 0 && selectedJob)
                    {
                        if (!selectedJob)
                        {
                            PluginLog.Information($"Option 0 was checked, and returned false");
                            continue;
                        }
                        else
                        {
                            PluginLog.Information($"Option 0 was valid! Continuing on");
                        }
                    }
                    if (QuickSelectedOption == 1)
                    {
                        if (!mission.Value.Enabled)
                        {
                            PluginLog.Information($"Option 1 was selected, but not enabled." +
                                                  $"Mission Id: {mission.Key} | Enabled? : {mission.Value.Enabled}" +
                                                  $"Skipping for now");
                            continue;
                        }
                        else
                        {
                            PluginLog.Information($"Option 1 was valid! Continuing on");
                        }
                    }

                    PluginLog.Information($"Mission is being modified: {id}");

                    if (TimedMission)
                    {
                        PluginLog.Information($"Timed Mission: \n" +
                                              $"Any: {QuickSetModes["Any"]}" +
                                              $"Gold: {QuickSetModes["Gold"]}" +
                                              $"Silver: {QuickSetModes["Silver"]}" +
                                              $"Bronze: {QuickSetModes["Bronze"]}" +
                                              $"Manual: {QuickSetModes["Manual"]}");

                        mission.Value.AutoTurnin = QuickSetModes["Any"];
                        mission.Value.TurninGold = false;
                        mission.Value.TurninSilver = false;
                        mission.Value.TurninBronze = false;
                        mission.Value.ManualMode = QuickSetModes["Manual"];
                    }
                    else
                    {
                        PluginLog.Information($"Non-timed Mission \n" +
                                              $"Any: {QuickSetModes["Any"]}" +
                                              $"Gold: {QuickSetModes["Gold"]}" +
                                              $"Silver: {QuickSetModes["Silver"]}" +
                                              $"Bronze: {QuickSetModes["Bronze"]}" +
                                              $"Manual: {QuickSetModes["Manual"]}");

                        mission.Value.AutoTurnin = QuickSetModes["Any"];
                        mission.Value.TurninGold = QuickSetModes["Gold"];
                        mission.Value.TurninSilver = QuickSetModes["Silver"];
                        mission.Value.TurninBronze = QuickSetModes["Bronze"];
                        mission.Value.ManualMode = QuickSetModes["Manual"];
                    }
                }

                C.Save();
            }
        }

        private float current = 25f;

        private class XPType
        {
            public uint CurrentXP { get; set; }
            public uint NeededXP { get; set; }
            public uint MaxXP { get; set; }
        }

        private unsafe void RelicXP()
        {
            var wksManager = WKSManager.Instance();
            if (wksManager == null || wksManager->ResearchModule == null || !wksManager->ResearchModule->IsLoaded)
                return;

            var job = selectedJob;
            var toolClassId = (byte)(job - 7);
            var stage = wksManager->ResearchModule->CurrentStages[toolClassId - 1];
            var nextstate = wksManager->ResearchModule->UnlockedStages[toolClassId - 1];

            // Unsure... why this is here? 
            if (Svc.Data.GetExcelSheet<WKSCosmoToolClass>().TryGetRow(toolClassId, out var row))
            {

            }

            Dictionary<uint, XPType> XPTable = new Dictionary<uint, XPType>();

            for (byte type = 1; type <= 4; type++)
            {
                if (!wksManager->ResearchModule->IsTypeAvailable(toolClassId, type))
                    break;

                var neededXP = wksManager->ResearchModule->GetNeededAnalysis(toolClassId, type);

                var maxXP = wksManager->ResearchModule->GetMaxAnalysis(toolClassId, type);

                var currentXp = wksManager->ResearchModule->GetCurrentAnalysis(toolClassId, type);
                if (!XPTable.ContainsKey(type))
                {
                    XPTable[type] = new XPType()
                    {
                        CurrentXP = currentXp,
                        NeededXP = neededXP,
                        MaxXP = maxXP,
                    };
                }
            }

            ImGui.Text($"Stage: {stage}");

            if (stage != 9)
            {
                foreach (var type in XPTable)
                {
                    uint current = type.Value.CurrentXP;
                    uint needed = type.Value.NeededXP;
                    uint max = type.Value.MaxXP;
                    float windowSize = ImGui.GetWindowSize().X - 20;
                    Vector2 size = new Vector2(windowSize, 10);

                    string overlay = $"ID: {current} / {max}";
                    string xpType = "";
                    if (type.Key == 1)
                        xpType = "I";
                    else if (type.Key == 2)
                        xpType = "II";
                    else if (type.Key == 3)
                        xpType = "III";
                    else if (type.Key == 4)
                        xpType = "IV";
                    else
                        xpType = "???";

                    DrawXPBar($"Type: {xpType}", current, needed, size, max);
                }
            }
            else
            {
                float windowSize = ImGui.GetWindowSize().X - 20;
                Vector2 size = new Vector2(windowSize, 10);

                var (classScore, cappedClassScore, totalScores, classId) = CosmicHandler.GetCosmicClassScores();

                DrawXPBar("Score", (uint)classScore, 0, size, 500_000);
            }
        }

        void DrawXPBar(string label, uint currentXP, uint neededXP, Vector2 size, uint maxXP = 0)
        {
            // Handle capped and invalid data
            float fraction;
            if (neededXP == 0)
            {
                fraction = Math.Clamp((float)currentXP / (float)maxXP, 0f, 1f);
            }
            else
            {
                fraction = Math.Clamp((float)currentXP / (float)neededXP, 0f, 1f);
            }

            // Display correct text
            string displayText = (neededXP == 0 && maxXP > 0)
                ? $"{label}: {currentXP:N0} / {maxXP:N0}"
                : $"{label}: {currentXP:N0} / {neededXP:N0}";

            ImGui.Text(displayText);

            // Draw bar
            var pos = ImGui.GetCursorScreenPos();
            var drawList = ImGui.GetWindowDrawList();

            var barStart = pos;
            var barEnd = new Vector2(pos.X + size.X, pos.Y + size.Y);

            drawList.AddRectFilled(barStart, barEnd, ImGui.GetColorU32(new Vector4(0.15f, 0.15f, 0.15f, 1f)));

            float filledWidth = size.X * fraction;
            if (filledWidth > 0f)
            {
                var filledEnd = new Vector2(pos.X + filledWidth, pos.Y + size.Y);

                drawList.AddRectFilledMultiColor(
                    barStart,
                    filledEnd,
                    ImGui.GetColorU32(new Vector4(0.2f, 0.6f, 1f, 1f)), // left
                    ImGui.GetColorU32(new Vector4(0.6f, 1f, 0.8f, 1f)), // right
                    ImGui.GetColorU32(new Vector4(0.6f, 1f, 0.8f, 1f)), // right
                    ImGui.GetColorU32(new Vector4(0.2f, 0.6f, 1f, 1f))  // left
                );
            }

            ImGui.Dummy(new Vector2(size.X, size.Y + 5));
        }

        #region Table Tools

        private void CenterTextInTableCell(string text)
        {
            float cellWidth = ImGui.GetContentRegionAvail().X;
            float textWidth = ImGui.CalcTextSize(text).X;
            float offset = (cellWidth - textWidth) * 0.5f;

            if (offset > 0f)
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);

            ImGui.TextUnformatted(text);
        }
        private bool CenterCheckbox(string label, ref bool value)
        {
            // Checkbox size is roughly the font size
            float checkboxSize = ImGui.GetFontSize();
            float availableWidth = ImGui.GetContentRegionAvail().X;
            float offset = Math.Max(0f, (availableWidth - checkboxSize) * 0.5f);

            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);
            return ImGui.Checkbox(label, ref value);
        }
        private bool CenterButton(string label, Vector2? size = null)
        {
            Vector2 buttonSize = size ?? ImGui.CalcTextSize(label) + ImGui.GetStyle().FramePadding * 2;
            float availableWidth = ImGui.GetContentRegionAvail().X;
            float offset = Math.Max(0f, (availableWidth - buttonSize.X) * 0.5f);

            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);
            return size.HasValue ? ImGui.Button(label, size.Value) : ImGui.Button(label);
        }
        private List<uint> GetOnlyPreviousMissionsRecursive(uint missionId)
        {
            if (!MissionInfoDict.TryGetValue(missionId, out var missionInfo) || missionInfo.PreviousMissionID == 0)
                return [];

            var chain = GetOnlyPreviousMissionsRecursive(missionInfo.PreviousMissionID);
            chain.Add(missionInfo.PreviousMissionID);
            return chain;
        }
        private List<uint> GetOnlyNextMissionsRecursive(uint missionId)
        {
            uint? nextMissionId = MissionInfoDict
                .Where(m => m.Value.PreviousMissionID == missionId)
                .Select(m => (uint?)m.Key)
                .FirstOrDefault();

            if (!nextMissionId.HasValue)
                return [];

            var chain = new List<uint> { nextMissionId.Value };
            chain.AddRange(GetOnlyNextMissionsRecursive(nextMissionId.Value));
            return chain;
        }

        #endregion

    }
}
