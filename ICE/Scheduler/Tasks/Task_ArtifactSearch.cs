using ECommons.GameHelpers;
using ICE.Utilities.Cosmic_Helper;
using System;
using System.Collections.Generic;
using System.Text;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal class Task_ArtifactSearch
    {
        public static void EnqueueBuy()
        {

        }

        private static bool? Drone_PathToVendor()
        {
            string handle = "[Task_Artifact: PathTo]";
            var zoneId = Player.Territory.RowId;
            var npcEntry = NpcData.MoonNpcs[zoneId].Where(x => x.type == NpcData.NpcType.Drone).FirstOrDefault();

            if (npcEntry != null)
            {
                Vector3 randomPos = NpcData.GetRandomPointInCircle(npcEntry.Location_Circle, 0.5f);
                if (!Task_NavmeshMove.Task_NavTo(randomPos, distance: 6, npcLoc: npcEntry.Location_Npc).Value)
                {
                    if (EzThrottler.Throttle("Repair move message", 1000))
                        IceLogging.Verbose($"Pathing to repair NPC. Current distance: {Player.DistanceTo(npcEntry.Location_Npc)}", handle);
                }
                else
                {
                    IceLogging.Debug("We're close enough to the repair npc! Continuing on", handle);
                    return true;
                }
            }
            else
            {
                if (EzThrottler.Throttle("Error message: NPC", 5000))
                    IceLogging.Error("Hey! We don't have this npc coded yet, which means I forgot bout it, could you let me know\n" +
                                     $"Planet Territory ID: {Player.Territory.RowId}", handle);
            }
            return false;
        }
        private static bool? TalkToDroneNpc()
        {
            if (GenericHelpers.TryGetAddonMaster<SelectIconString>("SelectIconString", out var iconString) && iconString.IsAddonReady)
            {
                IceLogging.Info("Icon string is visible! Time to shop");
                return true;
            }
            else if (GenericHelpers.TryGetAddonMaster<Talk>("Talk", out var talk) && talk.IsAddonReady)
            {
                if (EzThrottler.Throttle("Throttle talking"))
                    talk.Click();
            }
            else
            {
                var droneInfo = NpcData.MoonNpcs[Player.Territory.RowId].Where(x => x.type == NpcData.NpcType.Drone).FirstOrDefault().NpcId;

                Utils.TryGetObjectByDataId(droneInfo, out var droneNpc);
                if (EzThrottler.Throttle("Interacting with researchingway"))
                {
                    Utils.TargetgameObject(droneNpc);
                    Utils.InteractWithObject(droneNpc);
                }
            }

            return false;
        }
        private static bool? SelectShop()
        {
            if (GenericHelpers.TryGetAddonMaster<ShopExchangeCurrency>("ShopExchangeCurrency", out var shopExchange) && shopExchange.IsAddonReady)
            {
                IceLogging.Debug("Shop Exchange Currency Addon is Ready!");
                return true;
            }
            else if (GenericHelpers.TryGetAddonMaster<SelectIconString>("SelectIconString", out var iconString) && iconString.IsAddonReady)
            {
                if (EzThrottler.Throttle("Selecting Materia Selection"))
                {
                    var select = iconString.Entries[0];
                    IceLogging.Verbose($"Selecting: {select.Text}");
                    select.Select();
                }
            }

            return false;
        }
        private static void BuyDroneCrates()
        {
            if (GenericHelpers.TryGetAddonMaster<SelectYesno>("SelectYesno", out var YesNo) && YesNo.IsAddonReady)
            {
                if (EzThrottler.Throttle("Buy Drone Boxes"))
                {
                    YesNo.Yes();
                }
            }
            else if (GenericHelpers.TryGetAddonMaster<ShopExchangeCurrency>("ShopExchangeCurrency", out var shopExchange) && shopExchange.IsAddonReady)
            {
                // There's only 1 item here. So we just need to make sure we have enough to buy it
                var currency = shopExchange.CurrencyAmount;
                var item = shopExchange.BasicShopItems[0];

                if (item.CostAmount <= currency)
                {

                }
            }
        }
    }
}
