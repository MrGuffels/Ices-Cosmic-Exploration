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
            if (P.Artisan.IsBusy())
            {
                InsertArtisanWait();
                P.TaskManager.Enqueue(() => Task_CheckScore.Crafts(), "Checking score");
            }
            else
            {
                P.TaskManager.Enqueue(() => Task_CheckScore.Enqueue(), "Checking Score");
                P.TaskManager.EnqueueDelay(1000);
                P.TaskManager.Enqueue(() => CheckMaterials(), "Checking materials");
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
        private static void InsertArtisanWait()
        {
            P.TaskManager.Insert(() => WaitingForArtisan(), "Waiting for artisan to finish", Utils.TaskConfig);
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
                var mainCraft = mission.Crafts_Main.FirstOrDefault();

                var preItemId = preCraft.Value.ItemId;
                var mainItemId = mainCraft.Value.ItemId;

                PlayerHelper.GetItemCount(preCraft.Value.ItemId, out var preItemAmount);
                PlayerHelper.GetItemCount(preCraft.Value.RequiredItems.FirstOrDefault().Key, out var moonCrateCount);
                PlayerHelper.GetItemCount(mainCraft.Value.ItemId, out var mainItemCount);

                if (preItemAmount >= mainCraft.Value.RequiredItems[preItemId])
                {
                    IceLogging.Info($"Required pre-Item count: {mainCraft.Value.RequiredItems[preItemId]} | amount necessary: {preItemAmount}");

                    // There's enough items to craft the mainhand. Telling it to craft it instead. 
                    if (mainItemCount < mainCraft.Value.RequiredAmount)
                    {
                        // you don't have enough of the pre-crafts to craft the main item. 
                        // going to tell artisan to just kick it into gear
                        var craftAmount = mainCraft.Value.RequiredAmount - mainItemCount;
                        P.Artisan.CraftItem(mainCraft.Key, craftAmount);
                        InsertArtisanWait();
                        return true;
                    }
                    else
                    {
                        // you have enough of the main hand item. But you still are crafting. So time to just craft 1 more
                        P.Artisan.CraftItem(mainCraft.Key, 1);
                        InsertArtisanWait();
                        return true;
                    }

                }
                else if (moonCrateCount >= preCraft.Value.RequiredAmount)
                {
                    // You should have enough to make this pre-craft. Initiating the thing now.
                    var craftAmount = preCraft.Value.RequiredAmount - preItemAmount;

                    if (mainCraft.Value.RequiredAmount > 1 && mainItemCount == 0)
                    {
                        craftAmount = mainCraft.Value.RequiredAmount * (preCraft.Value.RequiredAmount - preItemAmount);
                    }
                    if (craftAmount < 1)
                        craftAmount = 1;

                    P.Artisan.CraftItem(preCraft.Key, craftAmount);
                    InsertArtisanWait();
                    IceLogging.Debug($"Found a material that still needed to be crafted");
                    return true;
                }
                else
                {
                    IceLogging.Debug($"Somehow, out of mats. Need to exit. And either attempt to turnin, or just straight up abandon.", "[Task Craft: Check Materials]");
                    SchedulerMain.State = IceState.AbandonMission;
                    return true;
                }
            }
            else
            {
                // This is the case when you need multiple items, or even just a single item.
                foreach (var craft in mission.Crafts_Main)
                {
                    PlayerHelper.GetItemCount(craft.Value.ItemId, out var reqAmount);
                    if (reqAmount < craft.Value.RequiredAmount)
                    {
                        // If you need less than what is necessary, this should change the count to be proper
                        reqAmount = craft.Value.RequiredAmount - reqAmount;

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
                            IceLogging.Debug("You have no remaining items to craft the main crafting items. Going to abandon the mission now");
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
                    IceLogging.Debug("You have no remaining items to craft the pre-crafts. Going to abandon the mission now");
                    P.TaskManager.Tasks.Clear();
                    return true;
                }

            }

            return false;
        }
    }
}
