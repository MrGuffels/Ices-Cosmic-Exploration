using ECommons.GameHelpers;
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

        public static bool? AbandonMission()
        {
            string tag = "Abandon Mission";

            if (CosmicHelper.CurrentLunarMission == 0)
            {
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

                if (GenericHelpers.TryGetAddonMaster<SelectYesno>("SelectYesno", out var select) && select.IsAddonReady)
                {
                    if (CosmicHandler.abandonStrings.Any(s => NormalizeWhitespace(select.Text).Contains(NormalizeWhitespace(s), StringComparison.OrdinalIgnoreCase)) || !C.RejectUnknownYesno)
                    {
                        if (EzThrottler.Throttle("Selecting Yes, mission is properly abandoning"))
                        {
                            IceLogging.Debug($"Expected abandon mission text... abandoning mission", "[Abandon Mission]");
                            select.Yes();
                            if (Mission_Settings.StopAfterCurrent)
                            {
                                SchedulerMain.State = IceState.Idle;
                                P.TaskManager.Tasks.Clear();
                            }
                            else
                            {
                                SchedulerMain.State = IceState.Start;
                            }
                        }
                    }
                    else
                    {
                        IceLogging.Debug($"Actual text: '{select.Text}'");
                        IceLogging.Debug($"Actual text length: {select.Text.Length}");
                        IceLogging.Debug($"Trimmed text: '{select.Text.Trim()}'");
                        IceLogging.Debug($"Trimmed length: {select.Text.Trim().Length}");

                        if (EzThrottler.Throttle("Unexpected Abandon Window..."))
                        {
                            var actualText = select.Text.Trim();
                            var expectedFrench = "Êtes-vous sûre de vouloir abandonner la mission en cours ?";

                            // Debug the ACTUAL text character by character
                            IceLogging.Error("=== ACTUAL TEXT BREAKDOWN ===");
                            for (int i = 0; i < actualText.Length; i++)
                            {
                                IceLogging.Error($"Actual char {i}: '{actualText[i]}' (Unicode: {(int)actualText[i]})");
                            }

                            // Debug the EXPECTED text character by character
                            IceLogging.Error("=== EXPECTED TEXT BREAKDOWN ===");
                            IceLogging.Error($"Expected: '{expectedFrench}'");
                            IceLogging.Error($"Expected length: {expectedFrench.Length}");
                            for (int i = 0; i < expectedFrench.Length; i++)
                            {
                                IceLogging.Error($"Expected char {i}: '{expectedFrench[i]}' (Unicode: {(int)expectedFrench[i]})");
                            }

                            IceLogging.Error($"Unexpected abandon window??? {select.Text}", "[Abandon Mission]");
                            select.No();
                        }
                    }
                }
                else if(GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var addon) && addon.IsAddonReady)
                {
                    if (EzThrottler.Throttle("Trying To Turnin/Abandon", 1000))

                    if (Player.Job == (Job)18 && Svc.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.Gathering])
                    {
                        if (EzThrottler.Throttle("Stop fishing so we can turn in this mission!", 2000))
                            Task_DualClass.StopFishing();

                        return false;
                    }

                    if (EzThrottler.Throttle("Attempt to turnin", 500))
                    {
                        addon.Report();
                    }
                    else if (EzThrottler.Throttle("Telling it to abandon the mission", 500))
                    {
                        IceLogging.Debug("Attempting to abandon.", "[Abandoning Mission]");
                        addon.Abandon();
                        WasAbandoned = true;
                    }
                }
                else if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var SpaceHud) && SpaceHud.IsAddonReady)
                {
                    if (EzThrottler.Throttle("Opening the current mission info Ui"))
                    {
                        IceLogging.Debug("WKSMissionInformation missing. Attempting opening.", "[Abandoning Mission]");
                        SpaceHud.Mission();
                    }
                }
            }

            return false;
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
