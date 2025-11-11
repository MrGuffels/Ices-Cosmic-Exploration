using ECommons.GameHelpers;
using ICE.Utilities.Cosmic_Helper;
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
                new(PathToRelicNPC, "Heading to the relic NPC for turnin"),
                new(TalkToResearchWay, "Talk to researchway"),
                new(SelectReport, "Selecting Report"),
                new(SelectRelicClass, "Selecting the class to turnin on")
            );
        }

        public static bool? PathToRelicNPC()
        {
            var zoneId = Player.Territory;
            var npcEntry = NpcData.MoonNpcs[zoneId].Where(x => x.type == NpcData.NpcType.Relic).FirstOrDefault();

            if (Player.DistanceTo(npcEntry.NpcLocation) <= 6.75f)
            {
                if (P.Navmesh.IsRunning())
                {
                    if (Player.DistanceTo(npcEntry.NpcLocation) > C.MountRadius)
                    {

                    }

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

                        Vector3 randomPoint = RandomUtil.GetRandomPointInBounds(npcEntry.Corner1, npcEntry.Corner2, npcEntry.Corner3, npcEntry.Corner4, npcEntry.NpcLocation.Y);
                        P.Navmesh.PathfindAndMoveTo(randomPoint, false);
                    }
                }
            }

            return false;
        }

        public static bool? TalkToResearchWay()
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

        public static bool? SelectReport()
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

        public static bool? SelectRelicClass()
        {
            uint selectedClass = Player.JobId - 8;
            uint selectedEntry = 0;
            foreach (var job in C.ClassesUnlocked)
            {
                if (job.Value)
                    selectedEntry += 1;
                if (job.Key == Player.JobId)
                {
                    selectedClass = selectedEntry - 1;
                    break;
                }

            }


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
                if (EzThrottler.Throttle("Clicking the talk dialog", 50))
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
