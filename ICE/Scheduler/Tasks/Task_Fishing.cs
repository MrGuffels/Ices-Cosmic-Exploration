using Dalamud.Game.ClientState.Conditions;
using FFXIVClientStructs.FFXIV.Client.Game;
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
            P.TaskManager.Enqueue(() => FishingCheck());
        }

        private static unsafe bool? FishingCheck()
        {
            string handle = "[Standard Fishing: Fishing Check]";
            IceLogging.Info("Checking to see where we need to be here", handle);

            if (CosmicHelper.CurrentBait == 0)
            {
                if (EzThrottler.Throttle("Equipping bait"))
                {
                    foreach (var bait in GatheringUtil.MoonBaits)
                    {
                        foreach (var baitId in bait.Value)
                        {
                            if (PlayerHelper.GetItemCount(baitId, out var count) && count > 0)
                            {
                                P.AutoHook.SwapBaitById(baitId);
                                IceLogging.Debug($"Telling it to equip bait ID: {baitId}", handle);
                                return false;
                            }
                        }
                    }

                    IceLogging.Info("If we've gotten here, that means we're out of bait. Proceeding to turnin/abandon the mission");
                    SchedulerMain.State = IceState.AbandonMission;
                    P.TaskManager.Tasks.Clear();
                    return true;
                }
                return false;
            }
            else if (!Svc.Condition[ConditionFlag.Gathering])
            {
                if (EzThrottler.Throttle("Starting to fish", 1000))
                {
                    IceLogging.Debug("Telling it to start fishing", handle);
                    ActionManager.Instance()->UseAction(ActionType.Action, 289);
                }
                return false;
            }
            else
            {
                // Means we are fishing, all we need to do is enable autohook then wait for us to get the amount of fish we need
                P.AutoHook.SetState(true);
                IceLogging.Info("We're starting to fish. So kicking it over to checking the fish items", handle);
                P.TaskManager.Insert(() => WaitToStartFishing(), "Waiting till we actually start fishing", Utils.TaskConfig);
                return true;
            }
        }

        private static unsafe bool? WaitToStartFishing()
        {
            if (Svc.Condition[ConditionFlag.Fishing])
            {
                IceLogging.Info("We've started fishing, just going to wait for it to finish", "[Fishing: Waiting]");
                P.TaskManager.Insert(() => FinishFishing(), "Waiting for fishing to finish", Utils.TaskConfig);
                return true;
            }
            if (!Svc.Condition[ConditionFlag.Gathering])
            {
                IceLogging.Info("Somehow, we've gotten here and we're not fishing?? Going to check the score for a timer check", "[Fishing: Waiting]");
                P.TaskManager.Tasks.Clear();
                return true;
            }

            return false;
        }

        private static unsafe bool? FinishFishing()
        {
            if (!Svc.Condition[ConditionFlag.Fishing])
            {
                IceLogging.Info("We're done fishing, time to go back to the score check", "[Fishing: Finished]");
                return true;
            }

            return false;
        }
    }
}
