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
        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => AbandonMission(), "Abandoning the current mission");
            P.TaskManager.Enqueue(() => CosmicHelper.CurrentLunarMission == 0, "Waiting till the current mission is 0");
        }

        public static bool? AbandonMission()
        {
            if (CosmicHelper.CurrentLunarMission == 0)
            {
                IceLogging.Debug("Current mission is 0, going back to initiating missions", "[Abandon Mission]");
                SchedulerMain.State = IceState.Start;
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
                    if (EzThrottler.Throttle("Telling it to abandon the mission"))
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
