using Dalamud.Interface.Utility.Raii;
using ICE.UiV2.Ui_Main.Sub_Windows.Ui_Selected.Tab_CosmicHelp;
using System.Collections.Generic;

namespace ICE.UiV2.Ui_Main.Sub_Windows
{
    internal class Child_SelectedView
    {
        private static readonly Dictionary<string, Action> SelectedView = new()
        {
            ["modeSelect_MissionSetup"] = () => MissionSetup.Draw(),
        };

        public static void Draw()
        {
            var region = ImGui.GetContentRegionAvail();
            var selectedWindow = C.MainUi_SelectedWindow;

            using (var selectedView = ImRaii.Child("Selected View Child Window", region, true))
            {
                if (SelectedView.TryGetValue(selectedWindow, out var drawAction))
                {
                    drawAction();
                }
                else
                {
                    ImGui.Text($"Woops. We're missing a window: {selectedWindow}");
                }
            }
        }
    }
}
