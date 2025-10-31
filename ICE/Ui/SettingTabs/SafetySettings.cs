using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.SettingTabs
{
    internal class SafetySettings
    {
        private static bool animationLockAbandon = C.AnimationLockAbandon;
        private static bool stopOnAbort = C.StopOnAbort;
        private static bool rejectUnknownYesNo = C.RejectUnknownYesno;
        private static bool delayGrabMission = C.DelayGrabMission;
        private static int delayAmount = C.DelayIncrease;
        private static bool delayCraft = C.DelayCraft;
        private static int delayCraftAmount = C.DelayCraftIncrease;

        public static void Draw()
        {
            if (ImGui.Checkbox("Ignore non-Cosmic prompts", ref rejectUnknownYesNo))
            {
                C.RejectUnknownYesno = rejectUnknownYesNo;
                C.Save();
            }
            ImGuiEx.HelpMarker(
                "Warning! This is a safety feature to avoid joining random parties!\n" +
                "If you you uncheck this, YOU WILL JOIN random party invites.\n" +
                "You have been warned. Disable at your own risk."
            );
            if (ImGui.Checkbox("Add delay to mission menu", ref delayGrabMission))
            {
                C.DelayGrabMission = delayGrabMission;
                C.Save();
            }
            ImGuiEx.HelpMarker(
                "This is here for safety! If you want to decrease the delay between missions be my guest.\n" +
                "Safety is around... 250? If you're having animation locks you can absolutely increase it higher\n" +
                "Or if you're feeling daredevil. Lower it. I'm not your dad (will tell dad jokes though.");
            if (delayGrabMission)
            {
                ImGui.SetNextItemWidth(150);
                ImGui.SameLine();
                if (ImGui.SliderInt("ms###Mission", ref delayAmount, 0, 1000))
                {
                    if (C.DelayIncrease != delayAmount)
                    {
                        C.DelayIncrease = delayAmount;
                        C.SaveDebounced();
                    }
                }
            }
            if (ImGui.Checkbox("Add delay to crafting menu", ref delayCraft))
            {
                C.DelayCraft = delayCraft;
                C.Save();
            }
            ImGuiEx.HelpMarker(
                "This is here for safety! If you want to decrease the delay before turnin be my guest.\n" +
                "Safety is around... 2500? If you're having animation locks you can absolutely increase it higher\n" +
                "Or if you're feeling daredevil. Lower it. I'm not your dad (will tell dad jokes though.");
            if (delayCraft)
            {
                ImGui.SetNextItemWidth(150);
                ImGui.SameLine();
                if (ImGui.SliderInt("ms###Crafting", ref delayCraftAmount, 6000, 20000))
                {
                    if (C.DelayCraftIncrease != delayCraftAmount)
                    {
                        C.DelayCraftIncrease = delayCraftAmount;
                        C.SaveDebounced();
                    }
                }
            }
        }
    }
}
