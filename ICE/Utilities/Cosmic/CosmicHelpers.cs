using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Utilities;

public static partial class CosmicHelper
{
    public static MissionListInfo CurrentMissionInfo => MissionInfoDict[CurrentLunarMission];
    public static MoonRecipieInfo CurrentMoonRecipe => MoonRecipies[CurrentLunarMission];
    /// <summary>
    /// Gives the current mission that is active
    /// </summary>
    public static unsafe uint CurrentLunarMission => WKSManager.Instance()->CurrentMissionUnitRowId;
    public static unsafe uint CurrentLunarDevelopment => ExcelHelper.DevGrade.GetRow(WKSManager.Instance()->DevGrade).Unknown6;

    public static Dictionary<int, string> ExpDictionary = new()
    {
        { 1, "I" },
        { 2, "II" },
        { 3, "III" },
        { 4, "IV" }
    };

    public static Vector3 RedAlertCollectionPoint = Vector3.Zero;


    // General use functions used across the codebase, specifically tied to cosmic related functions
    public static void OpenStellarMission()
    {
        if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var hud) && hud.IsAddonReady)
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMissionInfomation>("WKSMissionInfomation", out var missionInfo) && missionInfo.IsAddonReady)
            {
                return;
            }
            else
            {
                if (EzThrottler.Throttle("Opening Stellar Missions"))
                {
                    IceLogging.Debug("Opening Mission Menu");
                    hud.Mission();
                }
            }
        }
    }

    public static void UpdateStateFlags()
    {
        // Grabs the current mission entry from the dictionary, and all config information tied to it.
        CosmicMission? mission = C.Missions.SingleOrDefault(x => x.Id == CosmicHelper.CurrentLunarMission);

        // just a shorthand for me to be able to grab all of it, while also just snapshotting the mission we're currently running
        var missionInfo = CurrentMissionInfo; 
        if (missionInfo.Attributes.HasFlag(MissionAttributes.Critical))
        {
            // TODO:
            // I really need to just add a collection point to the critical mission infomation
            // From here, grab the mission info
            // RedAlertCollectionPoint = infohere
        }
        if (missionInfo.Attributes.HasFlag(MissionAttributes.Craft))
            SchedulerMain.State |= IceState.Craft;
        if 
    }
}
