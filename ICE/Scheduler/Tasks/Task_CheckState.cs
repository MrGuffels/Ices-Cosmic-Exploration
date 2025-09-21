using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using System.Collections.Generic;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;
using static ICE.Utilities.CosmicHelper;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_CheckState
    {
        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => CheckState(), "Checking to see what state we should be in");
        }

        private static unsafe bool? CheckState()
        {
            var currentMissionId = CosmicHelper.CurrentLunarMission;

            if (AddonHelper.IsAddonActive("WKSLottery"))
            {
                IceLogging.Info("Setting State to gambling");
                SchedulerMain.State = IceState.Gambling;
                return true;
            }
            else
            {
                if (C.StopWhenLevel && Player.Level >= C.TargetLevel)
                {
                    SchedulerMain.State = IceState.Idle;
                    IceLogging.ChatInfo("Stop At Player Level is enabled. \n" +
                                       $"Your current level is: {Player.Level} and Goal: {C.TargetLevel}", "[I.C.E.]");
                    return true;
                }
                if (C.StopOnceHitCosmicScore)
                {
                    var scores = CosmicHelper.GetCosmicClassScores();

                    if (scores.classScore >= C.CosmicScoreCap)
                    {
                        IceLogging.ChatInfo("Stop At Cosmic Score is enabled. \n" +
                                           $"Your current level is: {scores.classScore} and Goal: {C.CosmicScoreCap}", "[I.C.E.]");
                        SchedulerMain.State = IceState.Idle;
                        return true;
                    }
                }
                if (C.StopOnceHitLunarCredits)
                {
                    uint[] currencies = [45691, 48146, 48147, 48148];
                    var manager = WKSManager.Instance();
                    var zoneId = *((byte*)manager + 0x5D);
                    var itemId = currencies[zoneId];

                    PlayerHelper.GetItemCount(itemId, out var credits);
                    if (credits >= C.LunarCreditsCap)
                    {
                        IceLogging.ChatInfo($"You've either hit the Lunar Credit threshold, or gone above it.\n" +
                                            $"Stopping I.C.E.", "[I.C.E.]");
                        SchedulerMain.State = IceState.Idle;
                        return true;
                    }
                }
                if (C.StopOnceHitCosmoCredits)
                {
                    if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var hud) && hud.IsAddonReady && (hud.CosmoCredit >= C.CosmoCreditsCap))
                    {
                        IceLogging.ChatInfo($"Stopping the plugin as you have {hud.CosmoCredit} Cosmocredits.", "[I.C.E.]");
                        SchedulerMain.State = IceState.Idle;
                        return true;
                    }
                }
                if (C.StopOnceRelicFinished)
                {
                    var wksManager = WKSManager.Instance();
                    if (wksManager == null || wksManager->ResearchModule == null || !wksManager->ResearchModule->IsLoaded)
                        return null;

                    var job = Player.JobId;
                    var toolClassId = (byte)(job - 7);
                    var stage = wksManager->ResearchModule->CurrentStages[toolClassId - 1];
                    var nextstate = wksManager->ResearchModule->UnlockedStages[toolClassId - 1];

                    Dictionary<int, CosmicHelper.XPType> XPTable = new Dictionary<int, CosmicHelper.XPType>();

                    for (byte type = 1; type < 6; type++)
                    {
                        if (!wksManager->ResearchModule->IsTypeAvailable(toolClassId, type))
                        {

                        }

                        var neededXP = wksManager->ResearchModule->GetNeededAnalysis(toolClassId, type);

                        var currentXp = wksManager->ResearchModule->GetCurrentAnalysis(toolClassId, type);
                        var requiredXp = neededXP - currentXp;
                        if (!XPTable.ContainsKey(type))
                        {
                            XPTable[type] = new XPType()
                            {
                                CurrentXP = currentXp,
                                NeededXP = neededXP,
                            };
                        }
                    }

                    bool allComplete = true;

                    for (int i = 0; i < XPTable.Count; i++)
                    {
                        var bar = XPTable[i + 1];
                        if (bar.CurrentXP < bar.NeededXP)
                        {
                            allComplete = false;
                        }
                    }
                    if (allComplete)
                    {
                        IceLogging.Info("You have met all necessary relic xp, and you have \"Stop on Relic Completion\" enabled, so stopping for now");
                        SchedulerMain.State = IceState.Idle;
                        return true;
                    }
                    else
                    {
                        IceLogging.Info($"Stop on relic completion is checked, but you also aren't done. So going to continue on", "[Task: CheckState]");
                    }
                }
                if (currentMissionId != 0)
                {
                    IceLogging.Debug($"Current mission id is not 0, which means we're in the middle of a mission");
                    if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
                    {
                        IceLogging.Debug($"Mission Infomation was active, checking if a mission is timed out.");
                        if (CosmicHandler.IsMissionTimedOut())
                        {
                            // Mission time has reached 0, checking the score/aborting if necessary
                            IceLogging.Info("Mission is currently timed out. Going to abandon the mission state", "[Task: Check State]");
                            SchedulerMain.State = IceState.AbandonMission;
                            P.TaskManager.Tasks.Clear();
                            return true;
                        }
                        else
                        {
                            IceLogging.Debug($"Mission isn't timed out... checking other states");
                            UpdateMissionState(currentMissionId);
                            C.MissionConfig.TryGetValue(currentMissionId, out var config);

                            var s = SchedulerMain.MissionState;
                            bool dualMission = s.HasFlag(MissionAttributes.Craft) && s.HasFlag(MissionAttributes.Gather);
                            // In the middle of a dual mission. 
                            // First, checking to see if you're in the middle of a gathering or crafting action
                            if (C.OnlyGrabMission || config.ManualMode || s.HasFlag(MissionAttributes.Fish))
                            {
                                // TODO: Remove this once properly coded
                                if (s.HasFlag(MissionAttributes.Fish))
                                {
                                    IceLogging.Info("Currently not built in/supported yet. Swapping to manual mode");
                                }
                                else
                                {
                                    IceLogging.Info($"You have either manual mode enabled, or you have OnlyGrabMission enabled. Swapping to manual mode state");
                                }
                                SchedulerMain.State = IceState.ManualMode;
                            }
                            else if (dualMission)
                            {
                                IceLogging.Info("We're in a dual craft mission, going to kick it over there", "[Task: Check State]");
                                Mission_Settings.ResetNodeCounter();
                                SchedulerMain.State = IceState.DualClass;
                            }
                            else if (Svc.Condition[ConditionFlag.Crafting] || P.Artisan.IsBusy())
                            {
                                IceLogging.Info("We are on a crafter, and either in the middle of crafting or need to start.", "[Task: Check State]");
                                SchedulerMain.State = IceState.Craft;
                            }
                            else if (Svc.Condition[ConditionFlag.Gathering])
                            {
                                Mission_Settings.ResetNodeCounter();
                                IceLogging.Info("On a gathering class, kicking over to the gathering action", "[Task: Check State]");
                                SchedulerMain.State = IceState.Gather;
                            }
                            else
                            {
                                // Not currently in the middle of an action, so time to check score and go from there.
                                IceLogging.Debug("Not in the middle of an action, swapping to score checking", "[Task_CheckState]");
                                SchedulerMain.State = IceState.ScoreCheck;
                            }

                            return true;
                        }
                    }
                    else
                    {
                        // The mission info (the one that contains the timer + current score while a mission is active) isn't loaded. Going to fix that.
                        if (EzThrottler.Throttle("Attempting to open the mission information window"))
                        {
                            IceLogging.Info("Opening the mission information window, you're in the middle of one!", "[Check State]");
                            CosmicHelper.OpenStellarMission();
                        }
                        return false;
                    }
                }
                else
                {
                    var currentJob = Player.JobId;

                    bool repairVendor = C.RepairAtVendor && PlayerHelper.NeedsRepair(C.RepairPercent);
                    bool selfRepairCraft = C.SelfRepairCrafter && PlayerHelper.NeedsRepair(C.RepairPercent) && CosmicHelper.CrafterJobList.Contains(currentJob);
                    bool selfRepairGather = C.SelfRepairGather && PlayerHelper.NeedsRepair(C.RepairPercent) && CosmicHelper.GatheringJobList.Contains(currentJob);
                    bool extractSpiritbond = C.SelfSpiritbondGather && Task_Spiritbond.IsSpiritbondReadyAny();

                    if (extractSpiritbond && CosmicHelper.GatheringJobList.Contains(currentJob))
                    {
                        IceLogging.Info("Extracting spiritbond is enabled. And you have some to extract. Going to go do so now", "[Task: Check State]");
                        SchedulerMain.State = IceState.Spiritbond;
                    }
                    else if (repairVendor ||  selfRepairCraft || selfRepairGather)
                    {
                        IceLogging.Info("We need to repair! So going to go repair", "[Task: Check State]");
                        SchedulerMain.State = IceState.Repair;
                    }
                    else
                    {
                        IceLogging.Info("Not in the middle of a mission, and don't need to repair/extract materia. So going to grab mission", "[Task: Check State]");
                        SchedulerMain.State = IceState.GrabMission;
                    }

                    IceLogging.Info($"There is no physical possible way for you to not be in a different state here. . . So reporting back the current state upon exiting here: {SchedulerMain.State}");
                    return true;
                }
            }
        }

        private static void UpdateMissionState(uint missionId)
        {
            // Clearing the current mission modifiers.
            SchedulerMain.MissionState = MissionAttributes.None;

            // Grabbing the mission info from the dictionary entry
            var missionDictInfo = CosmicHelper.SheetMissionDict[missionId];

            // Updating the Mission state to be the same as the current mission that's fired.
            SchedulerMain.MissionState = missionDictInfo.Attributes;
        }
    }
}
