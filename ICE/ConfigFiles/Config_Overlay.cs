using System;
using System.Collections.Generic;
using System.Text;

namespace ICE.ConfigFiles;

public partial class Config
{
    public bool ShowOverlay { get; set; } = false;
    public bool ShowSeconds { get; set; } = false;
    public bool ShowTotalScore { get; set; } = true;
    public bool ShowExpBars { get; set; } = true;
}
