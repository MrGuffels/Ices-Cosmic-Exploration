using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Scheduler
{
    internal static class Task_HubActivities
    {
        public static bool RepairNpc = false;
        public static bool RelicTurnin = false;
        public static bool CosmoBuy = false;
        public static bool CanGamba = false;

        public static void Enqueue()
        {
            P.TaskManager.Enqueue(Task_RelicTurnin.RegisterCraftingPosition, "Registering crafting position for later");
            P.TaskManager.Enqueue(Task_Repair.HubCheck, "Checking to see if we're in hub area");
            if (RepairNpc)
            {
                P.TaskManager.EnqueueMulti
                (
                    new(() => SchedulerMain.State = IceState.Repair, "Changing the state to repair"),
                    new(Task_Repair.PathToRepair, "Pathing to the repair NPC"),
                    new(Task_Repair.RepairAtNpc, "Repairing at the NPC Vendor"),
                    new(Task_Repair.CloseRepair, "Closing the repair window")
                );
            }
            if (RelicTurnin)
            {
                P.TaskManager.Enqueue(() => SchedulerMain.State = IceState.Repair);
                Task_RelicTurnin.Enqueue();
            }
            if (CosmoBuy)
            {
                P.TaskManager.Enqueue(() => SchedulerMain.State = IceState.Shopping);
                Task_BuyCosmoItems.Enqueue();
            }
            if (CanGamba)
            {
                P.TaskManager.Enqueue(() => SchedulerMain.State = IceState.Gambling);
                Task_Gamba.Enqueue();
            }
            P.TaskManager.Enqueue(Task_RelicTurnin.PathBackToCraftingSpot, "Pathing back to our crafting spot");
            P.TaskManager.Enqueue(() =>
            {
                RepairNpc = false;
                RelicTurnin = false;
                CosmoBuy = false;
                CanGamba = false;
            });
            P.TaskManager.Enqueue(() => SchedulerMain.State = IceState.GrabMission, "Swapping back to start");
        }
    }
}
