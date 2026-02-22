using System;
using System.Collections.Generic;
using System.Text;

namespace ICE.ConfigFiles;

public partial class Config
{
    public bool ShowOverlay { get; set; } = false;
    public bool ShowSeconds { get; set; } = false;
    public bool ShowCurrentScore { get; set; } = true;
    public bool ShowTotalScore { get; set; } = true;
    public bool ShowExpBars { get; set; } = true;
    public bool ShowExpBars_HideWhenMaxed { get; set; } = false;
    public bool Overlay_AutoResize { get; set; } = true;
    public bool Overlay_FilterByJob { get; set; } = false;
    public bool Overlay_HighlightTokenWeather { get; set; } = true;
}
