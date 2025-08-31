using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_Fishing
    {
        // Something to note. 42, 43, 85 are the conditions that you get while you're fishing
        // 43 and 85 are active while you're fishing
        // 42 is active when reeling in a fish
        // Something to consider, start fishing... (that's condition 42 when you start)
        // Whenever all the conditions are cleared, check the inventory for the frame, see if you have enough/meet the score

        public static void Enqueue()
        {
            // think the process should be:
            // check score
            // if score not complete, check if can craft
            // if craft not required, fish
            // wait for fishing to be done

        }

        private static bool? CheckScore()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo))
            {
                // Hud info should be available. Now time to check the mission status.
                var id = CosmicHelper.CurrentLunarMission;
                var missionEntry = CosmicHelper.MissionInfoDict[id];

                if (missionEntry != null)
                {
                    // Should never be null, unless we somehow have a new mission that isn't in the plugin. 
                    if (missionEntry.Attributes.HasFlag(MissionAttributes.ScoreTimeRemaining))
                    {
                        // This can't really be dictated by score, it goes off of if you've met the threshold of
                        // -> Gathered enough fish in the time 
                        // -> Gathered enough "different" kinds of fish in the time 

                        if (missionEntry.Attributes.HasFlag(MissionAttributes.ScoreVariety))
                        {
                            // Need to check all fish and see if you've met the threshold
                            bool amountGathered = false;
                            if (CosmicHelper.CustomFishDict.TryGetValue(id, out var customDict))
                            {
                                // This was one that needed to be hardcoded *-sighs-* 
                                // Part of me wonders... if it would still be worth hard coding values for everything but. That's a different day
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            // Just need a number count at this point.
                            // Check to see how many fish you've gotten, and how many is required from the mission
                        }
                    }
                    else if (missionEntry.Attributes.HasFlag(MissionAttributes.Critical))
                    {
                        // Need to just generally check to see if you've gotten enough fish to turnin from here.
                    }
                    else
                    {
                        // This is just a general score check from here. Check to see if
                        // -> You have met the minimul score t
                        // -> 
                    }
                }

            }
            else
            {
                // Addon wasn't visiable/ready. Opening it up.
                if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud))
                {
                    if (EzThrottler.Throttle("Opening the moon hud", 1000))
                    {
                        moonHud.Mission();
                        IceLogging.Info("Hud wasn't visible. Opening it", "[Score Check]");
                    }
                }
            }

            return false;
        }

        public static bool? CheckCraft()
        {
            return true;
        }

        public static bool? IniateFishing()
        {
            return true;
        }
    }
}
