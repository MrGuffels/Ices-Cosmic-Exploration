using Dalamud.Interface.Utility.Raii;
using ECommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Ui_FishingMissionEditor
    {
        private static uint SelectedMission = 0;
        private static string newPresetItem = "";
        private static uint newBaitId = 0;
        private static string newFishCategory = "";
        private static uint newFishId = 0;
        private static string selectedFishCategory = "";
        private static string newBaitName = "";
        private static uint newBaitIdForName = 0;
        private static string selectedBaitFromDict = "";
        private static string selectedFishFromDict = "";

        public static void Draw()
        {
            if (ImGui.Button("Import missing missions"))
            {
                foreach (var mission in CosmicHelper.SheetMissionDict.Where(x => x.Value.Jobs.Contains(18)))
                {
                    if (!GatheringUtil.FishingPreset.ContainsKey(mission.Key))
                    {
                        GatheringUtil.FishingPreset[mission.Key] = new()
                        {
                            FishingPreset = new(),
                            Baits = new(),
                            RequiredFish = new(),
                        };
                    }
                }
            }

            ImGui.SameLine();
            if (ImGui.Button("Export All Fishing Presets"))
            {
                var exportData = ExportAllFishingPresets();
                ImGui.SetClipboardText(exportData);
                Svc.Chat.Print("All fishing presets exported to clipboard!");
            }

            ImGui.SameLine();
            using (ImRaii.Disabled(SelectedMission == 0))
            {
                if (ImGui.Button("Export Selected Mission"))
                {
                    var exportData = ExportSingleFishingPreset(SelectedMission);
                    ImGui.SetClipboardText(exportData);
                    Svc.Chat.Print($"Fishing preset for Mission {SelectedMission} exported to clipboard!");
                }
            }

            ImGui.Separator();

            if (ImGui.BeginTable("Fishing Mission Editor", 2, ImGuiTableFlags.Resizable | ImGuiTableFlags.SizingFixedFit))
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                if (ImGui.BeginChild("Mission Selection _ Fishing Edition", new Vector2(400, 0), true))
                {
                    foreach (var fishMission in GatheringUtil.FishingPreset.OrderBy(x => x.Key))
                    {
                        var id = fishMission.Key;

                        if (CosmicHelper.SheetMissionDict.TryGetValue(id, out var mission))
                        {
                            ImGui.PushID($"{id}_{mission.Name}");

                            bool isSelected = SelectedMission == id;
                            string label = isSelected ? $"→ [{id}] - {mission.Name}" : $"[{id}] - {mission.Name}";

                            if (ImGui.Selectable(label, isSelected))
                            {
                                SelectedMission = id;
                                selectedFishCategory = "";
                            }

                            // Right-click context menu for deletion
                            if (ImGui.IsMouseClicked(ImGuiMouseButton.Right) && ImGui.IsItemHovered())
                            {
                                ImGui.OpenPopup("Delete Mission");
                            }
                            if (ImGui.BeginPopup("Delete Mission"))
                            {
                                if (ImGui.MenuItem("Delete Mission Preset"))
                                {
                                    GatheringUtil.FishingPreset.Remove(id);
                                    if (SelectedMission == id) SelectedMission = 0;
                                }
                                ImGui.EndPopup();
                            }

                            ImGui.PopID();
                        }
                    }
                }
                ImGui.EndChild();

                ImGui.TableSetColumnIndex(1);
                if (ImGui.BeginChild("Mission Info _ Fishing Edition", new Vector2(0, 0), true))
                {
                    if (GatheringUtil.FishingPreset.TryGetValue(SelectedMission, out var missionData) &&
                        CosmicHelper.SheetMissionDict.TryGetValue(SelectedMission, out var mission))
                    {
                        ImGui.Text($"Editing Mission: [{SelectedMission}] {mission.Name}");
                        ImGui.Separator();

                        // Fishing Preset Section
                        ImGui.Text("Fishing Preset Items:");
                        ImGui.Indent();

                        // Add new preset item
                        ImGui.SetNextItemWidth(200);
                        ImGui.InputText("##NewPresetItem", ref newPresetItem, 100);
                        ImGui.SameLine();
                        if (ImGui.Button("Add Preset Item") && !string.IsNullOrEmpty(newPresetItem))
                        {
                            missionData.FishingPreset.Add(newPresetItem);
                            newPresetItem = "";
                        }

                        // List existing preset items
                        for (int i = 0; i < missionData.FishingPreset.Count; i++)
                        {
                            ImGui.PushID($"preset_{i}");

                            var item = missionData.FishingPreset[i];
                            if (ImGui.InputText($"##preset_{i}", ref item, 100))
                            {
                                missionData.FishingPreset[i] = item;
                            }
                            ImGui.SameLine();
                            if (ImGui.Button("Remove"))
                            {
                                missionData.FishingPreset.RemoveAt(i);
                                i--; // Adjust index after removal
                            }

                            ImGui.PopID();
                        }

                        ImGui.Unindent();
                        ImGui.Separator();

                        // Baits Section
                        ImGui.Text("Baits by Name:");
                        ImGui.Indent();

                        // Add bait from dictionary
                        ImGui.Text("Add from MoonBaits Dictionary:");
                        ImGui.SetNextItemWidth(250);
                        if (ImGui.BeginCombo("##SelectBaitFromDict", selectedBaitFromDict))
                        {
                            foreach (var baitEntry in GatheringUtil.MoonBaits.OrderBy(x => x.Key))
                            {
                                bool isSelected = selectedBaitFromDict == baitEntry.Key;
                                if (ImGui.Selectable($"{baitEntry.Key} ({baitEntry.Value.Count} IDs)", isSelected))
                                {
                                    selectedBaitFromDict = baitEntry.Key;
                                }
                            }
                            ImGui.EndCombo();
                        }
                        ImGui.SameLine();
                        if (ImGui.Button("Add Selected Bait") && !string.IsNullOrEmpty(selectedBaitFromDict))
                        {
                            if (GatheringUtil.MoonBaits.TryGetValue(selectedBaitFromDict, out var baitIds))
                            {
                                if (!missionData.Baits.ContainsKey(selectedBaitFromDict))
                                {
                                    missionData.Baits[selectedBaitFromDict] = new List<uint>();
                                }

                                // Add all IDs from the dictionary entry
                                foreach (var baitId in baitIds)
                                {
                                    if (!missionData.Baits[selectedBaitFromDict].Contains(baitId))
                                    {
                                        missionData.Baits[selectedBaitFromDict].Add(baitId);
                                    }
                                }
                                selectedBaitFromDict = "";
                            }
                        }

                        ImGui.Separator();
                        ImGui.Text("Add Custom Bait:");

                        // Add new bait manually
                        ImGui.SetNextItemWidth(150);
                        ImGui.InputText("Bait Name##NewBaitName", ref newBaitName, 50);
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(100);
                        ImGui.InputUInt("Bait ID##NewBaitIdForName", ref newBaitIdForName);
                        ImGui.SameLine();
                        if (ImGui.Button("Add Bait") && !string.IsNullOrEmpty(newBaitName) && newBaitIdForName > 0)
                        {
                            if (!missionData.Baits.ContainsKey(newBaitName))
                            {
                                missionData.Baits[newBaitName] = new List<uint>();
                            }
                            missionData.Baits[newBaitName].Add(newBaitIdForName);
                            newBaitIdForName = 0;
                        }

                        // List existing bait categories and IDs
                        var baitCategoriesToRemove = new List<string>();
                        foreach (var baitCategory in missionData.Baits.OrderBy(x => x.Key))
                        {
                            ImGui.PushID($"baitCategory_{baitCategory.Key}");

                            bool isExpanded = ImGui.TreeNode($"{baitCategory.Key} ({baitCategory.Value.Count} IDs)");

                            // Right-click to delete bait category
                            if (ImGui.IsMouseClicked(ImGuiMouseButton.Right) && ImGui.IsItemHovered())
                            {
                                ImGui.OpenPopup("Delete Bait Category");
                            }
                            if (ImGui.BeginPopup("Delete Bait Category"))
                            {
                                if (ImGui.MenuItem($"Delete Bait '{baitCategory.Key}'"))
                                {
                                    baitCategoriesToRemove.Add(baitCategory.Key);
                                }
                                ImGui.EndPopup();
                            }

                            if (isExpanded)
                            {
                                var baitIdsToRemove = new List<uint>();

                                for (int i = 0; i < baitCategory.Value.Count; i++)
                                {
                                    ImGui.PushID($"baitId_{i}");
                                    var baitId = baitCategory.Value[i];

                                    ImGui.Text($"Bait ID: {baitId}");
                                    ImGui.SameLine();
                                    if (ImGui.Button("Remove"))
                                    {
                                        baitIdsToRemove.Add(baitId);
                                    }

                                    ImGui.PopID();
                                }

                                // Remove bait IDs that were marked for removal
                                foreach (var baitId in baitIdsToRemove)
                                {
                                    baitCategory.Value.Remove(baitId);
                                }

                                // Remove empty bait categories
                                if (baitCategory.Value.Count == 0)
                                {
                                    baitCategoriesToRemove.Add(baitCategory.Key);
                                }

                                ImGui.TreePop();
                            }

                            ImGui.PopID();
                        }

                        // Remove bait categories that were marked for removal
                        foreach (var baitCategory in baitCategoriesToRemove)
                        {
                            missionData.Baits.Remove(baitCategory);
                        }

                        ImGui.Unindent();
                        ImGui.Separator();

                        // Required Fish Section
                        ImGui.Text("Required Fish by Category:");
                        ImGui.Indent();

                        // Add fish from dictionary
                        ImGui.Text("Add from MoonFish Dictionary:");
                        ImGui.SetNextItemWidth(150);
                        ImGui.InputText("Category##NewCategoryForDict", ref newFishCategory, 50);
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(250);
                        if (ImGui.BeginCombo("##SelectFishFromDict", selectedFishFromDict))
                        {
                            foreach (var fishEntry in GatheringUtil.MoonFish.OrderBy(x => x.Key))
                            {
                                bool isSelected = selectedFishFromDict == fishEntry.Key;
                                if (ImGui.Selectable($"{fishEntry.Key} ({fishEntry.Value.Count} IDs)", isSelected))
                                {
                                    selectedFishFromDict = fishEntry.Key;
                                }
                            }
                            ImGui.EndCombo();
                        }
                        ImGui.SameLine();
                        if (ImGui.Button("Add Selected Fish") && !string.IsNullOrEmpty(newFishCategory) && !string.IsNullOrEmpty(selectedFishFromDict))
                        {
                            if (GatheringUtil.MoonFish.TryGetValue(selectedFishFromDict, out var fishIds))
                            {
                                if (!missionData.RequiredFish.ContainsKey(newFishCategory))
                                {
                                    missionData.RequiredFish[newFishCategory] = new List<uint>();
                                }

                                // Add all IDs from the dictionary entry
                                foreach (var fishId in fishIds)
                                {
                                    if (!missionData.RequiredFish[newFishCategory].Contains(fishId))
                                    {
                                        missionData.RequiredFish[newFishCategory].Add(fishId);
                                    }
                                }
                                selectedFishFromDict = "";
                            }
                        }

                        ImGui.Separator();
                        ImGui.Text("Add Custom Fish:");

                        // Add new fish category manually
                        ImGui.SetNextItemWidth(150);
                        ImGui.InputText("Category##NewCategory", ref newFishCategory, 50);
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(100);
                        ImGui.InputUInt("Fish ID##NewFishId", ref newFishId);
                        ImGui.SameLine();
                        if (ImGui.Button("Add Fish") && !string.IsNullOrEmpty(newFishCategory) && newFishId > 0)
                        {
                            if (!missionData.RequiredFish.ContainsKey(newFishCategory))
                            {
                                missionData.RequiredFish[newFishCategory] = new List<uint>();
                            }
                            missionData.RequiredFish[newFishCategory].Add(newFishId);
                            newFishId = 0;
                        }

                        // List existing fish categories and fish
                        var categoriesToRemove = new List<string>();
                        foreach (var category in missionData.RequiredFish.OrderBy(x => x.Key))
                        {
                            ImGui.PushID($"category_{category.Key}");

                            bool isExpanded = ImGui.TreeNode($"{category.Key} ({category.Value.Count} fish)");

                            // Right-click to delete category
                            if (ImGui.IsMouseClicked(ImGuiMouseButton.Right) && ImGui.IsItemHovered())
                            {
                                ImGui.OpenPopup("Delete Category");
                            }
                            if (ImGui.BeginPopup("Delete Category"))
                            {
                                if (ImGui.MenuItem($"Delete Category '{category.Key}'"))
                                {
                                    categoriesToRemove.Add(category.Key);
                                }
                                ImGui.EndPopup();
                            }

                            if (isExpanded)
                            {
                                var fishToRemove = new List<uint>();

                                for (int i = 0; i < category.Value.Count; i++)
                                {
                                    ImGui.PushID($"fish_{i}");
                                    var fishId = category.Value[i];

                                    ImGui.Text($"Fish ID: {fishId}");
                                    ImGui.SameLine();
                                    if (ImGui.Button("Remove"))
                                    {
                                        fishToRemove.Add(fishId);
                                    }

                                    ImGui.PopID();
                                }

                                // Remove fish that were marked for removal
                                foreach (var fish in fishToRemove)
                                {
                                    category.Value.Remove(fish);
                                }

                                // Remove empty categories
                                if (category.Value.Count == 0)
                                {
                                    categoriesToRemove.Add(category.Key);
                                }

                                ImGui.TreePop();
                            }

                            ImGui.PopID();
                        }

                        // Remove categories that were marked for removal
                        foreach (var category in categoriesToRemove)
                        {
                            missionData.RequiredFish.Remove(category);
                        }

                        ImGui.Unindent();
                    }
                    else if (SelectedMission != 0)
                    {
                        ImGui.Text("Mission data not found or mission not in sheet dictionary.");
                    }
                    else
                    {
                        ImGui.Text("Select a mission to edit its fishing preset.");
                    }
                }
                ImGui.EndChild();

                ImGui.EndTable();
            }
        }

        private static string ExportAllFishingPresets()
        {
            var sb = new StringBuilder();
            sb.AppendLine("public static Dictionary<uint, FishingTools> FishingPreset = new()");
            sb.AppendLine("{");

            foreach (var preset in GatheringUtil.FishingPreset.OrderBy(x => x.Key))
            {
                sb.AppendLine($"\t[{preset.Key}] = new FishingTools()");
                sb.AppendLine("\t{");

                // Export FishingPreset list
                sb.AppendLine("\t\tFishingPreset = new List<string>()");
                sb.AppendLine("\t\t{");
                foreach (var item in preset.Value.FishingPreset)
                {
                    sb.AppendLine($"\t\t\t\"{item}\",");
                }
                sb.AppendLine("\t\t},");

                // Export Baits
                sb.AppendLine("\t\tBaits = new Dictionary<string, List<uint>>()");
                sb.AppendLine("\t\t{");
                foreach (var baitCategory in preset.Value.Baits.OrderBy(x => x.Key))
                {
                    sb.AppendLine($"\t\t\t[\"{baitCategory.Key}\"] = new List<uint>()");
                    sb.AppendLine("\t\t\t{");
                    foreach (var baitId in baitCategory.Value.OrderBy(x => x))
                    {
                        sb.AppendLine($"\t\t\t\t{baitId},");
                    }
                    sb.AppendLine("\t\t\t},");
                }
                sb.AppendLine("\t\t},");

                // Export RequiredFish
                sb.AppendLine("\t\tRequiredFish = new Dictionary<string, List<uint>>()");
                sb.AppendLine("\t\t{");
                foreach (var category in preset.Value.RequiredFish.OrderBy(x => x.Key))
                {
                    sb.AppendLine($"\t\t\t[\"{category.Key}\"] = new List<uint>()");
                    sb.AppendLine("\t\t\t{");
                    foreach (var fish in category.Value.OrderBy(x => x))
                    {
                        sb.AppendLine($"\t\t\t\t{fish},");
                    }
                    sb.AppendLine("\t\t\t},");
                }
                sb.AppendLine("\t\t},");

                sb.AppendLine("\t},");
            }

            sb.AppendLine("};");
            return sb.ToString();
        }

        private static string ExportSingleFishingPreset(uint missionId)
        {
            if (!GatheringUtil.FishingPreset.TryGetValue(missionId, out var preset))
            {
                return $"// No fishing preset found for mission {missionId}";
            }

            var sb = new StringBuilder();

            // Add mission name if available
            if (CosmicHelper.SheetMissionDict.TryGetValue(missionId, out var mission))
            {
                sb.AppendLine($"// Export for Mission [{missionId}] - {mission.Name}");
            }
            else
            {
                sb.AppendLine($"// Export for Mission [{missionId}]");
            }

            sb.AppendLine($"[{missionId}] = new FishingTools()");
            sb.AppendLine("{");

            // Export FishingPreset list
            sb.AppendLine("\tFishingPreset = new List<string>()");
            sb.AppendLine("\t{");
            foreach (var item in preset.FishingPreset)
            {
                sb.AppendLine($"\t\t\"{item}\",");
            }
            sb.AppendLine("\t},");

            // Export Baits
            sb.AppendLine("\tBaits = new Dictionary<string, List<uint>>()");
            sb.AppendLine("\t{");
            foreach (var baitCategory in preset.Baits.OrderBy(x => x.Key))
            {
                sb.AppendLine($"\t\t[\"{baitCategory.Key}\"] = new List<uint>()");
                sb.AppendLine("\t\t{");
                foreach (var baitId in baitCategory.Value.OrderBy(x => x))
                {
                    sb.AppendLine($"\t\t\t{baitId},");
                }
                sb.AppendLine("\t\t},");
            }
            sb.AppendLine("\t},");

            // Export RequiredFish
            sb.AppendLine("\tRequiredFish = new Dictionary<string, List<uint>>()");
            sb.AppendLine("\t{");
            foreach (var category in preset.RequiredFish.OrderBy(x => x.Key))
            {
                sb.AppendLine($"\t\t[\"{category.Key}\"] = new List<uint>()");
                sb.AppendLine("\t\t{");
                foreach (var fish in category.Value.OrderBy(x => x))
                {
                    sb.AppendLine($"\t\t\t{fish},");
                }
                sb.AppendLine("\t\t},");
            }
            sb.AppendLine("\t},");

            sb.AppendLine("},");
            return sb.ToString();
        }
    }
}