using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_TurninMission
    {
        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => TurninMission(), "Turning in the mission to the moon gods", Utils.TaskConfig);
        }

        public static unsafe bool? TurninMission()
        {
            var id = CosmicHelper.CurrentLunarMission;

            if (id == 0)
            {
                if (Mission_Settings.StopAfterCurrent)
                {
                    IceLogging.Debug($"Stop after current was enabled. Stopping now", "[Task Turnin]");
                    SchedulerMain.State = IceState.Idle;
                    Mission_Settings.StopAfterCurrent = false;
                    P.TaskManager.Tasks.Clear();
                    return true;
                }
                else
                {
                    IceLogging.Debug($"Stop after current wasn't enabled. Grabbing another mission", "[Task Turnin]");
                    SchedulerMain.State = IceState.Start;
                    return true;
                }
            }
            else
            {
                var critical = CosmicHelper.SheetMissionDict[id].Attributes.HasFlag(MissionAttributes.Critical);

                if (critical)
                {
                    var collectionPoint = Utils.TryGetObjectCollectionPoint();
                    if (collectionPoint != null && Player.DistanceTo(collectionPoint) < 5)
                    {
                        if (Svc.Targets.Target != collectionPoint)
                        {
                            if (EzThrottler.Throttle("Targeting collection point"))
                            {
                                Utils.TargetgameObject(collectionPoint);
                            }
                        }
                        else if (!Player.IsBusy)
                        {
                            if (EzThrottler.Throttle("Interacting with the collection point"))
                            {
                                Utils.InteractWithObject(collectionPoint);
                            }
                        }
                    }
                }
                else if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
                {
                    if (EzThrottler.Throttle("Turning in mission"))
                        missionInfo.Report();
                }
                else if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud))
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
