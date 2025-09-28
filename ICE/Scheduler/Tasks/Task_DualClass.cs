using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ICE.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_DualClass
    {
        public static void Enqueue()
        {
            Task_CheckScore.Enqueue();
            P.TaskManager.Enqueue(() => CheckMaterials(), "Checking status for dual craft missions");
        }

        private static unsafe bool? CheckMaterials()
        {
            var id = CosmicHelper.CurrentLunarMission;
            var mission = CosmicHelper.SheetMissionDict[id];
            var missionConfig = C.MissionConfig[id];
            var crafterJobId = mission.Jobs.Where(x => CosmicHelper.CrafterJobList.Contains(x)).FirstOrDefault();
            var gatheringJobId = mission.Jobs.Where(x => CosmicHelper.GatheringJobList.Contains(x)).FirstOrDefault();

            // Should only be one of these existing here for dual crafts.
            var mainCraft = mission.Crafts_Main.Values.FirstOrDefault();
            var recipeId = mission.Crafts_Main.Keys.FirstOrDefault();
            var itemId = mainCraft.ItemId;

            var gatherProfileId = C.MissionConfig[id].GatherProfileId;
            var dualCraftAmount = 3;


            if (missionConfig.TurninGold || missionConfig.AutoTurnin)
            {
                // Currently, gold needs x3 of the items
                dualCraftAmount = 3;
            }
            else if (missionConfig.TurninSilver)
            {
                // Currently, silver needs x2 of the items
                dualCraftAmount = 2;
            }
            if (PlayerHelper.GetItemCount(mainCraft.ItemId, out var craftAmount) && craftAmount >= dualCraftAmount)
            {
                // Anything extra after we initial crafts, really just want to only craft x1
                dualCraftAmount = 1;
            }

            foreach (var requiredItem in mainCraft.RequiredItems)
            {
                uint crateId = 48233;

                var materialItemId = requiredItem.Key;
                var amountNeeded = requiredItem.Value;

                if (materialItemId == crateId && PlayerHelper.GetItemCount(crateId, out var crateAmount) && crateAmount == 0)
                {
                    // This means we've ran out of crates. Going to just exit out and abandon
                    SchedulerMain.State = IceState.AbandonMission;
                    P.TaskManager.Tasks.Clear();
                    return true;
                }
                else
                {
                    if (PlayerHelper.GetItemCount(itemId, out var mainItemCount) && mainItemCount < dualCraftAmount)
                    {
                        amountNeeded = amountNeeded * dualCraftAmount;
                    }

                    if (PlayerHelper.GetItemCount(materialItemId, out var gatherAmount) && gatherAmount < amountNeeded)
                    {
                        // We don't have enough to craft. So going to actually exit out and enter the gathering state.
                        IceLogging.Info("We don't have enough to craft the dual class item, so proceeding to gather");
                        P.TaskManager.Tasks.Clear();
                        P.TaskManager.Insert(() => CheckGatheringState(), "Checking the current state of gathering");
                        return true;
                    }
                }
            }

            // If we've gotten this far. That means that we *-should-* be able to craft
            // Order of operations will be:
            // 1: If we're in the middle of a gathering node, need to exit out of it and wait for us to not be gathering/busy
            // 2: Swap to the crafter (if we're not there already)
            // 3: Tell Artisan to craft
            // 4: Wait for Artisan to not be busy

            if (Svc.Condition[ConditionFlag.Gathering])
            {
                if (GenericHelpers.TryGetAddonByName("Gathering", out AtkUnitBase* gather) && GenericHelpers.IsAddonReady(gather))
                {
                    if (EzThrottler.Throttle("Closing Gathering Window"))
                        ECommons.Automation.Callback.Fire(gather, true, -1);
                }
                else if (Player.JobId == 18)
                {
                    StopFishing();
                }
                return false;
            }

            if (Player.JobId != crafterJobId)
            {
                if (EzThrottler.Throttle("Swapping to crafter job"))
                    GearsetHandler.TaskClassChange((Job)crafterJobId);
                return false;
            }

            if (PlayerHelper.GetItemCount(itemId, out var count) && count < dualCraftAmount)
            {
                // We have enough to craft. Telling it to craft the item... x amount of times
                P.Artisan.CraftItem(recipeId, dualCraftAmount);
                P.TaskManager.Tasks.Clear();
                InsertArtisanWait();
                return true;
            }
            else
            {
                // We have enough for atleast 1 more craft, telling artisan to craft. Uno mas.
                P.Artisan.CraftItem(recipeId, 1);
                P.TaskManager.Tasks.Clear();
                InsertArtisanWait();
                return true;
            }
        }

        private static unsafe bool? CheckGatheringState()
        {
            IceLogging.Debug("Starting 'Check Gather State'");

            var id = CosmicHelper.CurrentLunarMission;
            var mission = CosmicHelper.SheetMissionDict[id];
            var crafterJobId = mission.Jobs.Where(x => CosmicHelper.CrafterJobList.Contains(x)).FirstOrDefault();
            var gatheringJobId = mission.Jobs.Where(x => CosmicHelper.GatheringJobList.Contains(x)).FirstOrDefault();

            if (Svc.Condition[ConditionFlag.Crafting] || (Svc.Condition[ConditionFlag.PreparingToCraft]))
            {
                if (GenericHelpers.TryGetAddonByName("WKSRecipeNotebook", out AtkUnitBase* moonCraft) && GenericHelpers.IsAddonReady(moonCraft))
                {
                    if (EzThrottler.Throttle("Exiting out of the gathering state"))
                        ECommons.Automation.Callback.Fire(moonCraft, true, -1);
                }
                return false;
            }
            else if (GenericHelpers.TryGetAddonMaster<Gathering>("Gathering", out var gatheringAddon))
            {
                IceLogging.Info($"We're currently in the middle of gathering, so going to just swap over to interacting with the gathering node");
                P.TaskManager.Enqueue(() => GatheringInteraction(), "Interacting with the gathering node");
                return true;
            }
            else if (Player.JobId != gatheringJobId)
            {
                if (EzThrottler.Throttle("Swapping to crafter job"))
                    GearsetHandler.TaskClassChange((Job)gatheringJobId);
                return false;
            }
            else if (Player.JobId == 16 || Player.JobId == 17)
            {
                bool selfRepairGather = C.SelfRepairGather && PlayerHelper.NeedsRepair(C.RepairPercent);

                if (selfRepairGather)
                {
                    IceLogging.Info("You have enabled self repair, and you are need in repair. throwing in task to repair self", "[Task_DualClass | Check Gather State]");
                    P.TaskManager.EnqueueMulti
                    (
                        new(Task_Repair.OpenSelfRepair, "Opening the self repair window"),
                        new(Task_Repair.SelfRepair, "Executing the self repair"),
                        new(Task_Repair.CloseRepair, "Closing Self Repair")
                    );
                }

                // Us getting here means that we're fresh into the node gathering. So just going to queue up the rest of the gathering process.
                IceLogging.Info("You've gotten to this point so. Queueing up checking the gathering location, pathing to node, and navmesh movement", "[Task_DualClass | Check Gather State]");
                P.TaskManager.Enqueue(() => CheckGatherLocation(), "Checking Gathering Location Info");
                P.TaskManager.Enqueue(() => PathToNode(), "Pathing to the gathering node");
                P.TaskManager.Enqueue(() => NavmeshMovement(), "Navmesh moving to the node, then checking for targetability");
                return true;
            }
            else if (Player.JobId == 18)
            {
                bool selfRepairGather = C.SelfRepairGather && PlayerHelper.NeedsRepair(C.RepairPercent);

                if (selfRepairGather)
                {
                    IceLogging.Info("You have enabled self repair, and you are need in repair. throwing in task to repair self", "[Task_DualClass | Check Gather State]");
                    P.TaskManager.EnqueueMulti
                    (
                        new(Task_Repair.OpenSelfRepair, "Opening the self repair window"),
                        new(Task_Repair.SelfRepair, "Executing the self repair"),
                        new(Task_Repair.CloseRepair, "Closing Self Repair")
                    );
                }

                P.TaskManager.Enqueue(() => FishingCheck(), "Checking to see what to fish for");
                return true;
            }

            return false;
        }

        private static bool? WaitingForArtisan()
        {
            if (!P.Artisan.IsBusy())
            {
                IceLogging.Info("Artisan is no longer running, continuing the process");
                P.TaskManager.Tasks.Clear();
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
                if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud))
                {
                    if (!AddonHelper.IsAddonActive("WKSMissionInfomation"))
                    {
                        if (EzThrottler.Throttle("Opening mission scoring info"))
                        {
                            moonHud.Mission();
                        }
                    }
                }
            }
            return false;
        }
        private static void InsertArtisanWait()
        {
            P.TaskManager.Insert(() => WaitingForArtisan(), "Waiting for artisan to finish", Utils.TaskConfig);
        }

        #region Gathering

        public static bool? CheckGatherLocation()
        {
            var zoneId = Player.Territory;
            var missionEntry = CosmicHelper.CurrentMissionInfo;
            var missionFlag = missionEntry.MapPosition;
            var gatherInfo = GatheringUtil.MoonGatherLocations[zoneId][missionFlag];
            if (Mission_Settings.previousMap != missionFlag)
            {
                Mission_Settings.previousMap = missionFlag;
                Mission_Settings.nodeCounter = 0;
            }
            else
            {
                var nodeId = gatherInfo[Mission_Settings.nodeCounter].NodeId;
                var node = Svc.Objects.Where(x => x.DataId == nodeId).FirstOrDefault();
                if (!node.IsTargetable)
                {
                    Mission_Settings.nodeCounter += 1;
                    Mission_Settings.nodeTotal += 1;
                }
            }

            IceLogging.Debug("Task Complete", "[Gathering: Check Gather Location]");
            return true;
        }
        private static bool? PathToNode()
        {
            if (P.Navmesh.IsRunning())
            {
                IceLogging.Info("Pathing to the gathering node has now started");
                return true;
            }
            else
            {
                var zoneId = Player.Territory;
                var missionEntry = CosmicHelper.CurrentMissionInfo;
                var missionFlag = missionEntry.MapPosition;
                var gatherInfo = GatheringUtil.MoonGatherLocations[zoneId][missionFlag];

                if (gatherInfo.Count-1 < Mission_Settings.nodeCounter)
                {
                    // Counter has hit the max capacity it can for this particular nodeset, resetting back to 0
                    Mission_Settings.nodeCounter = 0;
                }

                var location = gatherInfo[Mission_Settings.nodeCounter];
                if (location == null)
                {
                    IceLogging.Error("Somehow we ended up out of the bounds of the index. Stopping plugin");
                    SchedulerMain.DisablePlugin();
                }
                else
                {
                    if (EzThrottler.Throttle("Enabling pathfinding to navmesh"))
                    {
                        IceLogging.Debug($"Telling Navmesh to path to: {location.LandZone}", "[Gathering: Navmesh moveto]");
                        P.Navmesh.PathfindAndMoveTo(location.LandZone, false);
                    }
                }
            }

            return false;
        }
        private static bool? NavmeshMovement()
        {
            var missionEntry = CosmicHelper.CurrentMissionInfo;
            var zoneId = missionEntry.TerritoryId;
            var missionFlag = missionEntry.MapPosition;
            var gatherInfo = GatheringUtil.MoonGatherLocations[zoneId][missionFlag];
            var location = gatherInfo[Mission_Settings.nodeCounter];

            if (EzThrottler.Throttle("Distance to node debugger"))
            {
                IceLogging.Debug($"Distance to node position: {Player.DistanceTo(location.Position)}");
            }

            if (!P.Navmesh.IsRunning() && Player.DistanceTo(location.Position) <= 4)
            {
                // Time to check to see if the node is targetable 
                if (Svc.Condition[ConditionFlag.Gathering])
                {
                    P.TaskManager.Insert(() => GatheringInteraction(), "Gathering mode", Utils.TaskConfig);
                    return true;
                }
                else if (Svc.Objects.Where(x => x.DataId == location.NodeId).Where(t => t.IsTargetable) != null)
                {
                    // Target was a valid target, going to add a task to try and interact w/ the node now and get the gathering window up
                    IceLogging.Info("Targeting the target for gathering", "[Task_Gathering]");
                    P.TaskManager.Insert(() => OpenGatheringMenu(), "Opening the gathering menu");
                    return true;
                }
                else
                {
                    // No valid target was found. Going to continue onward to the next node. 
                    IceLogging.Info("No valid target was found for gathering, increasing counter", "[Task_Gathering]");
                    Mission_Settings.nodeCounter++;
                    return true;
                }

            }
            else if (P.Navmesh.IsRunning())
            {
                if (C.UseMountInMission && (Player.DistanceTo(location.Position) > C.MountRadius))
                {
                    if (!Player.Mounted && !Player.Mounting)
                    {
                        if (EzThrottler.Throttle("Mounting for mission"))
                        {
                            IceLogging.Debug($"Distance to node: {Player.DistanceTo(location.Position)} | Mount checking says you should mount so... mounting", "[Gather Task: Pathfind]");
                            Utils.MountAction();
                        }
                    }
                }
                else if (Player.Mounted && (Player.DistanceTo(location.Position) < C.DismountRadius))
                {
                    if (EzThrottler.Throttle("Dismounting mount in mission"))
                    {
                        IceLogging.Debug($"Distance to node: {Player.DistanceTo(location.Position)} | Mount checking says you should not be on a mount, dismounting", "[Gather Task: Pathfind]");
                        Utils.Dismount();
                    }
                }
            }

            return false;
        }
        private static bool? OpenGatheringMenu()
        {
            var zoneId = Player.Territory;
            var missionEntry = CosmicHelper.CurrentMissionInfo;
            var missionFlag = missionEntry.MapPosition;
            var gatherInfo = GatheringUtil.MoonGatherLocations[zoneId][missionFlag];
            var location = gatherInfo[Mission_Settings.nodeCounter];

            if (CosmicHandler.IsMissionTimedOut())
            {
                IceLogging.Info($"We've managed to time out the mission. Going to attempt to turnin, and abandon if not", "[Gathering: Open Gathering Menu]");
                SchedulerMain.State = IceState.AbandonMission;
                P.TaskManager.Tasks.Clear();
                return true;
            }
            else if (Svc.Condition[ConditionFlag.Gathering] && GenericHelpers.TryGetAddonMaster<Gathering>("Gathering", out var gather) && gather.IsAddonReady || GenericHelpers.TryGetAddonMaster<GatheringMasterpiece>("GatheringMasterpiece", out var collectable) && collectable.IsAddonReady)
            {
                Mission_Settings.CollectableStep = 0;

                IceLogging.Info($"Gathering window is now visible, continuing onto GatheringInteraction Task", "[Gathering: OpenGatheringMenu]");
                P.TaskManager.Insert(() => GatheringInteraction(), "Gathering at the node", Utils.TaskConfig);
                return true;
            }
            else
            {
                Utils.TryGetObjectByDataId(location.NodeId, out var node);
                if (node != null && !Player.IsJumping)
                {
                    if (node.IsTargetable)
                    {
                        if (EzThrottler.Throttle("Target + Interacting w/ node"))
                        {
                            Utils.TargetgameObject(node);
                            Utils.InteractWithObject(node);
                        }
                    }
                    else
                    {
                        // Node doesn't exist/isn't targetable. 
                        IceLogging.Info($"The current node doesn't exist, continuing onto the next", "[Gathering: OpenGatheringMenu]");
                        return true;
                    }
                }
            }

            return false;
        }
        public static unsafe bool? GatheringInteraction()
        {
            var missionInfo = CosmicHelper.CurrentMissionInfo;
            bool collectableItem = missionInfo.Attributes.HasFlag(MissionAttributes.Collectables);
            bool reduceItems = missionInfo.Attributes.HasFlag(MissionAttributes.ReducedItems);
            var configId = C.MissionConfig[CosmicHelper.CurrentLunarMission].GatherProfileId;
            var gatherConfig = C.GatherSettings[configId];
            var gathActions = GatheringUtil.GathActionDict;

            var collectorBuffs = GatheringUtil.GathCollectableBuffs;
            var collectorAction = GatheringUtil.GathCollectableActions;
            var jobId = Player.JobId;

            if (P.Navmesh.IsRunning())
            {
                if (EzThrottler.Throttle("Stopping navmesh, cause we shouldn't be running here"))
                    P.Navmesh.Stop();
            }

            if (Svc.Condition[ConditionFlag.Gathering])
            {
                // This should always be true while either
                // -> Enter gathering window
                // -> Using Actions
                // -> Gathering item
                // -> Exiting the gathering state.

                if (!Svc.Condition[ConditionFlag.ExecutingGatheringAction])
                {
                    // We don't want to try and execute another action while we're currently in the middle of one

                    if (GenericHelpers.TryGetAddonMaster<Gathering>("Gathering", out var gather) && gather.IsAddonReady)
                    {
                        // this is the first window that you see. 
                        if (gather.CurrentIntegrity != 0)
                        {
                            foreach (var action in Mission_Settings.SkillUseAmount)
                            {
                                var key = action.Key;
                                var amount = action.Value;
                                bool missingDur = gather.CurrentIntegrity != gather.TotalIntegrity;
                                int boonChance = gather.GatheredItems.FirstOrDefault().BoonChance;

                                bool useBuff = Task_Gather.CanUseGatheringAction(key, configId, missingDur, boonChance);
                                if (EzThrottler.Throttle($"Checking buff: {key}"))
                                {
                                    IceLogging.Debug($"Action name: {key} | Using? {useBuff}");
                                }
                                if (useBuff)
                                {
                                    if (EzThrottler.Throttle($"Using Gathering Action: {key}"))
                                    {
                                        IceLogging.Debug($"Using the following action: {key} in full durability section", debugOnly: true);
                                        var actionId = gathActions[key].ClassAction[jobId].ActionId;
                                        ActionManager.Instance()->UseAction(ActionType.Action, actionId);
                                        Mission_Settings.SkillUseAmount[key] += 1;
                                    }
                                    return false;
                                }
                            }

                            if (EzThrottler.Throttle("Gathering item for score"))
                            {
                                // if we're here, then we just need to gather for score. So... gathering for score lol
                                gather.GatheredItems.Where(x => x.ItemID != 0).FirstOrDefault().Gather();
                            }
                        }
                        else
                        {
                            // No more integrity is left, time to just wait for you to stop gathering
                        }
                    }
                }
                else
                {
                    P.TaskManager.Insert(() => WaitToGather());
                    return true;
                }
            }
            else
            {
                // No longer gathering an item. Time to check current state
                return true;
            }

            return false;
        }
        private static bool? WaitToGather()
        {
            if (!Svc.Condition[ConditionFlag.ExecutingGatheringAction])
            {
                IceLogging.Info("No longer executing a gathering action", "[Task Gather: Wait To Gather]");
                return true;
            }

            return false;
        }

        #endregion

        #region Fishing

        private static unsafe bool? FishingCheck()
        {
            if (!Svc.Condition[ConditionFlag.Gathering])
            {
                if (EzThrottler.Throttle("Starting to fish", 1000))
                {
                    ActionManager.Instance()->UseAction(ActionType.Action, 289);
                }
                return false;
            }
            else
            {
                // Means we are fishing, all we need to do is enable autohook then wait for us to get the amount of fish we need
                P.AutoHook.SetState(true);
                P.TaskManager.Insert(() => CheckItems(), "Checking for items to meet the quantity set", Utils.TaskConfig);
                return true;
            }
        }

        private static unsafe bool? CheckItems()
        {
            if (!Svc.Condition[ConditionFlag.Gathering])
            {
                IceLogging.Info("We've stopped fishing for some reason... going to go back and check if we have enough of the materials, or just ran out of bait");
                P.TaskManager.Tasks.Clear();
                return true;
            }
            else
            {
                // Currently in the middle of gathering here. Going to check for just general item progress.
                var mission = CosmicHelper.CurrentMissionInfo;
                if (EzThrottler.Throttle("Checking for current item count"))
                {
                    var mainCraft = mission.Crafts_Main.Values.FirstOrDefault();
                    var missionConfig = C.MissionConfig[CosmicHelper.CurrentLunarMission];

                    // Need to really figure out what all we need... 
                    // If going for gold, should really only need x3 of the item
                    // If going for silver, should really only need x2 of the item
                    // Once we get the initial multiplier amount, should only go for x1 more for the crafts

                    var itemAmount = 1;
                    if (missionConfig.TurninGold || missionConfig.AutoTurnin)
                    {
                        // Currently, gold needs x3 of the items
                        itemAmount = 3;
                    }
                    else if (missionConfig.TurninSilver)
                    {
                        // Currently, silver needs x2 of the items
                        itemAmount = 2;
                    }
                    if (PlayerHelper.GetItemCount(mainCraft.ItemId, out var craftAmount) && craftAmount >= itemAmount)
                    {
                        // Anything extra after we initial crafts, really just want to only craft x1
                        itemAmount = 1;
                    }
                    if (EzThrottler.Throttle("Item Multiplier Amount", 5000))
                    {
                        IceLogging.Info($"[Fishing] Item Multiplier: {itemAmount}", debugOnly: true);
                    }

                    foreach (var requiredItem in mainCraft.RequiredItems)
                    {
                        uint crateId = 48233;
                        var materialItemId = requiredItem.Key;
                        var amountNeeded = requiredItem.Value;
                        if (materialItemId == crateId) continue;
                        amountNeeded = amountNeeded * itemAmount; // This is to make sure the multiplier is there if needing to craft multiple (aka if 7 * 3 for gold)

                        if (PlayerHelper.GetItemCount(materialItemId, out var mainItemCount) && mainItemCount < amountNeeded)
                        {
                            // Still don't have enough items, so continuing on
                        }
                        else
                        {
                            IceLogging.Info($"Current Amount: {mainItemCount} | Amount needed: {amountNeeded} is complete", debugOnly: true);
                            StopFishing();
                        }
                    }
                }

                return false;
            }
        }

        private static unsafe void StopFishing()
        {
            if (EzThrottler.Throttle("Telling it to stop fishing"))
            {
                P.AutoHook.SetState(false);
                ActionManager.Instance()->UseAction(ActionType.Action, 299);
            }
        }

        #endregion
    }
}