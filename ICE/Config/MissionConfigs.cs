using System.Collections.Generic;
using System.IO;

namespace ICE.Config
{
    public class MissionConfigs : IYamlConfig
    {
        // Last edited version: 1
        public int ConfigVersion { get; set; } = 0;

        #region Safety Settings
        public bool StopOnAbort { get; set; } = true;
        public bool RejectUnknownYesno { get; set; } = true;
        public bool DelayGrabMission { get; set; } = true;
        public int DelayIncrease { get; set; } = 500;
        public bool DelayCraft { get; set; } = true;
        public int DelayCraftIncrease { get; set; } = 2500;
        public bool AnimationLockAbandon { get; set; } = true;

        #endregion

        #region Main Window

        public float MiddleColumnWidth { get; set; } = 1000f;
        public uint SelectedJob { get; set; } = 8;
        public bool XPRelicGrind { get; set; } = false;
        public bool XPRelicIgnoreManual { get; set; } = false;
        public bool XPRelicOnlyEnabled { get; set; } = false;
        public bool ShowCritical { get; set; } = true;
        public bool ShowSequential { get; set; } = true;
        public bool ShowWeather { get; set; } = true;
        public bool ShowTimeRestricted { get; set; } = true;
        public bool ShowClassA { get; set; } = true;
        public bool ShowClassB { get; set; } = true;
        public bool ShowClassC { get; set; } = true;
        public bool ShowClassD { get; set; } = true;

        #endregion

        #region Overlay Settings

        public bool ShowOverlay { get; set; } = false;
        public bool ShowSeconds { get; set; } = false;
        public bool ShowExpBars { get; set; } = true;

        #endregion

        #region MissionSettings

        public bool OnlyGrabMission { get; set; } = false;
        public int TargetLevel { get; set; } = 10;
        public bool StopWhenLevel { get; set; } = false;
        public bool StopOnceHitCosmoCredits { get; set; } = false;
        public int CosmoCreditsCap { get; set; } = 30000;
        public bool StopOnceHitLunarCredits { get; set; } = false;
        public int LunarCreditsCap { get; set; } = 10000;
        public bool StopOnceHitCosmicScore { get; set; } = false;
        public int CosmicScoreCap { get; set; } = 500000;
        public bool StopOnceRelicFinished { get; set; } = false;
        public byte SequenceMissionPriority { get; set; } = 1;
        public byte WeatherMissionPriority { get; set; } = 2;
        public byte TimedMissionPriority { get; set; } = 3;
        public List<ProvisionalTypes> MissionPrio { get; set; } = new()
        {
            ProvisionalTypes.ProvisionalWeather,
            ProvisionalTypes.ProvisionalSequential,
            ProvisionalTypes.ProvisionalTimed
        };
        public bool ShowSinusMissions { get; set; } = true;
        public bool ShowPhaennaMissions { get; set; } = true;

        #endregion

        #region Table Settings

        public int TableSortOption { get; set; } = 0;
        public bool ShowManualMode { get; set; } = false;
        public bool HideUnsupportedMissions { get; set; } = false;
        public bool AutoPickCurrentJob { get; set; } = false;

        #endregion

        #region Repair Settings

        public bool SelfRepairGather { get; set; } = true;
        public bool SelfRepairCrafter { get; set; } = false;
        public bool RepairAtVendor { get; set; } = false;
        public int RepairPercent { get; set; } = 50;
        public bool SelfSpiritbondGather { get; set; } = true;

        #endregion

        #region Gathering Settings
        public int SelectedGatherIndex { get; set; } = 0;

        #region Cordial Settings

        public bool AutoCordial { get; set; } = false;
        public bool inverseCordialPrio { get; set; } = false;
        public int CordialMinGp { get; set; } = 0;
        public bool UseOnFisher { get; set; } = false;
        public bool PreventOvercap { get; set; } = false;
        public bool UseOnlyInMission { get; set; } = false;

        #endregion

        public List<GatherProfile> GatherSettings { get; set; } = new()
        {
            new GatherProfile { Id = 0, Name = "Defualt"},
        };

        #endregion

        #region Gamba Settings

        // Gamba settings
        public List<Gamba> GambaItemWeights { get; set; } = new();
        public bool GambaEnabled { get; set; } = false;
        public bool GambaPreferSmallerWheel { get; set; } = false;
        public int GambaCreditsMinimum { get; set; } = 0;
        public int GambaDelay { get; set; } = 250;

        #endregion

        #region Misc

        public bool MoonSprint { get; set; } = true;
        public uint MountId { get; set; } = 0;
        public string MountName { get; set; } = "Mount Roulette";
        public float MountRadius { get; set; } = 15.0f;
        public bool UseMountOutsideMission { get; set; } = true;
        public bool UseMountInMission { get; set; } = true;
        public float LeftColumnWidth { get; set; } = 300f;

        #endregion

        public Dictionary<uint, MissionSettings> MissionConfig { get; set; } = new();

        #region Debug

        public bool FailsafeRecipeSelect { get; set; } = false;
        public bool UseDummyXp { get; set; } = false;
        public Dictionary<int, CosmicHelper.XPType> DummyXP { get; set; } = new()
        {
            { 1, new CosmicHelper.XPType { CurrentXP = 0, NeededXP = 100} },
            { 2, new CosmicHelper.XPType { CurrentXP = 50, NeededXP = 200} },
            { 3, new CosmicHelper.XPType { CurrentXP = 100, NeededXP = 300} },
            { 4, new CosmicHelper.XPType { CurrentXP = 150, NeededXP = 400} },
            { 5, new CosmicHelper.XPType { CurrentXP = 200, NeededXP = 500} },
        };
        public uint PictoColor_Circle { get; set; } = 2616716297;
        public uint PictoColor_Dot { get; set; } = 2616716297;

        #endregion

        #region Yaml Save Stuff

        public static string ConfigPath => Path.Combine(Svc.PluginInterface.ConfigDirectory.FullName, "Mission Config.yaml");
        public void Save() => YamlConfig.Save(this, ConfigPath);

        #endregion
    }

    public class MissionSettings
    {
        public bool Enabled { get; set; } = false;
        public bool ManualMode { get; set; } = false;
        public int GatherProfileId { get; set; } = 0;
        public bool AutoTurnin { get; set; } = true;
        public bool TurninGold { get; set; } = false;
        public bool TurninSilver { get; set; } = false;
        public bool TurninBronze { get; set; } = false;
        public bool Use_BuildinPreset { get; set; } = false;
        public string AutoHookPresetName { get; set; } = string.Empty;
    }

    public class GatherProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int MinimumGp { get; set; } = -1;
        public int DualClassCraftAmount { get; set; } = 1;
        public GatherBuffs GatherBuffs { get; set; } = new();
    }

    public class GatherBuff
    {
        public bool Enabled { get; set; } = false;
        public int MinGp { get; set; }
        public int MaxUse { get; set; } = -1;
    }

    public class GatherBuffs
    {
        public Dictionary<string, GatherBuff> Buffs { get; set; } = new()
        {
            ["BoonIncrease2"] = new() { MinGp = 100 },
            ["BoonIncrease1"] = new() { MinGp = 50 },
            ["Tidings"] = new() { MinGp = 200 },
            ["YieldII"] = new() { MinGp = 500 },
            ["YieldI"] = new() { MinGp = 400 },
            ["BountifulYieldII"] = new() { MinGp = 100 },
            ["BonusIntegrity"] = new() { MinGp = 300 }
        };

        public int BountifulMinItem { get; set; } = 4;
    }

    public class Gamba
    {
        public uint ItemId { get; set; }
        public int Weight { get; set; } = 0;
        public GambaType Type { get; set; }
    }
}
