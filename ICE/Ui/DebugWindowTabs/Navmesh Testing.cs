using ICE.Scheduler.Tasks.OldTask;
using Pictomancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class Navmesh_Testing
    {
        private static List<Vector3> finalPath = new List<Vector3>();
        private static Vector3 currentPos = new Vector3();
        private static Vector3 BaseCenter = new Vector3(0, 0, 0);
        private static float radius = 12f;
        private static int numCircleWps = 50;
        private static List<Vector3> wholePath = new List<Vector3>();

        // Picto Stuff
        private static float lineWidth = 0;
        private static float dotRadius = 4.2f;
        private static uint CircleColor = 2616716297;
        private static uint LineColor = 804847871;
        private static uint WPColor = 4294180358;
        private static uint TextCol = 2667577343;

        public static void Draw()
        {
            if (Svc.ClientState.LocalPlayer != null)
                currentPos = Svc.ClientState.LocalPlayer.Position;
            else
                currentPos = new Vector3(0, 0, 0);

            ImGui.Text($"Current pos: {currentPos.X:N2} | {currentPos.Y:N2} | {currentPos.Z:N2}");

            if (ImGui.Button("Add Position"))
            {
                finalPath.Add(currentPos);
            }
            for (int i = 0; i < finalPath.Count; i++)
            {
                var entry = finalPath[i];

                ImGui.Text($"X: {entry.X:N2}, Y: {entry.Y:N2}, Z: {entry.Z:N2}");
                ImGui.SameLine();
                if (ImGui.Button($"Adjust###Adjust_{entry}_{i}"))
                {
                    entry = currentPos;
                }
                ImGui.SameLine();
                if (ImGui.Button($"Remove###Remove_{entry}_{i}"))
                {
                    finalPath.Remove(entry);
                    i--;
                }
            }

            wholePath = TaskRelicCheck.BuildFullPath(currentPos, BaseCenter, radius, numCircleWps, finalPath);

            using (var drawList = PictoService.Draw())
            {
                int wpNumber = 0;

                if (drawList == null)
                    return;

                for (int x = 0; x < wholePath.Count; x++)
                {
                    var wp = wholePath[x];
                    if (x <  wholePath.Count - 1)
                    {
                        var nextWp = wholePath[x + 1];
                        drawList.AddLine(wp, nextWp, lineWidth, LineColor);
                    }

                    drawList.AddDot(wp, dotRadius, WPColor);
                    Vector3 wpText = new Vector3(wp.X, wp.Y + 5, wp.Z);
                    wpNumber++;
                    drawList.AddText(wpText, TextCol, $"{wpNumber}", 0);
                }
            }
        }
    }
}
