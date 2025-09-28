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

            // All the fishing missions esentially
            451,452,453,454,455,456,
            457,458,459,460,461,462,463,
            464,465,466,467,468,469,470,471,
            472,473,474,475,476,477,478,479,480,481,482,483,484,485,486,487,488,
            489,490,491,492,493,494,495,
            // 508,509, // Dual Craft (A Rank)
            511,510, // Dual Craft (Ex+ Rank)
            542,543,544,
            965,966,967,968,969,970,
            971,972,973,974,975,976,977,
            978,979,980,981,982,983,984,985,
            986,987,988,989,990,991,992,993,994,995,996,997,998,
            999,1000,1001,1002,1003,1004,1005,1006,1037,1038,1039
            // blacklisted mission ID
        };
    }
}
