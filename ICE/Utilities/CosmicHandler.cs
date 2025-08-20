using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Utilities
{
    internal class CosmicHandler
    {
        internal unsafe static bool IsMissionTimedOut()
        {
            if (AddonHelper.GetAtkTextNode("WKSMissionInfomation", 23)->IsVisible())
                return true;
            else
                return false;
        }
        internal unsafe static (int classScore, int cappedClassScore, int totalScores, uint classId) GetCosmicClassScores()
        {
            int classScore = 0;
            int cappedClassScore = 0;
            int totalScores = 0;
            var wksManager = WKSManager.Instance();
            var currentMissionId = wksManager->CurrentMissionUnitRowId;

            uint classId;
            if (currentMissionId > 0 &&
                CosmicHelper.MissionInfoDict.TryGetValue(currentMissionId, out var missionInfo))
                classId = missionInfo.JobId;
            else
                classId = (uint)(Svc.ClientState.LocalPlayer?.ClassJob.RowId);

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

        internal static void UpdateMissions()
        {

        }
    }
}
