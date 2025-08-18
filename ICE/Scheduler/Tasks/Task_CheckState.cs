using Dalamud.Game.ClientState.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_CheckState
    {
        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => CheckState(), "Checking to see what state we should be in");
        }

        private static bool? CheckState()
        {
            // Resetting the inital state, just to get a baseline set for everything.
            SchedulerMain.State = IceState.Start;
            var currentMissionId = CosmicHelper.CurrentLunarMission;

            if (currentMissionId != 0)
            {
                // A mission was found to be active [aka not 0]
                if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && !missionInfo.IsAddonReady)
                {
                    // The mission info (the one that contains the timer + current score while a mission is active) isn't loaded. Going to fix that.
                    if (EzThrottler.Throttle("Attempting to open the mission information window"))
                    {
                        CosmicHelper.OpenStellarMission();
                    }
                    return false;
                }
                else
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
                        if (Svc.Condition[ConditionFlag.Crafting] || P.Artisan.IsBusy())
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
                            SchedulerMain.State = IceState.ScoreCheck;
                        }
                    }
                }
            }
            else
            {
                // There is no active mission, time to find one.
                SchedulerMain.State = IceState.GrabMission;
            }

            return true;
        }

        private static void UpdateMissionState(uint missionId)
        {
            // Clearing the current mission modifiers.
            SchedulerMain.MissionState = MissionAttributes.None;

            // Grabbing the mission info from the dictionary entry
            var missionDictInfo = CosmicHelper.MissionInfoDict[missionId];

            // Updating the Mission state to be the same as the current mission that's fired.
            SchedulerMain.MissionState = missionDictInfo.Attributes;
        }
    }
}
