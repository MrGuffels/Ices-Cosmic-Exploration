namespace ICE.Scheduler.Tasks.OldTask
{
    internal class TaskManualMode
    {
        public static void ZenMode()
        {
            if (CosmicHelper.CurrentLunarMission == 0)
            {
                SchedulerMain.State = IceState.GrabMission;
            }
            if (!C.MissionConfig.SingleOrDefault(x => x.Key == CosmicHelper.CurrentLunarMission).Value.ManualMode && !C.OnlyGrabMission)
            {
                SchedulerMain.State &= ~IceState.ManualMode;
            }
        }
    }
}
