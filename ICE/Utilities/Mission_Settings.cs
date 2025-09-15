using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Utilities
{
    internal static class Mission_Settings
    {
        // States that get set in the main Ui
        internal static bool StopAfterCurrent = false;
        internal static uint previouslyAbandoned = 0;

        internal static bool Abandon = false;
        internal static bool AnimationLockAbandonState = false;
        internal static uint PossiblyStuck = 0;

        internal static Vector3? NearestCollectionPoint = null;
    }
}
