using Dalamud.Game.ClientState.Conditions;
using ECommons.Automation.NeoTaskManager;
using ECommons.GameHelpers;
using ECommons.UIHelpers.AddonMasterImplementations;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_FindMission
    {
        /// <summary>
        /// List of all available critical missions
        /// </summary>
        private static HashSet<uint> CriticalMissions = new HashSet<uint>();
        /// <summary>
        /// List of all the available<br></br>
        /// -> Timed <br></br>
        /// -> Weather <br></br>
        /// -> Sequence <br></br>
        /// </summary>
        private static HashSet<uint> WeatherMissions = new HashSet<uint>();
        private static HashSet<uint> TimedMissions = new HashSet<uint>();
        private static HashSet<uint> SequenceMissions = new HashSet<uint>();
        private static HashSet<uint> ExARankMissions = new HashSet<uint>();
        private static HashSet<uint> ARankMissions = new HashSet<uint>();
        private static HashSet<uint> BRankMissions = new HashSet<uint>();
        private static HashSet<uint> CRankMisisons = new HashSet<uint>();
        private static HashSet<uint> DRankMissions = new HashSet<uint>();

        private static int SpecialMissionCount = 0;
        private static int BasicMissionCount = 0;

        public static void Enqueue()
        {
            P.TaskManager.Enqueue(RefreshMissionUi, "Refreshing Mission UI");
            P.TaskManager.Enqueue(OpenMissionUi, "Opening it on proper class");
            P.TaskManager.Enqueue(RefreshSelectedMissions, "Refreshing the list of viable missions");
            if (C.XPRelicGrind)
            {
                P.TaskManager.Enqueue(() => OpenTab("Standard"), "Opening Standard tab for relic grind");
            }
            else
            {
                P.TaskManager.Enqueue(TabTasksCheck, "Checking which tabs to check for missions");
            }
        }
        private static bool? RefreshMissionUi()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var hud) && !hud.IsAddonReady)
            {
                return true;
            }
            else
            {
                if (EzThrottler.Throttle("Closing the hud to make sure it's on the right class"))
                {
                    if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                        moonHud.Mission();
                }
            }

            return false;
        }
        private static bool? OpenMissionUi()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var hud) && hud.IsAddonReady)
            {
                return true;
            }
            else
            {
                if (EzThrottler.Throttle("Opening the mission ui"))
                {
                    if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                        moonHud.Mission();
                }
            }

            return false;
        }
        private static bool? RefreshSelectedMissions()
        {
            CriticalMissions.Clear();
            WeatherMissions.Clear();
            TimedMissions.Clear();
            SequenceMissions.Clear();
            ExARankMissions.Clear();
            ARankMissions.Clear();
            BRankMissions.Clear();
            CRankMisisons.Clear();
            DRankMissions.Clear();

            uint? currentJobId = PlayerHelper.GetClassJobId();

            foreach (var mission in C.MissionConfig)
            {
                var enabled = mission.Value.Enabled;

                if (C.XPRelicGrind)
                {
                    if (!enabled && C.XPRelicOnlyEnabled)
                        continue;
                }
                else if (!enabled)
                    continue;

                var missionId = mission.Key;
                var MissionDictionary = CosmicHelper.MissionInfoDict.TryGetValue(missionId, out var missionInfo);
                if (currentJobId.Value != missionInfo.JobId || currentJobId.Value != missionInfo.JobId2)
                    continue;

                // Alright, mission was double checked to make sure it was enabled
                // And also checked to make sure that the current job is on the mission, time to actually add it to the mission info

                if (missionInfo.Attributes.HasFlag(MissionAttributes.Critical))
                    CriticalMissions.Add(missionId);
                else if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalSequential))
                    SequenceMissions.Add(missionId);
                else if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalTimed))
                    TimedMissions.Add(missionId);
                else if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalWeather))
                    WeatherMissions.Add(missionId);
                else if (missionInfo.Rank == 5)
                    ExARankMissions.Add(missionId);
                else if (missionInfo.Rank == 4)
                    ARankMissions.Add(missionId);
                else if (missionInfo.Rank == 3)
                    BRankMissions.Add(missionId);
                else if (missionInfo.Rank == 2)
                    CRankMisisons.Add(missionId);
                else if (missionInfo.Rank == 1)
                    DRankMissions.Add(missionId);

                if (missionInfo.Attributes.HasFlag(MissionAttributes.Critical))
                {
                    // Do nothing here, just let it continue onwards
                    continue;
                }
                else if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalSequential) || missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalTimed) || missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalWeather))
                {
                    SpecialMissionCount += 1;
                }
                else
                {
                    BasicMissionCount += 1;
                }
            }

            IceLogging.Info("Mission count has been updated to the following: \n" +
                $"Critical Count: {CriticalMissions.Count}\n" +
                $"Sequence Count: {SequenceMissions.Count}\n" +
                $"Timed Count: {TimedMissions.Count}\n" +
                $"Weather Count: {WeatherMissions.Count}\n" +
                $"A Rank: {ARankMissions.Count}" +
                $"B Rank: {BRankMissions.Count}" +
                $"C Rank: {CRankMisisons.Count}" +
                $"D Rank: {DRankMissions.Count}", "Mission Finder Task" +
                $"Total Critical Missions: {CriticalMissions.Count}" +
                $"Total Special Missions: {SpecialMissionCount}" +
                $"Total Basic Missions: {BasicMissionCount}");

            return true;
        }
        private static bool? TabTasksCheck()
        {
            if (CriticalMissions.Count > 0)
            {
                P.TaskManager.Enqueue(() => OpenTab("Critical"), "Opening the critical tab for missions");
                P.TaskManager.EnqueueDelay(500);
                P.TaskManager.Enqueue(CheckCritical, "Checking to see if current missions match up with the critical");
            }
            if (SpecialMissionCount > 0)
            {
                P.TaskManager.Enqueue(() => OpenTab("Provisional"), "Opening the provisional tab for missions");
                P.TaskManager.EnqueueDelay(500);
                P.TaskManager.Enqueue(CheckProvisional, "Checking to see if any provisional missions exist");
            }
            if (BasicMissionCount > 0)
            {
                P.TaskManager.Enqueue(() => OpenTab("Standard"), "Opening the standard mission tab");
                P.TaskManager.EnqueueDelay(500);
                P.TaskManager.Enqueue(CheckStandard, "Checking the standard missions for any potentional missions");
            }

            return true;
        }
        private static bool? OpenTab(string type)
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                switch (type)
                {
                    case "Critical":
                        x.CriticalMissions();
                        break;
                    case "Provisional":
                        x.ProvisionalMissions();
                        break;
                    default:
                        x.BasicMissions();
                        break;
                }

                return true;
            }
            else
            {
                if (EzThrottler.Throttle("Opening the mission ui"))
                {
                    if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                        moonHud.Mission();
                }
            }

            return false;
        }
        private static bool? CheckCritical()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                foreach (var mission in x.StellerMissions)
                {
                    if (CriticalMissions.Contains(mission.MissionId))
                    {
                        mission.Select();
                        // Insert the task for the following:
                        // -> Check if mission is a gathering or critical
                        //   -> If yes, insert a task to check if need to pathfind to area
                        // -> Insert delay here (small one, like 500 ms)
                        // -> Insert grab mission and switch states to whichever is necessary

                        P.TaskManager.InsertMulti(
                            new(() => Navmesh_MoveToMission(mission.MissionId), "Checking if movement is necessary", Utils.TaskConfig),
                            new(FrameDelay, "Waiting 8 frames before next action"),
                            new(GrabMission, "Selecting mission for grabbing")
                            );

                        return true;
                    }
                }

                // No mission was
            }

            return false;
        }
        private static bool? CheckProvisional()
        {
            return false;
        }
        private static bool? CheckStandard()
        {
            return false;
        }
        private static bool? GrabMission()
        {
            bool test = true;
            if (test)
            {

            }
            else
            {

            }

            return false;
        }
        public static unsafe bool? Navmesh_MoveToMission(uint missionId)
        {
            var missionEntry = CosmicHelper.MissionInfoDict[missionId];
            if (missionEntry.Attributes.HasFlag(MissionAttributes.Gather) || missionEntry.Attributes.HasFlag(MissionAttributes.Critical))
            {
                // Mission was found to be a gathering or critical mission, seeing if you're within range of it
                Vector2 PlayerPos = new Vector2(Player.Position.X, Player.Position.Z);
                Vector2 MapCenter = new Vector2(missionEntry.X, missionEntry.Y);

                float distance = missionEntry.MarkerId != 0 ? Vector2.Distance(PlayerPos, MapCenter) + 5 : 0;
                if (distance > missionEntry.Radius)
                {
                    if (PlayerHelper.IsPlayerNotBusy() && !Svc.Condition[ConditionFlag.Mounted])
                    {
                        // Mount Roulette. But need to look into assigning specific mounts
                        if (EzThrottler.Throttle("Attempting to mount up"))
                            ActionManager.Instance()->UseAction(ActionType.GeneralAction, 9);
                    }

                    if (!Svc.Condition[ConditionFlag.Unknown101])
                    {
                        // Condition unknown 101 pops up while you're on the flying... mount thingies. Need to make sure to try and not pathfind while on these/stop current pathfind if you manage to land on one.

                        if (!P.Navmesh.IsRunning())
                        {
                            var nodeLoc = GatheringUtil.GatheringLocation[MapCenter][0].LandZone;
                            if (EzThrottler.Throttle("Initiating navmesh to move to the first landing zone"))
                                P.Navmesh.PathfindAndMoveTo(nodeLoc, false);
                        }
                    }
                    else
                    {
                        if (P.Navmesh.IsRunning())
                            if (EzThrottler.Throttle("Special Movement + Navmesh found, temp stopping"))
                                P.Navmesh.Stop();
                    }

                    if (!P.Navmesh.IsRunning())
                    {
                        // Condition unknown 101 pops up while you're on the flying... mount thingies. Need to make sure to try and not pathfind while on these/stop current pathfind if you manage to land on one.
                        var nodeLoc = GatheringUtil.GatheringLocation[MapCenter][0].LandZone;
                        if (EzThrottler.Throttle("Initiating navmesh to move to the first landing zone"))
                            P.Navmesh.PathfindAndMoveTo(nodeLoc, false);
                    }
                    else if (P.Navmesh.IsRunning())
                    {
                        if (Svc.Condition[ConditionFlag.Unknown101])
                        {
                            P.Navmesh.Stop();
                        }
                    }
                }
                else
                {
                    // Distance has been met, moving onto next condition:
                    IceLogging.Debug("Distance to the node has been closed. Moving onto the next step:", "[Task_FindMission: Navmesh]");
                    return true;
                }
            }
            else
            {
                IceLogging.Debug("Mission was not a gathering or critical mission. Navmesh moving was not necessary. Moving onto next step", "[Task_FindMission: Navmesh]");
                return true;
            }

            return false;
        }
        private static void FrameDelay()
        {
            P.TaskManager.InsertDelay(8, true);
        }

    }
}
