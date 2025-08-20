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

        public static void Draw()
        {
            bool runningTask = P.TaskManager.NumQueuedTasks != 0;
            ImGui.Text($"Running task: {runningTask}");

            if (ImGui.Button("Stop Task"))
            {
                P.TaskManager.Abort();
            }

            ImGui.SetNextItemWidth(100);
            ImGui.InputUInt("Mission", ref mission);

            if (ImGui.Button("Path to mission"))
            {
                P.TaskManager.Enqueue(() => Task_FindMission.Navmesh_MoveToMission(mission), "Testing Moveto Task", Utils.TaskConfig);
            }
            bool navmeshRunning = P.Navmesh.IsRunning();
            bool escelator = Svc.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.Unknown101];
            ImGui.Text($"Navmesh Running: {navmeshRunning}");
            ImGui.Text($"Is taking escelator: {escelator}");
        }

        private static int Counter = 0;

        private static bool? TestInsert()
        {
            if (EzThrottler.Throttle("Adding 1"))
            {
                Counter += 1;
            }
            if (Counter > 5)
                return true;

            return false;
        }

        private static bool? ChatTest()
        {
            Svc.Chat.Print("Test Chat message");
            return true;
        }
    }
}
