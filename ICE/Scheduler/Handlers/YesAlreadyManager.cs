using ECommons.EzSharedDataManager;
using System.Collections.Generic;

namespace ICE.Scheduler.Handlers
{
    internal static class YesAlreadyManager
    {
        private static bool WasChanged = false;
        internal static void Tick()
        {
            var currentState = SchedulerMain.State;
            bool shouldDisable = SchedulerMain.State == IceState.GrabMission || SchedulerMain.State == IceState.AbandonMission;

            if (WasChanged)
            {
                if (!shouldDisable)
                {
                    WasChanged = false;
                    Unlock();
                    if (EzThrottler.Throttle("Unlocking YesAlready", 5000))
                        IceLogging.Debug($"YesAlready unlocked");
                }
            }
            else
            {
                if (shouldDisable)
                {
                    WasChanged = true;
                    Lock();
                    if (EzThrottler.Throttle("Locking YesAlready", 5000))
                        IceLogging.Debug($"YesAlready locked");
                }
            }
        }
        internal static void Lock()
        {
            if (EzSharedData.TryGet<HashSet<string>>("YesAlready.StopRequests", out var data))
            {
                data.Add(Name);
            }
        }

        internal static void Unlock()
        {
            if (EzSharedData.TryGet<HashSet<string>>("YesAlready.StopRequests", out var data))
            {
                data.Remove(Name);
            }
        }
    }
}
