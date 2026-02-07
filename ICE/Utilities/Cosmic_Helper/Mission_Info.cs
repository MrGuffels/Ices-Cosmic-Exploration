using System.Collections.Generic;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using ICE.Utilities.Cosmic_Helper;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Utilities;

public static partial class CosmicHelper
{
    public static CosmicInfo CurrentMissionInfo => SheetMissionDict[CurrentLunarMission];

    /// <summary>
    /// Gives the current mission that is active
    /// </summary>
    public static unsafe uint CurrentLunarMission
    {
        get
        {
            try
            {
                var manager = (WKSManagerCustom*)WKSManager.Instance();
                if (manager == null)
                    return 0; // or some default value

                return manager->CurrentMissionId;
            }
            catch (AccessViolationException)
            {
                IceLogging.Error("We're currently getting access violations with this, so returning 0");
                return 0;
            }
            catch (Exception)
            {
                IceLogging.Error("Welp. Somehow not getting it still. Exception exit");
                return 0;
            }
        }
    }
    public static unsafe uint? CurrentBait => WKSManager.Instance()->FishingBait;
    public static unsafe uint CurrentLunarDevelopment => ExcelHelper.DevGrade.GetRow(WKSManager.Instance()->DevGrade).Unknown6;

    public static int MaxXpKind = 6;

    public static Dictionary<int, string> ExpDictionary = new()
    {
        { 1, "I" },
        { 2, "II" },
        { 3, "III" },
        { 4, "IV" },
        { 5, "V" },
        { 6, "VI" }
    };

    public static readonly Dictionary<uint, uint> PlanetCreditInfo = new()
    {
        [1237] = 45691, // sinus
        [1291] = 48146, // phaenna
        [1310] = 48147, // Oizys
        // [] = 48148, // moon 4
    };

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

    public class ClassInfo
    {
        public int Score { get; set; } = 0;
        public int Stage_Current { get; set; } = 0;
        public int Stage_Next { get; set; } = 0;
        public Dictionary<int, ExpInfo> CurrentExp { get; set; } = new();
    }

    public class ExpInfo
    {
        public string Name { get; set; } = "";
        public int Current { get; set; } = 0;
        public int Needed { get; set; } = 0;
        public int Max { get; set; } = 0;
    }

    public unsafe static Dictionary<uint, ClassInfo> Cosmic_ClassInfo()
    {
        Dictionary<uint, ClassInfo> cosmicClassInfo = new();

        var wksManager = WKSManager.Instance();
        if (wksManager == null || wksManager->ResearchModule == null || !wksManager->ResearchModule->IsLoaded)
            return cosmicClassInfo;

        for (int i = 0; i < 11; i++)
        {
            uint jobId = (uint)i + 8;
            byte toolClassId = (byte)(jobId - 7);  // Match DrawRelicXP's calculation
            byte arrayIndex = (byte)(toolClassId - 1);  // This gives us 0-10 for array access

            var score = wksManager->Scores[arrayIndex];
            ClassInfo entry = new()
            {
                Score = score,
                Stage_Current = wksManager->ResearchModule->CurrentStages[arrayIndex],
                Stage_Next = wksManager->ResearchModule->UnlockedStages[arrayIndex]
            };

            for (byte type = 1; type <= MaxXpKind; type++)
            {
                if (!wksManager->ResearchModule->IsTypeAvailable(toolClassId, type))
                    break;

                var needed = wksManager->ResearchModule->GetNeededAnalysis(toolClassId, type);
                var current = wksManager->ResearchModule->GetCurrentAnalysis(toolClassId, type);
                var max = wksManager->ResearchModule->GetMaxAnalysis(toolClassId, type);
                var name = "???";
                if (ExpDictionary.TryGetValue(type, out var ExpName))
                {
                    name = ExpName;
                }

                entry.CurrentExp[type] = new()
                {
                    Name = name,
                    Needed = needed,
                    Current = current,
                    Max = max,
                };
            }

            cosmicClassInfo[jobId] = entry;
        }

        return cosmicClassInfo;
    }   


    public unsafe static (int classScore, int cappedClassScore, int totalScores, uint classId) GetCosmicClassScores(bool useSelectedJob = false, uint jobId = 0)
    {
        int classScore = 0;
        int cappedClassScore = 0;
        int totalScores = 0;
        var wksManager = WKSManager.Instance();
        var currentMissionId = wksManager->CurrentMissionUnitRowId;

        uint classId;

        if (useSelectedJob)
        {
            // For UI that should respect C.SelectedJob
            classId = C.SelectedJob;
        }
        else
        {
            // For overlay that should show current/mission job
            if (currentMissionId > 0 && CosmicHelper.SheetMissionDict.TryGetValue(currentMissionId, out var missionInfo))
            {
                if (jobId != 0)
                    classId = jobId;
                else if (missionInfo.Jobs.Contains((uint)Player.Job))
                    classId = (uint)Player.Job;
                else
                    classId = missionInfo.Jobs.First();
            }
            else if (CosmicHelper.CrafterJobList.Contains((uint)Player.Job) || CosmicHelper.GatheringJobList.Contains((uint)Player.Job))
                classId = (uint)Player.Job;
            else
                classId = C.SelectedJob;
        }

        if (classId is >= 8 and <= 18)
        {
            var scores = wksManager->Scores;

            classScore = scores[(int)classId - 8];
            cappedClassScore = Math.Min(500_000, classScore);

            totalScores = 0;
            for (int i = 0; i < scores.Length; ++i)
                totalScores += Math.Min(500_000, scores[i]);
        }

        return (classScore, cappedClassScore, totalScores, classId);
    }
}
