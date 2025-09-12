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

        public static unsafe bool? ForceTurnin()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady && (missionInfo.Addon->AtkValuesCount > 4))
            {
                var Id = CosmicHelper.CurrentLunarMission;

                if (Id == 0)
                {
                    IceLogging.Debug($"You can't abandon a mission that was not started to begin with. Idk how you got here but... yeah.");
                    return true;
                }
                else
                {
                    var mission = CosmicHelper.SheetMissionDict[Id];
                    if (CosmicHelper.CrafterJobList.Contains(Player.JobId))
                    {
                        // Checking crafters for atleast minimum score turnin
                        if (mission.BronzeScore != 0 && (missionInfo.CurrentScore <= mission.BronzeScore))
                        {
                            IceLogging.Debug("You didn't meet the minimum score for bronze on crafters. Force abandoning");
                            return true;
                        }
                        else
                        {
                            // them minimum score has been met, just straight up turning in.
                            P.TaskManager.Tasks.Clear();
                            SchedulerMain.State = IceState.TurninMission;
                        }
                    }

                    return true;
                }
            }
            else
            {
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

        public static bool? AbandonMission()
        {
            if (CosmicHelper.CurrentLunarMission == 0)
            {
                if (!C.StopOnAbort || Continue)
                {
                    IceLogging.Info("Current mission is 0, going back to initiating missions", "[Abandon Mission]");
                    SchedulerMain.State = IceState.Start;
                }
                else
                {
                    IceLogging.Info($"All conditions to continue is not true. Stopping the plugin");
                    SchedulerMain.State = IceState.Idle;
                }

                return true;
            }
            else
            {
                if (GenericHelpers.TryGetAddonMaster<SelectYesno>("SelectYesno", out var select) && select.IsAddonReady)
                {
                    if (CosmicHandler.abandonStrings.Any(s => select.Text.Contains(s)) || !C.RejectUnknownYesno)
                    {
                        if (EzThrottler.Throttle("Selecting Yes, mission is properly abandoning"))
                        {
                            IceLogging.Debug($"Expected abandon mission text... abandoning mission", "[Abandon Mission]");
                            select.Yes();
                            SchedulerMain.State = IceState.Start;
                            return true;
                        }
                    }
                    else
                    {
                        if (EzThrottler.Throttle("Unexpected Abandon Window..."))
                        {
                            IceLogging.Error($"Unexpected abandon window??? {select.Text}", "[Abandon Mission]");
                            select.No();
                        }
                    }
                }
                else if(GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var addon) && addon.IsAddonReady)
                {
                    // First things first. Going to see if you meet the score threshold for any of the classes.
                    if (CosmicHelper.CrafterJobList.Contains(Player.JobId))
                    {
                        var missionId = CosmicHelper.CurrentLunarMission;
                        var missionInfo = CosmicHelper.SheetMissionDict[missionId];
                        var mission = C.MissionConfig[missionId];

                        if (mission.AutoTurnin && (addon.CurrentScore >= missionInfo.BronzeScore))
                        {
                            Continue = true;
                        }
                        if (mission.TurninSilver && (addon.CurrentScore >= missionInfo.SilverScore))
                        {
                            Continue = true;
                        }
                        if (mission.TurninBronze && (addon.CurrentScore >= missionInfo.BronzeScore))
                        {
                            Continue = true;
                        }
                    }

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
    }
}
