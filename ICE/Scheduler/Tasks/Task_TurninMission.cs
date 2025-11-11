using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using ICE.Sounds;
using ICE.Ui;
using ICE.Utilities.Cosmic_Helper;
using ICE.Utilities.GatheringHelper;
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
        private static bool PathfoundToRed = false;

        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => TurninMission(), "Turning in the mission to the moon gods", Utils.TaskConfig);
            P.TaskManager.Enqueue(() => JobSwapCheck(), "Checking to see if you need to swap jobs");
            P.TaskManager.Enqueue(() => GoldCheck(), "Checking if Gold Check Task needs to be completed");
        }

        public static unsafe bool? TurninMission()
        {
            string tag = "[Turnin Mission]";
            var id = CosmicHelper.CurrentLunarMission;

            if (id == 0)
            {
                PathfoundToRed = false;

                // Complete the timer and get duration
                var duration = P.MissionTimer.CompleteMission();

                // Log the results
                if (C.MissionConfig.TryGetValue(PreviousMissionId, out var config))
                {
                    if (config.BestTime != double.MaxValue)
                        IceLogging.Info($"Mission [{PreviousMissionId}] [{CosmicHelper.SheetMissionDict[PreviousMissionId].Name}] completed in {duration:mm\\:ss\\.ff} | Best: {TimeSpan.FromSeconds(config.BestTime):mm\\:ss\\.ff} | Avg: {TimeSpan.FromSeconds(config.AverageTime):mm\\:ss\\.ff}", $"{tag} [Mission Timer]");
                }

                if (P.AutoHook.Installed)
                {
                    P.AutoHook.DeleteAllAnonymousPresets();
                }

                Mission_Settings.TurninState = TurninState.None;

                if (Mission_Settings.StopAfterCurrent)
                {
                    IceLogging.Debug($"Stop after current was enabled. Stopping now", "[Task Turnin]");
                    SchedulerMain.State = IceState.Idle;
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
                PreviousMissionId = id;

                if (critical)
                {
                    var collectionPoint = Utils.TryGetObjectCollectionPoint();
                    if (!PlayerHelper.CustomIsBusy)
                    {
                        if (collectionPoint != null && Player.DistanceTo(collectionPoint) <= 4)
                        {
                            if (EzThrottler.Throttle("Log Throttle", 1000))
                            {
                                IceLogging.Debug("Attempting to turnin/chekcing if we need to navmesh stop");
                            }

                            if (P.Navmesh.IsRunning())
                            {
                                if (EzThrottler.Throttle("Telling navmesh to stop"))
                                    P.Navmesh.Stop();

                                return false;
                            }

                            if (!Player.IsBusy)
                            {
                                if (EzThrottler.Throttle("Turning into colleciton point", 6000))
                                {
                                    Utils.TargetgameObject(collectionPoint);
                                    Utils.InteractWithObject(collectionPoint);
                                }
                            }
                        }
                        else
                        {
                            if (EzThrottler.Throttle("Log Throttle"))
                            {
                                IceLogging.Debug("Need to move closer to this turnin");
                            }

                            // We need to path to the collection point, and get as *-close-* as we can. 
                            if (GatheringUtil.CriticalLocations.TryGetValue(id, out var location) && location != Vector3.Zero)
                            {
                                if (collectionPoint == null)
                                {
                                    // We still need to get within range of it. So just going to tell it to pathfind and moveto if it wasn't already.
                                    if (!P.Navmesh.IsRunning())
                                    {
                                        if (EzThrottler.Throttle("Telling navmesh to move to the spot"))
                                        {
                                            IceLogging.Debug("We're not close enough to the turnin point to find out where one's at. So going to the location where it might be at");
                                            P.Navmesh.PathfindAndMoveTo(location, false);
                                        }
                                    }
                                    else
                                    {
                                        if (C.UseMountInMission && !Player.IsBusy && Player.DistanceTo(location) > C.MountRadius && !Svc.Condition[ConditionFlag.Mounted])
                                        {
                                            if (EzThrottler.Throttle("Mounting the mount"))
                                                Utils.MountAction();
                                        }
                                    }
                                }
                                else if (!C.DisablePathfindingToRedAlert)
                                {
                                    if (EzThrottler.Throttle("We're pathfinding wooo"))
                                        IceLogging.Debug("We're pathfinding to the turnin point!");

                                    if (Player.DistanceTo(location) > 75 && P.Navmesh.IsRunning())
                                    {
                                        if (EzThrottler.Throttle("Waiting to be in a better range", 1000))
                                        {
                                            IceLogging.Debug("Waiting to be within 50 yalms of the turnin point");
                                        }
                                    }
                                    else if (Player.DistanceTo(location) <= 75 || !P.Navmesh.IsRunning())
                                    {

                                        if (!PathfoundToRed)
                                        {
                                            P.Navmesh.Stop();
                                            P.Navmesh.PathfindAndMoveTo(collectionPoint.Position, false);
                                            PathfoundToRed = true;
                                        }
                                        else if (Player.DistanceTo(collectionPoint.Position) < C.DismountRadius && Svc.Condition[ConditionFlag.Mounted])
                                        {
                                            if (EzThrottler.Throttle("dismounting"))
                                                Utils.Dismount();
                                        }
                                        else if (C.UseMountInMission && !Player.IsBusy && Player.DistanceTo(location) > C.MountRadius && !Svc.Condition[ConditionFlag.Mounted])
                                        {
                                            if (EzThrottler.Throttle("Mounting the mount"))
                                                Utils.MountAction();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (EzThrottler.Throttle("Error message unfort"))
                                    IceLogging.Debug($"Failed to check for: {id}...");
                            }
                        }
                    }
                    else
                    {
                        if (EzThrottler.Throttle("Waiting for us to not be busy. . . ", 1000))
                            IceLogging.Debug("Waiting for player to not be in a busy state");
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
            if (C.GrindProvisionals)
            {
                IceLogging.Info("We're currently grinding out provisionals, and that means swapping jobs constantly would be... hella bad LOL. So just continuing on like normal");
                return true;
            }

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

            IceLogging.Info("Gold Check is complete, and checking to see what state we need to be in post cleanup");
            if (Mission_Settings.StopAfterCurrent)
            {
                IceLogging.Info("We're stopping after this mission", "[Gold Check Task]");
                Mission_Settings.StopAfterCurrent = false;
                SchedulerMain.State = IceState.Idle;

                if (C.PlaySoundAlert)
                    _ = SoundPlayer.PlaySoundAsync();
            }
            else
            {
                IceLogging.Info("We're continuing after this mission", "[Gold Check Task]");
                SchedulerMain.State = IceState.Start;
            }

            return true;
        }
    }
}
