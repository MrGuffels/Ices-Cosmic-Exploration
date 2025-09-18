using ECommons.GameHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_DualClass
    {
        public static void Enqueue()
        {
            Task_CheckScore.Enqueue();
            if (CosmicHelper.CurrentMissionInfo.Jobs.Contains(18))
            {
                // Need to insert the stuff for fishing dual class bullshit here
            }
            else
            {
                // First, check to see if we have the resource

                // MIN/BTN dual class stuff here
                if (GenericHelpers.TryGetAddonMaster<Gathering>("Gathering", out var gather) && gather.IsAddonReady)
                {
                    P.TaskManager.Enqueue(() => Task_Gather.GatheringInteraction(), Utils.TaskConfig);
                }
            }
        }
    }
}
