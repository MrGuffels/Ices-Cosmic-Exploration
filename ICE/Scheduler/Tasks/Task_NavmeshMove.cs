using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Gui.Toast;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using ICE.Utilities.Cosmic_Helper;

namespace ICE.Scheduler.Tasks
{
    internal class Task_NavmeshMove
    {
        public static bool? Task_NavTo(Vector3 pos, bool waitForBusy = true, float distance = 2.0f, bool stayMounted = false, Vector3? npcLoc = null)
        {
            string handle = "[Navmesh Task_NavTo]";

            bool usingCosmoliner = Svc.Condition[ConditionFlag.Unknown101];
            bool mounted = Player.Mounted;
            bool inMission = CosmicHelper.CurrentLunarMission != 0;
            float minMountDistance = C.MountRadius;
            float dismountDistance = C.DismountRadius;

            bool useInMission = C.UseMountInMission && inMission;
            bool useOutsideMission = C.UseMountOutsideMission && !inMission;
            bool useMount = useInMission || useOutsideMission;

            if (EzThrottler.Throttle("Navmesh message throttle", 3000))
                IceLogging.Verbose("Executing Navmesh Task", handle, debugOnly: true);

            if (!P.Navmesh.Installed)
            {
                IceLogging.Info("We seem to be missing navmesh... so we're just going to exit here", handle);
                return true;
            }
            else if (P.Navmesh.IsRunning())
            {
                if (usingCosmoliner || Player.IsJumping)
                    P.Navmesh.Stop();

                CheckifIsStuck();

                if (!mounted && Player.DistanceTo(pos) > minMountDistance)
                {
                    if (useMount)
                    {
                        if (EzThrottler.Throttle("Using mount"))
                            Utils.MountAction();
                    }
                }

                if (Player.DistanceTo(pos) <= dismountDistance && !stayMounted)
                {
                    if (EzThrottler.Throttle("Dismounting the mount"))
                    {
                        Utils.Dismount();
                    }
                }

                if (Player.IsMoving && waitForBusy)
                {
                    if (EzThrottler.Throttle("Throttle message tehe", 2000))
                        IceLogging.Verbose("We're currently moving, and we were told to wait for us to NOT be moving so... yeah, we waiting", handle);

                    return false;
                }
                else if (!waitForBusy)
                {
                    if (npcLoc != null)
                    {
                        if (Player.DistanceTo(npcLoc.Value) <= distance)
                        {
                            if (EzThrottler.Throttle("NPC Location Throttle"))
                                IceLogging.Debug("We're close to the npc, and we're not busy anymore, so stopping", handle);

                            P.Navmesh.Stop();
                        }
                    }
                    else if (Player.DistanceTo(pos) <= distance)
                    {
                        if (EzThrottler.Throttle("Normal distance throttle"))
                            IceLogging.Debug("We're within stopping distance, so stopping navmesh", handle);

                        P.Navmesh.Stop();
                    }
                }
            }
            else if (!P.Navmesh.IsReady())
            {
                if (EzThrottler.Throttle("Waiting on navmesh", 1000))
                {
                    var navProgress = P.Navmesh.BuildProgress();
                    IceLogging.Debug($"Waiting for navmesh to finish building. Currently at: {navProgress:N2}", handle);
                }
            }
            else if (!P.Navmesh.IsRunning())
            {
                // We're here, which means it's time to start fresh for navmesh
                if (usingCosmoliner || Player.IsJumping)
                {
                    // We don't want navmesh/any checks to be running while using the cosmoliner, so just exiting out
                    ResetInfo();
                    return false;
                }

                if (npcLoc != null)
                {
                    if (Player.DistanceTo(npcLoc.Value) < distance)
                    {
                        if (mounted && !stayMounted)
                        {
                            if (EzThrottler.Throttle("Dismounting the mount"))
                            {
                                Utils.Dismount();
                            }
                            return false;
                        }
                        else
                        {
                            IceLogging.Debug("We've met the distance threshold for the npc", handle);
                            IceLogging.Debug($"Player Distance: {Player.DistanceTo(npcLoc.Value)}");
                            IceLogging.Debug($"Expected Distance: {distance}");
                            ResetInfo();
                            return true;
                        }
                    }
                    else
                    {
                        if (EzThrottler.Throttle("Telling navmesh to start"))
                        {
                            IceLogging.Debug("Telling navmesh to start pathfinding");
                            whenStarted = DateTime.Now;
                            ResetInfo();
                            IceLogging.DestinationLogs.Log(pos);
                            P.Navmesh.SetTolerance(0.25f);
                            P.Navmesh.PathfindAndMoveTo(pos, false);
                        }
                    }
                }
                else if (Player.DistanceTo(pos) < distance)
                {
                    if (mounted && !stayMounted)
                    {
                        if (EzThrottler.Throttle("Dismounting the mount"))
                        {
                            Utils.Dismount();
                        }
                        return false;
                    }
                    else
                    {
                        IceLogging.Debug("We've met the distance threshold for our destination", handle);
                        IceLogging.Debug($"Player Distance: {Player.DistanceTo(pos)}");
                        IceLogging.Debug($"Expected Distance: {distance}");
                        ResetInfo();
                        return true;
                    }
                }
                else
                {
                    if (EzThrottler.Throttle("Telling navmesh to start"))
                    {
                        IceLogging.Debug("Telling navmesh to start pathfinding");
                        whenStarted = DateTime.Now;
                        ResetInfo();
                        IceLogging.DestinationLogs.Log(pos);
                        P.Navmesh.SetTolerance(0.25f);
                        P.Navmesh.PathfindAndMoveTo(pos, false);
                    }
                }
            }

            return false;
        }

        private static Vector3 lastPosition = Vector3.Zero;
        private static DateTime whenStarted = DateTime.Now;
        private static DateTime lastTimeTracked = DateTime.Now;
        private static float distanceToBeStuck = 1.0f;
        private static int stuckTimeThresh = 2000; // In ms (so 1 second esentially)

        private static void ResetInfo()
        {
            lastPosition = Vector3.Zero;
            lastTimeTracked = DateTime.Now;
        }

        private static unsafe void CheckifIsStuck()
        {
            var currentPos = Player.Position;
            var timeSinceLastChecked = (DateTime.Now - lastTimeTracked).TotalMilliseconds;
            var navmeshStartTime = (DateTime.Now - whenStarted).TotalMilliseconds;

            if (Vector3.Distance(currentPos, lastPosition) > distanceToBeStuck)
            {
                ResetInfo();
            }

            if (timeSinceLastChecked >= stuckTimeThresh && navmeshStartTime >= stuckTimeThresh)
            {
                if (EzThrottler.Throttle("Using jump action"))
                    ActionManager.Instance()->UseAction(ActionType.GeneralAction, 2);
                ResetInfo();
            }
        }
    }
}
