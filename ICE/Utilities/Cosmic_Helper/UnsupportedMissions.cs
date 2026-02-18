using System.Collections.Generic;

namespace ICE.Utilities.Cosmic
{
    /// <summary>
    /// IDs of missions that should be disabled and shown as unsupported.
    /// </summary>
    public static class UnsupportedMissions
    {
        public static readonly HashSet<uint> Ids = new HashSet<uint>
        {
            0,

            510, // Dual Craft (Ex+ Rank)

            1006, 

            // Oizyr Missions

            1289, 1290, 1291, 
            1317, 1318, 1319,

            // Fisher, the cursed lands
            1341, 1345, 1346, 1347, 


        };
    }
}
