using ECommons.GameHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_AbandonMission
    {
        private static bool Continue = false;

        public static void Enqueue()
        {
            Continue = false;
            P.TaskManager.Enqueue(() => AbandonMission(), "Abandoning the current mission");
            P.TaskManager.Enqueue(() => CosmicHelper.CurrentLunarMission == 0, "Waiting till the current mission is 0");
        }

        public static bool? AbandonMission()
        {
            if (CosmicHelper.CurrentLunarMission == 0)
            {
                IceLogging.Info("Current mission is 0, going back to initiating missions", "[Abandon Mission]");
                if (Mission_Settings.StopAfterCurrent)
                {
                    SchedulerMain.State = IceState.Idle;
                    P.TaskManager.Tasks.Clear();
                }
                else
                {
                    SchedulerMain.State = IceState.Start;
                }
                return true;
            }
            else
            {
                if (GenericHelpers.TryGetAddonMaster<SelectYesno>("SelectYesno", out var select) && select.IsAddonReady)
                {
                    if (CosmicHandler.abandonStrings.Any(s => string.Equals(NormalizeWhitespace(select.Text), NormalizeWhitespace(s), StringComparison.OrdinalIgnoreCase)) || !C.RejectUnknownYesno)
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
                            return true;
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
                    if (EzThrottler.Throttle("Attempt to turnin"))
                    {
                        addon.Report();
                    }
                    else if (EzThrottler.Throttle("Telling it to abandon the mission"))
                    {
                        IceLogging.Debug("Attempting to abandon.", "[Abandoning Mission]");
                        addon.Abandon();
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
