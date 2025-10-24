using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.SettingTabs
{
    internal class GambaWheel
    {
        private static bool gambaEnabled = C.GambaEnabled;
        private static int gambaDelay = C.GambaDelay;
        private static int gambaCreditsMinimum = C.GambaCreditsMinimum;
        private static bool gambaPreferSmallerWheel = C.GambaPreferSmallerWheel;

        public static void Draw()
        {
            if (ImGui.Checkbox("Enable Gamba", ref gambaEnabled))
            {
                C.GambaEnabled = gambaEnabled;
                C.Save();
            }
            ImGuiEx.HelpMarker("To run this, make sure you have the gamble wheels shown at Orbitingway, and press start. It will full auto from there.");
            if (gambaEnabled)
            {
                ImGui.SetNextItemWidth(150);
                if (ImGui.SliderInt("Gamba Delay", ref gambaDelay, 50, 2000))
                {
                    C.GambaDelay = gambaDelay;
                    C.SaveDebounced();
                }
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150);
                if (ImGui.SliderInt("Mininum credits to keep", ref gambaCreditsMinimum, 0, 10000))
                {
                    C.GambaCreditsMinimum = gambaCreditsMinimum;
                    C.SaveDebounced();
                }
            }
            if (ImGui.Checkbox("Prefer smaller wheel", ref gambaPreferSmallerWheel))
            {
                C.GambaPreferSmallerWheel = gambaPreferSmallerWheel;
                C.Save();
            }
            ImGuiEx.HelpMarker("This will make the Gamba prefer wheels with less items.");
            ImGui.Separator();
            ImGui.TextUnformatted("Configure the weights for each item in the Gamba. Higher weight = more desirable.");
            ImGui.Spacing();
            foreach (GambaType type in Enum.GetValues(typeof(GambaType)))
            {
                var itemsType = C.GambaItemWeights.Where(x => x.Type == type).OrderBy(x => x.ItemId).ToList();
                if (itemsType.Count == 0) continue;
                if (ImGui.TreeNodeEx($"{type} ({itemsType.Count})##gamba_type_{type}", ImGuiTreeNodeFlags.DefaultOpen))
                {
                    ImGui.Indent();
                    foreach (var gamba in itemsType)
                    {
                        var itemName = ExcelItemHelper.GetName(gamba.ItemId);
                        int weight = gamba.Weight;
                        ImGui.SetNextItemWidth(120f);
                        if (ImGui.InputInt($"[{gamba.ItemId}] {itemName}##gamba_weight", ref weight))
                        {
                            gamba.Weight = weight;
                            C.Save();
                        }
                    }
                    ImGui.Unindent();
                    ImGui.TreePop();
                }
            }
            if (ImGui.Button("Reset Weights"))
            {
                Task_Gamba.EnsureGambaWeightsInitialized(true);
            }
        }
    }
}
