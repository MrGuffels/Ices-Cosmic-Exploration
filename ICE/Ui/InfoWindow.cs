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
            DrawSection("What is this plugin?",
            new[]
            {
                "The jack of all trades to helping you automate your moon crimes, and helping you get the relics/missions that you need. It helps you do the following:",
                "Select which missions you would like to farm out, and reroll until it eventually gets it available.",
                "View the completion status of missions and see what've you done, completed, and gotten a gold rating on.",
                "Allows you to help automate farming of relics. There are some other plugin dependencies required for this if you don't have them already (more info is down below with each class section)",
                "Are you farming score? Select which missions you would like to do, and let it run. I do not advice running this overnight, that is a good way to get yourself banned. Especially it being a public area",
            });
            DrawSection("Crafting",
            [
                "Artisan is required for crafting to be fully automated",
                "All you need to do is make sure to have the missions that you want to run selected, and let it run.", 
                "Artisan is also has a built in function to repair at a certain point, you can let that run if you choose to let it repair, or can use the built in repair option that will run before missions.",
                "Some things to note: If you are using the setting \"Raphael Solver\", make sure that you set the timeout for generating macros accordingly.",
                "Also, unless you turn on \"Generate on Expert Recipies\", it will not generate on expert/master recipies. (These look very similar to normal ones, but they have special buffs that rotate in while crafting."
            ],
            new[]
            {
                new SectionButton 
                {
                    Label = "Install Artisan Repo", 
                    OnClick = () => InstallArtisanRepo(), 
                    IsVisible = () => !DalamudReflector.HasRepo("https://love.puni.sh/ment.json")
                },
                new SectionButton
                {
                    Label = isInstallingArtisan ? "Installing..." : "Install Artisan",
                    OnClick = () => _ = InstallArtisan(),
                    IsVisible = () => !Utils.HasPlugin("Artisan") && !isInstallingArtisan
                }
            });
            DrawSection("Gathering [BTN + MIN]",
            [
                "Gathering for botanist and miner are actually built into the plugin!",
                "All you need to do is setup a gathering profile (this can be done through the settings -> Gathering tab) to what kind of mission you are doing. And make sure that profile is selected for the mission.",
                "Collectables are also ran in through the plugin as well. The main thing you need for this to work is Vnavmesh, this will allow you to run between the gathering nodes at full automation",
            ],
            new[]
            {
                new SectionButton
                {
                    Label = "Install Navmesh Repo",
                    OnClick = () => InstallNavmeshRepo(),
                    IsVisible = () => !DalamudReflector.HasRepo("https://puni.sh/api/repository/veyn")
                },
                new SectionButton
                {
                    Label = isInstallingArtisan ? "Installing..." : "Install Navmesh",
                    OnClick = () => _ = InstallNavmesh(),
                    IsVisible = () => !Utils.HasPlugin("vnavmesh") && !isInstallingNavmesh
                }
            });
            DrawSection("Gathering [FISHER]",
            [
                "Fishing for cosmic... is pain. There's no getting around it. It's rng at it's finest. And you're not guarenteed gold score either.",
                "You need AutoHook installed. This is used to help automate the whole process.",
                "For missions that are supported, you don't actually need to do anything, the presets are built into the plugin and will automatically import/delete themselves when done.",
                "If you would like to use your own preset, just make sure to enable it in the \"Select Preset\" button, and type the name of the preset that you are using",
                "Navmesh is also required to make sure that you are in a correct spot of the fishing hole to be able to fish.",
                "If you need help on auto-accepting collectables, there's a guide also attached as a button below (it'll open a link to the wiki page for this."
            ],
            new[]
            {
                new SectionButton
                {
                    Label = "Install Autohook Repo",
                    OnClick = () => InstallAutoHookRepo(),
                    IsVisible = () => !DalamudReflector.HasRepo("https://love.puni.sh/ment.json")
                },
                new SectionButton
                {
                    Label = isInstallingArtisan ? "Installing..." : "Install Autohook",
                    OnClick = () => _ = InstallAutoHook(),
                    IsVisible = () => !Utils.HasPlugin("AutoHook") && !isInstallingAutoHook
                },
                new SectionButton
                {
                    Label = "How to auto accept collectables",
                    OnClick = () => GenericHelpers.ShellStart("https://github.com/PunishXIV/AutoHook/blob/main/AcceptCollectable.md")
                }
            });
        }

        private bool isInstallingArtisan = false;
        private void InstallArtisanRepo()
        {
            DalamudReflector.AddRepo("https://love.puni.sh/ment.json", true);
            DalamudReflector.SaveDalamudConfig();
        }
        private async Task InstallArtisan()
        {
            if (isInstallingArtisan) return; // Already installing

            isInstallingArtisan = true;
            try
            {
                await DalamudReflector.AddPlugin("https://love.puni.sh/ment.json", "Artisan");
                DalamudReflector.SaveDalamudConfig();
            }
            finally
            {
                isInstallingArtisan = false;
            }
        }

        private bool isInstallingNavmesh = false;
        private void InstallNavmeshRepo()
        {
            DalamudReflector.AddRepo("https://puni.sh/api/repository/veyn", true);
            DalamudReflector.SaveDalamudConfig();
        }

        private async Task InstallNavmesh()
        {
            if (isInstallingNavmesh) return; // Already installing

            isInstallingNavmesh = true;
            try
            {
                await DalamudReflector.AddPlugin("https://puni.sh/api/repository/veyn", "vnavmesh");
                DalamudReflector.SaveDalamudConfig();
            }
            finally
            {
                isInstallingNavmesh = false;
            }
        }

        private bool isInstallingAutoHook = false;

        private void InstallAutoHookRepo()
        {
            DalamudReflector.AddRepo("https://love.puni.sh/ment.json", true);
            DalamudReflector.SaveDalamudConfig();
        }

        private async Task InstallAutoHook()
        {
            if (isInstallingAutoHook) return; // Already installing

            isInstallingAutoHook = true;
            try
            {
                await DalamudReflector.AddPlugin("https://love.puni.sh/ment.json", "AutoHook");
                DalamudReflector.SaveDalamudConfig();
            }
            finally
            {
                isInstallingAutoHook = false;
            }
        }

        public class SectionButton
        {
            public string Label { get; set; }
            public Action OnClick { get; set; }
            public Func<bool> IsVisible { get; set; } // Returns true to show the button
        }

        private void DrawSection(string title, string[] bulletPoints, SectionButton[] buttons = null, string[] RequiredPlugins = null)
        {
            // Get the current window background color
            uint windowBg = ImGui.GetColorU32(ImGuiCol.WindowBg);

            // Convert to Vector4 for easier manipulation
            var bgColor = ImGui.ColorConvertU32ToFloat4(windowBg);

            // Darken it by a factor (e.g., 0.85 = 85% brightness)
            bgColor.X *= 0.85f; // R
            bgColor.Y *= 0.85f; // G
            bgColor.Z *= 0.85f; // B
                                // bgColor.W stays the same (alpha)

            // Convert back to uint
            uint darkerBg = ImGui.ColorConvertFloat4ToU32(bgColor);

            using var colorBackground = ImRaii.PushColor(ImGuiCol.ChildBg, darkerBg);

            // For border, you could also derive it from the current border or keep it as-is
            // Option 1: Use current border color
            uint currentBorder = ImGui.GetColorU32(ImGuiCol.Border);
            using var colorBorder = ImRaii.PushColor(ImGuiCol.Border, currentBorder);

            // Calculate the height needed
            float height = ImGui.GetStyle().WindowPadding.Y; // Top padding

            // Title height
            height += ImGui.CalcTextSize(title).Y;
            height += ImGui.GetStyle().ItemSpacing.Y;

            // Separator (roughly 1 pixel + spacing)
            height += 1 + ImGui.GetStyle().ItemSpacing.Y;

            // Calculate bullet point heights with proper wrapping width
            float availableWidth = ImGui.GetContentRegionAvail().X - ImGui.GetStyle().WindowPadding.X * 2;
            float bulletIndent = ImGui.GetFontSize() + ImGui.GetStyle().ItemSpacing.X; // Bullet size + spacing

            foreach (var point in bulletPoints)
            {
                var textSize = ImGui.CalcTextSize(point, true, availableWidth - bulletIndent);
                height += textSize.Y;
                height += ImGui.GetStyle().ItemSpacing.Y;
            }

            // Add button height if any visible buttons exist
            if (buttons != null && buttons.Any(b => b.IsVisible?.Invoke() ?? true))
            {
                height += ImGui.GetStyle().ItemSpacing.Y; // Extra spacing before buttons
                height += ImGui.GetFrameHeight(); // Button height
                height += ImGui.GetStyle().ItemSpacing.Y;
            }

            // Bottom padding
            height += ImGui.GetStyle().WindowPadding.Y;

            using var section = ImRaii.Child(title, new(0, height), false,
                ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse |
                ImGuiWindowFlags.AlwaysUseWindowPadding);
            if (!section)
                return;

            using (ImRaii.PushColor(ImGuiCol.Text, Utils.ToUintABGR(EColor.YellowBright)))
            {
                ImGui.TextUnformatted(title);
            }

            ImGui.Separator();

            foreach (var point in bulletPoints)
            {
                ImGui.Bullet();
                ImGui.SameLine();
                ImGui.TextWrapped(point);
            }

            // Draw buttons at the bottom
            if (buttons != null && buttons.Length > 0)
            {
                ImGui.Spacing();

                bool firstButton = true;
                for (int i = 0; i < buttons.Length; i++)
                {
                    // Check if button should be visible
                    bool isVisible = buttons[i].IsVisible?.Invoke() ?? true;

                    if (!isVisible)
                        continue;

                    if (!firstButton)
                        ImGui.SameLine();

                    if (ImGui.Button($"{buttons[i].Label}##{title}_{i}"))
                    {
                        buttons[i].OnClick?.Invoke();
                    }

                    firstButton = false;
                }
            }
        }
    }
}
