using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.GameHelpers;
using ICE.Ui.MainUi.ModeSelect;
using ICE.Ui.MainUi.Settings.Settings_Table;
using ICE.UiV2.Imgui_Tools;
using System.Collections.Generic;

namespace ICE.UiV2.Ui_Main.Sub_Windows.Ui_Selected.Tab_CosmicHelp
{
    internal class MissionSetup
    {
        private static readonly Dictionary<string, uint> BattleJobs = new()
        {
            // Tanks
            { "Paladin", 19 },
            { "Warrior", 21 },
            { "Dark Knight", 32 },
            { "Gunbreaker", 37 },
    
            // Healers
            { "White Mage", 24 },
            { "Scholar", 28 },
            { "Astrologian", 33 },
            { "Sage", 40 },
    
            // Melee DPS
            { "Monk", 20 },
            { "Dragoon", 22 },
            { "Ninja", 30 },
            { "Samurai", 34 },
            { "Reaper", 39 },
            { "Viper", 41 },
    
            // Physical Ranged DPS
            { "Bard", 23 },
            { "Machinist", 31 },
            { "Dancer", 38 },
    
            // Magical Ranged DPS
            { "Black Mage", 25 },
            { "Summoner", 27 },
            { "Red Mage", 35 },
            { "Pictomancer", 42 }
        };
        private static string newListName = "";

        public static void Draw()
        {
            using var style = ImRaii.PushStyle(ImGuiStyleVar.ChildRounding, 10).Push(ImGuiStyleVar.ChildBorderSize, 1);
            MissionHeader();

            TableSettings();
        }

        private static void MissionHeader()
        {
            float scale = ImGuiHelpers.GlobalScale;

            using (var headerChild = ImRaii.Child("##modeSelect_StandardHeader", new Vector2(0, 45 * scale), true, ImGuiWindowFlags.NoScrollbar))
            {
                if (!headerChild.Success) return;

                ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 10 * scale);
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 5 * scale);

                string modeType = string.Empty;
                FontAwesomeIcon modeIcon = FontAwesomeIcon.List;

                bool relicMode = C.XPRelicGrind;
                bool xpLeveling = C.XPLeveling_Mode;
                bool standard = (!relicMode && !xpLeveling);


                if (standard)
                    modeType = "Standard";
                else if (relicMode)
                {
                    modeType = "Relic Grind";
                    modeIcon = FontAwesomeIcon.ArrowUpRightDots;
                }
                else if (xpLeveling)
                {
                    modeType = "Leveling Grind";
                    modeIcon = FontAwesomeIcon.Leaf;
                }

                ImGuiEx.IconWithText(modeIcon, $"{modeType} Mode");

                ImGui.SameLine(0, 10 * scale);

                // Adjust the Y position to center the button vertically with the text
                float textHeight = ImGui.GetTextLineHeight();
                float buttonHeight = ImGui.GetFrameHeight();
                float yOffset = (textHeight - buttonHeight) / 2f;
                ImGui.SetCursorPosY(ImGui.GetCursorPosY() + yOffset);

                if (ImGuiEx.IconButtonWithText(FontAwesomeIcon.Play, "Mode Selection"))
                {
                    ImGui.OpenPopup("Mode Select | Select Mode Window");
                }
                if (ImGui.BeginPopup("Mode Select | Select Mode Window"))
                {
                    ImGui.Text("Select Mode");
                    ImGui.Separator();

                    if (ImGui.RadioButton("Standard", standard))
                    {
                        C.XPRelicGrind = false;
                        C.XPLeveling_Mode = false;
                        C.Save();
                    }
                    ImGui_Ice.IconWithTooltip(FontAwesomeIcon.QuestionCircle,
                        "Stand Mode \n" +
                        "-> Used to select which missions you want to grind. It'll priortize in the following order:\n" +
                        "-> Critical -> Provisional [Sequence/Timed/Weather] -> Standard [A->D]\n" +
                        "-> Select which missions you want to do, and go at it.");
                    if (ImGui.RadioButton("Relic Grind", relicMode))
                    {
                        C.XPRelicGrind = true;
                        C.XPLeveling_Mode = false;
                        C.Save();
                    }
                    ImGui_Ice.IconWithTooltip(FontAwesomeIcon.QuestionCircle,
                        "Relic Grind\n" +
                        "-> Automatically select which missions that are best to finish up your relic\n" +
                        "-> These are weighed based on what is needed to complete the tool to the next step\n" +
                        "-> If you want to only do certain missions, enable the option and select which ones you want to do");

                    if (ImGui.RadioButton("Leveling Grind", xpLeveling))
                    {
                        C.XPRelicGrind = false;
                        C.XPLeveling_Mode = true;
                        C.Save();
                    }
                    ImGui_Ice.IconWithTooltip(FontAwesomeIcon.QuestionCircle,
                        "Leveling Grind\n" +
                        "-> Will automatically select which mission is the best for leveling your current class based on what level bracket you're in\n" +
                        "-> These are hand picked by me, and determined by the time it takes to complete it\n" +
                        "-> For crafters it's whatever missions take the least amount of progress" +
                        "-> For gathering, it's whatever is the least pain to do w/ the minimum amount of skills\n" +
                        "These will automatically set settings for using these modes temporarily");

                    ImGui.EndPopup();
                }

                uint currentJobId = (uint)Player.Job;
                bool usingSupportedJob = CosmicHelper.CrafterJobList.Contains(currentJobId) || CosmicHelper.GatheringJobList.Contains(currentJobId);

                bool AnyStop = C.StopOnceHitCosmicScore
                            || C.StopWhenLevel
                            || C.StopOnceHitCosmoCredits
                            || C.StopOnceHitLunarCredits
                            || C.StopOnceRelicFinished;
                if (AnyStop)
                {
                    ImGui.SameLine(0, 10 * scale);
                    ImGui.SetCursorPosY(ImGui.GetCursorPosY() + yOffset);
                    ImGuiEx.Icon(FontAwesomeIcon.ExclamationTriangle);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();

                        ImGui.Text("It appears that you have on of the following enabled");
                        if (C.StopOnceHitCosmicScore)
                            ImGui.BulletText($"Stop at Cosmic Score [{C.CosmicScoreCap:N0}]");
                        if (C.StopWhenLevel)
                            ImGui.BulletText($"Stop When Level [{C.TargetLevel:N0}]");
                        if (C.StopOnceHitCosmoCredits)
                            ImGui.BulletText($"Stop once cosmo credit hit [{C.CosmoCreditsCap:N0}]");
                        if (C.StopOnceHitLunarCredits)
                            ImGui.BulletText($"Stop once planetary credit hit [{C.LunarCreditsCap:N0}]");
                        if (C.StopOnceRelicFinished)
                            ImGui.BulletText($"Stop once relic completed");

                        ImGui.Text("So if you stop and you're unsure why... this might be why");

                        ImGui.EndTooltip();
                    }
                }

                ImGui.SameLine(0, 10 * scale);
                ImGui.SetCursorPosY(ImGui.GetCursorPosY() + yOffset);

                bool unsupportedArtisan = xpLeveling && !P.Artisan.UpdatedArtisan() && CosmicHelper.CrafterJobList.Contains((uint)Player.Job);
                bool unsupportedMoon = PlayerHelper.IsInOizys() && xpLeveling;

                using (ImRaii.Disabled(SchedulerMain.State != IceState.Idle || !usingSupportedJob || unsupportedArtisan || unsupportedMoon))
                {
                    if (ImGui.Button("Start", new Vector2(150 * scale, 0)))
                    {
                        SchedulerMain.EnablePlugin();
                    }
                }

                if (unsupportedArtisan)
                {
                    ImGui.SameLine(0, 10 * scale);
                    ImGui.SetCursorPosY(ImGui.GetCursorPosY() + yOffset);
                    ImGuiEx.Icon(EColor.Red, FontAwesomeIcon.ExclamationTriangle);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text("Hey! You need to update artisan to use this mode, please update to at minimum:");
                        ImGui.Text("4.0.4.29");
                        ImGui.EndTooltip();
                    }
                }
                else if (unsupportedMoon)
                {
                    ImGui.SameLine(0, 10 * scale);
                    ImGui.SetCursorPosY(ImGui.GetCursorPosY() + yOffset);
                    ImGuiEx.Icon(EColor.Red, FontAwesomeIcon.ExclamationTriangle);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text("Hey! This moon is currently not supported for leveling yet. (It's also worse than sinus or phaenna)");
                        ImGui.Text("Please wait till I get the time to focus on this");
                        ImGui.EndTooltip();
                    }
                }

                ImGui.SameLine(0, 10 * scale);
                ImGui.SetCursorPosY(ImGui.GetCursorPosY() + yOffset);

                using (ImRaii.Disabled(SchedulerMain.State == IceState.Idle))
                {
                    using (ImRaii.PushColor(ImGuiCol.Button, new Vector4(0.8f, 0.2f, 0.2f, 1.0f)))
                    using (ImRaii.PushColor(ImGuiCol.ButtonHovered, new Vector4(0.9f, 0.3f, 0.3f, 1.0f)))
                    using (ImRaii.PushColor(ImGuiCol.ButtonActive, new Vector4(0.7f, 0.1f, 0.1f, 1.0f)))
                    {
                        if (ImGui.Button("Stop", new Vector2(150 * scale, 0)))
                        {
                            SchedulerMain.DisablePlugin();
                        }
                    }
                }
            }
        }

        private static void TableSettings()
        {
            if (ImGui.BeginTable("modeSelect_TableHeader", 5, ImGuiTableFlags.SizingFixedFit, Vector2.Zero))
            {
                ImGui.TableSetupColumn("Class Selector");
                ImGui.TableSetupColumn("Other Settings");

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                bool tableSettingExpanded = modeSelect_Tools.DrawCompactCategoryHeader("Table Settings", FontAwesomeIcon.Table);

                ImGui.TableNextColumn();
                bool missionSettingExpanded = modeSelect_Tools.DrawCompactCategoryHeader("Mission Settings", FontAwesomeIcon.UserCog);

                bool relicGrindExpanded = false;
                if (C.XPRelicGrind)
                {
                    ImGui.TableNextColumn();
                    relicGrindExpanded = modeSelect_Tools.DrawCompactCategoryHeader("Relic Grind Settings", FontAwesomeIcon.ArrowUpRightDots);
                }

                bool completionExpanded = false;
                if (C.ShowCompletionWindow)
                {
                    ImGui.TableNextColumn();
                    completionExpanded = modeSelect_Tools.DrawCompactCategoryHeader("Completion Table Settings", FontAwesomeIcon.Trophy);
                }

                bool showPlaylistExpanded = false;
                bool standard = !(C.XPRelicGrind || C.XPLeveling_Mode || C.ShowCompletionWindow);
                if (standard)
                {
                    ImGui.TableNextColumn();
                    showPlaylistExpanded = modeSelect_Tools.DrawCompactCategoryHeader("Mission Presets", FontAwesomeIcon.PlayCircle);
                }

                bool showJobSwapExpanded = false;
                bool relicJobSwap = C.TurninRelic;

                if (relicJobSwap)
                {
                    ImGui.TableNextColumn();
                    showJobSwapExpanded = modeSelect_Tools.DrawCompactCategoryHeader("Relic Job Swap", FontAwesomeIcon.Hammer);
                }

                bool showNextColumn = tableSettingExpanded || missionSettingExpanded || (relicGrindExpanded && C.XPRelicGrind) || (completionExpanded && C.ShowCompletionWindow) || showPlaylistExpanded || showJobSwapExpanded;

                if (showNextColumn)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    if (tableSettingExpanded)
                    {
                        Settings_TableColumns.ColumnSettings();
                    }

                    ImGui.TableNextColumn();
                    if (missionSettingExpanded)
                    {
                        Settings_TableColumns.GeneralMissionSettings();
                    }

                    if (C.XPRelicGrind)
                    {
                        ImGui.TableNextColumn();
                        if (relicGrindExpanded)
                        {
                            bool relicTurnin = C.TurninRelic;
                            if (ImGui.Checkbox($"Turnin if relic is complete##RelicTurnin_RelicGrind", ref relicTurnin))
                            {
                                C.TurninRelic = relicTurnin;
                                C.Save();
                            }
                            ImGui.SameLine();
                            ImGui.TextDisabled("?");
                            if (ImGui.IsItemHovered())
                            {
                                ImGui.SetTooltip("THIS IS YOUR HEADS UP ON HOW THIS WORKS. If I change this in the future, this tooltip will also change.\n" +
                                                 "1: This will check for your current CLASS [not menu class, actual current class] for relic turnin.\n" +
                                                 "2: This will take prio over \"Stop @ Relic Turnin\", in the sense that if you have both enabled, it will turnin vs stop. And continue about it's day\n" +
                                                 "3: If you're on a crafting class, it will return you back to the stop you were crafting post turnin. \n" +
                                                 "\t- This is optional, you can disable it at your own free will, I just like this so I can just go back to an isolated area of my choosing");
                            }

                            ImGui.Separator();

                            bool EnableRelicXp = C.XPRelicGrind;
                            if (ImGui.Checkbox("Auto-Pick For Relic XP", ref EnableRelicXp))
                            {
                                C.XPRelicGrind = EnableRelicXp;
                                C.Save();
                            }
                            ImGui.SameLine();
                            ImGui.TextDisabled("?");
                            if (ImGui.IsItemHovered())
                            {
                                ImGui.SetTooltip("Please note. This will ONLY grind for relic Exp under the basic mission tab. \n" +
                                                   "This will NOT work (even with missions selected) on the Sequence/Timed/Weather/Critical Missions");
                            }
                            if (EnableRelicXp)
                            {
                                bool OnlySelected = C.XPRelicOnlyEnabled;
                                if (ImGui.Checkbox("Only selected missions", ref OnlySelected))
                                {
                                    C.XPRelicOnlyEnabled = OnlySelected;
                                    C.Save();
                                }
                                if (C.ShowManualMode)
                                {
                                    bool IgnoreManual = C.XPRelicIgnoreManual;
                                    if (ImGui.Checkbox("Ignore Manual Mode Missions", ref IgnoreManual))
                                    {
                                        C.XPRelicIgnoreManual = IgnoreManual;
                                        C.Save();
                                    }
                                }
                            }
                        }
                    }

                    if (C.ShowCompletionWindow)
                    {
                        ImGui.TableNextColumn();
                        if (completionExpanded)
                        {
                            bool showSelectedJobOnly = C.ShowSelectedJobOnly;
                            if (ImGui.Checkbox("Show only selected job", ref showSelectedJobOnly))
                            {
                                C.ShowSelectedJobOnly = showSelectedJobOnly;
                                if (showSelectedJobOnly)
                                    C.ShowCompletionOnlyJob = false;
                                C.Save();
                            }

                            bool nonGold = C.ShowCompletion_MissingGold;
                            if (ImGui.Checkbox("Show Only Non-Gold Missions", ref nonGold))
                            {
                                C.ShowCompletion_MissingGold = nonGold;
                                C.Save();
                            }
                        }
                    }

                    if (standard)
                    {
                        ImGui.TableNextColumn();

                        if (showPlaylistExpanded)
                        {
                            if (ImGui.Button("Save Current Mission Preset"))
                            {
                                ImGui.OpenPopup("Preset Save Editor");
                            }

                            if (ImGui.BeginPopup("Preset Save Editor"))
                            {
                                ImGui.InputText($"Playlist Name", ref newListName);
                                using (ImRaii.Disabled(string.IsNullOrEmpty(newListName)))
                                {
                                    if (ImGui.Button("Save New List"))
                                    {
                                        List<uint> new_Playlist = new();
                                        foreach (var mission in C.MissionConfig.Where(x => x.Value.Enabled))
                                        {
                                            new_Playlist.Add(mission.Key);
                                        }
                                        if (C.Mission_Playlist.ContainsKey(newListName))
                                        {
                                            C.Mission_Playlist[newListName] = new_Playlist;
                                        }
                                        else
                                        {
                                            C.Mission_Playlist.Add(newListName, new_Playlist);
                                        }
                                        C.Save();
                                        ImGui.CloseCurrentPopup();
                                    }
                                }

                                ImGui.EndPopup();
                            }

                            if (C.Mission_Playlist.Count > 0)
                            {
                                if (ImGui.Button("View All Presets"))
                                {
                                    ImGui.OpenPopup("Preset: List Viewer");
                                }

                                if (ImGui.BeginPopup("Preset: List Viewer"))
                                {
                                    ImGui.Text($"Load Mission Preset");

                                    if (ImGui.BeginTable($"Preset: TableViewer", 3, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders))
                                    {
                                        ImGui.TableSetupColumn("Name");
                                        ImGui.TableSetupColumn("Amount Enabled");

                                        ImGui.TableHeadersRow();

                                        ImGui.TableNextRow();
                                        ImGui.TableSetColumnIndex(0);
                                        ImGui.AlignTextToFramePadding();
                                        ImGui.Text($"Clear All");
                                        ImGui.SameLine();
                                        if (ImGuiEx.IconButton(FontAwesomeIcon.ArrowUpRightFromSquare, $"FreshPreset_Button"))
                                        {
                                            foreach (var mission in C.MissionConfig)
                                            {
                                                mission.Value.Enabled = false;
                                            }
                                            C.Save();
                                            ImGui.CloseCurrentPopup();
                                        }

                                        foreach (var item in C.Mission_Playlist)
                                        {
                                            ImGui.TableNextRow();
                                            ImGui.TableSetColumnIndex(0);
                                            ImGui.AlignTextToFramePadding();
                                            ImGui.Text($"{item.Key}");
                                            ImGui.SameLine();
                                            if (ImGuiEx.IconButton(FontAwesomeIcon.ArrowUpRightFromSquare, $"{item.Key}_Button"))
                                            {
                                                foreach (var mission in C.MissionConfig)
                                                {
                                                    if (item.Value.Contains(mission.Key))
                                                        mission.Value.Enabled = true;
                                                    else
                                                        mission.Value.Enabled = false;
                                                }
                                                C.Save();
                                                ImGui.CloseCurrentPopup();
                                            }
                                            if (ImGui.IsItemHovered())
                                            {
                                                ImGui.SetTooltip("Import Missions");
                                            }

                                            ImGui.TableNextColumn();
                                            ImGui.AlignTextToFramePadding();
                                            ImGui.Text($"{item.Value.Count}");

                                            ImGui.TableNextColumn();
                                            if (ImGuiEx.IconButton(FontAwesomeIcon.Trash, $"{item.Key}_Remove"))
                                            {
                                                C.Mission_Playlist.Remove(item);
                                                C.Save();
                                            }
                                            if (ImGui.IsItemHovered())
                                            {
                                                ImGui.SetTooltip("Remove from list");
                                            }
                                        }

                                        ImGui.EndTable();
                                    }

                                    ImGui.EndPopup();
                                }
                            }
                        }
                    }

                    if (C.TurninRelic)
                    {
                        ImGui.TableNextColumn();
                        if (showJobSwapExpanded)
                        {
                            bool swapJobs = C.Relic_SwapJob;
                            if (ImGui.Checkbox("Swap jobs when turning in relic", ref swapJobs))
                            {
                                C.Relic_SwapJob = swapJobs;
                                C.Save();
                            }

                            string currentJobName = BattleJobs.FirstOrDefault(x => x.Value == C.Relic_BattleJob).Key ?? "None";

                            if (ImGui.BeginCombo("Battle Job", currentJobName))
                            {
                                foreach (var job in BattleJobs)
                                {
                                    bool isSelected = C.Relic_BattleJob == job.Value;
                                    if (ImGui.Selectable(job.Key, isSelected))
                                    {
                                        C.Relic_BattleJob = job.Value;
                                        C.Save();
                                    }
                                    if (isSelected)
                                        ImGui.SetItemDefaultFocus();
                                }
                                ImGui.EndCombo();
                            }

                            bool useStylist = C.Relic_Stylist;
                            if (ImGui.Checkbox($"Use Stylist to re-equip tools", ref useStylist))
                            {
                                C.Relic_Stylist = useStylist;
                                C.Save();
                            }
                        }
                    }
                }

                ImGui.EndTable();
            }
        }

        public static Dictionary<string, List<Mission>> missionList = new()
        {
            ["Red Alert"] = new(),
            ["Weather"] = new(),
            ["Timed"] = new(),
            ["Sequence"] = new(),
            ["ARank"] = new(),
            ["BRank"] = new(),
            ["CRank"] = new(),
            ["DRank"] = new(),
            ["All Enabled"] = new(),
        };

        public class Mission
        {
            public uint id;
            public bool enabled;
        }

        private static void MissionButtons()
        {
            foreach (var type in missionList)
            {
                type.Value.Clear();
            }

            foreach (var mission in CosmicHelper.SheetMissionDict)
            {
                var Jobs = mission.Value.Jobs;
                var territoryId = mission.Value.TerritoryId;
                uint selectedJob = C.SelectedJob;
                bool sinusEnabled = C.ShowSinusMissions;
                bool phaennaEnabled = C.ShowPhaennaMissions;
                bool oizysEnabled = C.ShowOizysMissions;

                if (!sinusEnabled && territoryId == 1237)
                    continue;

                if (!phaennaEnabled && territoryId == 1291)
                    continue;

                if (!oizysEnabled && territoryId == 1310)
                    continue;

                bool provisional = mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalWeather)
                || mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalTimed)
                || mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalSequential);

                if (provisional)
                {
                    if (!C.GrindAllProvisionals)
                    {
                        if (!Jobs.Contains(selectedJob))
                            continue;
                    }

                    if (mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalWeather))
                        missionList["Weather"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });
                    else if (mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalTimed))
                        missionList["Timed"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });
                    else if (mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalSequential))
                        missionList["Sequence"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });

                    if (C.MissionConfig.ContainsKey(mission.Key) && C.MissionConfig[mission.Key].Enabled)
                    {
                        missionList["All Enabled"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });
                    }
                }
                else
                {
                    if (!Jobs.Contains(selectedJob))
                        continue;

                    if (mission.Value.Attributes.HasFlag(MissionAttributes.Critical))
                        missionList["Critical"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });
                    else if (mission.Value.Rank > 3)
                        missionList["ARank"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });
                    else if (mission.Value.Rank == 3)
                        missionList["BRank"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });
                    else if (mission.Value.Rank == 2)
                        missionList["CRank"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });
                    else if (mission.Value.Rank == 1)
                        missionList["DRank"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });

                    if (C.MissionConfig.ContainsKey(mission.Key) && C.MissionConfig[mission.Key].Enabled)
                    {
                        missionList["All Enabled"].Add(new() { id = mission.Key, enabled = C.MissionConfig[mission.Key].Enabled });
                    }
                }
            }
        }
    }
}
