using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Scheduler.Tasks.OldTask
{
    internal class TaskRelicCheck
    {
        private static Vector3 baseCenter = new Vector3(0, 1.6f, 0);

        public static void Enqueue()
        {
            // Center of the moon base
            float radius = 12;
            int numWaypoints = 50;

            // Logic to return back to base

            if (PlayerHelper.GetDistanceToPlayer(baseCenter) > 25)
            {
                P.TaskManager.Enqueue(() => ReturnToBase());
            }
        }

        // - - - - - 
        // Task
        // - - - - - 

        internal static unsafe bool? ReturnToBase()
        {
            if (Player.DistanceTo(baseCenter) < 25)
                return true;
            else
            {
                if (!Player.IsBusy)
                {
                    if (EzThrottler.Throttle("Returning to base"))
                    {
                        Svc.Log.Information("Launching action to return to base");
                        ActionManager.Instance()->UseAction(ActionType.GeneralAction, 26);
                    }
                }
            }

            return false;
        }

        internal static unsafe bool? MoveToRelic()
        {
            Vector3 CosmicResearcher = new Vector3(-18.9f, 2.69f, 18.45f);
            if (Player.DistanceTo(CosmicResearcher) < 6 && !P.Navmesh.IsRunning())
            {
                return true;
            }
            else
            {

            }

            return false;
        }

        // - - - - - 
        // Functions
        // - - - - - 
        public static List<Vector3> BuildFullPath(Vector3 currentPos, Vector3 center, float radius, int numPoints, List<Vector3> finalPath)
        {
            List<Vector3> GenerateCirclePoints(Vector3 center, float radius, int numPoints)
            {
                List<Vector3> points = new();
                for (int i = 0; i < numPoints; i++)
                {
                    float angle = 2 * MathF.PI * i / numPoints;
                    float x = center.X + radius * MathF.Cos(angle);
                    float z = center.Z + radius * MathF.Sin(angle);
                    points.Add(new Vector3(x, center.Y, z));
                }
                return points;
            }

            int GetClosestPointIndex(List<Vector3> circlePoints, Vector3 currentPos)
            {
                int closestIndex = 0;
                float closestDist = float.MaxValue;

                for (int i = 0; i < circlePoints.Count; i++)
                {
                    float dist = Vector3.DistanceSquared(circlePoints[i], currentPos);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestIndex = i;
                    }
                }
                return closestIndex;
            }

            List<Vector3> GetShortestArc(List<Vector3> circlePoints, int startIndex, int endIndex)
            {
                int count = circlePoints.Count;

                // Clockwise
                List<Vector3> clockwise = new();
                for (int i = 0; i <= (endIndex - startIndex + count) % count; i++)
                    clockwise.Add(circlePoints[(startIndex + i) % count]);

                // Counter-clockwise
                List<Vector3> counter = new();
                for (int i = 0; i <= (startIndex - endIndex + count) % count; i++)
                    counter.Add(circlePoints[(startIndex - i + count) % count]);

                float distClockwise = 0;
                for (int i = 1; i < clockwise.Count; i++)
                    distClockwise += Vector3.Distance(clockwise[i - 1], clockwise[i]);

                float distCounter = 0;
                for (int i = 1; i < counter.Count; i++)
                    distCounter += Vector3.Distance(counter[i - 1], counter[i]);

                return distClockwise <= distCounter ? clockwise : counter;
            }

            int GetClosestPointIndexToTarget(List<Vector3> circlePoints, List<Vector3> finalPath)
            {
                if (finalPath == null || finalPath.Count == 0)
                    throw new ArgumentException("finalPath must have at least one point");

                Vector3 targetStart = finalPath[0];
                return GetClosestPointIndex(circlePoints, targetStart);
            }

            var circlePoints = GenerateCirclePoints(center, radius, numPoints);
            int closestIndex = GetClosestPointIndex(circlePoints, currentPos);
            int targetCircleIndex = GetClosestPointIndexToTarget(circlePoints, finalPath);

            List<Vector3> fullPath = new();
            fullPath.Add(circlePoints[closestIndex]);
            fullPath.AddRange(GetShortestArc(circlePoints, closestIndex, targetCircleIndex));
            fullPath.AddRange(finalPath);

            return fullPath;
        }
    }
}
