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
            496, 497, 498, 499, 500, 501,
            502, 503, 504, 505, 506, 507 // blacklisted mission ID
        };
    }
}
