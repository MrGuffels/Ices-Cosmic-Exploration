using Dalamud.Interface.Utility.Raii;
using ICE.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.SettingTabs
{
    internal class GatherSettings
    {
        private static bool SelfRepairGather = C.SelfRepairGather;
        private static float SelfRepairPercent = C.RepairPercent;
        private static bool SelfSpiritbondGather = C.SelfSpiritbondGather;
        private static bool AutoCordial = C.AutoCordial;
        private static bool InverseCordialPrio = C.inverseCordialPrio;
        private static bool UseOnFisher = C.UseOnFisher;
        private static bool PreventOvercap = C.PreventOvercap;
        private static int CordialMinGp = C.CordialMinGp;
        private static bool useOnlyInMission = C.UseOnlyInMission;
        private static string newProfileName = "";

        private static string[] MissionTypes = ["Limited Nodes", "Gather x Amount", "Time Attack", "Chained Scoring", "Boon Scoring", "Chain + Boon Scoring", "Dual Class"];
        private static int MissionIndex = 0;

        public static void Draw()
        {
            void DrawBuffSetting(string label, string uniqueId, bool currentEnabled, int currentMinGp, int minGpLimit, int maxGpLimit, string entryName, string ActionInfo, Action<bool> onEnabledChange, Action<int> onMinGpChange, int currentMaxUse, Action<int> onMaxUseChange)
            {
                bool enabled = currentEnabled;
                if (ImGui.Checkbox($"{label}###Enable{uniqueId}", ref enabled))
                {
                    if (enabled != currentEnabled)
                        onEnabledChange(enabled);
                }
                ImGuiEx.HelpMarker(ActionInfo);

                if (enabled)
                {
                    ImGui.Indent(15);

                    if (ImGui.TreeNode($"{label} Settings###Tree{uniqueId}{entryName}"))
                    {
                        int minGp = currentMinGp;
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text("Minimum GP");
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(200);
                        if (ImGui.SliderInt($"###Slider{uniqueId}{entryName}", ref minGp, minGpLimit, maxGpLimit))
                        {
                            if (minGp != currentMinGp)
                                onMinGpChange(minGp);
                        }
                        int maxUse = currentMaxUse;
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text("Maximum use count");
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(100);
                        if (ImGui.InputInt($"###Slider{uniqueId}{entryName}_1", ref maxUse, 1))
                        {
                            if (maxUse != currentMaxUse)
                                onMaxUseChange(maxUse);
                        }
                        ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                           "Set to 1-> X to set maximum amount of uses per mission");

                        ImGui.TreePop();
                    }
                    ImGui.Unindent(15);
                }
            }

            void DrawCustomBuffSetting(string label, string uniqueId, bool currentEnabled, int currentMinGp, int minGpLimit, int maxGpLimit, string entryName, string ActionInfo, Action<bool> onEnabledChange, Action<int> onMinGpChange, int currentMaxUse, Action<int> onMaxUseChange, int MinItemUsage, Action<int> onMinItemMaxUseChange)
            {
                bool enabled = currentEnabled;
                if (ImGui.Checkbox($"{label}###Enable{uniqueId}", ref enabled))
                {
                    if (enabled != currentEnabled)
                        onEnabledChange(enabled);
                }
                ImGuiEx.HelpMarker(ActionInfo);

                if (enabled)
                {
                    ImGui.Indent(15);

                    if (ImGui.TreeNode($"{label} Settings###Tree{uniqueId}{entryName}"))
                    {
                        int minGp = currentMinGp;
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text("Minimum GP");
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(200);
                        if (ImGui.SliderInt($"###Slider{uniqueId}{entryName}", ref minGp, minGpLimit, maxGpLimit))
                        {
                            if (minGp != currentMinGp)
                                onMinGpChange(minGp);
                        }
                        int maxUse = currentMaxUse;
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text("Maximum use count");
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(100);
                        if (ImGui.InputInt($"###Slider{uniqueId}{entryName}_1", ref maxUse, 1))
                        {
                            if (maxUse != currentMaxUse)
                                onMaxUseChange(maxUse);
                        }
                        ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                           "Set to 1-> X to set maximum amount of uses per mission");

                        int MinItem = MinItemUsage;
                        ImGui.Text($"Minimum BYII Item");
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(100);
                        if (ImGui.SliderInt($"###MinItemsBYII{uniqueId}{entryName}_1", ref MinItem, 2, 4))
                        {
                            if (MinItem != MinItemUsage)
                                onMinItemMaxUseChange(MinItem);
                        }
                        ImGuiEx.HelpMarker($"Set the minimum amount of items that you want BYII to activate on\n" +
                                           $"Ex. Setting it to 2 will make it to where if you only activate if you need need 2 or more items\n" +
                                           $"Useful if you're trying to save gp on gather x amount or dual class missions");

                        ImGui.TreePop();
                    }
                    ImGui.Unindent(15);
                }
            }

            int maxGp = 1200;

            if (ImGui.Checkbox("Self Repair on Gather", ref SelfRepairGather))
            {
                if (C.SelfRepairGather != SelfRepairGather)
                {
                    C.SelfRepairGather = SelfRepairGather;
                    C.Save();
                }
            }
            if (SelfRepairGather)
            {
                ImGui.Indent(15);
                ImGui.Text("Repair at");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150);
                if (ImGui.SliderFloat("###Repair %", ref SelfRepairPercent, 0f, 99f, "%.0f%%"))
                {
                    if (C.RepairPercent != SelfRepairPercent)
                    {
                        C.RepairPercent = (int)SelfRepairPercent;
                        C.Save();
                    }
                }
                ImGui.Unindent(15);
            }
            if (ImGui.Checkbox("Extract Spiritbond on Gather", ref SelfSpiritbondGather))
            {
                if (C.SelfSpiritbondGather != SelfSpiritbondGather)
                {
                    C.SelfSpiritbondGather = SelfSpiritbondGather;
                    C.Save();
                }
            }
            if (ImGui.Checkbox("Auto Cordial", ref AutoCordial))
            {
                C.AutoCordial = AutoCordial;
                C.Save();
            }
            ImGuiEx.HelpMarker("Will only work while using ICE and not manual mode\n" +
                               "Will also pause pandora cordial usage while on the moon");
            if (AutoCordial)
            {
                if (ImGui.TreeNode("Cordial Settings"))
                {
                    if (ImGui.Checkbox("Inverse Priority (Watered -> Regular -> Hi)", ref InverseCordialPrio))
                    {
                        C.inverseCordialPrio = InverseCordialPrio;
                        C.Save();
                    }
                    if (ImGui.Checkbox("Prevent Overcap", ref PreventOvercap))
                    {
                        C.PreventOvercap = PreventOvercap;
                        C.Save();
                    }
                    if (ImGui.Checkbox("Use on Fisher", ref UseOnFisher))
                    {
                        C.UseOnFisher = UseOnFisher;
                        C.Save();
                    }
                    if (ImGui.Checkbox("Only use in mission", ref useOnlyInMission))
                    {
                        C.UseOnlyInMission = useOnlyInMission;
                        C.Save();
                    }
                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Gp Threshold", ref CordialMinGp, 0, maxGp))
                    {
                        C.CordialMinGp = CordialMinGp;
                        C.Save();
                    }

                    ImGui.TreePop();
                }
            }

            ImGui.Dummy(new(0, 5));

            ImGui.SetNextItemWidth(200);
            ImGui.InputText("New Profile Name", ref newProfileName, 64);
            using (ImRaii.Disabled(newProfileName == ""))
            {
                if (ImGui.Button("Add Profile") && !string.IsNullOrWhiteSpace(newProfileName))
                {
                    if (!C.GatherSettings.Any(x => x.Name == newProfileName))
                    {
                        int newId = C.GatherSettings.Max(x => x.Id) + 1;
                        C.GatherSettings.Add(new Config.GatherProfile { Id = newId, Name = newProfileName });
                        C.Save();
                        newProfileName = ""; // Reset input
                    }
                }
            }

            ImGui.Columns(2, "Gather Settings Columns", false);

            // ------------------ 
            //  Left Column, Profile Settings
            // ------------------
            ImGui.SetColumnWidth(0, 350);

            ImGui.Text("Gather Profiles");

            bool canDelete = C.GatherSettings.Count > 1 && C.SelectedGatherIndex != 0;
            using (ImRaii.Disabled(!canDelete))
            {
                if (ImGui.Button("Delete Selected Profile"))
                {
                    var deletedProfile = C.GatherSettings[C.SelectedGatherIndex];
                    int deletedId = deletedProfile.Id;

                    // Remove the profile
                    C.GatherSettings.RemoveAt(C.SelectedGatherIndex);

                    // Update all missions using this GatherSettingId
                    foreach (var mission in C.MissionConfig)
                    {
                        if (mission.Value.GatherProfileId == deletedId)
                        {
                            mission.Value.GatherProfileId = C.GatherSettings[0].Id; // fallback to default
                        }
                    }

                    // Clamp the selected index and save
                    C.SelectedGatherIndex = Math.Clamp(C.SelectedGatherIndex, 0, C.GatherSettings.Count - 1);
                    C.Save();
                }
            }

            ImGui.BeginChild("GatherProfileChild", new Vector2(300, ImGui.GetTextLineHeightWithSpacing() * 5 + 10), true);
            for (int i = 0; i < C.GatherSettings.Count; i++)
            {
                bool isSelected = (i == C.SelectedGatherIndex);

                if (ImGui.Selectable(C.GatherSettings[i].Name, isSelected))
                {
                    C.SelectedGatherIndex = i;
                    C.Save();
                }

                if (isSelected)
                    ImGui.SetItemDefaultFocus();
            }
            ImGui.EndChild();

            GatherProfile entry = C.GatherSettings[C.SelectedGatherIndex];

            ImGui.Combo("Mission Type", ref MissionIndex, MissionTypes, MissionTypes.Length);
            if (ImGui.Button("Apply to Mission Types"))
            {
                foreach (var mission in C.MissionConfig)
                {
                    var id = mission.Key;

                    var missionDict = CosmicHelper.MissionInfoDict[id];

                    bool craftMission = missionDict.Attributes.HasFlag(MissionAttributes.Craft);
                    bool gatherMission = missionDict.Attributes.HasFlag(MissionAttributes.Gather);

                    bool LimitedQuant = missionDict.Attributes.HasFlag(MissionAttributes.Limited);
                    // Gather X Amount is just "Gather" 
                    bool TimedMission = missionDict.Attributes.HasFlag(MissionAttributes.ScoreTimeRemaining);
                    bool ChainedMission = missionDict.Attributes.HasFlag(MissionAttributes.ScoreChains);
                    bool BoonMission = missionDict.Attributes.HasFlag(MissionAttributes.ScoreGatherersBoon);
                    bool collectableMission = missionDict.Attributes.HasFlag(MissionAttributes.Collectables);
                    bool stellerReductionMission = missionDict.Attributes.HasFlag(MissionAttributes.ReducedItems);

                    bool GatherX = !stellerReductionMission && !collectableMission && !BoonMission && !ChainedMission && !TimedMission && !LimitedQuant;

                    void UpdateMissions()
                    {
                        mission.Value.GatherProfileId = entry.Id;
                    }

                    if (gatherMission && (!collectableMission && !stellerReductionMission))
                    {
                        if (MissionIndex == 0 && LimitedQuant)
                            UpdateMissions();
                        else if (MissionIndex == 2 && TimedMission)
                            UpdateMissions();
                        else if (MissionIndex == 3 && ChainedMission && !BoonMission)
                            UpdateMissions();
                        else if (MissionIndex == 4 && BoonMission && !ChainedMission)
                            UpdateMissions();
                        else if (MissionIndex == 5 && ChainedMission && BoonMission)
                            UpdateMissions();
                        else if (MissionIndex == 6 && craftMission)
                            UpdateMissions();
                        else if (MissionIndex == 1 && GatherX)
                            UpdateMissions();
                    }
                }

                C.Save();
            }

            // ---------------------------------
            // Right Column, Gathering setttings
            // ---------------------------------

            ImGui.NextColumn();
            ImGui.SetColumnWidth(1, ImGui.GetWindowWidth() - 300);

            /* Don't necessary want to get rid of this, it's good in practicality. Just need to bring to new system eventually...
             * 
            // Pathfinding
            int pathfinding = entry.Pathfinding;
            string[] modes = ["Simple", "Nearest", "Cyclic"];
            ImGui.SetNextItemWidth(100);
            if (ImGui.Combo("Pathfinding mode", ref pathfinding, modes, modes.Length))
            {
                entry.Pathfinding = pathfinding;
                C.Save();
            }
            ImGuiEx.HelpMarker("Simple - From 1st node in list until the last.\nNearest - Always go to Nearest node then find a path that minimises distance through all remaining nodes.\nCyclic - Find nodes that are close together and stick to those nodes only.");
            if (pathfinding == 2)
            {
                ImGui.SameLine();
                ImGui.SetNextItemWidth(100);
                int cycle = entry.TSPCycleSize;
                if (ImGui.InputInt("Cycle size", ref cycle, 1))
                {
                    entry.TSPCycleSize = cycle >= 2 ? cycle : 2;
                    C.Save();
                }
            }
            */

            // GP Settings
            int minGP = entry.MinimumGp;
            ImGui.SetNextItemWidth(100);
            if (ImGui.SliderInt("Minimum GP to start mission", ref minGP, -1, maxGp))
            {
                entry.MinimumGp = minGP;
                C.Save();
            }

            // Multiply gathered items on FIRST gather loop only. Should only be used for Dual Class really.
            int gatherMult = entry.DualClassCraftAmount;
            ImGui.SetNextItemWidth(100);
            if (ImGui.InputInt("Dual Class Craft Amount", ref gatherMult, 1))
            {
                entry.DualClassCraftAmount = gatherMult >= 1 ? gatherMult : 1;
                C.Save();
            }
            ImGuiEx.HelpMarker("This increases how many items you gather before you are 'done' before switching to crafting.\nSet this to however many items you need to craft to reach your target score.\nOnly affects Dual Class missions.");

            // Boon Increase 2 (+30% Increase)
            DrawBuffSetting(
                label: "Pioneer's / Mountaineer's Gift II",
                uniqueId: $"Boon2Inc{entry.Id}",
                currentEnabled: entry.GatherBuffs.Buffs["BoonIncrease2"].Enabled,
                currentMinGp: entry.GatherBuffs.Buffs["BoonIncrease2"].MinGp,
                minGpLimit: 100,
                maxGpLimit: maxGp,
                entryName: entry.Name,
                ActionInfo: "Apply a 30% buff to your boon chance.",
                onEnabledChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BoonIncrease2"].Enabled = newVal;
                    C.Save();
                },
                onMinGpChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BoonIncrease2"].MinGp = newVal;
                    C.Save();
                },
                currentMaxUse: entry.GatherBuffs.Buffs["BoonIncrease2"].MaxUse,
                onMaxUseChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BoonIncrease2"].MaxUse = newVal;
                    C.Save();
                }
            );

            // Boon Increase 1 (+10% Increase)
            DrawBuffSetting(
                label: "Pioneer's / Mountaineer's Gift I",
                uniqueId: $"Boon1Inc{entry.Id}",
                currentEnabled: entry.GatherBuffs.Buffs["BoonIncrease1"].Enabled,
                currentMinGp: entry.GatherBuffs.Buffs["BoonIncrease1"].MinGp,
                minGpLimit: 50,
                maxGpLimit: maxGp,
                entryName: entry.Name,
                ActionInfo: "Apply a 10% buff to your boon chance.",
                onEnabledChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BoonIncrease1"].Enabled = newVal;
                    C.Save();
                },
                onMinGpChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BoonIncrease1"].MinGp = newVal;
                    C.Save();
                },
                currentMaxUse: entry.GatherBuffs.Buffs["BoonIncrease1"].MaxUse,
                onMaxUseChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BoonIncrease1"].MaxUse = newVal;
                    C.Save();
                }
            );

            // Tidings (+2 to boon instead of +1)
            DrawBuffSetting(
                label: "Nophica's / Nald'thal's Tidings Buff",
                uniqueId: $"TidingsBuff{entry.Id}",
                currentEnabled: entry.GatherBuffs.Buffs["Tidings"].Enabled,
                currentMinGp: entry.GatherBuffs.Buffs["Tidings"].MinGp,
                minGpLimit: 200,
                maxGpLimit: maxGp,
                entryName: entry.Name,
                ActionInfo: "Increases item yield from Gatherer's Boon by 1",
                onEnabledChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["Tidings"].Enabled = newVal;
                    C.Save();
                },
                onMinGpChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["Tidings"].MinGp = newVal;
                    C.Save();
                },
                currentMaxUse: entry.GatherBuffs.Buffs["Tidings"].MaxUse,
                onMaxUseChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["Tidings"].MaxUse = newVal;
                    C.Save();
                }
            );

            // Yield II (+2 to all items on node)
            DrawBuffSetting(
                label: "Blessed / Kings Yield II",
                uniqueId: $"Blessed/KingsYieldIIBuff{entry.Id}",
                currentEnabled: entry.GatherBuffs.Buffs["YieldII"].Enabled,
                currentMinGp: entry.GatherBuffs.Buffs["YieldII"].MinGp,
                minGpLimit: 500,
                maxGpLimit: maxGp,
                entryName: entry.Name,
                ActionInfo: "Increases the number of items obtained when gathering by 2\n" +
                            "Will only apply when the gathering node has full durability",
                onEnabledChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["YieldII"].Enabled = newVal;
                    C.Save();
                },
                onMinGpChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["YieldII"].MinGp = newVal;
                    C.Save();
                },
                currentMaxUse: entry.GatherBuffs.Buffs["YieldII"].MaxUse,
                onMaxUseChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["YieldII"].MaxUse = newVal;
                    C.Save();
                }
            );

            // Yield I (+1 to all items on node)
            DrawBuffSetting(
                label: "Blessed / Kings Yield I",
                uniqueId: $"Blessed/KingsYieldIBuff{entry.Id}",
                currentEnabled: entry.GatherBuffs.Buffs["YieldI"].Enabled,
                currentMinGp: entry.GatherBuffs.Buffs["YieldI"].MinGp,
                minGpLimit: 400,
                maxGpLimit: maxGp,
                entryName: entry.Name,
                ActionInfo: "Increases the number of items obtained when gathering by 1\n" +
                            "Will only apply when the gathering node has full durability",
                onEnabledChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["YieldI"].Enabled = newVal;
                    C.Save();
                },
                onMinGpChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["YieldI"].MinGp = newVal;
                    C.Save();
                },
                currentMaxUse: entry.GatherBuffs.Buffs["YieldI"].MaxUse,
                onMaxUseChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["YieldI"].MaxUse = newVal;
                    C.Save();
                }
            );

            // Bonus Integrity (+1 integrity)
            DrawBuffSetting(
                label: "Ageless Words / Solid Reason",
                uniqueId: $"Incrase Intregity{entry.Id}",
                currentEnabled: entry.GatherBuffs.Buffs["BonusIntegrity"].Enabled,
                currentMinGp: entry.GatherBuffs.Buffs["BonusIntegrity"].MinGp,
                minGpLimit: 300,
                maxGpLimit: maxGp,
                entryName: entry.Name,
                ActionInfo: "Increase the Integrity by 1\n" +
                            "50% chance to grant Eureka Moment",
                onEnabledChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BonusIntegrity"].Enabled = newVal;
                    C.Save();
                },
                onMinGpChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BonusIntegrity"].MinGp = newVal;
                    C.Save();
                },
                currentMaxUse: entry.GatherBuffs.Buffs["BonusIntegrity"].MaxUse,
                onMaxUseChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BonusIntegrity"].MaxUse = newVal;
                    C.Save();
                }
            );

            // Bountiful Yield/Harvest II (+Amount based on gathering)
            DrawCustomBuffSetting(
                label: "Bountiful Yield II / Bountiful Harvest II",
                uniqueId: $"Bountiful Yield II {entry.Id}",
                currentEnabled: entry.GatherBuffs.Buffs["BountifulYieldII"].Enabled,
                currentMinGp: entry.GatherBuffs.Buffs["BountifulYieldII"].MinGp,
                minGpLimit: 100,
                maxGpLimit: maxGp,
                entryName: entry.Name,
                ActionInfo: "Increase item's gained on next gathering attempt by 1, 2, or 3 \n" +
                            "This is based on your gathering rating",
                onEnabledChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BountifulYieldII"].Enabled = newVal;
                    C.Save();
                },
                onMinGpChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BountifulYieldII"].MinGp = newVal;
                    C.Save();
                },
                currentMaxUse: entry.GatherBuffs.Buffs["BountifulYieldII"].MaxUse,
                onMaxUseChange: newVal =>
                {
                    entry.GatherBuffs.Buffs["BountifulYieldII"].MaxUse = newVal;
                    C.Save();
                },
                entry.GatherBuffs.BountifulMinItem,
                onMinItemMaxUseChange: newVal =>
                {
                    entry.GatherBuffs.BountifulMinItem = newVal;
                    C.Save();
                }
            );

            ImGui.Columns(1);
        }
    }
}
