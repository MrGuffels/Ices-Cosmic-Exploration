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
                if (usingCosmoliner)
                    P.Navmesh.Stop();

                if (C.JumpIfStuck)
                {
                    if (CheckAndHandleStuck())
                    {
                        return false; // Let the stuck handler take control
                    }
                }

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
                        IceLogging.Verbose("Telling navmesh to start pathfinding");
                        IceLogging.DestinationLogs.Log(pos);
                        P.Navmesh.SetTolerance(0.25f);
                        P.Navmesh.PathfindAndMoveTo(pos, false);
                    }
                }
            }

            return false;
        }

        public static bool? Task_NavMoveTo(Vector3 pos, bool waitForBusy = true, float distance = 2.0f, bool stayOnMount = false, Vector3? npcLoc = null)
        {
            string tag = "[Task: Navmesh Moveto]";

            bool usingCosmoliner = Svc.Condition[ConditionFlag.Unknown101];
            bool mounted = Player.Mounted;
            bool inMission = CosmicHelper.CurrentLunarMission != 0;
            float minMountDistance = C.MountRadius;
            float dismountDistance = C.DismountRadius;

            bool useInMission = C.UseMountInMission && inMission;
            bool useOutsideMission = C.UseMountOutsideMission && !inMission;
            bool useMount = useInMission || useOutsideMission;

            if (P.Navmesh.Installed)
            {
                if (!P.Navmesh.IsReady())
                {
                    if (EzThrottler.Throttle("Navmesh progress", 1000))
                    {
                        IceLogging.Debug($"Waiting for navmesh to be ready. Progress: {P.Navmesh.BuildProgress():N2}");
                    }
                }
                else
                {
                    if (P.Navmesh.IsRunning())
                    {
                        if (npcLoc != null)
                        {
                            var distanceToNpc = Player.DistanceTo(npcLoc.Value);

                            if (distanceToNpc <= dismountDistance && !stayOnMount)
                            {
                                if (EzThrottler.Throttle("Dismounting the mount"))
                                {
                                    Utils.Dismount();
                                }
                            }
                            else if (distanceToNpc > minMountDistance && !mounted && useMount)
                            {
                                if (EzThrottler.Throttle("Using mount"))
                                    Utils.MountAction();
                            }
                            else if (usingCosmoliner)
                            {
                                if (EzThrottler.Throttle("On Cosmoliner Tehe"))
                                {
                                    IceLogging.Verbose($"We're currently on a cosmoliner, going to just stop and wait");
                                    P.Navmesh.Stop();
                                }
                            }
                            else if (Player.IsJumping)
                            {
                                if (EzThrottler.Throttle("Player is jumping"))
                                {
                                    IceLogging.Verbose($"We're currently jumping... just to be on the safe side we gonna just stop navmesh so we don't do some jank movement");
                                    P.Navmesh.Stop();
                                }
                            }
                            else if (distanceToNpc <= distance)
                            {

                            }
                        }
                    }
                }
            }

            return false;
        }

        private static Vector3 _lastPosition = Vector3.Zero;
        private static DateTime _lastPositionChange = DateTime.Now;
        private static int _stuckAttempts = 0;
        private const float STUCK_DISTANCE_THRESHOLD = 1.0f; // Consider stuck if moved less than this
        private const int STUCK_TIME_THRESHOLD = 3000; // Time in ms before considering stuck

        private static unsafe bool CheckAndHandleStuck()
        {
            var currentPos = Player.Position;
            var timeSinceLastChange = (DateTime.Now - _lastPositionChange).TotalMilliseconds;

            // Check if we've moved significantly
            if (Vector3.Distance(currentPos, _lastPosition) > STUCK_DISTANCE_THRESHOLD)
            {
                // We moved, reset tracking
                ResetInfo();
                return false;
            }

            // We haven't moved much, check if we've been stuck long enough
            if (timeSinceLastChange > STUCK_TIME_THRESHOLD)
            {
                _stuckAttempts++;

                if (_stuckAttempts == 1)
                {
                    // First attempt: try jumping
                    if (EzThrottler.Throttle("Stuck - attempting jump", 1000))
                    {
                        IceLogging.Warning("Player appears stuck, attempting to jump");
                        ActionManager.Instance()->UseAction(ActionType.GeneralAction, 2);
                        _lastPositionChange = DateTime.Now; // Give it time to work
                    }
                    return true;
                }
                else if (_stuckAttempts >= 2)
                {
                    // Second attempt: stop navmesh after jump had time to execute
                    if (EzThrottler.Throttle("Stuck - stopping navmesh", 1000))
                    {
                        IceLogging.Warning("Player still stuck after jump attempt, stopping navmesh");
                        P.Navmesh.Stop();
                        _stuckAttempts = 0; // Reset for next time
                        _lastPositionChange = DateTime.Now;
                    }
                    return true;
                }
            }

            return false;
        }

        public static void ResetInfo()
        {
            _lastPosition = Vector3.Zero;
            _lastPositionChange = DateTime.Now;
            _stuckAttempts = 0;
        }
    }
}
