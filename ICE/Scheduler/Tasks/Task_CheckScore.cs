using Dalamud.Game.ClientState.Conditions;
using ECommons.Configuration;
using ECommons.GameHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dalamud.Interface.Utility.Raii.ImRaii;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_CheckScore
    {
        public static void Enqueue()
        {
            var Id = CosmicHelper.CurrentLunarMission;
            // var mission = CosmicHelper.Dict_CosmicMissions[Id];
            var mission = CosmicHelper.SheetMissionDict[Id];

            var jobs = mission.Jobs;

            if (CosmicHelper.CrafterJobList.Overlaps(jobs) && CosmicHelper.GatheringJobList.Overlaps(jobs))
            {
                P.TaskManager.Enqueue(() => DualClass());
                P.TaskManager.Enqueue(() => SchedulerMain.State = IceState.DualClass);
            }
            if (CosmicHelper.CrafterJobList.Overlaps(jobs))
            {
                IceLogging.Info("Currently on a crafting job");
                P.TaskManager.Enqueue(() => SchedulerMain.State = IceState.Craft);
                P.TaskManager.Enqueue(() => Crafts(), "Checking for crafting score mission");
            }
            else if (CosmicHelper.GatheringJobList.Overlaps(jobs))
            {
                IceLogging.Info("Currently on a gathering job");
                if (Player.JobId == 18)
                {
                    P.TaskManager.Enqueue(() => SchedulerMain.State = IceState.Fish);
                    P.TaskManager.Enqueue(() => Fish(), "Checking fishing missions for score");
                }
                else
                {
                    P.TaskManager.Enqueue(() => SchedulerMain.State = IceState.Gather);
                    P.TaskManager.Enqueue(() => Gather(), "Checking the gathering score");
                }
            }
        }

        public static bool? Fish()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
            {

                // Hud info should be available. Now time to check the mission status.
                var id = CosmicHelper.CurrentLunarMission;
                var missionEntry = CosmicHelper.SheetMissionDict[id];
                var fishingEntry = CosmicHelper.Dict_CosmicMissions[id];

                if (missionEntry != null)
                {
                    if (missionEntry.Attributes.HasFlag(MissionAttributes.ScoreTimeRemaining))
                    {
                        // Scoring mission based on the time. Divides up into 2 different types. Normal, and variety of fish
                        if (missionEntry.Attributes.HasFlag(MissionAttributes.ScoreVariety))
                        {
                            // mission requires atleast a minimum amount of items, and they all have to be different. 
                            uint requiredAmount = fishingEntry.FishCountRequired;
                            uint currentAmount = 0;
                            foreach (var requiredFish in fishingEntry.RequiredFish)
                            {
                                foreach (var fishId in requiredFish.Value)
                                {
                                    if (PlayerHelper.GetItemCount(fishId, out int count) && count > 0)
                                    {
                                        currentAmount += 1;
                                        break;
                                    }
                                }
                            }

                            if (requiredAmount >= currentAmount)
                            {
                                // Good news, you have met the requirement for the fishing mission to complete. Time to complete the turnin process.
                                P.TaskManager.Tasks.Clear();
                                SchedulerMain.State = IceState.TurninMission;
                                return true;
                            }
                        }
                        else
                        {
                            uint requiredAmount = fishingEntry.FishCountRequired;
                            uint currentAmount = 0;

                            // mission just requires a set amount of fish period. 
                            foreach (var requiredFish in fishingEntry.RequiredFish)
                            {
                                foreach (var fishId in requiredFish.Value)
                                {
                                    if (PlayerHelper.GetItemCount(fishId, out var count))
                                    {
                                        currentAmount += (uint)count;
                                    }
                                }
                            }

                            if (requiredAmount >= currentAmount)
                            {
                                // Good news, you have met the requirement for the fishing mission to complete. Time to complete the turnin process.
                                P.TaskManager.Tasks.Clear();
                                SchedulerMain.State = IceState.TurninMission;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        // These are just generic missions that are score based from here on out.
                        // First, checking to see if you even meet the minimum requirement to turnin.

                        var currentScore = missionInfo.CurrentScore;
                        var minAmount = fishingEntry.FishCountRequired;
                        var bronzeScore = fishingEntry.BronzeScore;
                        var silverScore = fishingEntry.SilverScore;
                        var goldScore = fishingEntry.GoldScore;

                        bool canTurnin = false;

                        if (minAmount != 0 && !missionEntry.Attributes.HasFlag(MissionAttributes.Craft))
                        {
                            var currentAmount = 0;

                            // There is a minimum requirement to be able to turn this in, even on bronze. Checking to see if we meet that condition first
                            foreach (var fishEntry in fishingEntry.RequiredFish)
                            {
                                foreach (var fishId in fishEntry.Value)
                                {
                                    if (!PlayerHelper.GetItemCount(fishId, out var count))
                                    {
                                        currentAmount += count;
                                    }
                                }
                            }

                            if (currentAmount >= minAmount)
                            {
                                canTurnin = true;
                                IceLogging.Debug("Minimum Scoring has been achieved for turnin.", "[Fish Scoring]");
                            }
                        }
                        else
                        {
                            // No minimum items are necessary for turnin, just checking for bronze score now.
                            if (currentScore >= bronzeScore)
                            {
                                canTurnin = true;
                                IceLogging.Debug($"Minimum scoring has met bronze scoring.", "[Fish Scoring]");
                            }
                        }

                        if (canTurnin)
                        {
                            bool shouldTurnin = false;

                            var config = C.MissionConfig[id];

                            bool AnyTurnin = config.AutoTurnin;
                            bool GoldGoal = goldScore <= currentScore;
                            bool SilverGoal = silverScore <= currentScore;
                            bool TurninBronze = config.TurninBronze;

                            if (config.AutoTurnin)
                            {
                                // AutoTurnin enabled, going to check for gold only since we have materials/time still
                                if (GoldGoal)
                                {
                                    shouldTurnin = true;
                                }
                            }
                            else
                            {
                                if (GoldGoal && config.TurninGold)
                                {
                                    shouldTurnin = true;
                                }
                                else if (SilverGoal && config.TurninSilver)
                                {
                                    if (!config.TurninGold) // Check is here, just to make sure we shouldn't still be aiming for gold
                                    {
                                        shouldTurnin = true;
                                    }
                                }
                                else if (config.TurninBronze)
                                {
                                    if (!config.TurninSilver && !config.TurninGold) // Checking to make sure that silver and gold scores both aren't true
                                    {
                                        shouldTurnin = true;
                                    }
                                }
                            }

                            if (shouldTurnin)
                            {
                                IceLogging.Debug("The threshold for scoring was met. Time to turnin", "[Fish Scoring]");

                                SchedulerMain.State = IceState.TurninMission;
                                P.TaskManager.Tasks.Clear();
                                return true;
                            }
                            else
                            {
                                IceLogging.Debug("Minimum scoring isn't met for your current preset. Continuing on", "[Fish Scoring]");
                                return true;
                            }
                        }
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

        public static unsafe bool? Crafts()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
            {
                if (missionInfo.Addon->AtkValuesCount > 4) // Really just here to make sure that the addon atkValues are fully loaded...
                {
                    var Id = CosmicHelper.CurrentLunarMission;
                    // var mission = CosmicHelper.Dict_CosmicMissions[Id];
                    var mission = CosmicHelper.SheetMissionDict[Id];
                    bool shouldTurnin = false;

                    if (mission.Attributes.HasFlag(MissionAttributes.Critical))
                    {
                        if (missionInfo.CriticalScore == 1)
                        {
                            shouldTurnin = true;
                        }
                    }
                    else
                    {
                        // First things first, have to check to see if you have enough of the initial crafts/meet the score threshold
                        foreach (var item in mission.Crafts_Main)
                        {
                            var itemId = item.Value.ItemId;
                            var recipeEntry = item.Value;

                            if (!PlayerHelper.GetItemCount(itemId, out var count) || count < recipeEntry.RequiredAmount)
                            {
                                IceLogging.Debug("Found an item that you didn't have the minumim amount of. Continuing on with our task", "[Task_CheckScore: Craft]");
                                IceLogging.Debug($"RecipeId: {item.Key} | Have: {count} | Expected amount: {recipeEntry.RequiredAmount}");
                                SchedulerMain.State = IceState.Craft;
                                return true;
                            }
                        }

                        if (mission.Crafts_Pre.Count > 0)
                        {

                        }

                        // Next, need to check to see if there is a bronze threshold that is required, and make sure we're hitting it (if there is any)

                        if (mission.BronzeScore != 0 && (missionInfo.CurrentScore <= mission.BronzeScore))
                        {
                            IceLogging.Info("Bronze score is recorded at not 0. Which means that it needs a minimum score. \n" +
                                            $"Current Score: {missionInfo.CurrentScore}\n" +
                                            $"Minimum Score: {mission.BronzeScore}\n" +
                                            $"Continuing on with the crafting process");
                            return true;
                        }
                        else
                        {
                            var currentScore = missionInfo.CurrentScore;
                            var bronzeScore = mission.BronzeScore;
                            var silverScore = mission.SilverScore;
                            var goldScore = mission.GoldScore;

                            var config = C.MissionConfig[Id];
                            bool AnyTurnin = config.AutoTurnin;
                            bool GoldGoal = goldScore <= currentScore;
                            bool SilverGoal = silverScore <= currentScore;
                            bool TurninBronze = config.TurninBronze;

                            if (config.AutoTurnin)
                            {
                                // AutoTurnin enabled, going to check for gold only since we have materials/time still
                                if (GoldGoal)
                                {
                                    IceLogging.Info("Auto turnin was enabled, and hit the max score.", "[Craft Scoring]");
                                    shouldTurnin = true;
                                }
                            }
                            else
                            {
                                if (GoldGoal && config.TurninGold)
                                {
                                    IceLogging.Info("Gold Turnin was enabled, and hit the max score.", "[Craft Scoring]");
                                    shouldTurnin = true;
                                }
                                else if (SilverGoal && config.TurninSilver)
                                {
                                    if (!config.TurninGold) // Check is here, just to make sure we shouldn't still be aiming for gold
                                    {
                                        IceLogging.Info("Silver Turnin was enabled, and you didn't have gold enabled.", "[Craft Scoring]");
                                        shouldTurnin = true;
                                    }
                                }
                                else if (config.TurninBronze)
                                {
                                    if (!config.TurninSilver && !config.TurninGold) // Checking to make sure that silver and gold scores both aren't true
                                    {
                                        IceLogging.Info("Silver Turnin was enabled, and you didn't have gold or silver enabled.", "[Craft Scoring]");
                                        shouldTurnin = true;
                                    }
                                }
                            }
                        }
                    }

                    if (shouldTurnin)
                    {
                        IceLogging.Debug("The threshold for scoring was met. Time to turnin", "[Craft Scoring]");

                        SchedulerMain.State = IceState.TurninMission;
                        P.TaskManager.Tasks.Clear();
                        return true;
                    }
                    else
                    {
                        IceLogging.Debug("Minimum scoring isn't met for your current preset. Continuing on", "[Craft Scoring]");

                        return true;
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

        public static unsafe bool? Gather()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
            {
                if (missionInfo.Addon ->AtkValuesCount > 4) // Really just here to make sure that the addon atkValues are fully loaded...
                {
                    if (CosmicHandler.IsMissionTimedOut())
                    {
                        SchedulerMain.State = IceState.AbandonMission;
                        return true;
                    }

                    IceLogging.Debug("Checking score for gathering. . .", "[Check Score: Gather]");
                    // Hud info should be available. Now time to check the mission status.
                    var id = CosmicHelper.CurrentLunarMission;
                    var mission = CosmicHelper.SheetMissionDict[id];

                    if (mission.Attributes.HasFlag(MissionAttributes.Critical))
                    {
                        if (missionInfo.CriticalScore == 1)
                        {
                            SchedulerMain.State = IceState.TurninMission;
                            P.TaskManager.Tasks.Clear();
                            return true;
                        }
                        else
                        {
                            // Still waiting for it to hit 1. So just returning true
                            return true;
                        }
                    }
                    else if (mission.Attributes.HasFlag(MissionAttributes.ScoreTimeRemaining))
                    {
                        // We're just checking to see if we have all the items for missions that have a score time remaining. 
                        // These are typically missions that have 6 gather points, and also require a certain amount of items.
                        foreach (var item in mission.Gathering_Min)
                        {
                            if (PlayerHelper.GetItemCount(item.Key, out var count) && count < item.Value)
                            {
                                // found an item that is less than what we need. Going back to continue on gathering.
                                return true;
                            }
                        }

                        // if we've gotten here, that means that we actually have all the items. Proceeding to turnin item
                        SchedulerMain.State = IceState.TurninMission;
                        P.TaskManager.Tasks.Clear();
                        return true;
                    }
                    else
                    {
                        if (mission.Attributes.HasFlag(MissionAttributes.Limited))
                        {
                            if (Mission_Settings.nodeTotal >= 7 && !Svc.Condition[ConditionFlag.Gathering])
                            {
                                // We've hit the node total, and can't gather anymore. Just going to try and turnin/abandon
                                SchedulerMain.State = IceState.AbandonMission;
                                Mission_Settings.nodeTotal = 0;
                            }
                        }

                        var canTurnin = false;

                        // Not retricted by time, but by either score or item's gathered.
                        if (mission.BronzeScore == 0)
                        {
                            // This is a mission that requires a certain amount of each item. Checking that first.
                            foreach (var item in mission.Gathering_Min)
                            {
                                if (PlayerHelper.GetItemCount(item.Key, out var count) && count < item.Value)
                                {
                                    // you don't have enough items to meet the turnin here. 
                                    return true;
                                }
                            }

                            // If we've gotten this far, than that means we've met the bronze threshold!
                            canTurnin = true;
                        }
                        else
                        {
                            // a minimum threshold of bronze scoring is required. Time to check that.
                            var currentScore = missionInfo.CurrentScore;
                            if (currentScore >= mission.BronzeScore)
                                canTurnin = true;
                        }

                        if (canTurnin)
                        {
                            // Turnin threshold has been met. Time to check to see if we're at the point where we want to turn in minimumly
                            var currentScore = missionInfo.CurrentScore;
                            var bronzeScore = mission.BronzeScore;
                            var silverScore = mission.SilverScore;
                            var goldScore = mission.GoldScore;

                            var config = C.MissionConfig[id];
                            bool AnyTurnin = config.AutoTurnin;
                            bool GoldGoal = goldScore <= currentScore;
                            bool SilverGoal = silverScore <= currentScore;
                            bool TurninBronze = config.TurninBronze;

                            bool shouldTurnin = false;

                            if (config.AutoTurnin)
                            {
                                // AutoTurnin enabled, going to check for gold only since we have materials/time still
                                if (GoldGoal)
                                {
                                    IceLogging.Info("Auto turnin was enabled, and hit the max score.", "[Craft Scoring]");
                                    shouldTurnin = true;
                                }
                            }
                            else
                            {
                                if (GoldGoal && config.TurninGold)
                                {
                                    IceLogging.Info("Gold Turnin was enabled, and hit the max score.", "[Craft Scoring]");
                                    shouldTurnin = true;
                                }
                                else if (SilverGoal && config.TurninSilver)
                                {
                                    if (!config.TurninGold) // Check is here, just to make sure we shouldn't still be aiming for gold
                                    {
                                        IceLogging.Info("Silver Turnin was enabled, and you didn't have gold enabled.", "[Craft Scoring]");
                                        shouldTurnin = true;
                                    }
                                }
                                else if (config.TurninBronze)
                                {
                                    if (!config.TurninSilver && !config.TurninGold) // Checking to make sure that silver and gold scores both aren't true
                                    {
                                        IceLogging.Info("Silver Turnin was enabled, and you didn't have gold or silver enabled.", "[Craft Scoring]");
                                        shouldTurnin = true;
                                    }
                                }
                            }

                            if (shouldTurnin)
                            {
                                IceLogging.Debug("The threshold for scoring was met. Time to turnin", "[Gathering Scoring]");

                                SchedulerMain.State = IceState.TurninMission;
                                P.TaskManager.Tasks.Clear();
                                return true;
                            }
                            else
                            {
                                IceLogging.Debug("Minimum scoring isn't met for your current preset. Continuing on", "[Gathering Scoring]");

                                return true;
                            }
                        }
                        else
                        {
                            IceLogging.Debug($"Minimum turnin hasn't been met yet. Continuing onto gathering", "[Score Check: Gather]");
                            return true;
                        }
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

        public static unsafe bool? DualClass()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
            {
                if (missionInfo.Addon->AtkValuesCount > 4) // Really just here to make sure that the addon atkValues are fully loaded...
                {
                    var Id = CosmicHelper.CurrentLunarMission;
                    // var mission = CosmicHelper.Dict_CosmicMissions[Id];
                    var mission = CosmicHelper.SheetMissionDict[Id];
                    bool shouldTurnin = false;

                    // First things first, have to check to see if you have enough of the initial crafts/meet the score threshold
                    foreach (var item in mission.Crafts_Main)
                    {
                        var itemId = item.Value.ItemId;
                        var recipeEntry = item.Value;

                        if (!PlayerHelper.GetItemCount(itemId, out var count) || count < recipeEntry.RequiredAmount)
                        {
                            IceLogging.Debug("Found an item that you didn't have the minumim amount of. Continuing on with our task", "[Task_CheckScore: Craft]");
                            IceLogging.Debug($"RecipeId: {item.Key} | Have: {count} | Expected amount: {recipeEntry.RequiredAmount}");
                            SchedulerMain.State = IceState.Craft;
                            return true;
                        }
                    }

                    // Next, need to check to see if there is a bronze threshold that is required, and make sure we're hitting it (if there is any)

                    if (mission.BronzeScore != 0 && (missionInfo.CurrentScore <= mission.BronzeScore))
                    {
                        IceLogging.Info("Bronze score is recorded at not 0. Which means that it needs a minimum score. \n" +
                                        $"Current Score: {missionInfo.CurrentScore}\n" +
                                        $"Minimum Score: {mission.BronzeScore}\n" +
                                        $"Continuing on with the crafting process");
                        return true;
                    }
                    else
                    {
                        var currentScore = missionInfo.CurrentScore;
                        var bronzeScore = mission.BronzeScore;
                        var silverScore = mission.SilverScore;
                        var goldScore = mission.GoldScore;

                        var config = C.MissionConfig[Id];
                        bool AnyTurnin = config.AutoTurnin;
                        bool GoldGoal = goldScore <= currentScore;
                        bool SilverGoal = silverScore <= currentScore;
                        bool TurninBronze = config.TurninBronze;

                        if (config.AutoTurnin)
                        {
                            // AutoTurnin enabled, going to check for gold only since we have materials/time still
                            if (GoldGoal)
                            {
                                IceLogging.Info("Auto turnin was enabled, and hit the max score.", "[Craft Scoring]");
                                shouldTurnin = true;
                            }
                        }
                        else
                        {
                            if (GoldGoal && config.TurninGold)
                            {
                                IceLogging.Info("Gold Turnin was enabled, and hit the max score.", "[Craft Scoring]");
                                shouldTurnin = true;
                            }
                            else if (SilverGoal && config.TurninSilver)
                            {
                                if (!config.TurninGold) // Check is here, just to make sure we shouldn't still be aiming for gold
                                {
                                    IceLogging.Info("Silver Turnin was enabled, and you didn't have gold enabled.", "[Craft Scoring]");
                                    shouldTurnin = true;
                                }
                            }
                            else if (config.TurninBronze)
                            {
                                if (!config.TurninSilver && !config.TurninGold) // Checking to make sure that silver and gold scores both aren't true
                                {
                                    IceLogging.Info("Silver Turnin was enabled, and you didn't have gold or silver enabled.", "[Craft Scoring]");
                                    shouldTurnin = true;
                                }
                            }
                        }
                    }

                    if (shouldTurnin)
                    {
                        IceLogging.Debug("The threshold for scoring was met. Time to turnin", "[Craft Scoring]");

                        SchedulerMain.State = IceState.TurninMission;
                        P.TaskManager.Tasks.Clear();
                        return true;
                    }
                    else
                    {
                        IceLogging.Debug("Minimum scoring isn't met for your current preset. Continuing on", "[Craft Scoring]");

                        return true;
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
    }
}
