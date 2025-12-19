using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.GameHelpers;
using ICE.Utilities.Cosmic_Helper;
using Pictomancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Ui_NpcViewer
    {
        public static void Draw()
        {
            var territoryid = Player.Territory;
            var moonNpcs = NpcData.MoonNpcs[territoryid.RowId];
            ImGui.Text($"Territory Id: {territoryid}");
            ImGui.Text($"Valid Moon NPC Info: {moonNpcs != null}");
            if (moonNpcs != null)
            {
                foreach (var npcEntry in moonNpcs)
                {
                    if (ImGui.CollapsingHeader($"NPC: {npcEntry.Name}"))
                    {
                        Vector3 randomPoint = RandomUtil.GetRandomPointInBounds(npcEntry.Corner1, npcEntry.Corner2, npcEntry.Corner3, npcEntry.Corner4, npcEntry.NpcLocation.Y);
                        if (ImGui.Button($"Path to random point##{npcEntry.Name}"))
                        {
                            IceLogging.DestinationLogs.Log(randomPoint);
                            P.Navmesh.PathfindAndMoveTo(randomPoint, false);
                        }

                        using (var drawList = PictoService.Draw(hints: Utils.GetPictoHints()))
                        {
                            drawList.AddQuadFilled(npcEntry.Corner1, npcEntry.Corner2, npcEntry.Corner3, npcEntry.Corner4, C.PictoColor_Circle);
                        }
                    }
                }
            }
        }
    }
}
