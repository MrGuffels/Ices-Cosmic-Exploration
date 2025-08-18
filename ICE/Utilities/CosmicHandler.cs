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

        internal static void UpdateMissions()
        {

        }
    }
}
