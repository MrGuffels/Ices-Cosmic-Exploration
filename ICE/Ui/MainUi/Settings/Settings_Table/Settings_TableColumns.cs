using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.MainUi.Settings.Settings_Table;

public static class Settings_TableColumns
{
    private static string[] missionSortOptions = ["Id", "Name", "Cosmo Credits", "Lunar Credits", "Exp I", "Exp II", "Exp III", "Exp IV", "Exp V", "Map Location"];

    public static void ColumnSettings()
    {
        int missionSelectedOption = C.TableSortOption;
        if (ImGui.BeginCombo("Sort By", missionSortOptions[missionSelectedOption]))
        {
            for (int i = 0; i < missionSortOptions.Length; i++)
            {
                bool isSelected = (i == missionSelectedOption);
                if (ImGui.Selectable(missionSortOptions[i], isSelected))
                {
                    missionSelectedOption = i;
                }
                if (isSelected)
                {
                    ImGui.SetItemDefaultFocus();
                }
                if (missionSelectedOption != C.TableSortOption)
                {
                    C.TableSortOption = missionSelectedOption;
                    C.Save();
                }
            }
            ImGui.EndCombo();
        }

        bool hideUnsupported = C.HideUnsupportedMissions;
        if (ImGui.Checkbox("Hide Unsupported Missions", ref hideUnsupported))
        {
            C.HideUnsupportedMissions = hideUnsupported;
            C.Save();
        }

        bool showExtraInfo = C.ShowExtraMissionInfo;
        if (ImGui.Checkbox("Show Extra Mission Info Side-Window", ref showExtraInfo))
        {
            C.ShowExtraMissionInfo = showExtraInfo;
            C.Save();
        }

        bool autoShowToken = C.Auto_ShowTokens;
        if (ImGui.Checkbox("Auto Hide/Show Planet Tokens", ref autoShowToken))
        {
            C.Auto_ShowTokens = autoShowToken;
            C.Save();
        }



        bool showManualMode = C.ShowManualMode;
        if (ImGui.Checkbox("Show Manual Mode Column", ref showManualMode))
        {
            C.ShowManualMode = showManualMode;
            if (!showManualMode)
            {
                foreach (var mission in C.MissionConfig)
                {
                    mission.Value.ManualMode = false;
                }
            }
            C.Save();
        }
        ImGuiEx.HelpMarker("Only enable this if you want plan on doing missions YOURSELF. AND NOT AUTOMATING IT. " +
                           "Or if you're letting a different plugin do all the automating of turning in, craftings, gathering... and not letting I.C.E. handle interacting with those plugins");
    }
    public static void GeneralMissionSettings()
    {
        bool onlyGrabMission = C.OnlyGrabMission;
        if (ImGui.Checkbox($"Only grab mission", ref onlyGrabMission))
        {
            C.OnlyGrabMission = onlyGrabMission;
            C.Save();
        }

        bool removeGold = C.RemoveAfterGold;
        if (ImGui.Checkbox("Remove Mission Upon Gold Completion", ref removeGold))
        {
            C.RemoveAfterGold = removeGold;
            C.Save();
        }

        ImGui.Checkbox("Stop after current mission", ref Mission_Settings.StopAfterCurrent);
        bool relicTurnin = C.TurninRelic;
        if (ImGui.Checkbox($"Turnin if relic is complete##RelicTurnin_GeneralSetting", ref relicTurnin))
        {
            if (relicTurnin)
                C.GrindProvisionals = false;

            C.TurninRelic = relicTurnin;
            C.Save();
        }
        ImGui.SameLine();
        ImGui.TextDisabled("?");
        if (ImGui.IsItemHovered())
        {
            ImGui.SetTooltip("THIS IS YOUR HEADS UP ON HOW THIS WORKS. If I change this in the future, this tooltip will also change.\n" +
                             "1: This will check for your current CLASS [not menu class, actual current class] for relic turnin.\n" +
                             "2: You must not have the tool eqipped for this to run full auto. \n" +
                             "\t- This is due to the fact that I cba coding this in at this time. (might change my mind in the future *shrugs*)\n" +
                             "3: This will take prio over \"Stop @ Relic Turnin\", in the sense that if you have both enabled, it will turnin vs stop. And continue about it's day\n" +
                             "4: If you're on a crafting class, it will return you back to the stop you were crafting post turnin. \n" +
                             "\t- This is optional, you can disable it at your own free will, I just like this so I can just go back to an isolated area of my choosing");
        }
    }
}
