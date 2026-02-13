using Dalamud.Interface;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.GameHelpers;
using ICE.Ui.MainUi.ModeSelect_Modes;
using ICE.Utilities.ImGuiTools;
using SharpDX.Direct2D1.Effects;
using System.Collections.Generic;
using System.Reflection;

namespace ICE.Ui.MainUi
{
    internal class SelectableSidebar
    {
        public static void Draw()
        {
            var scale = ImGuiHelpers.GlobalScaleSafe;
            int baseSize = 200;
            var scaledWidth = baseSize * scale;
            var height = ImGui.GetContentRegionAvail().Y;

            using (var MainUi_Sidebar = ImRaii.Child("MainUi_Sidebar", new Vector2(scaledWidth, height), true))
            {
                PluginIcon();

                // this is here to be running globally while window is loaded vs only while expanded
                bool autoSelectMoon = C.AutoSelectMoon;
                AutoSelectMoonUpdate(autoSelectMoon);

                if (ImGui_Ice.Sidebar_CollaspableHeader("Cosmic Helper", icon: FontAwesomeIcon.ListAlt))
                {
                    ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.List, "Mission Setup", "modeSelect_MissionSetup");
                    ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.Trophy, "Complete Overview", "modeSelect_Completion");
                    ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.ClipboardList, "Cosmic Agenda", "modeSelect_CosmicAgenda");
                }
                if (ImGui_Ice.Sidebar_CollaspableHeader("Settings", icon: FontAwesomeIcon.Cog))
                {
                    if (C.Show_StopWhen)
                        ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.Stop, "Stop When...", "setting_StopWhen");
                    if (C.Show_GatheringProfile)
                        ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.Leaf, "Gathering Profile", "setting_GatheringProfile");
                    if (C.Show_MissionPriority)
                        ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.SortAmountUp, "Mission Priority", "setting_MissionPriority");
                    if (C.Show_MiscSettings)
                        ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.UserCog, "Misc Settings", "setting_Misc");

                    ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.Cog, "All Settings", "helpSelect_AllSettings");
                }
                if (C.Show_HubActivities)
                {
                    if (ImGui_Ice.Sidebar_CollaspableHeader("Hub Activities", icon: FontAwesomeIcon.Home))
                    {
                        ImGui_Ice.DrawSelectable_Image(65112, "Credit Shopping", "hubActivities_CreditShopping");
                        ImGui_Ice.DrawSelectable_Image(65127, "Gambling Settings", "hubActivites_GambaSetting");
                        ImGui_Ice.DrawSelectable_Image(65138, "Dronebit Settings", "hubActivies_DroneSetting");
                    }
                }
                if (ImGui_Ice.Sidebar_CollaspableHeader("Planet Selection", FontAwesomeIcon.Moon))
                {
                    if (ImGui.Checkbox("Auto Select Moon", ref autoSelectMoon))
                    {
                        C.AutoSelectMoon = autoSelectMoon;
                        C.Save();
                    }
                    ImGui.Dummy(new(0, 3));

                    float iconSize = 26 * scale;
                    float iconSpacing = 4;
                    float leftOffset = 10f; // Simple offset from the current position

                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + leftOffset);

                    var moons = new (string Name, string Asset, Func<bool> GetEnabled, Action<bool> SetEnabled)[]
                    {
                            ("Sinus Ardorum", "ICE.Resources.Sinus_Ardorum.png", () => C.ShowSinusMissions, val => C.ShowSinusMissions = val),
                            ("Phaenna", "ICE.Resources.Phaenna.png", () => C.ShowPhaennaMissions, val => C.ShowPhaennaMissions = val),
                            ("Oizys", "ICE.Resources.Oizys.png", () => C.ShowOizysMissions, val => C.ShowOizysMissions = val)
                    };

                    for (int i = 0; i < moons.Length; i++)
                    {
                        if (i > 0) ImGui.SameLine(0, iconSpacing);

                        var moon = moons[i];
                        bool isEnabled = moon.GetEnabled();
                        var texture = Svc.Texture.GetFromManifestResource(Assembly.GetExecutingAssembly(), moon.Asset).GetWrapOrEmpty();

                        if (ImGui_Ice.DrawStyledImageButton(texture, new Vector2(iconSize, iconSize), isEnabled))
                        {
                            moon.SetEnabled(!isEnabled);
                            C.AutoSelectMoon = false;
                            C.Save();
                        }

                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip(moon.Name);
                        }
                    }
                }

                var currentClass = C.SelectedJob;
                var classIcon = ImGui_Ice.GetGreyscaleJob(currentClass);
                if (ImGui_Ice.Sidebar_CollaspableHeader("Select Class", imageTexture: classIcon))
                {
                    Dictionary<uint, string> ClassDict = new()
                    {
                        [8] = "CRP",
                        [9] = "BSM",
                        [10] = "ARM",
                        [11] = "GSM",
                        [12] = "LTW",
                        [13] = "WVR",
                        [14] = "ALC",
                        [15] = "CUL",
                        [16] = "MIN",
                        [17] = "BTN",
                        [18] = "FSH",
                    };
                    int itemsPerRow = 4;
                    int currentItem = 0;

                    float iconSize = 26 * scale;
                    float iconSpacing = 4;
                    float leftOffset = 10f; // Simple offset from the current position

                    for (uint i = 8; i < 19; ++i)
                    {
                        // Set cursor position at start of each new row
                        if (currentItem % itemsPerRow == 0)
                            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + leftOffset);

                        ImGui_Ice.DrawJobButtons(i, ClassDict[i]);

                        currentItem++;

                        if (currentItem % itemsPerRow != 0 && i != 18)
                            ImGui.SameLine(0, iconSpacing);
                    }
                }
                if (ImGui_Ice.Sidebar_CollaspableHeader("Current Tool XP", FontAwesomeIcon.ArrowUpRightDots))
                {
                    var image = CosmicHelper.JobIconDict[currentClass];
                    ImGui_Ice.Draw_ExpTable(currentClass);
                }
                if (ImGui_Ice.Sidebar_CollaspableHeader("Need Help?", FontAwesomeIcon.QuestionCircle))
                {
                    ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.QuestionCircle, "Plugin Requirements", "help_PluginInstall");
                    ImGui_Ice.DrawSelectable_Icon(FontAwesomeIcon.Book, "Plugin Logs", "help_PluginLogs");
                }
            }
        }
        private static void PluginIcon()
        {
            string PluginIcon = "ICE.Resources.Icon.png";

            // Image/Icon of the plugin
            var pluginIcon = Svc.Texture.GetFromManifestResource(Assembly.GetExecutingAssembly(), PluginIcon).GetWrapOrEmpty();

            if (pluginIcon != null)
            {
                // Getting the image size via the texture wrap (using the * after to manipulate the size cause, she big)
                Vector2 imageSize = new Vector2(pluginIcon.Width, pluginIcon.Height) * 0.35f;

                // Calculate the offset/centering here
                float sidebarWidth = ImGui.GetContentRegionAvail().X;
                float offsetX = (sidebarWidth - imageSize.X) / 2.0f;

                // Center and drawing the image now
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offsetX);
                ImGui.Image(pluginIcon.Handle, imageSize);
                if (ImGui.IsItemClicked())
                {
                    var random = new Random();
                    modeSelect_TableInfo.jokeId = random.Next(0, modeSelect_TableInfo.JokeList.Count - 1);
                    modeSelect_TableInfo.selectedMission = 0;
                }

                // Add spacing after image
                ImGui.Dummy(new Vector2(0, 10));
                ImGui.Separator();
                ImGui.Dummy(new Vector2(0, 10));
            }
        }
        private static void AutoSelectMoonUpdate(bool autoSelectMoon)
        {
            bool NeedsUpdate(bool sinus, bool phaenna, bool oizys)
            {
                return C.ShowSinusMissions != sinus ||
                       C.ShowPhaennaMissions != phaenna ||
                       C.ShowOizysMissions != oizys;
            }

            void SetMoonVisibility(bool sinus, bool phaenna, bool oizys)
            {
                C.ShowSinusMissions = sinus;
                C.ShowPhaennaMissions = phaenna;
                C.ShowOizysMissions = oizys;
                C.Save();
            }

            if (autoSelectMoon)
            {
                if (PlayerHelper.IsInSinusArdorum() && NeedsUpdate(true, false, false))
                    SetMoonVisibility(sinus: true, phaenna: false, oizys: false);
                else if (PlayerHelper.IsInPhaenna() && NeedsUpdate(false, true, false))
                    SetMoonVisibility(sinus: false, phaenna: true, oizys: false);
                else if (PlayerHelper.IsInOizys() && NeedsUpdate(false, false, true))
                    SetMoonVisibility(sinus: false, phaenna: false, oizys: true);
            }
        }
    }
}
