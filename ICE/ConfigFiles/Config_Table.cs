

namespace ICE.ConfigFiles;

public partial class Config
{
    public int TableSortOption { get; set; } = 0;
    public bool HideUnsupportedMissions { get; set; } = false;
    public bool AutoPickCurrentJob { get; set; } = false;
    public bool ShowCompletionWindow { get; set; } = false;
    public bool ShowCompletionOnlyJob { get; set; } = false;
    public bool ShowSelectedJobOnly { get; set; } = false;
    public bool ShowCompletion_MissingGold { get; set; } = false;
    public bool ShowManualMode { get; set; } = false;
    public bool Auto_ShowTokens { get; set; } = true;

    public bool Show_StopWhen { get; set; } = true;
    public bool Show_GatheringProfile { get; set; } = true;
    public bool Show_MissionPriority { get; set; } = true;
    public bool Show_MiscSettings { get; set; } = true;
    public bool Show_TravelSettings { get; set; } = true;
    public bool Show_HubActivities { get; set; } = true;
}
