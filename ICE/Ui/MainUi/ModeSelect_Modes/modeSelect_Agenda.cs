using Dalamud.Interface.Utility.Raii;
using System;
using System.Collections.Generic;
using System.Text;
using TerraFX.Interop.Windows;
using static ICE.ConfigFiles.Config;

namespace ICE.Ui.MainUi.ModeSelect_Modes
{
    internal class modeSelect_Agenda
    {
        public static List<uint> JobOptions = new() { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };

        public static uint SelectedJob = 8;
        public static PlaylistOptions SelectedOption = PlaylistOptions.None;

        public static void Draw()
        {
            var selectedJobIcon = CosmicHelper.JobIconDict[SelectedJob];
            var selectedJobName = CosmicHelper.GetJobName(SelectedJob);

            ImGui.Image(selectedJobIcon.GetWrapOrEmpty().Handle, new Vector2(20, 20));
            ImGui.SameLine();
            ImGui.SetNextItemWidth(200);

            if (ImGui.BeginCombo("##JobCombo", selectedJobName))
            {
                using (var table = ImRaii.Table("JobSelectionTable", 2, ImGuiTableFlags.BordersInnerV))
                {
                    if (table)
                    {
                        ImGui.TableSetupColumn("Icon", ImGuiTableColumnFlags.WidthFixed, 24);
                        ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch);

                        foreach (var jobId in JobOptions)
                        {
                            var jobIcon = CosmicHelper.JobIconDict[jobId];
                            var jobName = CosmicHelper.GetJobName(jobId);
                            bool isSelected = jobId == SelectedJob;

                            ImGui.TableNextRow();
                            ImGui.TableNextColumn();

                            // Icon column
                            ImGui.Image(jobIcon.GetWrapOrEmpty().Handle, new Vector2(20, 20));

                            ImGui.TableNextColumn();

                            // Name column with selectable
                            if (ImGui.Selectable($"{jobName}##{jobName}_{jobId}", isSelected, ImGuiSelectableFlags.SpanAllColumns))
                            {
                                SelectedJob = jobId;
                            }

                            if (isSelected)
                            {
                                ImGui.SetItemDefaultFocus();
                            }
                        }
                    }
                }

                ImGui.EndCombo();
            }

            ImGui.SameLine();
            var optionName = PlaylistOptionString(SelectedOption);

            ImGui.SetNextItemWidth(200);
            if (ImGui.BeginCombo("##Playlist Options", optionName))
            {
                foreach (PlaylistOptions option in Enum.GetValues<PlaylistOptions>())
                {
                    var displayName = PlaylistOptionString(option);
                    bool isSelected = SelectedOption == option;

                    if (ImGui.Selectable($"{displayName}##{option}", isSelected))
                    {
                        SelectedOption = option;
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }

                ImGui.EndCombo();
            }

            ImGui.SameLine();
            using (ImRaii.Disabled(SelectedOption == PlaylistOptions.None))
            {
                if (ImGui.Button("Add to Cosmic Agenda"))
                {
                    var newAgenda = new AgendaInfo()
                    {
                        SelectedOption = SelectedOption,
                        SelectedJob = SelectedJob
                    };

                    C.Cosmic_Agenda.Add(newAgenda);
                    C.SaveDebounced();
                }
            }

            CosmicAgendaTable();
        }

        private static string PlaylistOptionString(PlaylistOptions option)
        {
            return option switch
            {
                PlaylistOptions.None => "None",
                PlaylistOptions.SinusMax => "Max Sinus Relic [Lv. 9]",
                PlaylistOptions.PhaennaMax => "Max Phaenna Relic [Lv. 14]",
                PlaylistOptions.OizysMax => "Max Oizys Relic [Lv. 17]",
                PlaylistOptions.SelectedRelicLv => "Selected Relic Level",
                PlaylistOptions.CreditAmount => "Credit Amount",
                PlaylistOptions.PlanetAmount => "Planetary Credit Amount",
                PlaylistOptions.DronebitAmount => "Planetary Dronebit Amount",
                PlaylistOptions.ClassLevel => "Class Level",
                PlaylistOptions.ClassScore => "Class Score",
                PlaylistOptions.GoldClassMissions => "All Missions Golded",
                _ => "???"
            };
        }

        private static string ModeSelectString(ModeSelect mode)
        {
            return mode switch
            {
                ModeSelect.Standard => "Standard",
                ModeSelect.RelicMode => "Relic Grind Mode",
                ModeSelect.LevelMode => "Leveling Mode",
                ModeSelect.ScoreMode => "Scoring Mode",
                ModeSelect.AgendaMode => "Cosmic Agenda Mode",
                _ => $"??? {mode}"
            };
        }

        private static void CosmicAgendaTable()
        {
            using (var PlaylistTable = ImRaii.Table("Cosmic Agenda Table", 6, ImGuiTableFlags.Borders | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.RowBg))
            {
                ImGui.TableSetupColumn("##Reorder");
                ImGui.TableSetupColumn("Job");
                ImGui.TableSetupColumn("Agenda");
                ImGui.TableSetupColumn("Run Until..");
                ImGui.TableSetupColumn("Mode Select");
                ImGui.TableSetupColumn("Remove");

                ImGui.TableHeadersRow();
                for (int i = 0; i < C.Cosmic_Agenda.Count; i++)
                {
                    ImGui.PushID(i);

                    var agendaInfo = C.Cosmic_Agenda[i];

                    var selectedOption = agendaInfo.SelectedOption;

                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGuiEx.IconButton(Dalamud.Interface.FontAwesomeIcon.Wrench);
                    // Reorder thing here, will do after

                    ImGui.TableNextColumn();
                    var jobImage = CosmicHelper.JobIconDict[agendaInfo.SelectedJob];
                    float zoom = 0.15f;

                    if (ImGui.ImageButton(
                        jobImage.GetWrapOrEmpty().Handle,
                        new Vector2(20, 20),
                        new Vector2(zoom, zoom),        // UV0 - top-left of texture
                        new Vector2(1 - zoom, 1 - zoom) // UV1 - bottom-right of texture
                    ))
                    {
                        ImGui.OpenPopup("Job Selection");
                    }
                    using (var popup = ImRaii.Popup("Job Selection"))
                    {
                        if (popup)
                        {
                            using (var table = ImRaii.Table("JobTable", 2, ImGuiTableFlags.BordersInnerV))
                            {
                                if (table)
                                {
                                    ImGui.TableSetupColumn("Icon", ImGuiTableColumnFlags.WidthFixed, 24);
                                    ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch);

                                    foreach (var jobId in JobOptions)
                                    {
                                        var jobIcon = CosmicHelper.JobIconDict[jobId];
                                        var jobName = CosmicHelper.GetJobName(jobId);
                                        bool isSelected = jobId == SelectedJob;

                                        ImGui.TableNextRow();
                                        ImGui.TableNextColumn();

                                        // Icon column
                                        ImGui.Image(jobIcon.GetWrapOrEmpty().Handle, new Vector2(20, 20));

                                        ImGui.TableNextColumn();

                                        // Name column with selectable
                                        if (ImGui.Selectable($"{jobName}##{jobName}_{jobId}", isSelected, ImGuiSelectableFlags.SpanAllColumns))
                                        {
                                            agendaInfo.SelectedJob = jobId;
                                            C.Save();
                                        }

                                        if (isSelected)
                                        {
                                            ImGui.SetItemDefaultFocus();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ImGui.TableNextColumn();
                    ImGui.SetNextItemWidth(200);
                    var optionName = PlaylistOptionString(selectedOption);
                    if (ImGui.BeginCombo("##Playlist Options", optionName))
                    {
                        foreach (PlaylistOptions option in Enum.GetValues<PlaylistOptions>())
                        {
                            var displayName = PlaylistOptionString(option);
                            bool isSelected = agendaInfo.SelectedOption == option;

                            if (ImGui.Selectable($"{displayName}##{option}", isSelected))
                            {
                                agendaInfo.SelectedOption = option;
                                C.Save();
                            }

                            if (isSelected)
                            {
                                ImGui.SetItemDefaultFocus();
                            }
                        }

                        ImGui.EndCombo();
                    }

                    ImGui.TableNextColumn();
                    ImGui.SetNextItemWidth(150);
                    if (selectedOption == PlaylistOptions.SelectedRelicLv)
                    {
                        var level = agendaInfo.SelectedRelicLevel;
                        if (ImGui.InputInt("##Relic Level", ref level))
                        {
                            agendaInfo.SelectedRelicLevel = level;
                            C.SaveDebounced();
                        }
                    }
                    else if (selectedOption == PlaylistOptions.CreditAmount)
                    {
                        var creditAmount = agendaInfo.CreditAmount;
                        if (ImGui.SliderInt("##Credit Amount", ref creditAmount, 0, 30_000))
                        {
                            agendaInfo.CreditAmount = creditAmount;
                            C.SaveDebounced();
                        }
                    }
                    else if (selectedOption == PlaylistOptions.PlanetAmount)
                    {
                        var planetCredit = agendaInfo.PlanetAmount;
                        if (ImGui.SliderInt("##Planet Amount", ref planetCredit, 0, 10_000))
                        {
                            agendaInfo.PlanetAmount = planetCredit;
                            C.SaveDebounced();
                        }
                    }
                    else if (selectedOption == PlaylistOptions.DronebitAmount)
                    {
                        var dronebitAmount = agendaInfo.DronebitAmount;
                        if (ImGui.SliderInt("##Dronebit Amount", ref dronebitAmount, 0, 5_000))
                        {
                            agendaInfo.DronebitAmount = dronebitAmount;
                            C.SaveDebounced();
                        }
                    }
                    else if (selectedOption == PlaylistOptions.ClassLevel)
                    {
                        var classLevel = agendaInfo.ClassLevel;
                        if (ImGui.InputInt("##ClassLevel", ref classLevel))
                        {
                            agendaInfo.ClassLevel = classLevel;
                            C.SaveDebounced();
                        }
                    }
                    else if (selectedOption == PlaylistOptions.ClassScore)
                    {
                        var score = agendaInfo.ClassScore;
                        if (ImGui.SliderInt("##ClassScore", ref score, 0, 500_000))
                        {
                            agendaInfo.ClassScore = score;
                            C.SaveDebounced();
                        }
                    }

                    ImGui.TableNextColumn();
                    var currentMode = agendaInfo.SelectedMode;
                    ImGui.SetNextItemWidth(150);
                    if (ImGui.BeginCombo("##Mode Selection", ModeSelectString(currentMode)))
                    {
                        foreach (ModeSelect option in Enum.GetValues(typeof(ModeSelect)))
                        {
                            if (option == ModeSelect.AgendaMode)
                                continue;
                            else
                            {
                                var displayName = ModeSelectString(option);
                                bool isSelected = agendaInfo.SelectedMode == option;

                                if (ImGui.Selectable($"{displayName}##{option}", isSelected))
                                {
                                    agendaInfo.SelectedMode = option;
                                    C.Save();
                                }

                                if (isSelected)
                                {
                                    ImGui.SetItemDefaultFocus();
                                }
                            }
                        }

                        ImGui.EndCombo();
                    }

                    ImGui.PopID();
                }
            }
        }

        private static void Option_RelicLv()
        {

        }
    }
}
