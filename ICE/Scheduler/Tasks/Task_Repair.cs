using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Component.GUI;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_Repair
    {
        public static void Enqueue()
        {
            if (PlayerHelper.NeedsRepair(C.RepairPercent))
            {
                var currentJob = PlayerHelper.GetClassJobId().Value;

                if (C.RepairAtVendor)
                {
                    P.TaskManager.EnqueueMulti(
                        new(HubCheck, "Checking to see if we're in hub area"),
                        new(PathToRepair, "Pathing to the repair NPC"),
                        new(RepairAtNpc, "Repairing at the NPC Vendor"),
                        new(CloseRepair, "Closing the repair window")
                        );
                }
                else if ((C.SelfRepairGather && CosmicHelper.GatheringJobList.Contains(currentJob)) 
                       || (C.SelfRepairCrafter && CosmicHelper.CrafterJobList.Contains(currentJob)))
                {
                    P.TaskManager.EnqueueMulti
                    (
                        new(OpenSelfRepair, "Opening the self repair window"),
                        new(SelfRepair, "Executing the self repair"),
                        new(CloseRepair, "Closing Self Repair")
                    );
                }
                SchedulerMain.State = IceState.GrabMission;
            }
        }
        public static unsafe bool? HubCheck()
        {
            Vector2 HubCenter = Vector2.Zero;
            Vector2 PlayerPos = new Vector2(Player.Position.Z, Player.Position.Z);

            if (Player.DistanceTo(HubCenter) < 45)
            {
                IceLogging.Debug("Player is in the range of the main hub area right now", "[Vendor Repair Check]");
                return true;
            }
            else
            {
                //Not within the vicinity of the hub area, time to return
                if (!Player.IsBusy)
                {
                    if (EzThrottler.Throttle("Returning back to the moon base"))
                        ActionManager.Instance()->UseAction(ActionType.GeneralAction, 26);
                }
            }

            return false;
        }
        public static unsafe bool? PathToRepair()
        {
            var npcEntry = NpcInfo.NpcLibrary[1052610];

            if (Player.DistanceTo(npcEntry.NpcLocation) <= 6.75f)
            {
                if (P.Navmesh.IsRunning())
                {
                    if (Player.DistanceTo(npcEntry.NpcLocation) < 5)
                    {
                        IceLogging.Debug("Pathing to NPC has reached the distance thresh, stopping");
                        P.Navmesh.Stop();
                        return true;
                    }
                }
                else
                {
                    IceLogging.Debug($"Distance to the npc is correct, commending repair");
                    return true;
                }
            }
            else
            {
                if (!P.Navmesh.IsRunning())
                {
                    if (EzThrottler.Throttle("Pathing to repair NPC"))
                    {
                        IceLogging.Debug($"Pathing to: {npcEntry.Name}");

                        Vector3 randomPoint = RandomUtil.GetRandomPointInBounds(npcEntry.BoxCorner1.X, npcEntry.BoxCorner2.X, npcEntry.BoxCorner1.Y, npcEntry.BoxCorner2.Y, npcEntry.NpcLocation.Y);
                        P.Navmesh.PathfindAndMoveTo(randomPoint, false);
                    }
                }
            }

            return false;
        }
        public static unsafe bool? RepairAtNpc()
        {
            IGameObject? gameObject = null;
            Utils.TryGetObjectByDataId(1052610, out gameObject);
            var currentTarget = Svc.Targets.Target;

            if (!PlayerHelper.NeedsRepair(99.9f))
            {
                IceLogging.Debug("Repair Complete! Finishing task and closing window");
                return true;
            }
            else if (GenericHelpers.TryGetAddonMaster<Repair>("Repair", out var repair) && repair.IsAddonReady)
            {
                if (PlayerHelper.NeedsRepair(99.9f))
                {
                    if (GenericHelpers.TryGetAddonMaster<SelectYesno>("SelectYesno", out var Yesno) && Yesno.IsAddonReady)
                    {
                        if (FrameThrottler.Throttle("Saying yes to the gil"))
                            Yesno.Yes();
                    }
                    else if (EzThrottler.Throttle("Firing off repair request", 300))
                    {
                        IceLogging.Debug("Repair Callback", "[Self Repair Task]");
                        repair.RepairAll();
                    }
                }
            }
            else if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("SelectIconString", out var iconString) && GenericHelpers.IsAddonReady(iconString))
            {
                if (FrameThrottler.Throttle("Firing off repair string"))
                {
                    IceLogging.Debug("Selecting repair from vendor", "[Self Repair Task]");
                    ECommons.Automation.Callback.Fire(iconString, true, 6);
                }
            }
            else if (currentTarget != null && currentTarget.DataId == 1052610)
            {
                if (EzThrottler.Throttle("Attempting to interact with repair NPC"))
                {
                    Utils.InteractWithObject(gameObject);
                    IceLogging.Info($"Interacting with: {gameObject.DataId}");
                }
            }
            else
            {
                if (EzThrottler.Throttle("Attempting to target the repair NPC"))
                    Utils.TargetgameObject(gameObject);
            }

            return false;
        }
        private unsafe static bool OpenSelfRepair()
        {
            if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("Repair", out var x) && GenericHelpers.IsAddonReady(x))
            {
                return true;
            }

            if (EzThrottler.Throttle("Opening Self Repair", 1000))
                ActionManager.Instance()->UseAction(ActionType.GeneralAction, 6);
            return false;
        }
        private unsafe static bool SelfRepair()
        {
            if (!PlayerHelper.NeedsRepair(C.RepairPercent))
            {
                return true;
            }
            else if (Svc.Condition[ConditionFlag.Mounted])
            {
                if (EzThrottler.Throttle("Attempting to dismount for repairing"))
                {
                    IceLogging.Debug("Dismounting for self repair", "[Self Repair Task]");
                    ActionManager.Instance()->UseAction(ActionType.GeneralAction, 9);
                }
            }
            else if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("SelectYesno", out var addon) && GenericHelpers.IsAddonReady(addon))
            {
                if (FrameThrottler.Throttle("SelectYesnoThrottle", 300))
                {
                    IceLogging.Debug("SelectYesno Callback", "Self Repair Task");
                    ECommons.Automation.Callback.Fire(addon, true, 0);
                }
            }
            else if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("Repair", out var addon2) && GenericHelpers.IsAddonReady(addon2))
            {
                if (FrameThrottler.Throttle("Firing off repair request", 300))
                {
                    IceLogging.Debug("Repair Callback", "[Self Repair Task]");
                    ECommons.Automation.Callback.Fire(addon2, true, 0);
                }
            }
            return false;
        }
        private unsafe static bool CloseRepair()
        {
            if (GenericHelpers.TryGetAddonMaster<SelectYesno>("SelectYesno", out var Yesno) && Yesno.IsAddonReady)
            {
                if (FrameThrottler.Throttle("Closing surprise repair window"))
                {
                    ECommons.Automation.Callback.Fire(Yesno.Base, true, -1);
                }
            }
            else if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("Repair", out var repairWindow))
            {
                if (GenericHelpers.IsAddonReady(repairWindow))
                {
                    if (EzThrottler.Throttle("Attempting to close out the repair window", 300))
                    {
                        IceLogging.Debug("Closing the repair window", "[Repair Task]");
                        ECommons.Automation.Callback.Fire(repairWindow, true, -1);
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}
