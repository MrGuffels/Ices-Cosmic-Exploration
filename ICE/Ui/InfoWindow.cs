using Dalamud.Interface.Utility.Raii;
using ECommons.Reflection;
using FFXIVClientStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICE.Ui
{
    internal class InfoWindow : Window
    {
        public InfoWindow() : base($"Ice's Cosmic Exploration - Info")
        {
            Flags = ImGuiWindowFlags.None;
            SizeConstraints = new()
            {
                MinimumSize = new Vector2(100, 100),
                MaximumSize = new Vector2(4000, 4000),
            };

            P.windowSystem.AddWindow(this);
            AllowPinning = true;
            AllowClickthrough = true;
        }

        public void Dispose()
        {
            P.windowSystem.RemoveWindow(this);
        }

        public override void Draw()
        {

        }
    }
}
