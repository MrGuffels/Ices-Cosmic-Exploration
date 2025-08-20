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
        public static void Draw()
        {
            if (P.TaskManager.MaxTasks != 0)
                ImGui.Text($"Task Name: {P.TaskManager.CurrentTask.Name} | Number of task: {P.TaskManager.MaxTasks}");
            else
                ImGui.Text($"No Task Active");
            ImGui.Text($"Counter: {Counter}");

            if (ImGui.Button("Task Find Mission"))
            {
                P.TaskManager.Enqueue(TestInsert, "Testing Adding 1");
            }
            if (ImGui.Button("Add Delay"))
            {
                P.TaskManager.InsertDelay(2000);
                P.TaskManager.Insert(ChatTest, "Sending chat message");
            }
            if (ImGui.Button("Insert Reset"))
            {
                P.TaskManager.Insert(() => Counter = 0, "Resetting Counter");
            }
            if (ImGui.Button("Reset Counter"))
            {
                Counter = 0;
            }
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
