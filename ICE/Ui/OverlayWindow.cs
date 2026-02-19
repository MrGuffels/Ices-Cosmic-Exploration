using Dalamud.Game.Text;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using ICE.Utilities.ImGuiTools;
using Lumina.Excel.Sheets;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TerraFX.Interop.Windows;
using static ICE.Utilities.CosmicHelper;

namespace ICE.Ui
{
    internal class OverlayWindow : Window
    {
        public OverlayWindow() : base("ICE Overlay")
        {
            Flags = ImGuiWindowFlags.None;
     
            P.windowSystem.AddWindow(this);
        }

        public void Dispose()
        {
            P.windowSystem.RemoveWindow(this);
        }

        public override bool DrawConditions()
        {
            return C.ShowOverlay
                && (PlayerHelper.IsInCosmicZone());
        }

        public override void PreDraw()
        {
            Flags = C.Overlay_AutoResize
                ? ImGuiWindowFlags.AlwaysAutoResize
                : ImGuiWindowFlags.None;

            if (C.Overlay_AutoResize)
                ImGui.SetNextWindowSizeConstraints(Vector2.Zero, new Vector2(float.MaxValue, float.MaxValue));
        }

        public override void Draw()
        {
            MissionDetails();

            if (ImGui.BeginTable("Weather/Time Info", 3, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("");
                ImGui.TableSetupColumn("Current");
                ImGui.TableSetupColumn("Next");

                ImGui.TableHeadersRow();

                WeatherForcastForTerritory(1291, "ICE.Resources.Phaenna.png");
                TimedMissionDetailsForTerritory(1291, "ICE.Resources.Phaenna.png");
                WeatherForcastForTerritory(1310, "ICE.Resources.Oizys.png");
                TimedMissionDetailsForTerritory(1310, "ICE.Resources.Oizys.png");

                ImGui.EndTable();
            }

            HomeButtons();

            ClassExpDetails();

            ClassRelicDetails();
        }

        private void MissionDetails()
        {
            ImGui.Text($"Current state: " + SchedulerMain.State.ToString());
            if (CosmicHelper.SheetMissionDict.TryGetValue(CosmicHelper.CurrentLunarMission, out var missionName) && SchedulerMain.State != IceState.AbandonMission)
            {
                ImGui.TextWrapped($"Current Mission: [{CosmicHelper.CurrentLunarMission}] {missionName.Name}");
            }
            else
            {
                ImGui.Text("Current Mission: None");
            }
#if DEBUG
            if (C.ShowDebugGatherInfo)
            {
                ImGui.Text($"Total Node: {Mission_Settings.nodeTotal}");
                ImGui.Text($"Node Counter: {Mission_Settings.nodeCounter}");
            }
#endif
        }
        private void WeatherForcastForTerritory(ushort territoryId, string moonAsset)
        {
            var weatherForecasts = WeatherForecastHandler.GetTerritoryForecast(territoryId);

            if (weatherForecasts.Count == 0) return;

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            DrawMoonAndIcon(moonAsset, FontAwesomeIcon.Cloud);

            // Current weather column
            ImGui.TableNextColumn();
            if (weatherForecasts.Count > 0)
            {
                var current = weatherForecasts[0];
                if (Svc.Texture.TryGetFromGameIcon(current.IconId, out var weatherIcon))
                {
                    ImGui.Image(weatherIcon.GetWrapOrEmpty().Handle, new Vector2(23, 23));

                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text($"{current.Name}");
                        ImGui.EndTooltip();
                    }
                }
            }

            // Next weather column - show next 4 weathers
            ImGui.TableNextColumn();
            for (int i = 1; i < Math.Min(6, weatherForecasts.Count); i++)
            {
                if (i > 1)
                    ImGui.SameLine(0, 2);

                var weather = weatherForecasts[i];
                if (Svc.Texture.TryGetFromGameIcon(weather.IconId, out var weatherIcon))
                {
                    ImGui.Image(weatherIcon.GetWrapOrEmpty().Handle, new Vector2(23, 23));

                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text($"{weather.Name}");
                        ImGui.Text($"In: {WeatherForecastHandler.FormatForecastTime(weather.Time)}");
                        ImGui.EndTooltip();
                    }
                }
            }
        }
        private unsafe void TimedMissionDetailsForTerritory(uint territoryId, string moonAsset)
        {
            var eorzeaTime = DateTimeOffset.FromUnixTimeSeconds(Framework.Instance()->ClientTime.EorzeaTime);
            int currentHour = eorzeaTime.Hour;
            int nextHour = (currentHour + 1) % 24;

            var jobFilter = C.Overlay_FilterByJob ? (uint?)Player.Job : null;

            var currentHourMissions = SheetMissionDict
                .Where(kvp => IsAvailableAtHour(kvp.Value, currentHour))
                .Where(kvp => kvp.Value.TerritoryId == territoryId)
                .Where(kvp => jobFilter == null || kvp.Value.Jobs.Contains(jobFilter.Value))
                .OrderBy(kvp => kvp.Value.Jobs.FirstOrDefault())
                .ToList();

            var nextHourMissions = SheetMissionDict
                .Where(kvp => IsAvailableAtHour(kvp.Value, nextHour))
                .Where(kvp => kvp.Value.TerritoryId == territoryId)
                .Where(kvp => jobFilter == null || kvp.Value.Jobs.Contains(jobFilter.Value))
                .OrderBy(kvp => kvp.Value.Jobs.FirstOrDefault())
                .ToList();

            ImGui.TableNextRow();
            ImGui.TableSetColumnIndex(0);
            DrawMoonAndIcon(moonAsset, FontAwesomeIcon.Clock);

            ImGui.TableNextColumn();
            for (int i = 0; i < currentHourMissions.Count; i++)
            {
                if (i > 0)
                    ImGui.SameLine(0, 2);

                var mission = currentHourMissions[i];
                if (CosmicHelper.JobIconDict.TryGetValue(mission.Value.Jobs[0], out var jobIcon))
                {
                    var imageSize = new Vector2(23, 23);
                    ImGui.Image(jobIcon.GetWrapOrEmpty().Handle, imageSize);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text($"[{mission.Key}]");
                        ImGui.SameLine(0, 2);
                        ImGui.Text($"{mission.Value.Name}");
                        ImGui.EndTooltip();
                    }
                }
            }

            ImGui.TableNextColumn();
            for (int i = 0; i < nextHourMissions.Count; i++)
            {
                if (i > 0)
                    ImGui.SameLine(0, 2);

                var mission = nextHourMissions[i];
                if (CosmicHelper.JobIconDict.TryGetValue(mission.Value.Jobs[0], out var jobIcon))
                {
                    var imageSize = new Vector2(23, 23);
                    ImGui.Image(jobIcon.GetWrapOrEmpty().Handle, imageSize);
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text($"[{mission.Key}]");
                        ImGui.SameLine(0, 2);
                        ImGui.Text($"{mission.Value.Name}");
                        ImGui.EndTooltip();
                    }
                }
            }
        }
        private static readonly Dictionary<string, string> MoonNames = new()
        {
            ["ICE.Resources.Phaenna.png"] = "Phaenna",
            ["ICE.Resources.Oizys.png"] = "Oizys",
        };
        private void DrawMoonAndIcon(string moonAsset, FontAwesomeIcon icon)
        {
            var moonTexture = Svc.Texture.GetFromManifestResource(Assembly.GetExecutingAssembly(), moonAsset).GetWrapOrEmpty();
            ImGui.Image(moonTexture.Handle, new Vector2(23, 23));
            if (ImGui.IsItemHovered() && MoonNames.TryGetValue(moonAsset, out var moonName))
            {
                ImGui.BeginTooltip();
                ImGui.Text(moonName);
                ImGui.EndTooltip();
            }
            ImGui.SameLine(0, 4);
            ImGui.AlignTextToFramePadding();
            ImGui.PushFont(UiBuilder.IconFont);
            ImGui.Text(icon.ToIconString());
            ImGui.PopFont();
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text(icon == FontAwesomeIcon.Cloud ? "Weather" : "Timed");
                ImGui.EndTooltip();
            }
        }
        private bool IsAvailableAtHour(CosmicInfo mission, int hour)
        {
            // If StartTime <= EndTime: normal range (e.g., 8-16)
            if (mission.StartTime <= mission.EndTime)
            {
                return hour >= mission.StartTime && hour < mission.EndTime;
            }
            // If StartTime > EndTime: crosses midnight (e.g., 20-4)
            else
            {
                return hour >= mission.StartTime || hour < mission.EndTime;
            }
        }
        private void HomeButtons()
        {
            if (ImGuiEx.IconButton(FontAwesomeIcon.Home, "Open ICE"))
            {
                P.mainWindow.IsOpen = true;
            }
            ImGui.SameLine();

            // Start button (disabled while already ticking).
            bool xpLeveling = C.SelectedMode == ModeSelect.LevelMode;
            bool unsupportedArtisan = xpLeveling && !P.Artisan.UpdatedArtisan() && CosmicHelper.CrafterJobList.Contains((uint)Player.Job);
            bool unsupportedClass = !PlayerHelper.UsingSupportedJob();

            bool unsupported = SchedulerMain.State != IceState.Idle || !PlayerHelper.UsingSupportedJob() || unsupportedArtisan;

            using (ImRaii.Disabled(unsupported))
            {
                var defaultButtonColor = ImGui.GetStyle().Colors[(int)ImGuiCol.Button];
                var color = unsupported ? EColor.Red : defaultButtonColor;

                using var tempButton = ImRaii.PushColor(ImGuiCol.Button, color);
                if (ImGui.Button("Start"))
                {
                    SchedulerMain.EnablePlugin();
                }
            }

            ImGui.SameLine();

            // Stop button (disabled while not ticking).
            using (ImRaii.Disabled(SchedulerMain.State == IceState.Idle))
            {
                if (ImGui.Button("Stop"))
                {
                    SchedulerMain.DisablePlugin();
                }
            }
            ImGui.SameLine();
            ImGui.Checkbox("Stop after current mission", ref Mission_Settings.StopAfterCurrent);

            // Drone finder toggle
            ImGui.SameLine();
            bool droneActive = SchedulerMain.State == IceState.ArtifactSearch;
            if (droneActive)
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0.6f, 0.2f, 1.0f));
            if (ImGuiEx.IconButton(FontAwesomeIcon.SearchLocation, "##DroneFinder"))
            {
                if (droneActive)
                    SchedulerMain.DisablePlugin();
                else
                    SchedulerMain.State = IceState.ArtifactSearch;
            }
            if (droneActive)
                ImGui.PopStyleColor();
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text(droneActive ? "Stop Drone Finder" : "Run Drone Finder");
                ImGui.EndTooltip();
            }
        }
        private void ClassExpDetails()
        {
            var ScoreInfo = CosmicHelper.Cosmic_ClassInfo();

            if (C.ShowCurrentScore)
            {
                if (CosmicHelper.SheetMissionDict.TryGetValue(CosmicHelper.CurrentLunarMission, out var sheetInfo))
                {
                    foreach (var job in sheetInfo.Jobs)
                    {
                        if (ScoreInfo.TryGetValue(job, out var classInfo))
                        {
                            if (CosmicHelper.JobIconDict.TryGetValue(job, out var icon))
                            {
                                ImGui.Image(icon.GetWrapOrEmpty().Handle, new(25, 25));
                                ImGui.SameLine();
                                ImGui.AlignTextToFramePadding();
                            }

                            ImGui_Ice.Draw_XPBar(classInfo.Score, 0, 500_000, $"{classInfo.Score:N0} / {500_000:N0}");
                        }
                    }
                }
                else
                {
                    var job = (uint)Player.Job;
                    if (ScoreInfo.TryGetValue(job, out var classInfo))
                    {
                        if (CosmicHelper.JobIconDict.TryGetValue(job, out var icon))
                        {
                            ImGui.Image(icon.GetWrapOrEmpty().Handle, new(25, 25));
                            ImGui.SameLine();
                            ImGui.AlignTextToFramePadding();
                        }

                        ImGui_Ice.Draw_XPBar(classInfo.Score, 0, 500_000, $"{classInfo.Score:N0} / {500_000:N0}");
                    }
                }
            }

            if (C.ShowTotalScore)
            {
                int maxScore = 5_500_000;
                int currentTotal = 0;
                int actualTotal = 0;
                int totalCompleted = 0;

                foreach (var job in ScoreInfo)
                {
                    currentTotal += Math.Min(job.Value.Score, 500_000);
                    if (job.Value.Score >= 500_000)
                        totalCompleted += 1;
                    actualTotal += job.Value.Score;
                }

                if (totalCompleted != 11)
                {
                    ImGui_Ice.Draw_XPBar(currentTotal, 0, maxScore, label: $"Total: {currentTotal:N0} / {maxScore:N0} [{totalCompleted} / 11]");
                }
                else
                {
                    ImGui_Ice.Draw_XPBar(actualTotal, 0, maxScore, label: $"Total Score: {actualTotal:N0}");
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();

                    foreach (var job in ScoreInfo)
                    {
                        var jobIdInfo = job.Key;
                        var jobScore = job.Value.Score;
                        var jobImage = CosmicHelper.JobIconDict[jobIdInfo];
                        ImGui.Image(jobImage.GetWrapOrEmpty().Handle, new Vector2(23, 23));
                        ImGui.SameLine();
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text($"Score: {jobScore:N0}");
                    }

                    ImGui.EndTooltip();
                }
            }
        }
        private void ClassRelicDetails()
        {
            if (C.ShowExpBars)
            {
                var currentJobId = (uint)Player.Job;
                if (ImGui.CollapsingHeader("Relic Tool XP"))
                {
                    ImGui_Ice.Draw_ExpTable(currentJobId);
                }
            }
        }
    }
}