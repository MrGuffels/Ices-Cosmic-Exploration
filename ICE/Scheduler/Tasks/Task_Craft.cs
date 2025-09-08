using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_Craft
    {
        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => Task_CheckScore.Crafts());
            if (P.Artisan.IsBusy())
            {
                P.TaskManager.Enqueue(() => WaitingForArtisan(), Utils.TaskConfig);
            }
            else
            {
                P.TaskManager.Enqueue(() => CheckMaterials());
            }
        }

        private static bool? WaitingForArtisan()
        {
            if (!P.Artisan.IsBusy())
            {
                IceLogging.Debug("Artisan is no longer running, continuing the process");
                return true;
            }
            else
            {
                if (Svc.Condition[ConditionFlag.ExecutingCraftingAction])
                {
                    // Need to add a timer check here. Make it configuarable maybe... 10s?
                    // If the timer exceeds 10 seconds, then that means we're stuck in an animation lock
                    // then need to cancel them all and just force abandon lock failsafe
                }
            }
            return false;
        }

        private static bool? CheckMaterials()
        {
            var id = CosmicHelper.CurrentLunarMission;
            // var mission = CosmicHelper.Dict_CosmicMissions[id];
               var mission = CosmicHelper.SheetMissionDict[id];

            if (mission.Crafts_Pre.Count > 0)
            {
                // Mission has pre-crafts that are required. 
                // Checking to see if you have enough pre-crafts first
                var preCraft = mission.Crafts_Pre.FirstOrDefault();
                PlayerHelper.GetItemCount(preCraft.Value.ItemId, out var preAmount);
                if (preAmount >= preCraft.Value.Amount)
                {
                    // You have enough of the precrafts to make the item, time to make the actual item.
                    var mainItem = mission.Crafts_Main.FirstOrDefault();
                    P.Artisan.CraftItem(mainItem.Key, mainItem.Value.Amount);
                    P.TaskManager.Insert(() => WaitingForArtisan());
                    return true;
                }
                else
                {
                    // Not enough of the pre-crafts to make the actual item. Check to see if we have the base material
                    var craftMaterial = ExcelHelper.RecipeSheet.GetRow(preCraft.Key).Ingredient[0].RowId;
                    PlayerHelper.GetItemCount(craftMaterial, out var count);

                    if (count >= preCraft.Value.Amount)
                    {
                        // You have enough to craft the precrafts atleast. Going to do that now
                        P.Artisan.CraftItem(preCraft.Key, preCraft.Value.Amount);
                        P.TaskManager.Insert(() => WaitingForArtisan(), Utils.TaskConfig);
                    }
                    else
                    {
                        // You've ran out of materials at this point, which means that we can't continue on. 
                        // Going to just check if we should straight up quit for `Out of Materials` or if we should try again
                        SchedulerMain.State = IceState.AbandonMission;
                    }
                }
            }
            else
            {
                // This is the case when you need multiple items, or even just a single item.
                foreach (var craft in mission.Crafts_Main)
                {
                    PlayerHelper.GetItemCount(craft.Value.ItemId, out var reqAmount);
                    if (reqAmount < craft.Value.Amount)
                    {
                        // If you need less than what is necessary, this should change the count to be proper
                        reqAmount = craft.Value.Amount - reqAmount;

                        // Found an item that needs to be crafted. Time to check if you have enough of the material
                        var craftMaterial = ExcelHelper.RecipeSheet.GetRow(craft.Key).Ingredient[0].RowId;
                        if (PlayerHelper.GetItemCount(craftMaterial, out var itemAmount) && itemAmount >= reqAmount)
                        {
                            P.Artisan.CraftItem(craft.Key, reqAmount);
                            P.TaskManager.Insert(() => WaitingForArtisan(), Utils.TaskConfig);
                            return true;
                        }
                        else
                        {
                            // You don't have enough to craft this for the mission. Exiting out and checking for score/force abandon
                            SchedulerMain.State = IceState.AbandonMission;
                            P.TaskManager.Tasks.Clear();
                            return true;
                        }
                    }
                }

                var moreCraft = mission.Crafts_Main.FirstOrDefault();
                // If you've gotten this far, that means you still need scoring. Just going to queue up the first mission (if possible)
                // If you need less than what is necessary, this should change the count to be proper
                var AdditionalItem = 1;

                // Found an item that needs to be crafted. Time to check if you have enough of the material
                var moreCraftMaterial = ExcelHelper.RecipeSheet.GetRow(moreCraft.Key).Ingredient[0].RowId;
                if (PlayerHelper.GetItemCount(moreCraftMaterial, out var moreItemAmount) && moreItemAmount >= AdditionalItem)
                {
                    P.Artisan.CraftItem(moreCraft.Key, AdditionalItem);
                    P.TaskManager.Insert(() => WaitingForArtisan(), Utils.TaskConfig);
                    return true;
                }
                else
                {
                    // You don't have enough to craft this for the mission. Exiting out and checking for score/force abandon
                    SchedulerMain.State = IceState.AbandonMission;
                    P.TaskManager.Tasks.Clear();
                    return true;
                }

            }

            return false;
        }
    }
}
