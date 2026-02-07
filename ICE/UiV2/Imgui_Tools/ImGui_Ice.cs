using Dalamud.Interface;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using System.Reflection;

namespace ICE.UiV2.Imgui_Tools;

public static partial class ImGui_Ice
{
    // General Functions that are used everywhere across the plugin
    /// <summary>
    /// Greyscale icon that I have saved for general ease of use. Looks better than the GC icons
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public static IDalamudTextureWrap? GetGreyscaleJob(uint jobId = 0)
    {
        if (jobId == 0)
            jobId = C.SelectedJob;

        string greyJobIcon = jobId switch
        {
            8 => "ICE.Resources.GreyscaleJobs.CRP.png",
            9 => "ICE.Resources.GreyscaleJobs.BSM.png",
            10 => "ICE.Resources.GreyscaleJobs.ARM.png",
            11 => "ICE.Resources.GreyscaleJobs.GSM.png",
            12 => "ICE.Resources.GreyscaleJobs.LTW.png",
            13 => "ICE.Resources.GreyscaleJobs.WVR.png",
            14 => "ICE.Resources.GreyscaleJobs.ALC.png",
            15 => "ICE.Resources.GreyscaleJobs.CUL.png",
            16 => "ICE.Resources.GreyscaleJobs.MIN.png",
            17 => "ICE.Resources.GreyscaleJobs.BTN.png",
            18 => "ICE.Resources.GreyscaleJobs.FSH.png",
            _ => "ICE.Resources.GreyscaleJobs.Default.png",
        };

        return Svc.Texture.GetFromManifestResource(Assembly.GetExecutingAssembly(), greyJobIcon).GetWrapOrEmpty();
    }
    public static bool Sidebar_CollaspableHeader(string label, FontAwesomeIcon? icon = null, IDalamudTextureWrap? imageTexture = null)
    {
        float scale = ImGuiHelpers.GlobalScale;

        // Default Colors for Theming. This is really here to make sure it's formatted as I want it to be
        var headerColor = ImGui.GetColorU32(ImGuiCol.Header);
        var textColor = ImGui.GetColorU32(ImGuiCol.Text);

        // This is here to make sure that
        // A: If it doesn't already exist, add it and just make it false (This makes it to where it's not expanded by default)
        //    - Could absolutely change that to true if I want to make it shown on inital creation
        // B: Returns the state in a form to where if that's true, then I could display the elements below it properly
        string categoryId = label;
        if (!C.MainUi_CustomHeader.ContainsKey(categoryId))
        {
            C.MainUi_CustomHeader[categoryId] = false;
            C.SaveDebounced();
        }

        bool isExpanded = C.MainUi_CustomHeader[categoryId];

        // Need these here for two reasons:
        // 1: drawList allows me to create un-conventional things that isn't included in the Imgui Library
        // 2: curserPos allows me to grab the absolute position, which is necessary to make sure things are lined up properly
        var drawList = ImGui.GetWindowDrawList();
        var cursorPos = ImGui.GetCursorScreenPos();

        // This is currently how I'm going to autosize it based on the contents of the window.
        // (as of typing this). It gets the width of the window and expands it on that (these are meant for the sidebar)
        // Height is scaled based on the global font scale
        float width = ImGui.GetContentRegionAvail().X;
        float height = 30 * scale;

        // Check for click, nice little rectangle area where it can be clicked at. This takes in account the above things to make sure it's only clicking within this area
        bool isHovered = ImGui.IsMouseHoveringRect(cursorPos, new Vector2(cursorPos.X + width, cursorPos.Y + height));
        bool isClicked = isHovered && ImGui.IsMouseClicked(ImGuiMouseButton.Left);

        if (isClicked)
        {
            C.MainUi_CustomHeader[categoryId] = !C.MainUi_CustomHeader[categoryId];
            isExpanded = C.MainUi_CustomHeader[categoryId];
            C.SaveDebounced();
        }

        // Change header color slightly on hover, just a nice QOL
        if (isHovered)
            headerColor = ImGui.GetColorU32(ImGuiCol.HeaderHovered);

        // Drawing the rectangle itself/ custom thingy. This is the container for all the fancy smancy stuff.
        // Border radius scaled
        drawList.AddRectFilled(cursorPos, new Vector2(cursorPos.X + width, cursorPos.Y + height), headerColor, 5.0f * scale);

        // Calculating the vertical spacing here, need to make sure it fits within our custom box nice and cozy
        float imageSize = 23 * scale; // Used for images specifically, since I like things being aligned with each other
        float textHeight = ImGui.CalcTextSize(label).Y;
        float verticalPadding = (height - textHeight) / 2;

        // Adding some padding to the left, don't need it feeling like it's right against the box. We're making somewhat bubbly things
        ImGui.SetCursorPosY(ImGui.GetCursorPosY() + verticalPadding);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 8 * scale);

        // Drawing either an image, icon, or nothing at all (should really only be the first 2, but on the off chance I decide to just use text)
        if (imageTexture != null)
        {
            // Calculating the offset here to center the image with the text (OCD here)
            float imageYOffset = (textHeight - imageSize) / 2;
            ImGui.SetCursorPosY(ImGui.GetCursorPosY() + imageYOffset);

            // Adding a little bit of padding here for the image -> text (scaled)
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() - 4 * scale); // Current set to -4 to bring it closer, but can be changed to give it more space (lowering the number) or removing more space (increasing number)

            ImGui.Image(imageTexture.Handle, new Vector2(imageSize, imageSize));

            // Resetting the Y position for text, to make sure it lines up (scaled spacing)
            ImGui.SameLine(0, 2 * scale);
            ImGui.SetCursorPosY(ImGui.GetCursorPosY() - imageYOffset);
        }
        else if (icon.HasValue)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, textColor); // Disabled text color here to match the rest of it
            ImGuiEx.Icon(icon.Value);
            ImGui.PopStyleColor();
            ImGui.SameLine();
        }
        else
        {
            // No icon, just adding some sameline spacing
            ImGui.SameLine();
        }

        // Actual label here
        ImGui.PushStyleColor(ImGuiCol.Text, textColor);
        ImGui.Text(label);
        ImGui.PopStyleColor();

        // Replace the badge count section with the caret icon (scaled padding)
        float iconSize = ImGui.CalcTextSize(FontAwesomeIcon.CaretDown.ToIconString()).X;
        float rightPadding = 10 * scale;

        float iconXPos = cursorPos.X + width - iconSize - rightPadding;
        float iconYPos = cursorPos.Y + verticalPadding;

        ImGui.SetCursorScreenPos(new Vector2(iconXPos, iconYPos));
        ImGui.PushStyleColor(ImGuiCol.Text, textColor);
        ImGuiEx.Icon(isExpanded ? FontAwesomeIcon.CaretSquareDown : FontAwesomeIcon.CaretSquareRight);
        ImGui.PopStyleColor();

        // Last but not least, resetting it for the next thing here:
        ImGui.SetCursorScreenPos(new Vector2(cursorPos.X, cursorPos.Y + height));
        ImGui.Spacing();

        return isExpanded;
    }
    public static void DrawSelectable_Icon(FontAwesomeIcon icon, string label, string id)
    {
        bool isSelected = C.MainUi_SelectedWindow == id;
        float scale = ImGuiHelpers.GlobalScale;

        // Change background color if selected
        if (isSelected)
        {
            ImGui.PushStyleColor(ImGuiCol.Header, ImGui.GetColorU32(ImGuiCol.HeaderActive));
        }

        // Indent for items under categories (scaled)
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 16 * scale);

        float width = ImGui.GetContentRegionAvail().X;

        // Invisible selectable as the clickable area (scaled height)
        if (ImGui.Selectable($"##{id}", isSelected, ImGuiSelectableFlags.None, new Vector2(width, 25 * scale)))
        {
            C.MainUi_SelectedWindow = id;
            C.SaveDebounced();
        }

        if (isSelected)
        {
            ImGui.PopStyleColor();

            // Draw colored bar on the left side
            var drawList = ImGui.GetWindowDrawList();
            var rectMin = ImGui.GetItemRectMin();
            var rectMax = ImGui.GetItemRectMax();

            // Draw a 3-4 pixel wide bar on the left (scaled)
            drawList.AddRectFilled(
                rectMin,
                new Vector2(rectMin.X + 3 * scale, rectMax.Y),
                ImGui.GetColorU32(new Vector4(0.4f, 0.7f, 1.0f, 1.0f)) // Your accent color here
            );
        }

        // Get the position of that selectable we just drew
        float itemY = ImGui.GetItemRectMin().Y;

        // Set cursor back to draw icon and text on top (scaled offsets)
        ImGui.SetCursorScreenPos(new Vector2(ImGui.GetItemRectMin().X + 8 * scale, itemY + 4 * scale));

        ImGuiEx.Icon(icon);
        ImGui.SameLine();
        ImGui.Text(label);

        // Add small spacing between items (scaled)
        ImGui.Dummy(new Vector2(0, 2 * scale));
    }
    public static void DrawSelectable_Image(uint iconId, string label, string id)
    {
        bool isSelected = C.MainUi_SelectedWindow == id;
        float scale = ImGuiHelpers.GlobalScale;

        // Change background color if selected
        if (isSelected)
        {
            ImGui.PushStyleColor(ImGuiCol.Header, ImGui.GetColorU32(ImGuiCol.HeaderActive));
        }

        // Indent for items under categories (scaled)
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 16 * scale);

        float width = ImGui.GetContentRegionAvail().X;

        // Invisible selectable as the clickable area (scaled height)
        if (ImGui.Selectable($"##{id}", isSelected, ImGuiSelectableFlags.None, new Vector2(width, 25 * scale)))
        {
            C.MainUi_SelectedWindow = id;
            C.SaveDebounced();
        }

        if (isSelected)
        {
            ImGui.PopStyleColor();

            // Draw colored bar on the left side
            var drawList = ImGui.GetWindowDrawList();
            var rectMin = ImGui.GetItemRectMin();
            var rectMax = ImGui.GetItemRectMax();

            // Draw a 3-4 pixel wide bar on the left (scaled)
            drawList.AddRectFilled(
                rectMin,
                new Vector2(rectMin.X + 3 * scale, rectMax.Y),
                ImGui.GetColorU32(new Vector4(0.4f, 0.7f, 1.0f, 1.0f)) // Your accent color here
            );
        }

        // Get the position of that selectable we just drew
        float itemY = ImGui.GetItemRectMin().Y;

        // Set cursor back to draw image and text on top (scaled offsets)
        // Match the Icon version's offset pattern
        ImGui.SetCursorScreenPos(new Vector2(ImGui.GetItemRectMin().X + 8 * scale, itemY + 4 * scale));

        Svc.Texture.TryGetFromGameIcon(iconId, out var iconImage);
        if (iconImage != null)
        {
            var image = iconImage.GetWrapOrEmpty();
            Vector2 imageSize = new Vector2(25 * scale, 25 * scale);
            // Remove the vertical centering - just draw at current position
            ImGui.Image(image.Handle, imageSize);
        }

        ImGui.SameLine();
        ImGui.Text(label);
    }
    public static bool DrawStyledImageButton(IDalamudTextureWrap? icon, Vector2 size, bool enabled = true)
    {
        Vector4 buttonColor, borderColor;
        float borderSize;

        if (enabled)
        {
            buttonColor = new Vector4(0.3f, 0.3f, 0.35f, 0.7f);
            borderColor = new Vector4(0.898f, 0.8f, 0.501f, 1f);
            borderSize = 1.0f;
        }
        else
        {
            buttonColor = new Vector4(0.2f, 0.2f, 0.2f, 0.1f);
            borderColor = new Vector4(0.4f, 0.4f, 0.4f, 0.5f);
            borderSize = 0.5f;
        }

        // Apply the custom styling
        ImGui.PushStyleColor(ImGuiCol.Button, buttonColor);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, buttonColor * 1.1f); // Slightly brighter on hover
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, buttonColor * 0.9f);  // Slightly darker when pressed
        ImGui.PushStyleColor(ImGuiCol.Border, borderColor);
        ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, borderSize);

        // Create the ImageButton
        bool clicked = ImGui.ImageButton(icon.Handle, size);

        // Restore original styling
        ImGui.PopStyleVar(); // FrameBorderSize
        ImGui.PopStyleColor(4); // Button, ButtonHovered, ButtonActive, Border

        return clicked;
    }
    public static bool DrawStyledImageButton(ISharedImmediateTexture? icon, Vector2 size, bool enabled = true)
    {
        Vector4 buttonColor, borderColor;
        float borderSize;

        if (enabled)
        {
            buttonColor = new Vector4(0.3f, 0.3f, 0.35f, 0.7f);
            borderColor = new Vector4(0.898f, 0.8f, 0.501f, 1f);
            borderSize = 1.0f;
        }
        else
        {
            buttonColor = new Vector4(0.2f, 0.2f, 0.2f, 0.1f);
            borderColor = new Vector4(0.4f, 0.4f, 0.4f, 0.5f);
            borderSize = 0.5f;
        }

        float zoomFactor = 0.25f; // 25% zoom-in
        float cropAmount = zoomFactor / 2; // Crop equally from all sides

        Vector2 uv0 = enabled ? new Vector2(0, 0) : new Vector2(cropAmount, cropAmount);
        Vector2 uv1 = enabled ? new Vector2(1, 1) : new Vector2(1 - cropAmount, 1 - cropAmount);

        // Applies the custom code
        ImGui.PushStyleColor(ImGuiCol.Button, buttonColor);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, buttonColor * 1.1f); // Slightly brighter on hover
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, buttonColor * 0.9f);  // Slightly darker when pressed
        ImGui.PushStyleColor(ImGuiCol.Border, borderColor);
        ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, borderSize);

        bool clicked = ImGui.ImageButton(icon.GetWrapOrEmpty().Handle, size, uv0, uv1);

        // Restore original styling
        ImGui.PopStyleVar(); // FrameBorderSize
        ImGui.PopStyleColor(4); // Button, ButtonHovered, ButtonActive, Border

        return clicked;
    }
    public static void DrawJobButtons(uint jobId, string tooltip)
    {
        float scale = ImGuiHelpers.GlobalScale;

        uint selectedJob = C.SelectedJob;
        bool state = selectedJob == jobId;
        var iconEnabled = CosmicHelper.JobIconDict[jobId];
        var iconDisabled = GetGreyscaleJob(jobId);
        Vector2 size = new Vector2(26 * scale, 26 * scale);
        bool autoPickCurrentJob = C.AutoPickCurrentJob;

        if (state)
        {
            if (DrawStyledImageButton(iconEnabled, size, state))
            {
                if (autoPickCurrentJob)
                {
                    autoPickCurrentJob = false;
                    C.AutoPickCurrentJob = autoPickCurrentJob;
                }

                C.SelectedJob = jobId;
                C.Save();
            }
        }
        else if (!state)
        {
            if (DrawStyledImageButton(iconDisabled, size, state))
            {
                if (autoPickCurrentJob)
                {
                    autoPickCurrentJob = false;
                    C.AutoPickCurrentJob = autoPickCurrentJob;
                }

                C.SelectedJob = jobId;
                C.Save();
            }
        }
        if (ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.Text(tooltip);
            ImGui.EndTooltip();
        }
    }
    public static void Draw_XPBar(int current, int needed, int max = 0, string label = null, Vector2? size = null)
    {
        // If we want it to have a standard label above the bar. Not required but for small things it's nice to just have the option
        if (label != null)
        {
            ImGui.TextWrapped(label);
        }

        // Setting the dimensions of the custom bar/drawing it.
        // Usual stuff of drawlist being OP
        var pos = ImGui.GetCursorScreenPos();
        var drawList = ImGui.GetWindowDrawList();

        // Calculating the size of the bar and everything here.
        // If size is null, then it just defaults to the norm. Otherwise it uses whatever size we set (nice in case I want to use this for other things besides XP/Modify it a bit easier)
        var barStart = pos;
        var actualSize = size ?? new Vector2(ImGui.GetContentRegionAvail().X, 10);
        var barEnd = new Vector2(pos.X + actualSize.X, pos.Y + actualSize.Y);

        // Draw background (dark gray)
        drawList.AddRectFilled(barStart, barEnd, ImGui.GetColorU32(new Vector4(0.15f, 0.15f, 0.15f, 1f)));

        // Now comes the fun part, actually creating the filling (that sounds bad)

        // Defining the colors globaly here just cause they're used across the board
        var blueColor = new Vector4(0.2f, 0.6f, 1f, 1f);      // Blue #3399ff  - Fill Bar Part #1 (Left Side) 
        var greenColor = new Vector4(0.6f, 1f, 0.8f, 1f);     // Green #99ffcc - Fill Bar Part #2 (Right Side)
        var goldColor = new Vector4(1f, 0.84f, 0f, 1f);       // Gold #ffd600  - Overcap color

        // Case 1: At or above cap when needed == max (show full gold) [Really only used when at max stage for that planet when a new one comes out)
        if (needed > 0 && needed == max && current >= needed)
        {
            drawList.AddRectFilled(barStart, barEnd, ImGui.GetColorU32(goldColor));
        }
        // Case 2: Normal progression (not overcapped)
        else if (current <= needed && needed > 0)
        {
            float fraction = Math.Clamp((float)current / needed, 0f, 1f);
            float filledWidth = actualSize.X * fraction;

            if (filledWidth > 0f)
            {
                var filledEnd = new Vector2(pos.X + filledWidth, pos.Y + actualSize.Y);
                drawList.AddRectFilledMultiColor(
                    barStart, filledEnd,
                    ImGui.GetColorU32(blueColor),  // top-left
                    ImGui.GetColorU32(greenColor), // top-right
                    ImGui.GetColorU32(greenColor), // bottom-right
                    ImGui.GetColorU32(blueColor)   // bottom-left
                );
            }
        }
        // Case 3: Overcapped (show gradient + gold overlay)
        else if (current > needed && max > 0 && needed > 0)
        {
            // Full blue-green gradient background
            drawList.AddRectFilledMultiColor(
                barStart, barEnd,
                ImGui.GetColorU32(blueColor),
                ImGui.GetColorU32(greenColor),
                ImGui.GetColorU32(greenColor),
                ImGui.GetColorU32(blueColor)
            );

            // Gold overlay for overcap amount
            int overcapAmount = current - needed;
            int overcapRange = max - needed;
            float overcapFraction = Math.Clamp((float)overcapAmount / overcapRange, 0f, 1f);
            float goldWidth = actualSize.X * overcapFraction;

            if (goldWidth > 0f)
            {
                var goldEnd = new Vector2(pos.X + goldWidth, pos.Y + actualSize.Y);
                drawList.AddRectFilled(barStart, goldEnd, ImGui.GetColorU32(goldColor));
            }
        }
        // Case 4: No needed XP (cosmic score scenario - just show progress to max) [Nice for just pure gold bar to fill]
        else if (needed == 0 && max > 0)
        {
            float fraction = Math.Clamp((float)current / max, 0f, 1f);
            float filledWidth = actualSize.X * fraction;

            if (filledWidth > 0f)
            {
                var filledEnd = new Vector2(pos.X + filledWidth, pos.Y + actualSize.Y);
                drawList.AddRectFilled(barStart, filledEnd, ImGui.GetColorU32(goldColor));
            }
        }

        ImGui.Dummy(actualSize);
    }
    public static void IconWithTooltip(Vector4 col, FontAwesomeIcon icon, string? tooltip = null)
    {
        ImGui.SameLine();
        ImGui.PushStyleColor(ImGuiCol.Text, col);
        ImGui.PushFont(UiBuilder.IconFont);
        ImGui.TextUnformatted(icon.ToIconString());
        ImGui.PopFont();
        ImGui.PopStyleColor();

        if (tooltip != null)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(tooltip);
            }
        }
    }
    public static void IconWithTooltip(FontAwesomeIcon icon, string? tooltip = null)
    {
        ImGui.SameLine();
        ImGui.PushFont(UiBuilder.IconFont);
        ImGui.TextUnformatted(icon.ToIconString());
        ImGui.PopFont();

        if (tooltip != null)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(tooltip);
            }
        }
    }

    // Quick access functions that are used in multiple places
    public static void Draw_ExpTable(uint jobId)
    {
        var ExpInfo = CosmicHelper.Cosmic_ClassInfo();
        if (ExpInfo.TryGetValue(jobId, out var jobInfo))
        {
            foreach (var exp in jobInfo.CurrentExp.Values)
            {
                ImGui.Text($"Exp {exp.Name}: {exp.Current} / {exp.Needed}");
                Draw_XPBar(exp.Current, exp.Needed, exp.Max);
                if (ImGui.IsItemHovered())
                {
                    using (var expTooltip = ImRaii.Tooltip())
                    {
                        ImGui.Text($"Type: {exp.Name}");
                        ImGui.Separator();

                        ImGui.Text($"Current: {exp.Current}");
                        ImGui.Text($"Need: {exp.Needed}");
                        ImGui.Text($"Max: {exp.Max}");
                    }
                }
            }
        }
    }
}
