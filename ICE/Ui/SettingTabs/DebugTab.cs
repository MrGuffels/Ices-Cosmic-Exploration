using Dalamud.Interface.Utility.Raii;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.SettingTabs
{
    internal class DebugTab
    {
        public static void Draw()
        {
            ImGui.Checkbox("Force OOM Main", ref SchedulerMain.DebugOOMMain);
            ImGui.Checkbox("Force OOM Sub", ref SchedulerMain.DebugOOMSub);
            ImGui.Checkbox("Legacy Failsafe WKSRecipe Select", ref OldConfig.FailsafeRecipeSelect);

            var missionMap = new List<(string name, Func<byte> get, Action<byte> set)>
                {
                    ("Sequence Missions", new Func<byte>(() => OldConfig.SequenceMissionPriority), new Action<byte>(v => { OldConfig.SequenceMissionPriority = v; OldConfig.Save(); })),
                    ("Timed Missions", new Func<byte>(() => OldConfig.TimedMissionPriority), new Action<byte>(v => { OldConfig.TimedMissionPriority = v; OldConfig.Save(); })),
                    ("Weather Missions", new Func<byte>(() => OldConfig.WeatherMissionPriority), new Action<byte>(v => { OldConfig.WeatherMissionPriority = v; OldConfig.Save(); }))
                };

            var sorted = missionMap
                .Select((m, i) => new { Index = i, Name = m.name, Priority = m.get() })
                .OrderBy(m => m.Priority)
                .ToList();
            /*
            ImGuiHelpers.ScaledDummy(5, 0);
            ImGui.SameLine();
            if (ImGui.CollapsingHeader("Provision Mission Priority"))
            {
                for (int i = 0; i < sorted.Count; i++)
                {
                    var item = sorted[i];
                    ImGuiHelpers.ScaledDummy(5, 0);
                    ImGui.SameLine();
                    ImGui.Selectable(item.Name);
                    if (ImGui.IsItemActive() && !ImGui.IsItemHovered())
                    {
                        int nextIndex = i + (ImGui.GetMouseDragDelta(0).Y < 0f ? -1 : 1);
                        if (nextIndex >= 0 && nextIndex < sorted.Count)
                        {
                            // Swap the priority values
                            var otherItem = sorted[nextIndex];

                            // Swap their priority values via the original setters
                            byte temp = missionMap[item.Index].get();
                            missionMap[item.Index].set(missionMap[otherItem.Index].get());
                            missionMap[otherItem.Index].set(temp);
                            ImGui.ResetMouseDragDelta();
                        }
                    }
                }
            }
            */

            if (ImGui.Button("Get Sinus Forecast"))
            {
                List<WeatherForecast> forecast = WeatherForecastHandler.GetTerritoryForecast(1237);
                Func<WeatherForecast, string> formatTime = (forecast) => WeatherForecastHandler.FormatForecastTime(forecast.Time);

                Svc.Chat.Print(new Dalamud.Game.Text.XivChatEntry()
                {
                    Message = $"Sinus Ardorum Weather - {forecast[0].Name}",
                    Type = Dalamud.Game.Text.XivChatType.Echo,
                });
                for (int i = 1; i < forecast.Count; i++)
                {
                    Svc.Chat.Print(new Dalamud.Game.Text.XivChatEntry()
                    {
                        Message = $"{forecast[i].Name} In {formatTime(forecast[i])}",
                        Type = Dalamud.Game.Text.XivChatType.Echo,
                    });
                }
            }

            using (ImRaii.Disabled(!PlayerHelper.IsInCosmicZone()))
            {
                if (ImGui.Button("Refresh Forecast"))
                {
                    WeatherForecastHandler.GetForecast();
                }
            }
        }
    }
}
