using Dalamud.Interface.Utility.Raii;
using ICE.Ui.MainUi;
using ICE.Ui.MainUi.HelpFolder;
using ICE.Ui.MainUi.ModeSelect;
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

            SelectableSidebar.Draw();
            
            ImGui.SameLine(0, 5);

            var windowSizeRemaining = ImGui.GetContentRegionAvail();
            using (var mainBody = ImRaii.Child("mainBody_WindowV3", windowSizeRemaining))
            {
                if (!mainBody.Success) return;
                MainBody();
            }
        }

        private static void MainBody()
        {
            switch (SelectableSidebar.currentSelection)
            {
                case "modeSelect_Standard": modeSelect_Standard.Draw(); break;
                case "helpSelect_Logs": helpSelect_Logs.Draw_Helper(); break;
                default: ImGui.Text("Hehe"); break;
            }
        }
    }
}
