using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_Craft
    {
        public static void Enqueue()
        {
            P.TaskManager.Enqueue(() => Task_CheckScore.Crafts());
        }
    }
}
