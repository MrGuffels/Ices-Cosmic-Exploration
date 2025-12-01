using Dalamud.Interface.Utility.Raii;
using ICE.Config;
using ICE.Utilities.Cosmic_Helper;
using ICE.Utilities.GatheringHelper;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace ICE.Ui.MainUi.Settings.Settings_Table
{
    internal class GatherSettings
    {
        private static string newProfileName = "";
        private static string[] MissionTypes = ["Limited Nodes", "Gather x Amount", "Time Attack", "Chained Scoring", "Boon Scoring", "Chain + Boon Scoring", "Dual Class"];
        private static int MissionIndex = 0;

        private static readonly string PROFILE_PREFIX = "IceGatherProfile_";

        public static string ExportGatherProfile(int profileId)
        {
            if (!C.GatherProfiles.TryGetValue(profileId, out var profile))
                return string.Empty;

            var json = JsonSerializer.Serialize(profile, new JsonSerializerOptions
            {
                WriteIndented = false
            });

            var bytes = Encoding.UTF8.GetBytes(json);
            var base64 = Convert.ToBase64String(bytes);

            return PROFILE_PREFIX + base64;
        }

        public static bool ImportGatherProfile(string importString, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Check for and remove the prefix
                if (!importString.StartsWith(PROFILE_PREFIX))
                {
                    errorMessage = "Invalid import string: Missing prefix";
                    return false;
                }

                var base64String = importString.Substring(PROFILE_PREFIX.Length);

                var bytes = Convert.FromBase64String(base64String);
                var json = Encoding.UTF8.GetString(bytes);

                var profile = JsonSerializer.Deserialize<GatherProfile>(json);
                if (profile == null)
                {
                    errorMessage = "Failed to deserialize profile";
                    return false;
                }

                // Get the next available ID
                int nextId = C.GatherProfiles.Keys.Count > 0
                    ? C.GatherProfiles.Keys.Max() + 1
                    : 0;

                profile.Id = nextId;
                C.GatherProfiles[nextId] = profile;

                // Save the configuration
                C.Save();

                return true;
            }
            catch (FormatException)
            {
                errorMessage = "Invalid import string: Not valid base64";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = $"Import failed: {ex.Message}";
                return false;
            }
        }

        public static void Draw()
        {
            int maxGp = 1200;

            bool SelfSpiritbondGather = C.SelfSpiritbondGather;
            if (ImGui.Checkbox("Extract Spiritbond on Gather", ref SelfSpiritbondGather))
            {
                if (C.SelfSpiritbondGather != SelfSpiritbondGather)
                {
                    C.SelfSpiritbondGather = SelfSpiritbondGather;
                    C.Save();
                }
            }

            bool AutoCordial = C.AutoCordial;
            if (ImGui.Checkbox("Auto Cordial", ref AutoCordial))
            {
                C.AutoCordial = AutoCordial;
                C.Save();
            }
            ImGuiEx.HelpMarker("Will only work while using ICE and not manual mode\n" +
                               "Will also pause pandora cordial usage while on the moon");
            if (ImGui.CollapsingHeader("Cordial Settings"))
            {
                bool InverseCordialPrio = C.inverseCordialPrio;
                bool PreventOvercap = C.PreventOvercap;
                int CordialMinGp = C.CordialMinGp;

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
                ImGui.SetNextItemWidth(200);
                if (ImGui.SliderInt("Use cordial when below the following GP", ref CordialMinGp, 0, maxGp))
                {
                    C.CordialMinGp = CordialMinGp;
                    C.SaveDebounced();
                }
                ImGui.SameLine();
                ImGuiEx.HelpMarker("What's the minimum gp you can have before it uses a cordial.\n" +
                                   "If set to 0, it'll never use a cordial even with it enabled (because... you'll never have 0 gp)");
            }

            ImGui.Separator();

            if (ImGui.BeginTable("Gathering Profile Settings", 2, ImGuiTableFlags.SizingFixedFit))
            {
                ImGui.TableSetupColumn("Profile Selection");
                ImGui.TableSetupColumn("Gathering Settings");

                // 1st Row, technically only really used for the gather profile name creator
                ImGui.TableNextRow();

                ImGui.TableSetColumnIndex(0);
                ImGui.SetNextItemWidth(200);
                ImGui.InputText("New Profile Name", ref newProfileName, 64);
                using (ImRaii.Disabled(newProfileName == ""))
                {
                    if (ImGui.Button("Add Profile") && !string.IsNullOrWhiteSpace(newProfileName))
                    {
                        var newId = C.GatherProfiles.Keys.Max() + 1;
                        C.GatherProfiles[newId] = new()
                        {
                            Name = newProfileName,
                        };
                        C.Save();
                        newProfileName = "";
                    }
                }

                // 2nd Row, Actually profile selector
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                #region Profile Selection

                ImGui.Text("Gather Profiles");

                bool canDelete = C.GatherProfiles.Count > 1 && C.SelectedGatherIndex != 0;
                using (ImRaii.Disabled(!canDelete))
                {
                    if (ImGui.Button("Delete Selected Profile"))
                    {
                        int deletedId = C.SelectedGatherIndex;

                        // Don't allow deleting the default profile
                        if (deletedId == 0)
                        {
                            return;
                        }

                        // Remove the profile
                        C.GatherProfiles.Remove(deletedId);

                        // Update all missions using this GatherSettingId
                        foreach (var mission in C.MissionConfig)
                        {
                            if (mission.Value.GProfileId == deletedId)
                            {
                                mission.Value.GProfileId = 0; // fallback to default
                            }
                        }

                        // Clamp the selected index and save
                        C.SelectedGatherIndex = 0;
                        C.Save();
                    }
                }

                ImGui.BeginChild("GatherProfileChild", new Vector2(300, ImGui.GetTextLineHeightWithSpacing() * 5 + 10), true);
                foreach (var profile in C.GatherProfiles)
                {
                    var id = profile.Key;
                    bool isSelected = C.SelectedGatherIndex == id;
                    if (ImGui.Selectable($"{profile.Value.Name}##{profile.Value.Name}_{id}", isSelected))
                    {
                        C.SelectedGatherIndex = id;
                        C.Save();
                    }

                    if (isSelected)
                        ImGui.SetItemDefaultFocus();
                }
                ImGui.EndChild();

                GatherProfile entry = C.GatherProfiles[C.SelectedGatherIndex];

                ImGui.Combo("Mission Type", ref MissionIndex, MissionTypes, MissionTypes.Length);
                if (ImGui.Button("Apply to Mission Types"))
                {
                    foreach (var mission in C.MissionConfig)
                    {
                        var id = mission.Key;

                        var missionDict = CosmicHelper.SheetMissionDict[id];

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
                            mission.Value.GProfileId = entry.Id;
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

                #endregion

                #region Profile Editor

                ImGui.TableNextColumn();
                #region Minimum GP + Dual Class Info

                int minGP = entry.MinimumGp;
                ImGui.SetNextItemWidth(100);
                if (ImGui.SliderInt("Minimum GP to start mission", ref minGP, -1, maxGp))
                {
                    entry.MinimumGp = minGP;
                    C.SaveDebounced();
                }

                ImGui.Text("Where'd the dual craft amount go?");
                ImGui.SameLine();
                ImGui.Dummy(new(5, 0));
                ImGui.SameLine();
                ImGui.TextDisabled("?");
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetNextWindowSize(new(400.0f, 0.0f)); // Fixed width, auto height
                    ImGui.BeginTooltip();

                    ImGui.TextWrapped("Short answer: It's built in now\n" +
                     "Long answer: Honestly, this was a cumbersome system in itself. And with square deciding to not continue on with dual crafting missions going into the 2nd moon, I figured it would be better to just tie it into the scoring system. You realistically only need:\n" +
                     "Gold: 3 Items\n" +
                     "Silver: 2 Items\n" +
                     "Bronze: 1 Item\n" +
                     "to be able to hit the threshold. And even then, if you manage to not hit it on the first attempt, it'll just keep gathering. Plus. This makes it to where I can not have to worry about profile managing on fishing for... 4 missions? Seemed minorly reduntant in my eyes.\n" +
                     "So now how it'll work. Select the turnin option (Gold/Any both work the same) and it will now gather up to the necessary amount -> turnin when it's ready.\n" +
                     "NOW NONE OF YOU CAN TELL IT TO CRAFT 27 ITEMS. STOP IT. IT SAID CRAFT (╯°Д°)╯︵/(.□ . \\)");
                    ImGui.EndTooltip();
                }

                #endregion

                #region Boon Increase 2

                if (ImGui.CollapsingHeader("Pioneer's | Mountaineer's Gift II"))
                {
                    string buffName = "BoonIncrease2";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Apply a 30% buff to your boon chance.";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }

                    ImGui.PopID();
                }

                #endregion

                #region Boon Increase 1

                if (ImGui.CollapsingHeader("Pioneer's | Mountaineer's Gift I"))
                {
                    string buffName = "BoonIncrease1";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Apply a 10% buff to your boon chance.";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.PopID();
                }

                #endregion

                #region Nophica's / Nald'thal's Tidings

                if (ImGui.CollapsingHeader("Nophica's / Nald'thal's Tidings Buff"))
                {
                    string buffName = "Tidings";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Increases item yield from Gatherer's Boon by 1";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.PopID();
                }

                #endregion

                #region Blessed / Kings Yield II

                if (ImGui.CollapsingHeader("Blessed / Kings Yield II"))
                {
                    string buffName = "YieldII";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Increases the number of items obtained when gathering by 2\n" +
                                        "Will only apply when the gathering node has full durability";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.PopID();
                }

                #endregion

                #region Blessed / Kings Yield I

                if (ImGui.CollapsingHeader("Blessed / Kings Yield I"))
                {
                    string buffName = "YieldI";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Increases the number of items obtained when gathering by 1\n" +
                                        "Will only apply when the gathering node has full durability";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.PopID();
                }

                #endregion

                #region Bonus Integrity

                if (ImGui.CollapsingHeader("Ageless Words / Solid Reason"))
                {
                    string buffName = "BonusIntegrity";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Increase the Integrity by 1\n" +
                                        "50% chance to grant Eureka Moment";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.PopID();
                }

                #endregion

                #region Bountiful Yield II

                if (ImGui.CollapsingHeader("Bountiful Yield II / Bountiful Harvest II"))
                {
                    string buffName = "BountifulYieldII";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Increases the number of items obtained when gathering by 2\n" +
                                        "Will only apply when the gathering node has full durability";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.Text("Minumum Items To Gather");
                    ImGui.SameLine();
                    int minItems = entry.GatherBuffs.BountifulMinItem;
                    if (ImGui.DragInt("##MinItemsGather", ref minItems, 1, 2, 4))
                    {
                        entry.GatherBuffs.BountifulMinItem = minItems;
                        C.SaveDebounced();
                    }

                    ImGui.PopID();
                }

                #endregion

                #region Field Mastery (Gather Chance)

                #region Field Mastery III

                if (ImGui.CollapsingHeader("Field Mastery | Sharp Vision III"))
                {
                    string buffName = "FieldMasteryIII";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Increases the gather chance by 50%\n" +
                                        "Please note: You can have multiple enabled, but only the one that will get you the closest to " +
                                        "100% the cheapest will be applied";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.PopID();
                }

                #endregion

                #region Field Mastery II

                if (ImGui.CollapsingHeader("Field Mastery | Sharp Vision II"))
                {
                    string buffName = "FieldMasteryII";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Increases the gather chance by 15%\n" +
                                        "Please note: You can have multiple enabled, but only the one that will get you the closest to " +
                                        "100% the cheapest will be applied";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.PopID();
                }

                #endregion

                #region Field Mastery I

                if (ImGui.CollapsingHeader("Field Mastery | Sharp Vision I"))
                {
                    string buffName = "FieldMasteryI";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Increases the gather chance by 5%\n" +
                                        "Please note: You can have multiple enabled, but only the one that will get you the closest to " +
                                        "100% the cheapest will be applied";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.PopID();
                }

                #endregion

                #region Field Mastery [Temp]

                if (ImGui.CollapsingHeader("Flora Mastery | Clear Vision [Temp]"))
                {
                    string buffName = "FieldMasteryTemp";

                    ImGui.PushID(buffName);

                    bool currentlyEnabled = entry.GatherBuffs.Buffs[buffName].Enabled;
                    int minUseGp = entry.GatherBuffs.Buffs[buffName].MinGp;
                    int minActionGp = GatheringUtil.GathActionDict[buffName].RequiredGp;
                    int maxActionUsage = entry.GatherBuffs.Buffs[buffName].MaxUse;
                    string ActionInfo = "Increases the gather chance by 15%\n" +
                                        "This can be applied with normal field mastery, but will only apply per hit";

                    ImGui.Text($"Action Info: ");
                    ImGuiEx.HelpMarker(ActionInfo);

                    if (ImGui.Checkbox("Enable", ref currentlyEnabled))
                    {
                        entry.GatherBuffs.Buffs[buffName].Enabled = currentlyEnabled;
                        C.Save();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.SliderInt("Minimum Gp for Usage", ref minUseGp, minActionGp, maxGp))
                    {
                        entry.GatherBuffs.Buffs[buffName].MinGp = minUseGp;
                        C.SaveDebounced();
                    }

                    ImGui.SetNextItemWidth(200);
                    if (ImGui.InputInt("Max Use", ref maxActionUsage))
                    {
                        entry.GatherBuffs.Buffs[buffName].MaxUse = maxActionUsage;
                        C.SaveDebounced();
                    }
                    ImGuiEx.HelpMarker("Set to -1 to allow for infinite uses \n" +
                                       "Set to 1-> X to set maximum amount of uses per mission");

                    ImGui.PopID();
                }

                #endregion

                #endregion

                #endregion

                ImGui.EndTable();
            }

            ImGui.Separator();
            if (ImGui.Button("Copy Selected Profile"))
            {
                string export = ExportGatherProfile(C.SelectedGatherIndex);
                ImGui.SetClipboardText(export);
            }

            if (ImGui.Button("Import Selected Profile"))
            {
                string importProfile = ImGui.GetClipboardText();
                string errorMessage = "";
                ImportGatherProfile(importProfile, out errorMessage);
                if (errorMessage != "")
                {
                    IceLogging.Error(errorMessage);
                }
                C.Save();
            }
        }
    }
}