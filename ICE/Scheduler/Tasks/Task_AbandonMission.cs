using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using ICE.Utilities.Cosmic_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_AbandonMission
    {
        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => AbandonMission(), "Abandoning the current mission");
            P.TaskManager.Enqueue(() => Task_TurninMission.JobSwapCheck(), "Checking to see if we need to swap jobs");
            P.TaskManager.Enqueue(() => Task_TurninMission.GoldCheck(), "Checking post mission state + gold state condition");
            P.TaskManager.Enqueue(() => Task_TurninMission.CommandCheck(), "Checking for post mission commands");
            if (C.DelayGrabMission)
                P.TaskManager.EnqueueDelay(C.DelayIncrease);
        }

        public static bool WasAbandoned = false;
        public static bool ForceAbandon = false;

        public static bool? AbandonMission()
        {
            string tag = "Abandon Mission";

            if (CosmicHelper.CurrentLunarMission == 0)
            {
                ForceAbandon = false;
                if (WasAbandoned)
                {
                    IceLogging.Debug("Mission was abandoned");
                    P.MissionTimer.AbandonMission();
                }
                else
                {
                    Task_TurninMission.UpdateScoreInfo();
                    var duration = P.MissionTimer.CompleteMission();
                    Mission_Settings.TurninState = TurninState.None;

                    // Log the results
                    if (C.MissionConfig.TryGetValue(Task_TurninMission.PreviousMissionId, out var config))
                    {
                        IceLogging.Info($"Mission [{Task_TurninMission.PreviousMissionId}] [{CosmicHelper.SheetMissionDict[Task_TurninMission.PreviousMissionId].Name}] completed in {duration:mm\\:ss\\.ff} | Best: {TimeSpan.FromSeconds(config.BestTime):mm\\:ss\\.ff} | Avg: {TimeSpan.FromSeconds(config.AverageTime):mm\\:ss\\.ff}", $"{tag} [Mission Timer]");
                    }
                }

                WasAbandoned = false;

                if (P.AutoHook.Installed)
                {
                    P.AutoHook.DeleteAllAnonymousPresets();
                }

                IceLogging.Info("Current mission is 0, checking to see where we need to be now", "[Abandon Mission]");
                return true;
            }
            else
            {
                Task_TurninMission.PreviousMissionId = CosmicHelper.CurrentLunarMission;
                if (EzThrottler.Throttle("Score Check Update"))
                    Task_TurninMission.ScoreCheck();

                if (Player.Job == (Job)18 && Svc.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.Gathering])
                {
                    if (EzThrottler.Throttle("Stop fishing so we can turn in this mission!", 2000))
                        Task_DualClass.StopFishing();

                    return false;
                }

                if (!ForceAbandon)
                {
                    if (EzThrottler.Throttle("Trying to turnin/abandon", 250))
                    {
                        if (EzThrottler.Throttle("Attempt to Turnin", 1000))
                        {
                            ReportMissionInstance();
                        }
                        else if (EzThrottler.Throttle("Attempting to abandon", 1000))
                        {
                            AbandonMissionInstance();
                            IceLogging.Debug("Attempting to abandon.", "Abandon Mission");
                            WasAbandoned = true;
                        }
                    }
                }
                else
                {
                    if (EzThrottler.Throttle("Attempting to abandon", 250))
                    {
                        AbandonMissionInstance();
                        IceLogging.Debug("Attempting to abandon.", "Abandon Mission");
                        WasAbandoned = true;
                    }
                }
            }

            return false;
        }

        private static unsafe void AbandonMissionInstance()
        {
            var WKSInstance = WKSManager.Instance();
            WKSInstance->MissionModule->AbandonMission();
        }

        private static unsafe void ReportMissionInstance()
        {
            var WKSInstance = WKSManager.Instance();
            WKSInstance->MissionModule->ReportMission();
        }

        private static string NormalizeWhitespace(string text)
        {
            return text.Trim()
                       .Replace('\u00A0', ' ')  // Non-breaking space to regular space
                       .Replace('\u2009', ' ')  // Thin space to regular space
                       .Replace('\u202F', ' ')  // Narrow no-break space to regular space
                       .Replace('\u3000', ' '); // Ideographic space to regular space
        }
    }
}
