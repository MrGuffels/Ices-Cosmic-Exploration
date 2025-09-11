using ECommons.GameHelpers;
using static ICE.Enums.IceState;

namespace ICE.Scheduler
{
    internal static unsafe class SchedulerMain
    {
        internal static bool EnablePlugin()
        {
            State = Start;
            StartClassJob = Player.Job;
            return true;
        }
        internal static bool DisablePlugin()
        {
            IceLogging.Debug("Stopping the plugin state", "[Schedular - Disable Plugin]");
            P.TaskManager.Abort();
            State = IceState.Idle;
            StartClassJob = Job.ADV;
            if (P.Navmesh.IsRunning() && P.Navmesh.IsReady())
                P.Navmesh.Stop();
            return true;
        }

        // Debug only settings
        internal static bool DebugOOMMain = false;
        internal static bool DebugOOMSub = false;

        internal static IceState State = Idle;
        internal static MissionAttributes MissionState = MissionAttributes.None;
        internal static Job StartClassJob = Job.ADV;

        // <summary>
        // Main Scheduler. General flow is to raise flags as necessary and resolve them based on priority:
        // Idle - do nothing.
        // On start, check what state we are in and set flags as needed.
        // If Craft && Waiting - Wait for craft loop to exit. Raise ScoringMission + lower Waiting on exit.
        // If ScoringMission flag is set, run score check, reset state to Idle or Grab if turned in, otherwise unset ScoringMission flag (Returning us to Cradt/Gather/Fish)
        // If AnimationLock flag is set, attempt unstuck, unset flag after.
        // If Gamba flag is set, run gamba, reset to Idle.
        // If GrabMission && Waiting - wait for non-standard mission conditions to be true before resuming.
        // If GrabMission flag is set, get a mission. Once obtained raise Craft/Gather/Fish flags and ExecutingMission flag. Otherwise if no standards - raise Waiting. If no missions at all - set state to Idle.
        // If Manual is set on a mission - Zen. (Also Fish, for now.)
        // If Gather && ExecutingMission flag is set, run gathering. If DualClass - lower Gather flag on enough mats. Raise ScoringMission flag on completion of a loop.
        // If Craft && ExecutingMission flag is set, run crafting. If DualClass - raise Gather flag on if not enough mats. Raise ScoringMission flag on completion of a loop.
        // </summary>
        internal static void Tick()
        {
            if (Throttles.GenericThrottle && P.TaskManager.Tasks.Count == 0 && State != Idle)
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