using Dalamud.Interface.Utility.Raii;
using ICE.Ui.MainUi;
using ICE.Ui.MainUi.HelpFolder;
using ICE.Ui.MainUi.ModeSelect;
using ICE.Ui.MainUi.Settings;
using ICE.Ui.MainUi.Settings.Settings_Table;
using ICE.Ui.SettingTabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui
{
    internal class DummyWindow : Window
    {
        public DummyWindow() : base($"Ice Dummy Window")
        {
            P.windowSystem.AddWindow( this );
        }

        public void Dispose()
        {
            P.windowSystem.RemoveWindow( this );
        }

        public override void Draw()
        {
            using var style = ImRaii.PushStyle(ImGuiStyleVar.ChildRounding, 10).Push(ImGuiStyleVar.ChildBorderSize, 1);

            SelectableSidebar.Draw();
            
            ImGui.SameLine(0, 5);

            var windowSizeRemaining = ImGui.GetContentRegionAvail();
            using (var mainBody = ImRaii.Child("mainBody_WindowV3", windowSizeRemaining, true))
            {
                if (!mainBody.Success) return;
                MainBody();
            }
        }

        private static void MainBody()
        {
            switch (SelectableSidebar.currentSelection)
            {
                // Cosmic Helper
                case "modeSelect_Standard":
                    if (C.ShowCompletionWindow)
                    {
                        C.ShowCompletionWindow = false;
                        C.Save();
                    }
                    modeSelect_Standard.Draw();
                    break;
                case "modeSelect_Completion":
                    if (!C.ShowCompletionWindow)
                    {
                        C.ShowCompletionWindow = true;
                        C.Save();
                    }
                    modeSelect_Standard.Draw();
                    break;

                // Settings
                case "setting_StopWhen": 
                    StopWhen.Draw(); 
                    break;
                case "setting_GatheringProfile":
                    GatherSettings.Draw();
                    break;
                case "setting_MissionPriority":
                    Priority_Settings.Draw();
                    break;
                case "helpSelect_AllSettings":
                    helpSelect_AllSettings.Draw();
                    break;


                // Hub Activities
                case "hubActivities_CreditShopping": 
                    ShoppingTab.Draw(); 
                    break;
                case "hubActivites_GambaSetting": 
                    GambaWheel.Draw(); 
                    break;

                // Help Section
                case "helpSelect_Requirements":
                    helpSelect_Required.Draw();
                    break;
                case "helpSelect_Logs":
                    helpSelect_Logs.Draw_Helper();
                    break;

                default: 
                    ImGui.Text("Hehe"); 
                    break;
            }
        }

        public static List<uint> GetOnlyPreviousMissionsRecursive(uint missionId)
        {
            if (!CosmicHelper.SheetMissionDict.TryGetValue(missionId, out var missionInfo) || missionInfo.PreviousMissions.Contains(0))
                return [];

            var chain = GetOnlyPreviousMissionsRecursive(missionInfo.PreviousMissions.First());
            chain.Add(missionInfo.PreviousMissions.First());
            return chain;
        }
        private List<uint> GetOnlyNextMissionsRecursive(uint missionId)
        {
            uint? nextMissionId = CosmicHelper.SheetMissionDict
                .Where(m => m.Value.PreviousMissions.First() == missionId)
                .Select(m => (uint?)m.Key)
                .FirstOrDefault();

            if (!nextMissionId.HasValue)
                return [];

            var chain = new List<uint> { nextMissionId.Value };
            chain.AddRange(GetOnlyNextMissionsRecursive(nextMissionId.Value));
            return chain;
        }
    }
}
