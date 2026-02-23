using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using ICE.Utilities.ImGuiTools;
using Lumina.Excel.Sheets;
using Pictomancy;
using System.Collections.Generic;
using static ICE.ConfigFiles.Config;

namespace ICE.Ui.MainUi.Settings.Settings_Table
{
    internal class Misc_Settings
    {
        public static void Draw()
        {
            OverlaySettings();
            Separator();
            AutoUse();
            Separator();
            RepairSettings();
            Separator();
            SafetySettings.Draw();
            Separator();
            ArtisanSettings();
            Separator();
            TimeRecords();
            Separator();
            PostMissionCommands();
#if DEBUG
            Separator();
            DebugTab.Draw();
#endif
        }

        public static void OverlaySettings()
        {
            ImGuiEx.IconWithText(FontAwesomeIcon.WindowMaximize, "Overlay Window");
            ImGui.Dummy(new (0, 5));

            bool showOverlay = C.ShowOverlay;
            if (ImGui.Checkbox("Show Overlay", ref showOverlay))
            {
                C.ShowOverlay = showOverlay;
                C.Save();
            }
            ImGui.SameLine();
            bool useCogsIcon = C.Overlay_UseCogsIcon;
            if (ImGui.Checkbox("Use cogs button instead of home", ref useCogsIcon))
            {
                C.Overlay_UseCogsIcon = useCogsIcon;
                C.Save();
            }

            bool ShowSeconds = C.ShowSeconds;
            if (ImGui.Checkbox("Show Seconds", ref ShowSeconds))
            {
                C.ShowSeconds = ShowSeconds;
                C.Save();
            }

            bool showExpOverlay = C.ShowExpBars;
            if (ImGui.Checkbox("Show Experience Bars on Overlay", ref showExpOverlay))
            {
                C.ShowExpBars = showExpOverlay;
                C.Save();
            }
            if (showExpOverlay)
            {
                ImGui.SameLine();
                bool hideWhenMaxed = C.ShowExpBars_HideWhenMaxed;
                if (ImGui.Checkbox("Until maxed only", ref hideWhenMaxed))
                {
                    C.ShowExpBars_HideWhenMaxed = hideWhenMaxed;
                    C.Save();
                }
            }

            bool showClassScore = C.ShowCurrentScore;
            if (ImGui.Checkbox("Show Current Class Score", ref showClassScore))
            {
                C.ShowCurrentScore = showClassScore;
                C.Save();
            }
            ImGui.SameLine();
            bool showTotalScore = C.ShowTotalScore;
            if (ImGui.Checkbox("Show Total Score", ref showTotalScore))
            {
                C.ShowTotalScore = showTotalScore;
                C.Save();
            }

            bool AutoResize = C.Overlay_AutoResize;
            if (ImGui.Checkbox("Auto Resize Overlay", ref AutoResize))
            {
                C.Overlay_AutoResize = AutoResize;
                C.Save();
            }


            bool highlightTokenWeather = C.Overlay_HighlightTokenWeather;
            if (ImGui.Checkbox("Highlight EX+ token weathers", ref highlightTokenWeather))
            {
                C.Overlay_HighlightTokenWeather = highlightTokenWeather;
                C.Save();
            }

            bool filterByJob = C.Overlay_FilterByJob;
            if (ImGui.Checkbox("Filter timed/weather missions and weather highlights by current job", ref filterByJob))
            {
                C.Overlay_FilterByJob = filterByJob;
                C.Save();
            }

            bool disableHudClipping = C.DisableHudClipping;
            if (ImGui.Checkbox("Disable HUD Clipping", ref disableHudClipping))
            {
                C.DisableHudClipping = disableHudClipping;
                C.Save();
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("When enabled, overlays will render over the native UI elements");
            }

        }

        private static void AutoUse()
        {
            ImGuiEx.IconWithText(FontAwesomeIcon.PersonRays, "Auto-Use");
            ImGui.Dummy(new Vector2(0, 5));

            bool DisableLunarAura = C.RemoveStellarStatus;
            if (ImGui.Checkbox("Auto-Remove Stellar Status", ref DisableLunarAura))
            {
                C.RemoveStellarStatus = DisableLunarAura;
                C.Save();
            }
            ImGui.SameLine();
            ImGuiEx.IconWithTooltip(FontAwesomeIcon.InfoCircle,
                                   "Automatically removes the Star Contributor visual effect (the glow you get for being a top contributor).\n" +
                                   "The buff restores itself when you re-enter the zone.");

            bool autoStartOnMoonEnter = C.StartUponEnterMoon;
            if (ImGui.Checkbox("Auto start upon entering a Cosmic Exploration area", ref autoStartOnMoonEnter))
            {
                C.StartUponEnterMoon = autoStartOnMoonEnter;
                C.Save();
            }
            ImGui.SameLine();
            ImGuiEx.IconWithTooltip(FontAwesomeIcon.QuestionCircle,
                                   "This will check to see if you're on a gathering/crafting class upon first entering the moon.\n" +
                                   "If you are, it will automatically start as if you had pressed the start button yourself\n" +
                                   "Really useful if you have a tool to auto-log you in/if you just want to enter the moon and go\n" +
                                   "This will ONLY run upon first entry.");
            ImGui.Dummy(Vector2.Zero);
        }

        private static void RepairSettings()
        {
            ImGuiEx.IconWithText(FontAwesomeIcon.Hammer, "Repair Settings");
            ImGui.Dummy(new Vector2(0, 5));

            bool repairAtVendor = C.RepairAtVendor;
            if (ImGui.Checkbox("Repair at Vendor", ref repairAtVendor))
            {
                C.RepairAtVendor = repairAtVendor;
                C.Save();
            }

            using (ImRaii.Disabled(repairAtVendor))
            {
                bool selfRepairGather = C.SelfRepairGather;
                if (ImGui.Checkbox("Self Repair Gather", ref selfRepairGather))
                {
                    C.SelfRepairGather = selfRepairGather;
                    C.Save();
                }

                bool selfRepairCrafter = C.SelfRepairCrafter;
                if (ImGui.Checkbox("Self Repair Crafter", ref selfRepairCrafter))
                {
                    C.SelfRepairCrafter= selfRepairCrafter;
                    C.Save();
                }
            }

            float repairAmount = C.RepairPercent;
            ImGui.SetNextItemWidth(150);
            if (ImGui.SliderFloat("###Repair %", ref repairAmount, 0f, 99f, "%.0f%%"))
            {
                if (C.RepairPercent != repairAmount)
                {
                    C.RepairPercent = (int)repairAmount;
                    C.SaveDebounced();
                }
            }
        }

        private static void TimeRecords()
        {
            ImGuiEx.IconWithText(FontAwesomeIcon.Clock, "Record Settings");
            ImGui.Dummy(new Vector2(0, 5));

            int TimeHistory = C.TimeHistoryLimit;
            ImGui.SetNextItemWidth(100);
            if (ImGui.InputInt("Average Time History to keep", ref TimeHistory))
            {
                C.TimeHistoryLimit = TimeHistory;
                C.Save();
            }
            ImGui.SameLine();
            ImGui.TextDisabled("?");
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Anything below 0 to keep all logs\n" +
                                 "Above 0 to keep a set limit");
            }
        }

        private static void PostMissionCommands()
        {
            ImGuiEx.IconWithText(FontAwesomeIcon.Play, "Post Mission Commands");
            ImGui.Dummy(new Vector2(0, 5));

            ImGui.TextWrapped("Input below a list of commands that you would like to run after a run has been completed. \n" +
                              "This is kind of my way of letting you somewhat script/set up a sequence of other things that you would like to do that might not be included in the plugin itself. \n" +
                              "If you want something more complex, just make an SND script at that point. And have this run that script post lol.");

            if (ImGui.Button("Add New Command"))
            {
                C.PostMissionCommands.Add(new MissionCommand
                {
                    command = "",
                    Delay = 0,
                });
                C.Save();
            }

            MissionCommand? toRemove = null;
            int entryCounter = 0;

            if (ImGui.BeginTable("Mission Commands", 3, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("Command");
                ImGui.TableSetupColumn("Delay");
                ImGui.TableSetupColumn("Remove");

                ImGui.TableHeadersRow();

                foreach (var entry in C.PostMissionCommands)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.SetNextItemWidth(200);

                    ImGui.PushID($"{entryCounter}_MissionCommand");
                    string command = entry.command;
                    if (ImGui.InputText("##Command", ref command))
                    {
                        entry.command = command;
                        C.SaveDebounced();
                    }

                    ImGui.TableNextColumn();
                    ImGui.SetNextItemWidth(100);
                    int delay = entry.Delay;
                    if (ImGui.InputInt("###Delay", ref delay))
                    {
                        entry.Delay = delay;
                        C.SaveDebounced();
                    }

                    ImGui.TableNextColumn();
                    if (ImGuiEx.IconButton(FontAwesomeIcon.Trash, $"remove{C.PostMissionCommands.IndexOf(entry)}"))
                    {
                        toRemove = entry;
                    }
                    ImGui.PopID();
                    entryCounter += 1;
                }

                if (toRemove != null)
                {
                    C.PostMissionCommands.Remove(toRemove);
                    C.Save();
                }

                ImGui.EndTable();
            }
        }

        private static void ArtisanSettings()
        {
            ImGuiEx.IconWithText(FontAwesomeIcon.Wrench, "Artisan Settings");
            ImGui.Dummy(new Vector2(0, 5));

            bool force_Raphael = C.Artisan_RaphaelForce;
            bool expertRaphael = C.Artisan_RaphaelMaster;

            if (ImGui.Checkbox("Enforce Raphael Solver", ref force_Raphael))
            {
                C.Artisan_RaphaelForce = force_Raphael;
                C.Save();
            }
            ImGui.SameLine();
            ImGui_Ice.IconWithTooltip(FontAwesomeIcon.QuestionCircle);
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text($"Will force all crafts while the plugin is running to use the raphael solver.");
                ImGui.Text($"This excludes the expert solver crafts due to their nature of how they function");
                ImGui.EndTooltip();
            }
            if (force_Raphael)
            {
                if (ImGui.Checkbox("Use Raphael Solver on Expert Recipe", ref expertRaphael))
                {
                    C.Artisan_RaphaelMaster = expertRaphael;
                    C.Save();
                }
                ImGui.SameLine();
                ImGui_Ice.IconWithTooltip(FontAwesomeIcon.QuestionCircle);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text($"Will force crafts that would normally use the Expert Solver to instead use Raphael.");
                    ImGuiEx.Icon(new Vector4(1.0f, 0.4f, 0.0f, 1.0f), FontAwesomeIcon.Diamond);
                    ImGui.SameLine();
                    ImGui.Text($"This is the icon within the recipe details btw");
                    ImGui.Text($"I would not recommend this on Oizys, it's not perfect and has been causing a lot of issues for peeps.");
                    ImGui.EndTooltip();
                }
            }

            ImGui.TextDisabled("More Coming Soon. . . ");
        }

        private static void Separator()
        {
            ImGui.Dummy(new Vector2(0, 5));
            ImGui.Separator();
            ImGui.Dummy(new Vector2(0, 5));
        }
    }
}
