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

            P.TaskManager.Enqueue(() => Task_CheckScore.Fish());
            P.TaskManager.Enqueue(() => CheckCraft());
            P.TaskManager.Enqueue(() => IniateFishing());

        }

        public static bool? CheckCraft()
        {
            return true;
        }

        public static bool? IniateFishing()
        {
            // Things to do here
            // Check to see if you currently have bait / have it equipped.

            if (CosmicHelper.CurrentBait == 0)
            {
                uint baitId = 0;
                var missionId = CosmicHelper.CurrentLunarMission;
                var mission = CosmicHelper.Dict_CosmicMissions[missionId];

                foreach (var bait in mission.FishingBaits)
                {
                    foreach (var baitIds in mission.FishingBaits)
                    {
                        PlayerHelper.GetItemCount(baitIds, out var count);
                        if (count > 0)
                        {
                            baitId = baitIds;
                            break;
                        }
                    }
                }

                if (EzThrottler.Throttle("Telling autohook to equip the bait by Id"))
                    P.AutoHook.SwapBaitById(baitId);
                return false;
            }
            else
            {
                // Ideally, we only tell it this once and then we never had to do it again. Problem is there's not *-really-* a good way of telling this. 
            }

            return true;
        }
    }
}
