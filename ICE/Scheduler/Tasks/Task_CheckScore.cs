using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_CheckScore
    {
        private static Dictionary<string, int> ScoreKeeper = new Dictionary<string, int>()
        {
            ["Current"] = 0,
            ["Silver"] = 0,
            ["Gold"] = 0,
            ["Critical"] = 1
        };

        public static void Enqueue()
        {

        }

        private static bool? CheckScore(bool forceTurnin = false)
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
            {
                uint currentMission = CosmicHelper.CurrentLunarMission;
                var missionEntry = CosmicHelper.MissionInfoDict[currentMission];
                bool canTurnin = true;
                bool gatherMission = missionEntry.Attributes.HasFlag(MissionAttributes.Gather) || missionEntry.Attributes.HasFlag(MissionAttributes.Fish);

                if (CosmicHandler.IsMissionTimedOut())
                {
                    P.TaskManager.Tasks.Clear();
                    P.TaskManager.Insert(() => ForceTurnin(), "Forcing mission turnin, or abandon");
                    return true;
                }

                if (missionEntry.Attributes.HasFlag(MissionAttributes.ScoreTimeRemaining) && gatherMission)
                {
                    // This should really only apply to gathering/fishing. Crafters don't have this particular one... I believe. Can't wait to be wrong here.
                    var gatherEntry = CosmicHelper.GatheringItemDict[currentMission];
                    foreach (var item in gatherEntry.MinGatherItems)
                    {

                    }
                }
            }
            else
            {
                // The addon wasn't ready, fire off the mooon hud addon to open it (not sure how it got closed to begin with
                if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                {
                    if (FrameThrottler.Throttle("Opening the mission information screen", 60))
                    {
                        IceLogging.Info("Opening the mission information screen (hate these names)");
                        moonHud.Mission();
                    }
                }
            }

                return false;
        }

        private static bool? ForceTurnin()
        {
            return false;
        }
    }
}
