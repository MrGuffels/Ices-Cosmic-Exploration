using ICE.Config;
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
        
        // Gather Specifics
        internal static Vector2 previousMap = Vector2.Zero;
        internal static int nodeCounter = 0;
        internal static int nodeTotal = 0;
        internal static uint item_collectableId = 0;
        internal static Dictionary<string, uint> SkillUseAmount { get; set; } = new()
        {
            ["BoonIncrease2"] = 0,
            ["BoonIncrease1"] = 0,
            ["Tidings"] = 0,
            ["YieldII"] = 0,
            ["YieldI"] = 0,
            ["BountifulYieldII"] = 0,
            ["BonusIntegrityChance"] = 0,
            ["BonusIntegrity"] = 0,
        };

        internal static bool Abandon = false;
        internal static bool AnimationLockAbandonState = false;
        internal static uint PossiblyStuck = 0;

        internal static Vector3? NearestCollectionPoint = null;
    }
}
