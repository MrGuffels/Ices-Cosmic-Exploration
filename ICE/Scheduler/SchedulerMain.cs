using ECommons.GameHelpers;
using static ICE.Enums.IceState;

namespace ICE.Scheduler
{
    internal static unsafe class SchedulerMain
    {
        internal static bool EnablePlugin()
        {
            State = Start;
            return true;
        }
        internal static bool DisablePlugin()
        {
            IceLogging.Debug("Stopping the plugin state", "[Schedular - Disable Plugin]");
            P.TaskManager.Abort();
            State = IceState.Idle;
            if (P.Navmesh.IsRunning() && P.Navmesh.IsReady())
                P.Navmesh.Stop();
            return true;
        }

        // Debug only settings
        internal static bool DebugOOMMain = false;
        internal static bool DebugOOMSub = false;

        internal static IceState State = Idle;
        internal static MissionAttributes MissionState = MissionAttributes.None;

        internal static void Tick()
        {
            if (Throttles.GenericThrottle && P.TaskManager.NumQueuedTasks == 0 && State != Idle)
            {
                switch (State)
                {
                    case Gambling:
                        Task_Gamba.TryHandleGamba();
                        break;
                    case Start:
                        Task_CheckState.Enqueue();
                        break;
                    case Repair:
                        Task_Repair.Enqueue();
                        break;
                    case Spiritbond:
                        Task_Spiritbond.TryExtractMateria();
                        break;
                    case GrabMission:
                        Task_FindMission.Enqueue();
                        break;
                    case AbandonMission:
                        Task_AbandonMission.Enqueue();
                        break;
                    case ExecutingMission:
                        Task_ExecuteMission.Enqueue();
                        break;
                    case ScoreCheck:
                        Task_CheckScore.Enqueue();
                        break;
                    case TurninMission:
                        Task_TurninMission.Enqueue();
                        break;
                    case Craft:
                        Task_Craft.Enqueue();
                        break;
                    case Gather:
                        Task_Gather.Enqueue();
                        break;
                    // case Fish:
                    case ManualMode:
                        Task_Manual.Enqueue();
                        break;
                    default:
                        DisablePlugin();
                        break;
                }
            }
        }

        /*
        public static void EnqueueResumeCheck()
        {
            // Start the check by making the state idle, this clears all flags.
            State = Idle;
            if (CosmicHelper.CurrentLunarMission != 0)
            {
                // Mission was not 0, which means there's currently one active.
                if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && !missionInfo.IsAddonReady)
                {
                    CosmicHelper.OpenStellarMission();
                    State = Start;
                    return; // Makes sure that none of the other flags can be set, and returns back to start until the mission information is open
                }
                else
                {
                    // Checking for the mission, seeing if it's timed out. If so, then initiating the timeout sequence (aka trying to turnin/abort)
                    if (MissionHandler.IsMissionTimedOut())
                        State |= AbortInProgress;

                    // Updating the flags for the state. 
                    TaskMissionFind.UpdateStateFlags();
                    if (State.HasFlag(Craft) && P.Artisan.IsBusy())
                        State |= Waiting;
                    State |= ScoringMission;
                }
            }
            else if (AddonHelper.IsAddonActive("WKSLottery"))
                State = Gambling;
            else
                State = GrabMission;
            if (AnimationLockAbandonState || (!(AddonHelper.IsAddonActive("WKSRecipeNotebook") || AddonHelper.IsAddonActive("RecipeNote")) && Svc.Condition[ConditionFlag.Crafting] && Svc.Condition[ConditionFlag.PreparingToCraft]))
                State |= AnimationLock;
        }
        */
    }
}