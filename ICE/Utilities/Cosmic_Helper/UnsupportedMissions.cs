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

            479, 
            481, 482, 
            484, 
            486, 487,
            489, 490, 491, 492, 493, 494, 495,
            // 508,509, // Dual Craft (A Rank)
            511, 510, // Dual Craft (Ex+ Rank)
            543,

            1003, 
            // 1004, 1005, 
            1006, 
            // blacklisted mission ID

            // Oizyr Missions



            // Fisher, the cursed lands
            1320, 1321, 1322, 1323, 1324, 1325, 1326, 1327, 1328, 1329, 1330, 1331, 1332, 1333, 1334, 1335, 
            1336, 1337, 1338, 1339, 1340, 1341, 1342, 1343, 1344, 1345, 1346, 1347, 
            
            // Critical Missions
            1364, 1365, 1366, 1367, 1368, 1369,

            1281
        };
    }
}
