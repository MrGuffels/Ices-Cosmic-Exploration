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
            if (CosmicHelper.CurrentLunarMission != 0)
            {
                // A mission was found to be active [aka not 0]
                if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && !missionInfo.IsAddonReady)
                {
                    // The mission info (the one that contains the timer + current score while a mission is active) isn't loaded. Going to fix that.
                    if (EzThrottler.Throttle("Attempting to open the mission information window"))
                    {
                        CosmicHelper.OpenStellarMission();
                        return false;
                    }
                }
                else
                {
                    // Mission info window is now ready to be read, time to check out the current progress.
                    if (MissionHandler.IsMissionTimedOut())
                    {
                        // Mission time has reached 0, checking the score/aborting if necessary
                        SchedulerMain.State |= IceState.AbortInProgress;


                    }

                }
            }

            return true;
        }
    }
}
