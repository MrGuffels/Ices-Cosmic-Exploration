using ICE.Scheduler.Tasks.OldTask;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class NewTask
    {
        private static uint mission = 0;
        private static int frameDelay = 4;

        public static void Draw()
        {
            bool runningTask = P.TaskManager.NumQueuedTasks != 0;
            ImGui.Text($"Running task: {runningTask}");
            string currentTask = P.TaskManager.CurrentTask?.Name ?? "";
            ImGui.Text($"Current task running: {currentTask}");
            if (ImGui.Button("Set State to Idle"))
            {
                SchedulerMain.State = IceState.Idle; 
            }

            if (ImGui.Button("Stop Task"))
            {
                P.TaskManager.Tasks.Clear();
                P.TaskManager.Abort();
            }

            ImGui.SetNextItemWidth(100);
            ImGui.InputUInt("Mission", ref mission);

            if (ImGui.Button("Path to mission"))
            {
                P.TaskManager.Enqueue(() => Task_FindMission.Navmesh_MoveToMission(mission), "Testing Moveto Task", Utils.TaskConfig);
            }
            ImGui.InputInt("Frame Delay", ref frameDelay);
            if (ImGui.Button("Running Mission Test"))
            {
                Task_FindMission.Enqueue();
            }
            if (ImGui.Button("Abandon Mission"))
            {
                Task_AbandonMission.Enqueue();
            }
            if (ImGui.Button("Path to repair NPC"))
            {
                P.TaskManager.Enqueue(() => Task_Repair.PathToRepair(), "Pathing to repair NPC");
            }
            if (ImGui.Button("Test Repair Function"))
            {
                Task_Repair.Enqueue();
            }
        }

        private static int Counter = 0;
    }
}
