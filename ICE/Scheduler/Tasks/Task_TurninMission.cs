using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using ICE.Sounds;
using ICE.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dalamud.Interface.Utility.Raii.ImRaii;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_TurninMission
    {
        public static uint PreviousMissionId = 0;

        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => TurninMission(), "Turning in the mission to the moon gods", Utils.TaskConfig);
            P.TaskManager.Enqueue(() => JobSwapCheck(), "Checking to see if you need to swap jobs");
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
                    if (C.RemoveAfterGold)
                    {
                        P.TaskManager.Enqueue(() => GoldCheck());
                    }
                    if (C.PlaySoundAlert)
                    {
                        _ = SoundPlayer.PlaySoundAsync();
                    }
                    return true;
                }
                else
                {
                    IceLogging.Debug($"Stop after current wasn't enabled. Grabbing another mission", "[Task Turnin]");
                    SchedulerMain.State = IceState.Start;
                    if (C.RemoveAfterGold)
                    {
                        P.TaskManager.Enqueue(() => GoldCheck());
                    }
                    return true;
                }
            }
            else
            {
                var critical = CosmicHelper.SheetMissionDict[id].Attributes.HasFlag(MissionAttributes.Critical);
                PreviousMissionId = id;

                if (critical)
                {
                    var collectionPoint = Utils.TryGetObjectCollectionPoint();
                    if (collectionPoint != null && Player.DistanceTo(collectionPoint) < 5 && !Player.IsBusy)
                    {
                        if (EzThrottler.Throttle("Turning into colleciton point"))
                        {
                            Utils.TargetgameObject(collectionPoint);
                            Utils.InteractWithObject(collectionPoint);
                        }
                    }
                }
                else if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
                {
                    if (Player.JobId == 18 && Svc.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.Gathering])
                    {
                        if (EzThrottler.Throttle("Stop fishing so we can turn in this mission!", 2000))
                            Task_DualClass.StopFishing();

                        return false;
                    }

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

        public static bool? JobSwapCheck()
        {
            if (Player.JobId != Mission_Settings.StartJob && Mission_Settings.StartJob != 0)
            {
                if (EzThrottler.Throttle("Swapping to crafter job", 1000))
                    GearsetHandler.TaskClassChange((Job)Mission_Settings.StartJob);

                return false;
            }
            else
            {
                return true;
            }
        }

        public static unsafe bool? GoldCheck()
        {
            var managerPtr = WKSManager.Instance();
            if (managerPtr == null) return false;

            var manager = (WKSManagerCustom*)managerPtr;
            var isGold = manager->IsMissionGolded(PreviousMissionId);

            if (C.RemoveAfterGold && isGold)
            {
                C.MissionConfig[PreviousMissionId].Enabled = false;
            }
            if (C.RemoveAfterGold && !isGold)
            {
                if (MainWindow.GetOnlyPreviousMissionsRecursive(PreviousMissionId).Count > 0)
                {
                    foreach (var prevMission in MainWindow.GetOnlyPreviousMissionsRecursive(PreviousMissionId))
                    {
                        C.MissionConfig[prevMission].Enabled = true;
                        C.Save();
                    }
                }
            }

            return true;
        }
    }
}
