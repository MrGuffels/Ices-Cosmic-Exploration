using Dalamud.Interface.Utility.Raii;
using ICE.Utilities.Cosmic_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICE.Utilities.Cosmic_Helper.IceLogging;

namespace ICE.Ui.MainUi.HelpFolder
{
    internal class helpSelect_Logs
    {
        private static string searchFilter = string.Empty;

        public static void Draw_Helper()
        {
            using (var headerChild = ImRaii.Child("##helpSelect_Logs", new Vector2(0, 0), true, ImGuiWindowFlags.NoScrollbar))
            {
                if (!headerChild.Success) return; // Ensures that it was loaded properly before continuing.
                LogHelperViewer();
            }
        }

        public static void Draw_Debug()
        {
            if (ImGui.Button("Copy logs to clipboard"))
            {
                LogSystem.CopyToClipboard();
            }
            LogHelperViewer();
        }

        private static void LogHelperViewer()
        {
            // Search input
            ImGui.SetNextItemWidth(300);
            ImGui.InputTextWithHint("##LogSearch", "Search logs...", ref searchFilter, 256);

            ImGui.SameLine();
            if (ImGui.Button("Clear"))
            {
                searchFilter = string.Empty;
            }

            ImGui.Spacing();

            ImGuiTableFlags flags = ImGuiTableFlags.RowBg |
            ImGuiTableFlags.Borders |
            ImGuiTableFlags.ScrollY |
            ImGuiTableFlags.SizingFixedFit;

            if (ImGui.BeginTable("LogTable", 4, flags))
            {
                ImGui.TableSetupColumn("Time");
                ImGui.TableSetupColumn("Level");
                ImGui.TableSetupColumn("Category");
                ImGui.TableSetupColumn("Message");
                ImGui.TableHeadersRow();

                // Filter logs based on search input
                var filteredLogs = LogSystem.Logs.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(searchFilter))
                {
                    filteredLogs = filteredLogs.Where(log =>
                        log.Message.Contains(searchFilter, StringComparison.OrdinalIgnoreCase) ||
                        (log.Category?.Contains(searchFilter, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        log.Level.ToString().Contains(searchFilter, StringComparison.OrdinalIgnoreCase)
                    );
                }

                foreach (var log in filteredLogs.OrderByDescending(l => l.Timestamp))
                {
                    ImGui.TableNextRow();

                    ImGui.TableNextColumn();
                    ImGui.Text(log.Timestamp.ToString("HH:mm:ss"));

                    ImGui.TableNextColumn();
                    // Color-code by level
                    var color = log.Level switch
                    {
                        LogLevel.Error => new Vector4(1, 0, 0, 1),
                        LogLevel.Warning => new Vector4(1, 1, 0, 1),
                        LogLevel.Info => new Vector4(0, 1, 1, 1),
                        _ => new Vector4(0.7f, 0.7f, 0.7f, 1)
                    };
                    ImGui.TextColored(color, log.Level.ToString());

                    ImGui.TableNextColumn();
                    ImGui.Text(log.Category ?? "");

                    ImGui.TableNextColumn();
                    ImGui.Text(log.Message);
                }

                ImGui.EndTable();
            }
        }
    }
}