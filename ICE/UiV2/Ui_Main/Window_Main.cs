using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ICE.UiV2.Ui_Main.Sub_Windows;

namespace ICE.UiV2.Ui_Main
{
    internal class Window_Main : Window
    {
        public Window_Main() :
#if DEBUG
        base($"Ice's Cosmic Exploration {P.GetType().Assembly.GetName().Version} [Debug Build] ###ICEMainWindow2")
#else
        base($"Ice's Cosmic Exploration {P.GetType().Assembly.GetName().Version} ###ICEMainWindow2")
#endif
        {
            Flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
            SizeConstraints = new()
            {
                MinimumSize = new Vector2(100, 100),
                MaximumSize = new Vector2(4000, 4000),
            };
            TitleBarButtons.Add(new() { ShowTooltip = () => ImGui.SetTooltip("♥ Ko-fi (Buy me an ice coffee)"), Icon = FontAwesomeIcon.Heart, IconOffset = new(1, 1), Click = _ => GenericHelpers.ShellStart("https://ko-fi.com/ice643269") });

            P.windowSystem.AddWindow(this);
        }

        public void Dispose()
        {
            P.windowSystem.RemoveWindow(this);
        }

        public override void Draw()
        {
            var scale = ImGuiHelpers.GlobalScaleSafe;

            using (var mainTable = ImRaii.Table("Main Window_Main Settings", 2, ImGuiTableFlags.SizingFixedFit))
            {
                ImGui.TableSetupColumn("Mission Control Panel", ImGuiTableColumnFlags.WidthFixed, 201 * scale);
                ImGui.TableSetupColumn("Detailed Window View", ImGuiTableColumnFlags.WidthStretch);

                ImGui.TableNextRow();

                ImGui.TableSetColumnIndex(0);
                Child_Selectable.Draw();

                ImGui.TableNextColumn();
                Child_SelectedView.Draw();
            }
        }
    }
}
