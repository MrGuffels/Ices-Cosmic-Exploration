using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Collections.Generic;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_Gather
    {

        public static void Enqueue()
        {
            if (GenericHelpers.TryGetAddonMaster<Gathering>("Gathering", out var gather) && gather.IsAddonReady 
             || GenericHelpers.TryGetAddonMaster<GatheringMasterpiece>("GatheringMasterpiece", out var collectable) && collectable.IsAddonReady)
            {
                IceLogging.Debug("Current in a gathering session");
                Task_CheckScore.Enqueue();
                P.TaskManager.Enqueue(() => GatheringInteraction(), Utils.TaskConfig);
            }
            else
            {
                IceLogging.Debug("Not currently gathering, starting fresh instead");
                Task_CheckScore.Enqueue();
                P.TaskManager.Enqueue(() => CheckReduceMission(), "Checking to see if we need to reduce items");
                P.TaskManager.Enqueue(() => Mission_Settings.ResetCollectableState());
                Task_CheckScore.Enqueue();
                P.TaskManager.Enqueue(() => UseCordial(), "Checking to see if we should use cordial or not");
                P.TaskManager.Enqueue(() => CheckGatherLocation(), "Checking to see if gathering flags needs updated");
                P.TaskManager.Enqueue(() => PathToNode());
                P.TaskManager.Enqueue(() => NavmeshMovement());
            }
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
                UseCordial();
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
                            if (collectableItem || reduceItems)
                            {
                                // no buffs are needed to apply before we go into the collectable window
                                foreach (var item in gather.GatheredItems)
                                {
                                    if (item.IsCollectable)
                                    {
                                        if (EzThrottler.Throttle("Swapping to collectable menu"))
                                        {
                                            item.Gather();
                                            Mission_Settings.item_collectableId = item.ItemID;

                                            if (PlayerHelper.GetGp() >= 400)
                                            {
                                                Mission_Settings.SelectedRotation = 1;
                                            }
                                            else
                                            {
                                                Mission_Settings.SelectedRotation = 0;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var action in Mission_Settings.SkillUseAmount)
                                {
                                    var key = action.Key;
                                    var amount = action.Value;
                                    bool missingDur = gather.CurrentIntegrity != gather.TotalIntegrity;
                                    int boonChance = gather.GatheredItems.FirstOrDefault().BoonChance;

                                    bool useBuff = CanUseGatheringAction(key, configId, missingDur, boonChance);
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

                                foreach (var item in CosmicHelper.CurrentMissionInfo.Gathering_Min.OrderByDescending(x => x.Value))
                                {
                                    if (PlayerHelper.GetItemCount(item.Key, out var count) && count < item.Value)
                                    {
                                        if (EzThrottler.Throttle("Gathering Item"))
                                        {
                                            gather.GatheredItems.Where(x => x.ItemID == item.Key).FirstOrDefault().Gather();
                                        }
                                        return false;
                                    }
                                }

                                if (EzThrottler.Throttle("Gathering item for score"))
                                {
                                    // if we're here, then we just need to gather for score. So... gathering for score lol
                                    gather.GatheredItems.Where(x => x.ItemID != 0).FirstOrDefault().Gather();
                                }
                                return false;
                            }
                        }
                        else
                        {
                            // No more integrity is left, time to just wait for you to stop gathering
                        }
                    }
                    else if (GenericHelpers.TryGetAddonMaster<GatheringMasterpiece>("GatheringMasterpiece", out var collectable) && collectable.IsAddonReady)
                    {
                        // Specifically for gathering collectables at the nodes (this also includes the collectables -> reducables... ugh)
                        var currentQuality = collectable.CurrentCollectability;
                        var minQuality = collectable.MinCollectability;
                        var midQuality = collectable.MidCollectability;
                        var highQuality = collectable.HighCollectability;
                        var currentDur = collectable.CurrentIntegrity;
                        var maxDur = collectable.TotalIntegrity;
                        bool missingDur = currentDur < maxDur;

                        if (Mission_Settings.item_collectableId != collectable.ItemID)
                        {
                            IceLogging.Debug($"Setting Mission CollectableId to: {collectable.ItemID}", "[Gather: Collectable Interacting]");
                            Mission_Settings.item_collectableId = collectable.ItemID;
                        }

                        // Something to note. It sometimes doesn't have all 3. One of these could be a 0... something to think about/need to check
                        // Think the process is going to be 
                        // Check to see if you meet tier 2/3 thresh
                        // If you have > 2 durability && If you don't meet these requirements
                        //   If you don't have the increase stat buff, and have it for this mission, use it
                        //   Purple Button on the bottom left -> Increase Quality + Chance to not use dur
                        // If you meet requirements
                        //   -> If missing durability, check to see if increaseInteg Skill is usable
                        //   -> If not missing durability, collect

                        if (Mission_Settings.SelectedRotation == 1)
                        {
                            NormalGpRotation(currentQuality, missingDur);
                        }
                        else
                        {
                            NoGpRotation(currentDur, currentQuality, highQuality);
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
            else
            {
                if (Mission_Settings.NextCollectableStep != Mission_Settings.CollectableStep)
                {
                    IceLogging.Debug($"Current Collectable Step: {Mission_Settings.CollectableStep} | Setting it to: {Mission_Settings.NextCollectableStep}");
                    Mission_Settings.CollectableStep = Mission_Settings.NextCollectableStep;
                }
                return false;
            }
        }

        public static bool CanUseGatheringAction(string actionName, int profileId, bool missingDur, int? boonChance = null, bool gather1More = true)
        {
            var actionInfo = GatheringUtil.GathActionDict[actionName];
            bool hasStatus = PlayerHelper.HasStatusId(actionInfo.StatusId);
            bool hasGp = PlayerHelper.GetGp() >= actionInfo.RequiredGp;
            var used = Mission_Settings.SkillUseAmount[actionName];

            if (actionName == "BonusIntegrityChance")
            {
                return hasStatus && missingDur;
            }

            var gatherBuff = C.GatherSettings[profileId].GatherBuffs.Buffs[actionName];

            return actionName switch
            {
                "BoonIncrease1" => gatherBuff.Enabled 
                                && boonChance < 100 
                                && !hasStatus
                                && !missingDur 
                                && hasGp 
                                && PlayerHelper.GetGp() >= gatherBuff.MinGp
                                && (gatherBuff.MaxUse == -1 || gatherBuff.MaxUse > used),
                "BoonIncrease2" => gatherBuff.Enabled 
                                && boonChance < 100 
                                && !hasStatus 
                                && !missingDur 
                                && hasGp 
                                && PlayerHelper.GetGp() >= gatherBuff.MinGp
                                && (gatherBuff.MaxUse == -1 || gatherBuff.MaxUse > used),
                "Tidings" => gatherBuff.Enabled 
                          && !hasStatus 
                          && !missingDur 
                          && hasGp 
                          && PlayerHelper.GetGp() >= gatherBuff.MinGp
                          && (gatherBuff.MaxUse == -1 || gatherBuff.MaxUse > used),
                "YieldI" => gatherBuff.Enabled 
                          && !hasStatus 
                          && !missingDur 
                          && hasGp 
                          && PlayerHelper.GetGp() >= gatherBuff.MinGp
                          && (gatherBuff.MaxUse == -1 || gatherBuff.MaxUse > used),
                "YieldII" => gatherBuff.Enabled 
                         && !hasStatus 
                         && !missingDur 
                         && hasGp 
                         && PlayerHelper.GetGp() >= gatherBuff.MinGp
                         && (gatherBuff.MaxUse == -1 || gatherBuff.MaxUse > used),
                "BonusIntegrity" => gatherBuff.Enabled 
                                    && missingDur 
                                    && hasGp 
                                    && PlayerHelper.GetGp() >= gatherBuff.MinGp 
                                    && (gatherBuff.MaxUse == -1 || gatherBuff.MaxUse > used),
                "BountifulYieldII" => gatherBuff.Enabled 
                                   && !hasStatus 
                                   && hasGp 
                                   && PlayerHelper.GetGp() >= gatherBuff.MinGp
                                   && (gatherBuff.MaxUse == -1 || gatherBuff.MaxUse > used) 
                                   && gather1More == true,
                _ => false,
            };
        }

        private static bool CanUseCollectableAction(string action, bool missingDur = false)
        {
            var actionInfo = GatheringUtil.GathCollectableBuffs[action];
            bool hasStatus = PlayerHelper.HasStatusId(actionInfo.StatusId);
            bool hasGp = PlayerHelper.GetGp() >= actionInfo.RequiredGp;

            return action switch
            {
                "Scrutiny" => !hasStatus
                           && hasGp,
                "Focus" => !hasStatus
                        && hasGp,
                "Priming" => !hasStatus 
                          && hasGp,
                "CollectorsHigh" => !hasStatus
                                 && hasGp,
                "BonusIntegrityChance" => hasStatus
                                       && missingDur,
                "BonusIntegrity" => hasGp
                                 && missingDur
                                 && PlayerHelper.GetGp() >= 300,
                _ => false,
            };
        }

        public static unsafe bool NormalGpRotation(int collectability, bool missingDur = false)
        {
            if (EzThrottler.Throttle("Executing HighGPRotation", 100))
            {
                // 400+ gp
                int step = Mission_Settings.CollectableStep;

                if (step == 0)
                {
                    if (!PlayerHelper.HasStatusId(3911) && GatheringUtil.CollectStandardCharges() > 0)
                    {
                        if (EzThrottler.Throttle("Using special buff", 1000))
                        {
                            ActionManager.Instance()->UseAction(ActionType.GeneralAction, 27);
                        }
                    }
                    else if (CanUseCollectableAction("Scrutiny"))
                    {
                        UseCollectableBuff("Scrutiny");
                    }
                    else
                    {
                        UseCollectableAction("Meticulous");
                        Mission_Settings.NextCollectableStep = 1;
                    }
                }
                else if (step == 1)
                {
                    // Option 1
                    if (!PlayerHelper.HasStatusId(3911))
                    {
                        if (!PlayerHelper.HasStatusId(3911) && GatheringUtil.CollectStandardCharges() > 0)
                        {
                            if (EzThrottler.Throttle("Using special buff"))
                            {
                                ActionManager.Instance()->UseAction(ActionType.GeneralAction, 27);
                            }
                        }
                        else if (CanUseCollectableAction("Scrutiny"))
                        {
                            UseCollectableBuff("Scrutiny");
                        }
                        else
                        {
                            UseCollectableAction("Meticulous");
                            Mission_Settings.NextCollectableStep = 2;
                        }
                    }
                    else // Option 2, Has "Collector's High Standard"
                    {
                        if (CanUseCollectableAction("Scrutiny"))
                        {
                            UseCollectableBuff("Scrutiny");
                        }
                        else
                        {
                            UseCollectableAction("Brazen");
                            Mission_Settings.NextCollectableStep = 3;
                        }
                    }
                }
                else if (step == 2)
                {
                    if ((PlayerHelper.HasStatusId(3911) && collectability > 800) || (collectability >= 850 && collectability <= 999))
                    {
                        // Top Row option, 
                        UseCollectableAction("Meticulous");
                    }
                    else if (collectability == 1000)
                    {
                        Mission_Settings.CollectableStep = 4;
                    }
                    else
                    {
                        UseCollectableAction("Scour");
                        Mission_Settings.NextCollectableStep = 4;
                    }
                }
                else if (step == 3)
                {
                    if (collectability < 1000)
                    {
                        UseCollectableAction("Meticulous");
                        Mission_Settings.NextCollectableStep = 4;
                    }
                    else
                    {
                        Mission_Settings.CollectableStep = 4;
                    }
                }
                else if (step == 4)
                {
                    IceLogging.Debug($"Missing durability: {missingDur}");
                    if (CanUseCollectableAction("BonusIntegrityChance", missingDur))
                    {
                        UseCollectableAction("BonusIntegrityChance");
                    }
                    else if (CanUseCollectableAction("BonusIntegrity", missingDur))
                    {
                        UseCollectableAction("BonusIntegrity");
                    }
                    else
                    {
                        UseCollectableAction("Collect");
                    }
                }
            }

            return false;
        }
        public static bool NoGpRotation(uint currentDur, int collectability, uint hqCollectability)
        {
            if (currentDur > 1 && collectability < hqCollectability)
            {
                UseCollectableAction("Meticulous");
            }
            else
            {
                UseCollectableAction("Collect");
            }

            return false;
        }

        public static unsafe void UseCollectableBuff(string action)
        {
            var collectorBuffs = GatheringUtil.GathCollectableBuffs;
            var jobId = Player.JobId;

            var actionId = collectorBuffs[action].ClassAction[jobId].ActionId;
            if (EzThrottler.Throttle("Using Action Buff", 500))
            {
                ActionManager.Instance()->UseAction(ActionType.Action, actionId);
            }
        }

        public static unsafe void UseCollectableAction(string action)
        {
            var collectorAction = GatheringUtil.GathCollectableActions;
            var jobId = Player.JobId;

            var actionId = collectorAction[action].ClassAction[jobId].ActionId;
            if (EzThrottler.Throttle("using Action Action for collectables"))
            {
                ActionManager.Instance()->UseAction(ActionType.Action, actionId);
            }
        }

        public static bool? CheckReduceMission()
        {
            IceLogging.Info($"Current itemId: {Mission_Settings.item_collectableId}", "[Gather: Check Reduce Mission]");
            bool hasCollectable = PlayerHelper.GetItemCount(Mission_Settings.item_collectableId, out var count) && count > 0;
            bool isReducableMission = CosmicHelper.CurrentMissionInfo.Attributes.HasFlag(MissionAttributes.ReducedItems);
            if (hasCollectable && isReducableMission)
            {
                P.TaskManager.InsertMulti(
                                            new(() => CheckReduceItems(), "Starting the desynth process"),
                                            new(() => WaitForDesynthCompletion(), "Waiting for desyntht to complete")
                                         );
            }

            return true;
        }

        public static unsafe bool? CheckReduceItems()
        {
            if (Svc.Condition[ConditionFlag.Occupied39])
            {
                IceLogging.Info("We're currently desynthing an item, continuing on to wait to stop", "[Task Gather: Reducing Item Check]");
                return true;
            }
            else
            {
                if (GenericHelpers.TryGetAddonMaster<Gathering>("Gathering", out var gather) && gather.IsAddonReady)
                {
                    // we shouldn't have this open while we're desynthing. . . closing it out.
                    if (EzThrottler.Throttle("Closing gather window"))
                    {
                        // 
                    }
                }

                // We have items to desynth! Time to check and see which window we need to interact with... or just wait. 
                if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("PurifyItemSelector", out var desynthWindow) && desynthWindow->IsReady)
                {
                    if (EzThrottler.Throttle("Desynthing the item"))
                    {
                        if (!Player.IsBusy)
                            ECommons.Automation.Callback.Fire(desynthWindow, true, 12, 0);
                    }
                }
                else if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
                {
                    if (EzThrottler.Throttle("Opening the desynth window"))
                    {
                        missionInfo.StellerReduction();
                    }
                }
                else if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                {
                    if (EzThrottler.Throttle("Opening the moon hud"))
                    {
                        moonHud.Mission();
                    }
                }
            }

            return false;
        }

        public static bool? WaitForDesynthCompletion()
        {
            if (!Svc.Condition[ConditionFlag.Occupied39])
            {
                PlayerHelper.GetItemCount(Mission_Settings.item_collectableId, out var count);
                if (count != 0)
                {
                    // Still have some more items to desynth, going to reset the current task count and re-check
                    P.TaskManager.Tasks.Clear();
                }

                return true;
            }

            return false;
        }

        public static unsafe void UseCordial()
        {
            if (EzThrottler.Throttle("Cordial usage check while moving"))
            {
                if (!Player.IsBusy)
                {
                    IceLogging.Debug("Cordial Checkers");
                    if (C.AutoCordial)
                    {
                        IceLogging.Debug($"Min GP: {C.CordialMinGp} <= {PlayerHelper.GetGp()}");

                        if (PlayerHelper.GetGp() <= C.CordialMinGp)
                        {
                            Dictionary<uint, int> cordials = new()
                            {
                                { 12669, 400}, // Hi
                                { 1006141, 350}, // HQ Regular
                                { 6141, 300}, // NQ Regular
                                { 1016911, 200}, // HQ Watered
                                { 16911, 150} // HQ Watered
                            };

                            foreach (var cordial in C.inverseCordialPrio ? cordials.Reverse() : cordials)
                            {
                                IceLogging.Debug($"Checking Cordial: {cordial.Key}");
                                bool hq = cordial.Key >= 1_000_000;
                                if (PlayerHelper.GetItemCount(cordial.Key, out var amount, hq, !hq) && amount > 0)
                                {
                                    if (ActionManager.Instance()->GetActionStatus(ActionType.Item, cordial.Key) == 0)
                                    {
                                        if (!C.PreventOvercap || (C.PreventOvercap && !WillOvercap(cordial.Value)))
                                        {
                                            if (EzThrottler.Throttle("Using the cordial"))
                                            {
                                                ActionManager.Instance()->UseAction(ActionType.Item, cordial.Key, extraParam: 65535);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    IceLogging.Info("Cordial Check Complete");
                }
            }
        }

        private static bool WillOvercap(int recoveryGP)
        {
            return ((PlayerHelper.GetGp() + recoveryGP) > PlayerHelper.MaxGp());
        }
    }
}
