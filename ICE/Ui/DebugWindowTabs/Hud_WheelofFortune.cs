using ICE.Scheduler.Tasks.OldTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Hud_WheelofFortune
    {
        public static void Draw()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSLottery>("WKSLottery", out var lotto) && lotto.IsAddonReady)
            {
                ImGui.Text($"Lottery addon is visible!");

                if (ImGui.Button($"Left wheel select"))
                {
                    TaskGamba.SelectWheelLeft(lotto);
                }
                ImGui.SameLine();

                if (ImGui.Button($"Right wheel select"))
                {
                    TaskGamba.SelectWheelRight(lotto);
                }

                ImGui.SameLine();
                if (ImGui.Button($"Confirm"))
                {
                    lotto.ConfirmButton();
                }

                if (ImGui.Button($"Auto Gamba (Once)"))
                {
                    TaskGamba.TryHandleGamba();
                }

                ImGui.Text($"Items in left wheel");
                foreach (var l in lotto.LeftWheelItems)
                {
                    ImGui.Text($"Name: {l.itemName} | Id: {l.itemId} | Amount: {l.itemAmount}");
                }

                ImGui.Spacing();
                foreach (var m in lotto.RightWheelItems)
                {
                    ImGui.Text($"Name: {m.itemName} | Id: {m.itemId} | Amount: {m.itemAmount}");
                }
            }
            else
            {
                ImGui.Text("Waiting for \"WKSLottery\" to be visible");
            }
        }
    }
}
