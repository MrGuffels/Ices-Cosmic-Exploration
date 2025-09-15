using ICE.Config;
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

                var mission = CosmicHelper.SheetMissionDict[missionId];
                bool fishingMission = mission.Attributes.HasFlag(MissionAttributes.Fish);
                C.MissionConfig.TryGetValue(missionId, out var config);

                if (C.OnlyGrabMission || (config != null && config.ManualMode) || mission.Attributes.HasFlag(MissionAttributes.Gather) || mission.Attributes.HasFlag(MissionAttributes.Fish))
                {
                    SchedulerMain.State = IceState.ManualMode;
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
                    IceLogging.Debug("Mission is a fishing mission, might also contain crafting in it but. For now starting off with the fishing portion");
                }
                else if (mission.Attributes.HasFlag(MissionAttributes.Gather))
                {
                    SchedulerMain.State = IceState.Gather;
                    IceLogging.Debug("Mission is a gathering mission. Need to gather inial resources. But first going to do a check to make sure where we're at.", "[Task_ExecuteMission]");
                    var missionFlag = mission.MapPosition;
                    // TODO: Add things to checking for last gather ring, and reset current nodeId from there
                }
                else if (mission.Attributes.HasFlag(MissionAttributes.Craft))
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
