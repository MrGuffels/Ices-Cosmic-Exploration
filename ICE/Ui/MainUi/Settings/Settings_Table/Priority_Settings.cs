using Dalamud.Interface;
using ICE.Utilities.ImGuiTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICE.ConfigFiles.Config;

namespace ICE.Ui.SettingTabs
{
    internal class Priority_Settings
    {
        public static void Draw()
        {
            MissionTypeOrderUi();

            TypePriorityUi();

            JobPriorityUi();
        }

        private static ImGuiEx.RealtimeDragDrop<ProvisionalTypes>? _dragDrop_ProvisionalType;

        private static void TypePriorityUi()
        {
            _dragDrop_ProvisionalType ??= new ImGuiEx.RealtimeDragDrop<ProvisionalTypes>(
                "ProvisionalTypeDragDrop",
                (info) => $"{info}_{info.GetHashCode()}",
                smallButton: false
            );

            _dragDrop_ProvisionalType.Begin();

            if (ImGui.BeginTable("Type Priority Table", 3, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("ReOrder");
                ImGui.TableSetupColumn("Icon");
                ImGui.TableSetupColumn("Type");

                ImGui.TableHeadersRow();

                for (int i = 0; i < C.MissionPrio.Count; i++)
                {
                    ImGui.PushID(i);
                    var entry = C.MissionPrio[i];

                    ImGui.TableNextRow();
                    _dragDrop_ProvisionalType.NextRow();
                    _dragDrop_ProvisionalType.SetRowColor(entry);

                    ImGui.TableSetColumnIndex(0);
                    _dragDrop_ProvisionalType.DrawButtonDummy(entry, C.MissionPrio, i);

                    ImGui.TableNextColumn();
                    ImGui.AlignTextToFramePadding();
                    FontAwesomeIcon icon = entry switch
                    {
                        ProvisionalTypes.ProvisionalTimed => FontAwesomeIcon.Clock,
                        ProvisionalTypes.ProvisionalSequential => FontAwesomeIcon.ListOl,
                        ProvisionalTypes.ProvisionalWeather => FontAwesomeIcon.Cloud,
                        _ => FontAwesomeIcon.Question,
                    };

                    ImGuiEx.Icon(icon);

                    ImGui.TableNextColumn();
                    ImGui.AlignTextToFramePadding();
                    string type = entry switch
                    {
                        ProvisionalTypes.ProvisionalTimed => "Timed",
                        ProvisionalTypes.ProvisionalSequential => "Sequence",
                        ProvisionalTypes.ProvisionalWeather => "Weather",
                        _ => entry.ToString()
                    };
                    ImGui.Text($"{type}");

                    ImGui.PopID();
                }

                ImGui.EndTable();
            }

            _dragDrop_ProvisionalType.End();
        }

        private static ImGuiEx.RealtimeDragDrop<MissionTypes>? _dragDrop_MissionType;

        private static void MissionTypeOrderUi()
        {
            _dragDrop_MissionType ??= new ImGuiEx.RealtimeDragDrop<MissionTypes>(
                "MissionTypeDragDrop",
                (info) => $"{info}_{info.GetHashCode()}",
                smallButton: false
            );

            _dragDrop_MissionType.Begin();

            if (ImGui.BeginTable("Mission Type Table", 3, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("ReOrder");
                ImGui.TableSetupColumn("Icon");
                ImGui.TableSetupColumn("Type");

                ImGui.TableHeadersRow();

                for (int i = 0; i < C.MissionTypePrio.Count; i++)
                {
                    ImGui.PushID(i);
                    var entry = C.MissionTypePrio[i];

                    ImGui.TableNextRow();
                    _dragDrop_MissionType.NextRow();
                    _dragDrop_MissionType.SetRowColor(entry);

                    ImGui.TableSetColumnIndex(0);
                    _dragDrop_MissionType.DrawButtonDummy(entry, C.MissionTypePrio, i);

                    ImGui.TableNextColumn();
                    ImGui.AlignTextToFramePadding();
                    FontAwesomeIcon icon = entry switch
                    {
                        MissionTypes.DroneSearch => FontAwesomeIcon.Satellite,
                        MissionTypes.RedAlert => FontAwesomeIcon.Bell,
                        MissionTypes.Provisional => FontAwesomeIcon.HourglassHalf,
                        MissionTypes.Standard => FontAwesomeIcon.Star,
                        _ => FontAwesomeIcon.Question
                    };
                    ImGuiEx.Icon(icon);

                    ImGui.TableNextColumn();
                    ImGui.AlignTextToFramePadding();
                    string name = entry switch
                    {
                        MissionTypes.DroneSearch => "Drone Search",
                        MissionTypes.RedAlert => "Red Alert",
                        MissionTypes.Provisional => "Provisional Missions [Weather/Timed/Sequence]",
                        MissionTypes.Standard => "Standard Missions [A->D]",
                        _ => $"{entry}"
                    };
                    ImGui.Text($"{name}");
                    if (entry == MissionTypes.DroneSearch && !C.Cosmodrone_Run)
                    {
                        ImGui.SameLine();
                        ImGui_Ice.IconWithTooltip(FontAwesomeIcon.ExclamationTriangle,
                            "Finding drone locations is turned off, so we're just going to ignore this. If you want to run this, please enable it");
                    }

                    ImGui.PopID();
                }

                ImGui.EndTable();
            }

            _dragDrop_MissionType.End();
        }

        private static ImGuiEx.RealtimeDragDrop<uint>? _dragDrop_JobPrio;

        private static void JobPriorityUi()
        {
            _dragDrop_JobPrio ??= new ImGuiEx.RealtimeDragDrop<uint>(
                "JobPrioDragDrop",
                (info) => ($"{info}_{info.GetHashCode()}"),
                smallButton: false
            );

            _dragDrop_JobPrio.Begin();

            if (ImGui.BeginTable("Job Priority Order", 3, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("ReOrder");
                ImGui.TableSetupColumn("Icon");
                ImGui.TableSetupColumn("Type");

                ImGui.TableHeadersRow();

                for (int i = 0; i < C.JobPrio.Count(); i++)
                {
                    ImGui.PushID(i);

                    var entry = C.JobPrio[i];
                    ImGui.TableNextRow();
                    _dragDrop_JobPrio.NextRow();
                    _dragDrop_JobPrio.SetRowColor(entry);

                    ImGui.TableSetColumnIndex(0);
                    _dragDrop_JobPrio.DrawButtonDummy(entry, C.JobPrio, i);

                    ImGui.TableNextColumn();
                    if (CosmicHelper.JobIconDict.TryGetValue(entry, out var icon))
                    {
                        ImGui.Image(icon.GetWrapOrEmpty().Handle, new(24, 24));
                    }

                    ImGui.TableNextColumn();
                    ImGui.AlignTextToFramePadding();
                    ImGui.Text($"{GetJobName(entry)}");

                    ImGui.PopID();
                }

                ImGui.EndTable();
            }

            _dragDrop_JobPrio.End();
        }

        // Quick way of assigning Icons to the mission types
        private static string GetMissionTypeIcon(ProvisionalTypes type)
        {
            return type switch
            {
                ProvisionalTypes.ProvisionalWeather => FontAwesomeIcon.Cloud.ToIconString(),
                ProvisionalTypes.ProvisionalSequential => FontAwesomeIcon.ListOl.ToIconString(),
                ProvisionalTypes.ProvisionalTimed => FontAwesomeIcon.Clock.ToIconString(),
                _ => FontAwesomeIcon.Question.ToIconString()
            };
        }

        // Job name helper
        private static string GetJobName(uint jobId)
        {
            return jobId switch
            {
                8 => "Carpenter",
                9 => "Blacksmith",
                10 => "Armorer",
                11 => "Goldsmith",
                12 => "Leatherworker",
                13 => "Weaver",
                14 => "Alchemist",
                15 => "Culinarian",
                16 => "Miner",
                17 => "Botanist",
                18 => "Fisher",
                _ => "Unknown Job"
            };
        }

        // Job icon helper
        private static string GetJobIcon(uint jobId)
        {
            return jobId switch
            {
                8 or 9 or 10 or 11 or 12 or 13 or 14 or 15 => FontAwesomeIcon.Hammer.ToIconString(), // Crafters
                16 or 17 => FontAwesomeIcon.Mountain.ToIconString(), // MIN/BTN
                18 => FontAwesomeIcon.Fish.ToIconString(), // FSH
                _ => FontAwesomeIcon.Question.ToIconString()
            };
        }
    }
}