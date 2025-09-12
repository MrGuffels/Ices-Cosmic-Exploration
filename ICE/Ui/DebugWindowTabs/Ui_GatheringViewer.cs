using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Ui_GatheringViewer
    {
        private static Dictionary<Vector2, uint> SavedNodeDict => D.NodeDict;
        private static List<GatheringUtil.GathNodeInfo> TempNodeInfo => D.CustomNodeInfoList;

        private static Dictionary<Vector2, uint> selectedDict = new();
        private static List<GatheringUtil.GathNodeInfo> selectedList = new();

        private static Vector2 PositionEntry = new Vector2(0, 0);
        private static uint ListingEntry = 0;
        private static bool showBuiltIn = true;
        private static bool showCustom = false;

        public static unsafe void Draw()
        {

        }
    }
}
