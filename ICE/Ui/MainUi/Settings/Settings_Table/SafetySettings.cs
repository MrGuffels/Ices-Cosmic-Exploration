using Dalamud.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.MainUi.Settings.Settings_Table
{
    internal class SafetySettings
    {
        private static bool rejectUnknownYesNo = C.RejectUnknownYesno;
        private static bool delayGrabMission = C.DelayGrabMission;
        private static int delayAmount = C.DelayIncrease;
        private static bool delayCraft = C.DelayCraft;
        private static int delayCraftAmount = C.DelayCraftIncrease;

        public static void Draw()
        {
            ImGuiEx.IconWithText(FontAwesomeIcon.ExclamationTriangle, "Safety Settings");
            ImGui.Dummy(new Vector2(0, 5));

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
                if (ImGui.SliderInt("ms###Crafting", ref delayCraftAmount, 500, 5000))
                {
                    if (C.DelayCraftIncrease != delayCraftAmount)
                    {
                        C.DelayCraftIncrease = delayCraftAmount;
                        C.SaveDebounced();
                    }
                }
            }
            bool unstuckEnabled = C.JumpIfStuck || C.RetargetIfStuck;
            if (ImGui.Checkbox("If stuck during nav movement:", ref unstuckEnabled))
            {
                if (unstuckEnabled)
                    C.JumpIfStuck = true;
                else
                {
                    C.JumpIfStuck = false;
                    C.RetargetIfStuck = false;
                }
                C.Save();
            }
            ImGui.SameLine();
            ImGuiEx.HelpMarker(
                "When stuck during navmesh movement for the configured delay:\n" +
                "- Jump: attempts to jump over the obstacle\n" +
                "- Retarget: stops and re-pathfinds to the destination (re-randomizes if enabled)");
            if (!unstuckEnabled) ImGui.BeginDisabled();
            if (ImGui.RadioButton("Jump", C.JumpIfStuck && !C.RetargetIfStuck))
            {
                C.JumpIfStuck = true;
                C.RetargetIfStuck = false;
                C.Save();
            }
            ImGui.SameLine();
            if (ImGui.RadioButton("Retarget", C.RetargetIfStuck))
            {
                C.RetargetIfStuck = true;
                C.JumpIfStuck = false;
                C.Save();
            }
            ImGui.SameLine();
            ImGui.Text("after");
            ImGui.SameLine();
            int stuckDelay = C.StuckDelayMs;
            ImGui.SetNextItemWidth(100);
            if (ImGui.SliderInt("ms stuck###StuckDelay", ref stuckDelay, 500, 3000))
            {
                if (C.StuckDelayMs != stuckDelay)
                {
                    C.StuckDelayMs = stuckDelay;
                    C.SaveDebounced();
                }
            }
            if (!unstuckEnabled) ImGui.EndDisabled();
            ImGui.Dummy(Vector2.Zero);

            int delayRelic = C.DelayPostRelic;
            ImGui.SetNextItemWidth(150);
            if (ImGui.SliderInt("Delay Post Relic Turnin", ref delayRelic, 0, 5000))
            {
                C.DelayPostRelic = delayRelic;
                C.SaveDebounced();
            }
        }
    }
}
