using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.Sheets;
using Pictomancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.SettingTabs
{
    internal class MiscTab
    {
        // Overlay Settings
        private static bool showOverlay = C.ShowOverlay;
        private static bool ShowSeconds = C.ShowSeconds;
        private static Dictionary<uint, string> availableMounts = new();

        private static string mountSearchText = "";
        private static int mountDisplayOffset = 0;
        private static int mountItemsPerPage = 10;

        private static bool AutoMoonSprint = C.MoonSprint;

        // Mission Priority Settings

        public static void Draw()
        {
            bool showInfoButton = C.ShowInfoButton;
            if (ImGui.Checkbox("Show Info Button", ref showInfoButton))
            {
                C.ShowInfoButton = showInfoButton;
                C.Save();
            }

            ImGui.Dummy(new Vector2(0, 5));
            ImGui.Separator();
            ImGui.Dummy(new Vector2(0, 5));

            ImGui.Text("Overlay Settings");

            if (ImGui.Checkbox("Show Overlay", ref showOverlay))
            {
                C.ShowOverlay = showOverlay;
                C.Save();
            }

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

            ImGui.Dummy(new(0, 2));

            ImGui.Separator();

            ImGui.Dummy(new (0, 2));

            if (ImGui.Checkbox("Auto-Use Moon Sprint", ref AutoMoonSprint))
            {
                C.MoonSprint = AutoMoonSprint;
                C.Save();
            }

            ImGui.Dummy(new(0, 2));

            ImGui.Separator();

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
                    C.Save();
                }
            }

            ImGui.Separator();
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

            ImGui.Separator();

            MountSelection();
        }

        private static bool visualizeRadius = false;
        private static bool visualizeDismountRadius = false;

        private static unsafe void MountSelection()
        {
            bool mountOutsideMission = C.UseMountOutsideMission;
            bool mountInMission = C.UseMountInMission;
            float minMountRange = C.MountRadius;
            float dismountRange = C.DismountRadius;

            if (ImGui.Button("Select Mounting Option"))
            {
                availableMounts.Clear();
                availableMounts[0] = "Mount Roulette";

                var mountSheet = Svc.Data.GetExcelSheet<Mount>();

                foreach (var mountItem in mountSheet)
                {
                    //Checking to see if the current mount is unlocked
                    if (!PlayerState.Instance()->IsMountUnlocked(mountItem.RowId)) continue;

                    string mountName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mountItem.Singular.ToString().ToLower());
                    uint id = mountItem.RowId;

                   availableMounts[id] = mountName;
                }

                mountSearchText = "";
                mountDisplayOffset = 0;

                ImGui.OpenPopup("Mount Options");
            }
            ImGui.SameLine();
            ImGui.AlignTextToFramePadding();
            ImGui.Text($"Mount: {C.MountName}");

            if (ImGui.BeginPopup("Mount Options"))
            {
                // Search box
                ImGui.InputText("Search", ref mountSearchText, 100);

                // Filter mounts based on search
                var filteredMounts = availableMounts
                    .Where(kvp => string.IsNullOrEmpty(mountSearchText) ||
                                  kvp.Value.Contains(mountSearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                // Calculate page count here, just to peeps know how many pages there are
                int totalItems = filteredMounts.Count;
                int maxOffset = Math.Max(0, totalItems - mountItemsPerPage);
                mountDisplayOffset = Math.Min(mountDisplayOffset, maxOffset);

                // Display current page of mounts
                var displayMounts = filteredMounts
                    .Skip(mountDisplayOffset)
                    .Take(mountItemsPerPage);

                foreach (var mount in displayMounts)
                {
                    if (ImGui.Selectable($"{mount.Value}##{mount.Key}"))
                    {
                        C.MountId = mount.Key;
                        C.MountName = mount.Value;
                        C.Save();
                        ImGui.CloseCurrentPopup();
                    }
                }

                // Navigation buttons
                ImGui.Separator();

                if (ImGui.Button("Previous") && mountDisplayOffset > 0)
                {
                    mountDisplayOffset = Math.Max(0, mountDisplayOffset - mountItemsPerPage);
                }

                ImGui.SameLine();
                ImGui.Text($"{mountDisplayOffset + 1}-{Math.Min(mountDisplayOffset + mountItemsPerPage, totalItems)} of {totalItems}");

                ImGui.SameLine();
                if (ImGui.Button("Next") && mountDisplayOffset < maxOffset)
                {
                    mountDisplayOffset = Math.Min(maxOffset, mountDisplayOffset + mountItemsPerPage);
                }

                ImGui.EndPopup();
            }

            if (ImGui.Checkbox("Use mount outside mission", ref mountOutsideMission))
            {
                C.UseMountOutsideMission = mountOutsideMission;
                C.Save();
            }

            if (ImGui.Checkbox("Use mount in mission", ref mountInMission))
            {
                C.UseMountInMission = mountInMission;
                C.Save();
            }

            ImGui.SetNextItemWidth(100);
            if (ImGui.DragFloat("Minimum Mounting Range", ref minMountRange, 1))
            {
                C.MountRadius = minMountRange;
                C.Save();
            }
            ImGui.SameLine();
            ImGui.Checkbox("Visualize radius", ref visualizeRadius);
            ImGui.SetNextItemWidth(100);
            if (ImGui.DragFloat("Dismount Target Range", ref dismountRange, 1))
            {
                C.DismountRadius = dismountRange;
                C.Save();
            }
            ImGui.SameLine();
            ImGui.Checkbox("Visualize Dismount Radius", ref visualizeDismountRadius);

            using (var drawList = PictoService.Draw())
            {
                if (drawList == null)
                    return;

                var playerPos = Player.Position;

                if (visualizeRadius)
                    PictoService.VfxRenderer.AddCircle("Mount_Radius Circle", playerPos, C.MountRadius, Utils.FromUintABGR(2616716297));
                if (visualizeDismountRadius)
                    PictoService.VfxRenderer.AddCircle("Dismount_Radius Circle", playerPos, C.DismountRadius, Utils.FromUintABGR(2601121571));
            }
        }
    }
}
