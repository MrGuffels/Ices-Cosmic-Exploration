using ECommons.GameHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal class Task_RelicTurnin
    {

        public static void Enqueue()
        {
            P.TaskManager.EnqueueMulti
            (
                new(TalkToResearchWay, "Talk to researchway"),
                new(SelectReport, "Selecting Report"),
                new(SelectRelicClass, "Selecting the class to turnin on")
            );
        }

        private static bool? ReturnToBase()
        {
            return false;
        }

        private static bool? TalkToResearchWay()
        {
            if (GenericHelpers.TryGetAddonMaster<SelectString>("SelectString", out var selectString) && selectString.IsAddonReady)
            {
                IceLogging.Info("Talk to researchway complete");
                return true;
            }
            else if (GenericHelpers.TryGetAddonMaster<Talk>("Talk", out var talk) && talk.IsAddonReady)
            {
                if (EzThrottler.Throttle("Clicking the talk dialog", 100))
                {
                    talk.Click();
                }
            }

            var researchId = NpcData.MoonNpcs[Player.Territory].Where(x => x.type == NpcData.NpcType.Relic).FirstOrDefault().NpcId;

            Utils.TryGetObjectByDataId(researchId, out var researchNpc);
            if (EzThrottler.Throttle("Interacting with researchingway"))
            {
                Utils.TargetgameObject(researchNpc);
                Utils.InteractWithObject(researchNpc);
            }

            return false;
        }

        private static bool? SelectReport()
        {
            if (GenericHelpers.TryGetAddonMaster<SelectIconString>("SelectIconString", out var selectIconString) && selectIconString.IsAddonReady)
            {
                IceLogging.Info("We're onto selecting the class to turnin, woo!");
                return true;
            }
            else if (GenericHelpers.TryGetAddonMaster<SelectString>("SelectString", out var selectString) && selectString.IsAddonReady)
            {
                if (EzThrottler.Throttle("Selecting the research one"))
                    selectString.Entries[0].Select();
            }

            return false;
        }

        private static bool? SelectRelicClass()
        {
            uint selectedClass = Player.JobId - 8;
            if (GenericHelpers.TryGetAddonMaster<SelectIconString>("SelectIconString", out var selectIconString) && selectIconString.IsAddonReady)
            {
                if (EzThrottler.Throttle($"Selecting jobId: {Player.JobId}"))
                    selectIconString.Entries[selectedClass].Select();
            }
            else if (GenericHelpers.TryGetAddonMaster<SelectYesno>("SelectYesno", out var selectYesno) && selectYesno.IsAddonReady)
            {
                if (EzThrottler.Throttle("Selecting yes for turnin"))
                    selectYesno.Yes();
            }
            else if (GenericHelpers.TryGetAddonMaster<Talk>("Talk", out var talk) && talk.IsAddonReady)
            {
                if (EzThrottler.Throttle("Clicking the talk dialog"))
                {
                    talk.Click();
                }
            }
            else if (!Player.IsBusy)
            {
                IceLogging.Info("No longer busy talking to researchingway, to we're done");
                return true;
            }

            return false;

        }
    }
}
