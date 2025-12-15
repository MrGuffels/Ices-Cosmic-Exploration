using Dalamud.Game.ClientState.Conditions;
using ECommons.GameHelpers;
using ICE.Utilities.Cosmic_Helper;

namespace ICE.Scheduler.Tasks
{
    internal class Task_NavmeshMove
    {
        public static bool? Task_NavTo(Vector3 pos, bool waitForBusy = true, float distance = 2.0f, bool stayMounted = false)
        {
            bool usingCosmoliner = Svc.Condition[ConditionFlag.Unknown101];
            bool mounted = Player.Mounted;
            bool inMission = CosmicHelper.CurrentLunarMission == 0;
            float minMountDistance = C.MountRadius;
            float dismountDistance = C.DismountRadius;

            if (!P.Navmesh.Installed)
            {
                IceLogging.Info("We seem to be missing navmesh... so we're just going to exit here");
                return true;
            }
            else if (P.Navmesh.IsRunning())
            {
                if (!mounted && Player.DistanceTo(pos) > minMountDistance)
                {
                    if (inMission && C.UseMountInMission)
                    {
                        if (EzThrottler.Throttle("Using mount"))
                            Utils.MountAction();
                    }
                    else if (!inMission && C.UseMountOutsideMission)
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
                    if (EzThrottler.Throttle("Throttle message tehe"))
                        IceLogging.Verbose("We're currently moving, and we were told to wait for us to NOT be moving so... yeah, we waiting");

                    return false;
                }
                else if (!waitForBusy && Player.DistanceTo(pos) <= distance)
                {
                    if (EzThrottler.Throttle("Telling navmesh to stop"))
                    {
                        IceLogging.Debug("We're within stopping distance, so stopping navmesh");
                        P.Navmesh.Stop();
                    }
                }

                if (usingCosmoliner)
                {
                    if (EzThrottler.Throttle("Telling navmesh to stop"))
                        P.Navmesh.Stop();
                }
            }
            else if (!P.Navmesh.IsReady())
            {
                if (EzThrottler.Throttle("Waiting on navmesh", 1000))
                {
                    var navProgress = P.Navmesh.BuildProgress();
                    IceLogging.Debug($"Waiting for navmesh to finish building. Currently at: {navProgress:N2}");
                }
            }
            else if (!P.Navmesh.IsRunning())
            {
                // We're here, which means it's time to start fresh for navmesh
                if (usingCosmoliner)
                {
                    // We don't want navmesh/any checks to be running while using the cosmoliner, so just exiting out
                    return false;
                }

                if (Player.DistanceTo(pos) < distance)
                {
                    if (mounted && !stayMounted)
                    {
                        if (EzThrottler.Throttle("Dismounting the mount"))
                        {
                            Utils.Dismount();
                        }
                        return false;
                    }
                    else if (Player.IsJumping)
                    {
                        return false;
                    }
                    else
                    {
                        IceLogging.Debug("We've met the distance threshold, continuing on");
                        return true;
                    }
                }
                else
                {
                    if (EzThrottler.Throttle("Telling navmesh to start"))
                    {
                        P.Navmesh.PathfindAndMoveTo(pos, false);
                    }
                }
            }

            return false;
        }

        public static bool NavToDestination(Vector3 pos, bool waitForBusy = true, float distance = 2.0f, bool stayMounted = false)
        {
            bool usingCosmoliner = Svc.Condition[ConditionFlag.Unknown101];
            bool mounted = Player.Mounted;
            bool inMission = CosmicHelper.CurrentLunarMission == 0;
            float minMountDistance = C.MountRadius;
            float dismountDistance = C.DismountRadius;

            if (!P.Navmesh.Installed)
            {
                IceLogging.Info("We seem to be missing navmesh... so we're just going to exit here");
                return true;
            }
            else if (P.Navmesh.IsRunning())
            {
                if (!mounted && Player.DistanceTo(pos) > minMountDistance)
                {
                    if (inMission && C.UseMountInMission)
                    {
                        if (EzThrottler.Throttle("Using mount"))
                            Utils.MountAction();
                    }
                    else if (!inMission && C.UseMountOutsideMission)
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
                        IceLogging.Verbose("We're currently moving, and we were told to wait for us to NOT be moving so... yeah, we waiting");

                    return false;
                }
                else if (!waitForBusy && Player.DistanceTo(pos) <= distance)
                {
                    if (EzThrottler.Throttle("Telling navmesh to stop"))
                    {
                        IceLogging.Debug("We're within stopping distance, so stopping navmesh");
                        P.Navmesh.Stop();
                    }
                }

                if (usingCosmoliner)
                {
                    if (EzThrottler.Throttle("Telling navmesh to stop"))
                        P.Navmesh.Stop();
                }
            }
            else if (!P.Navmesh.IsReady())
            {
                if (EzThrottler.Throttle("Waiting on navmesh", 1000))
                {
                    var navProgress = P.Navmesh.BuildProgress();
                    IceLogging.Debug($"Waiting for navmesh to finish building. Currently at: {navProgress:N2}");
                }
            }
            else if (!P.Navmesh.IsRunning())
            {
                // We're here, which means it's time to start fresh for navmesh
                if (usingCosmoliner)
                {
                    // We don't want navmesh/any checks to be running while using the cosmoliner, so just exiting out
                    return false;
                }

                if (Player.DistanceTo(pos) < distance)
                {
                    if (mounted && !stayMounted)
                    {
                        if (EzThrottler.Throttle("Dismounting the mount"))
                        {
                            Utils.Dismount();
                        }
                        return false;
                    }
                    else if (Player.IsJumping)
                    {
                        return false;
                    }
                    else
                    {
                        IceLogging.Debug("We've met the distance threshold, continuing on");
                        return true;
                    }
                }
                else
                {
                    if (EzThrottler.Throttle("Telling navmesh to start"))
                    {
                        IceLogging.DestinationLogs.Log(pos);
                        P.Navmesh.PathfindAndMoveTo(pos, false);
                    }
                }
            }

            return false;
        }
    }
}
