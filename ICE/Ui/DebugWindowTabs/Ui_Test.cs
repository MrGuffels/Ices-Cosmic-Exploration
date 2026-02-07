using FFXIVClientStructs.FFXIV.Client.Game;
using ICE.UiV2.Ui_Main.Sub_Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class UI_Test
    {
        public static unsafe void Draw()
        {
            Child_Selectable.Draw();
        }
    }

    
}
