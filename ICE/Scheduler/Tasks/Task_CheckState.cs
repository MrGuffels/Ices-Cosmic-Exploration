using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // Resetting the inital state, just to get a baseline set for everything.
            SchedulerMain.State = IceState.Start;
            var currentMissionId = CosmicHelper.CurrentLunarMission;

            if (AddonHelper.IsAddonActive("WKSLottery"))
            {
                SchedulerMain.State = IceState.Gambling;
                return true;
            }
            else
            {
                if (C.StopWhenLevel)
                {
                    if (Player.Level >= C.TargetLevel)
                    {
                        {
                            SchedulerMain.State = IceState.Idle;
                            Svc.Chat.Print("Stop At Player Level is enabled. \n" +
                                           $"Your current level is: {Player.Level} and Goal: {C.TargetLevel}", "[I.C.E.]");
                            return true;
                        }
                    }
                }
                if (C.StopOnceHitCosmicScore)
                {
                    var scores = CosmicHelper.GetCosmicClassScores();

                    if (scores.classScore >= C.CosmicScoreCap)
                    {
                        Svc.Chat.Print("Stop At Cosmic Score is enabled. \n" +
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

                    if (PlayerHelper.GetItemCount(itemId, out var credits))
                    {
                        if (credits >= C.LunarCreditsCap)
                        {
                            SchedulerMain.State = IceState.Idle;
                            return true;
                        }
                    }
                }
                if (C.StopOnceHitCosmoCredits)
                {
                    if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var hud) && hud.IsAddonReady)
                    {
                        if (hud.CosmoCredit >= C.CosmoCreditsCap)
                        {
                            DuoLog.Information($"Stopping the plugin as you have {hud.CosmoCredit} Cosmocredits.");
                            SchedulerMain.State = IceState.Idle;
                            return true;
                        }
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
                        SchedulerMain.State = IceState.Idle;
                        return true;
                    }
                }

                if (currentMissionId != 0)
                {
                    // A mission was found to be active [aka not 0]
                    if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
                    {
                        // Mission info window is now ready to be read, time to check out the current progress.
                        if (CosmicHandler.IsMissionTimedOut())
                        {
                            // Mission time has reached 0, checking the score/aborting if necessary
                            SchedulerMain.State = IceState.ForceTurnin;
                        }
                        else
                        {
                            // Mission is still active. Time to check if it's a crafting or gathering mission
                            UpdateMissionState(currentMissionId);
                            C.MissionConfig.TryGetValue(currentMissionId, out var config);

                            var s = SchedulerMain.MissionState;
                            bool dualMission = s.HasFlag(MissionAttributes.Craft) && s.HasFlag(MissionAttributes.Gather);
                            if (s.HasFlag(MissionAttributes.Critical))
                            {
                                // Critical mission info. Need to grab/update where to turn these into. 
                                // Really... only matters for gathering (specifically, the PITA ones where the gather points are off in narnia
                                // TODO: Grab/tell where the turnin point is for that mission
                            }
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
                            else if (Svc.Condition[ConditionFlag.Crafting] || P.Artisan.IsBusy())
                            {
                                SchedulerMain.State = IceState.Craft;
                            }
                            else if (Svc.Condition[ConditionFlag.Gathering])
                            {
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
                else if (AddonHelper.IsAddonActive("WKSLottery"))
                    SchedulerMain.State = IceState.Gambling;
                else
                {
                    var currentJob = Player.JobId;

                    bool repairVendor = C.RepairAtVendor && PlayerHelper.NeedsRepair(C.RepairPercent);
                    bool selfRepairCraft = C.SelfRepairCrafter && PlayerHelper.NeedsRepair(C.RepairPercent) && CosmicHelper.CrafterJobList.Contains(currentJob);
                    bool selfRepairGather = C.SelfRepairGather && PlayerHelper.NeedsRepair(C.RepairPercent) && CosmicHelper.GatheringJobList.Contains(currentJob);

                    if (repairVendor ||  selfRepairCraft || selfRepairGather)
                    {
                        SchedulerMain.State = IceState.Repair;
                    }
                    else
                    {
                        SchedulerMain.State = IceState.GrabMission;
                    }
                }
            }

            return true;
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
