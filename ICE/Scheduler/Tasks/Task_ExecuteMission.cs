using ICE.Config;
using ICE.Utilities.Cosmic;
using ICE.Utilities.Cosmic_Helper;
using ICE.Utilities.GatheringHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_ExecuteMission
    {
        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => ExecuteMission(), "Finding proper mission state");
        }

        private static bool? ExecuteMission()
        {
            if (CosmicHelper.CurrentLunarMission != 0)
            {
                var missionId = CosmicHelper.CurrentLunarMission;
                P.MissionTimer.StartMission(missionId);

                var mission = CosmicHelper.SheetMissionDict[missionId];
                bool fishingMission = mission.Jobs.Contains(18);
                bool gatherMission = mission.Jobs.Contains(16) || mission.Jobs.Contains(17);
                bool craftMission = mission.Jobs.Overlaps(CosmicHelper.CrafterJobList);

                C.MissionConfig.TryGetValue(missionId, out var config);
                bool dualClass = (gatherMission && craftMission) || (fishingMission && craftMission);

                if (C.OnlyGrabMission || (config != null && config.ManualMode) || UnsupportedMissions.Ids.Contains(missionId))
                {
                    SchedulerMain.State = IceState.ManualMode;
                }
                else if (dualClass)
                {
                    IceLogging.Info("We've found a dual class mission! Kicking it off with that.", "[Task: Execute Mission]");
                    SchedulerMain.State = IceState.DualClass;
                    if (fishingMission)
                    {
                        if (config.Use_BuildinPreset)
                        {
                            P.AutoHook.DeleteAllAnonymousPresets();
                            foreach (var preset in GatheringUtil.FishingPreset[missionId].FishingPreset)
                            {
                                P.AutoHook.CreateAndSelectAnonymousPreset(preset);
                            }
                        }
                    }
                }
                else if (fishingMission)
                {
                    // Check exist twice, one here is to actually enable the fishing profile that is selected.
                    var missionConfig = C.MissionConfig[missionId];
                    if (missionConfig.Use_BuildinPreset)
                    {
                        // Using the build in presets that are included in the plugin.
                        P.AutoHook.DeleteAllAnonymousPresets();
                        var presetList = GatheringUtil.FishingPreset[missionId];
                        foreach (var preset in presetList.FishingPreset)
                        {
                            P.AutoHook.CreateAndSelectAnonymousPreset(preset);
                        }
                    }
                    else
                    {
                        string presetName = missionConfig.AutoHookPresetName;
                        P.AutoHook.SetPreset(presetName);
                    }

                    SchedulerMain.State = IceState.Fish;
                    IceLogging.Debug("Mission is a fishing mission, so going to the fishing task");
                }
                else if (gatherMission)
                {
                    SchedulerMain.State = IceState.Gather;
                    IceLogging.Info("Mission is a gathering mission. Need to gather inial resources. But first going to do a check to make sure where we're at.", "[Task_ExecuteMission]");
                }
                else if (craftMission)
                {
                    IceLogging.Debug("Mission is purely a crafting mission (yay), checking current state next", "[Task_ExecuteMission]");
                    SchedulerMain.State = IceState.Craft;
                }
            }
            else if (CosmicHelper.CurrentLunarMission == 0)
            {
                IceLogging.Debug("Hmm... somehow we got in this state. And we shouldn't be? Returning back to the grab mission state");
                SchedulerMain.State = IceState.GrabMission;
            }

            return true;
        }
    }
}
