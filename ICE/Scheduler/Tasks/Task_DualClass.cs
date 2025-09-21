using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Component.GUI;
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
            if (CosmicHelper.CurrentMissionInfo.Jobs.Contains(18))
            {
                // Need to insert the stuff for fishing dual class bullshit here
            }
            else
            {
                P.TaskManager.Enqueue(() => CheckMaterials(), "Checking dual class mission for enough items to craft");
                P.TaskManager.Enqueue(() => CheckGatherLocation(), "Checking for next gather location to head to");
                P.TaskManager.Enqueue(() => PathToNode(), "Pathing to the next node");
                P.TaskManager.Enqueue(() => NavmeshMovement(), "Telling navmesh to move");
            }
        }

        private static unsafe bool? CheckMaterials()
        {
            var id = CosmicHelper.CurrentLunarMission;
            var mission = CosmicHelper.SheetMissionDict[id];
            var crafterJobId = mission.Jobs.Where(x => CosmicHelper.CrafterJobList.Contains(x)).FirstOrDefault();
            var gatheringJobId = mission.Jobs.Where(x => CosmicHelper.GatheringJobList.Contains(x)).FirstOrDefault();

            if (P.TaskManager.IsBusy)
            {
                if (Player.JobId != crafterJobId)
                {
                    if (EzThrottler.Throttle("Swapping to crafter job"))
                        GearsetHandler.TaskClassChange((Job)crafterJobId);
                    return false;
                }

                IceLogging.Info("Need to wait for artisan to not be busy before continuing onwards");
                InsertArtisanWait();
                return true;
            }
            else
            {
                var mainCraft = mission.Crafts_Main.Values.FirstOrDefault();
                var recipeId = mission.Crafts_Main.Keys.FirstOrDefault();
                var itemId = mainCraft.ItemId;
                var gatherEntry = mainCraft.RequiredItems.FirstOrDefault();
                var gatheredItem = gatherEntry.Key;
                var gatheredAmount = gatherEntry.Value;

                var gatherProfileId = C.MissionConfig[id].GatherProfileId;
                var dualCraftAmount = C.GatherSettings[gatherProfileId].DualClassCraftAmount;

                if (PlayerHelper.GetItemCount(itemId, out var count) && count < dualCraftAmount)
                {
                    // We're missing items set to what is configured to the gathering profile. 
                    // So, going to check if we have enough for it. 
                    if (PlayerHelper.GetItemCount(gatheredItem, out var gatherItem) && gatheredItem >= (gatheredAmount * dualCraftAmount))
                    {
                        if (Svc.Condition[ConditionFlag.Gathering])
                        {
                            if (GenericHelpers.TryGetAddonByName("Gathering", out AtkUnitBase* gather) && GenericHelpers.IsAddonReady(gather))
                            {
                                if (EzThrottler.Throttle("Closing Gathering Window"))
                                    ECommons.Automation.Callback.Fire(gather, true, -1);
                            }
                        }
                        else if (Player.JobId != crafterJobId)
                        {
                            if (EzThrottler.Throttle("Swapping to crafter job"))
                                GearsetHandler.TaskClassChange((Job)crafterJobId);
                        }
                        else
                        {
                            // We have enough to craft. Telling it to craft the item... x amount of times
                            P.Artisan.CraftItem(recipeId, dualCraftAmount);
                            P.TaskManager.Tasks.Clear();
                            InsertArtisanWait();
                            return true;
                        }
                    }
                    else
                    {
                        if (Player.JobId != gatheringJobId)
                        {
                            if (EzThrottler.Throttle("Swapping to crafter job"))
                                GearsetHandler.TaskClassChange((Job)crafterJobId);
                        }
                        else
                        {
                            // We don't have enough for the inital crafting. 
                            // Time to gather
                            IceLogging.Info("WE SHOULD. BE ON A GATHERING CLASS | A");
                            IceLogging.Info("We don't have enough to gather, so going to continue onto the next step");
                            return true;
                        }
                    }
                }
                else
                {
                    // Initial dual craft amount has been met, but we still need to craft more to meet score. 
                    if (PlayerHelper.GetItemCount(gatheredItem, out var gatherItem) && gatheredItem >= (gatheredAmount))
                    {
                        if (Svc.Condition[ConditionFlag.Gathering])
                        {
                            if (GenericHelpers.TryGetAddonByName("Gathering", out AtkUnitBase* gather) && GenericHelpers.IsAddonReady(gather))
                            {
                                if (EzThrottler.Throttle("Closing Gathering Window"))
                                    ECommons.Automation.Callback.Fire(gather, true, -1);
                            }
                        }
                        else if (Player.JobId != crafterJobId)
                        {
                            if (EzThrottler.Throttle("Swapping to crafter job"))
                                GearsetHandler.TaskClassChange((Job)crafterJobId);
                        }
                        else
                        {
                            // We have enough to craft. Telling it to craft the item... x amount of times
                            IceLogging.Info($"Have enough to get an extra item crafted! Telling artisan to craft: Recipe ID: {recipeId} | ItemId: {itemId}");
                            P.Artisan.CraftItem(recipeId, 1);
                            P.TaskManager.Tasks.Clear();
                            InsertArtisanWait();
                            return true;
                        }
                    }
                    else
                    {
                        if (Player.JobId != gatheringJobId)
                        {
                            if (EzThrottler.Throttle("Swapping to crafter job"))
                                GearsetHandler.TaskClassChange((Job)crafterJobId);
                        }
                        else
                        {
                            // We don't have enough for the inital crafting. 
                            // Time to gather
                            IceLogging.Info("WE SHOULD. BE ON A GATHERING CLASS");
                            IceLogging.Info("We don't have enough to gather, so going to continue onto the next step");
                            return true;
                        }
                    }
                }
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
            var zoneId = Player.Territory;
            var missionEntry = CosmicHelper.CurrentMissionInfo;
            var missionFlag = missionEntry.MapPosition;
            var gatherInfo = GatheringUtil.MoonGatherLocations[zoneId][missionFlag];
            var location = gatherInfo[Mission_Settings.nodeCounter];

            if (EzThrottler.Throttle("Distance to node debugger"))
            {
                IceLogging.Debug($"Distance to node position: {Player.DistanceTo(location.Position)}");
            }

            if (!P.Navmesh.IsRunning() && Player.DistanceTo(location.Position) <= 3)
            {
                // Time to check to see if the node is targetable 
                if (Svc.Condition[ConditionFlag.Gathering])
                {
                    P.TaskManager.Insert(() => GatheringInteraction(), "Gathering mode", Utils.TaskConfig);
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
    }
}
