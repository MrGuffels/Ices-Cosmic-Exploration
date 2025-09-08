using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dalamud.Interface.Utility.Raii.ImRaii;
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

        public static bool? Fish()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo))
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
                            bool GoldGoal = goldScore >= currentScore;
                            bool SilverGoal = silverScore >= currentScore;
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

        public static bool? Crafts()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo))
            {
                var Id = CosmicHelper.CurrentLunarMission;
                // var mission = CosmicHelper.Dict_CosmicMissions[Id];
                var mission = CosmicHelper.SheetMissionDict[Id];

                bool hasMinItems = true; // Have this as true, because just want to run a check for all items

                // First things first, have to check to see if you have enough of the initial crafts/meet the score threshold
                foreach (var item in mission.Crafts_Main)
                {
                    var itemId = item.Key;
                    var recipeEntry = item.Value;

                    if (!PlayerHelper.GetItemCount(itemId, out var count) || count < recipeEntry.Amount)
                    {
                        hasMinItems = false;
                        break;
                    }
                }

                // Next, need to check to see if there is a bronze threshold that is required, and make sure we're hitting it (if there is any)
                if (missionInfo.CurrentScore == null)
                    return false;

                if (mission.BronzeScore != 0 && !(missionInfo.CurrentScore >= mission.BronzeScore))
                {
                    IceLogging.Info("Bronze score is recorded at not 0. Which means that it needs a minimum score. \n" +
                                    $"Current Score: {missionInfo.CurrentScore}\n" +
                                    $"Minimum Score: {mission.BronzeScore}" +
                                    $"Continuing on with the crafting process");
                    return true;
                }
                else
                {
                    bool shouldTurnin = false;
                    var currentScore = missionInfo.CurrentScore;
                    var bronzeScore = mission.BronzeScore;
                    var silverScore = mission.SilverScore;
                    var goldScore = mission.GoldScore;

                    var config = C.MissionConfig[Id];
                    bool AnyTurnin = config.AutoTurnin;
                    bool GoldGoal = goldScore >= currentScore;
                    bool SilverGoal = silverScore >= currentScore;
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

        private static bool? ForceTurnin()
        {
            return false;
        }
    }
}
