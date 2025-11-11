using Dalamud.Interface;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility.Raii;
using System.Collections.Generic;
using System.Reflection;

namespace ICE.Ui.MainUi
{
    internal class SelectableSidebar
    {
        private static Dictionary<string, bool> categoryStates = new Dictionary<string, bool>();

        private static string PluginIcon = "ICE.Resources.Icon.png";
        public static string currentSelection = "modeSelect_Standard";

        public static void Draw()
        {
            using var style = ImRaii.PushStyle(ImGuiStyleVar.ChildRounding, 10).Push(ImGuiStyleVar.ChildBorderSize, 1);

            if (ImGui.BeginChild("MainUi_Sidebar", new Vector2(200, -1), true))
            {
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

                    // Add spacing after image
                    ImGui.Dummy(new Vector2(0, 10));
                    ImGui.Separator();
                    ImGui.Dummy(new Vector2(0, 10));
                }

                if (DrawCategoryHeader(FontAwesomeIcon.ListAlt, "Mode Select", 4))
                {
                    DrawSelectableWithIcon(FontAwesomeIcon.List, "Standard", "modeSelect_Standard");
                    DrawSelectableWithIcon(FontAwesomeIcon.Cloud, "Provisional", "modeSelect_Provisional");
                    DrawSelectableWithIcon(FontAwesomeIcon.FlagCheckered, "Relic", "modeSelect_Relic");
                    DrawSelectableWithIcon(FontAwesomeIcon.Trophy, "Completion", "modeSelect_Completion");
                }
                if (DrawCategoryHeader(FontAwesomeIcon.Cog, "Settings", 4))
                {
                    DrawSelectableWithIcon(FontAwesomeIcon.Stop, "Stop When...", "setting_StopWhen");
                    DrawSelectableWithIcon(FontAwesomeIcon.Leaf, "Gathering Profile", "setting_GatheringProfile");
                    DrawSelectableWithIcon(FontAwesomeIcon.SortAmountUp, "Mission Priority", "setting_MissionPriority");
                    DrawSelectableWithIcon(FontAwesomeIcon.Book, "Ice Logs", "helpSelect_Logs");
                }
                if (DrawCategoryHeader(FontAwesomeIcon.Home, "Hub Activities", 2))
                {
                    DrawSelectableWithImage(65112, "Credit Shopping", "hubActivities_CreditShopping");
                    DrawSelectableWithImage(65127, "Gambling Settings", "hubActivites_GambaSetting");
                }
                if (DrawCategoryHeader(FontAwesomeIcon.ArrowUpRightDots, "Tool Relic XP"))
                {
                    Relic_XP.DrawXPBar("Test", 200, 500, new Vector2(180, 10));
                    Relic_XP.DrawXPBar("Test", 1000, 500, new Vector2(180, 10), 2000);
                }
            }
            ImGui.EndChild();
        }

        private static bool DrawCategoryHeader(FontAwesomeIcon icon, string label, int? badgeCount = null)
        {
            var drawList = ImGui.GetWindowDrawList();
            var cursorPos = ImGui.GetCursorScreenPos();

            // Get colors from current theme
            var headerColor = ImGui.GetColorU32(ImGuiCol.Header);
            var textColor = ImGui.GetColorU32(ImGuiCol.Text);
            var textDisabledColor = ImGui.GetColorU32(ImGuiCol.TextDisabled);

            float width = ImGui.GetContentRegionAvail().X;
            float height = 30;

            // Check if this category is expanded (default to true)
            string categoryId = label;
            if (!categoryStates.ContainsKey(categoryId))
                categoryStates[categoryId] = true;

            bool isExpanded = categoryStates[categoryId];

            // Check for click
            bool isHovered = ImGui.IsMouseHoveringRect(cursorPos,
                new Vector2(cursorPos.X + width, cursorPos.Y + height));
            bool isClicked = isHovered && ImGui.IsMouseClicked(ImGuiMouseButton.Left);

            if (isClicked)
            {
                categoryStates[categoryId] = !categoryStates[categoryId];
                isExpanded = categoryStates[categoryId];
            }

            // Change header color slightly on hover
            if (isHovered)
                headerColor = ImGui.GetColorU32(ImGuiCol.HeaderHovered);

            // Draw background rectangle WITH ROUNDED CORNERS
            drawList.AddRectFilled(cursorPos,
                new Vector2(cursorPos.X + width, cursorPos.Y + height),
                headerColor,
                5.0f);

            // Add left padding and draw icon
            ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 7);
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 8);

            ImGui.PushStyleColor(ImGuiCol.Text, textDisabledColor);
            ImGuiEx.Icon(icon);
            ImGui.PopStyleColor();

            ImGui.SameLine();
            ImGui.PushStyleColor(ImGuiCol.Text, textDisabledColor);
            ImGui.Text(label);
            ImGui.PopStyleColor();

            // Draw badge if count provided
            if (badgeCount.HasValue && badgeCount.Value > 0)
            {
                float badgeSize = 24;
                float rightPadding = 10;

                float badgeXPos = cursorPos.X + width - badgeSize - rightPadding;
                float badgeYPos = cursorPos.Y + (height / 2);

                var badgeColor = ImGui.GetColorU32(ImGuiCol.ButtonActive);
                var badgeCenter = new Vector2(badgeXPos + (badgeSize / 2), badgeYPos);

                drawList.AddCircleFilled(badgeCenter, 12, badgeColor);

                var numberStr = badgeCount.Value.ToString();
                var textSize = ImGui.CalcTextSize(numberStr);
                drawList.AddText(
                    new Vector2(badgeCenter.X - textSize.X / 2, badgeCenter.Y - textSize.Y / 2),
                    textColor,
                    numberStr);
            }

            ImGui.Dummy(new Vector2(0, 5));

            return isExpanded;
        }

        private static void DrawSelectableWithIcon(FontAwesomeIcon icon, string label, string id)
        {
            bool isSelected = currentSelection == id;

            // Change background color if selected
            if (isSelected)
            {
                ImGui.PushStyleColor(ImGuiCol.Header, ImGui.GetColorU32(ImGuiCol.HeaderActive));
            }

            // Indent for items under categories
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 16);

            float width = ImGui.GetContentRegionAvail().X;

            // Invisible selectable as the clickable area
            if (ImGui.Selectable($"##{id}", isSelected, ImGuiSelectableFlags.None, new Vector2(width, 25)))
            {
                currentSelection = id;
            }

            if (isSelected)
            {
                ImGui.PopStyleColor();

                // Draw colored bar on the left side
                var drawList = ImGui.GetWindowDrawList();
                var rectMin = ImGui.GetItemRectMin();
                var rectMax = ImGui.GetItemRectMax();

                // Draw a 3-4 pixel wide bar on the left
                drawList.AddRectFilled(
                    rectMin,
                    new Vector2(rectMin.X + 3, rectMax.Y),
                    ImGui.GetColorU32(new Vector4(0.4f, 0.7f, 1.0f, 1.0f)) // Your accent color here
                );
            }

            // Get the position of that selectable we just drew
            float itemY = ImGui.GetItemRectMin().Y;

            // Set cursor back to draw icon and text on top
            ImGui.SetCursorScreenPos(new Vector2(ImGui.GetItemRectMin().X + 8, itemY + 4));

            ImGuiEx.Icon(icon);
            ImGui.SameLine();
            ImGui.Text(label);

            // Add small spacing between items
            ImGui.Dummy(new Vector2(0, 2));
        }

        private static void DrawSelectableWithImage(uint iconId, string label, string id)
        {
            bool isSelected = currentSelection == id;

            // Change background color if selected
            if (isSelected)
            {
                ImGui.PushStyleColor(ImGuiCol.Header, ImGui.GetColorU32(ImGuiCol.HeaderActive));
            }

            // Indent for items under categories
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 16);

            float width = ImGui.GetContentRegionAvail().X;

            // Invisible selectable as the clickable area
            if (ImGui.Selectable($"##{id}", isSelected, ImGuiSelectableFlags.None, new Vector2(width, 25)))
            {
                currentSelection = id;
            }

            if (isSelected)
            {
                ImGui.PopStyleColor();
            }

            // Get the position of that selectable we just drew
            float itemY = ImGui.GetItemRectMin().Y;

            // Set cursor back to draw image and text on top
            ImGui.SetCursorScreenPos(new Vector2(ImGui.GetItemRectMin().X + 8, itemY + 2));

            Svc.Texture.TryGetFromGameIcon(iconId, out var iconImage);
            if (iconImage != null)
            {
                var image = iconImage.GetWrapOrEmpty();
                Vector2 imageSize = new Vector2(25, 25);
                // Center image vertically in the 25px height
                float imageYOffset = (25 - imageSize.Y) / 2;
                ImGui.SetCursorScreenPos(new Vector2(ImGui.GetCursorScreenPos().X, ImGui.GetCursorScreenPos().Y + imageYOffset));
                ImGui.Image(image.Handle, imageSize);
            }

            ImGui.SameLine();
            ImGui.AlignTextToFramePadding();
            ImGui.Text(label);

            // Add small spacing between items
            ImGui.Dummy(new Vector2(0, 2));
        }
    }
}
