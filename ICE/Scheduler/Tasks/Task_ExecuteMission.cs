using ICE.Config;
using ICE.Utilities.Cosmic;
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
                bool gatherMission = mission.Attributes.HasFlag(MissionAttributes.Gather);
                bool craftMission = mission.Attributes.HasFlag(MissionAttributes.Craft);

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
                else if (fishingMission && !config.ManualMode)
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
