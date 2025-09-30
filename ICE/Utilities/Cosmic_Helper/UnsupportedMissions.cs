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
            511,510, // Dual Craft (Ex+ Rank)
            542,543,544,

            986,987,988,989,990,991,992,993,994,995,996,997,998,
            999,1000,1001,1002,1003,1004,1005,1006,1037,1038,1039
            // blacklisted mission ID
        };
    }
}
