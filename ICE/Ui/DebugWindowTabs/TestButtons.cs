using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using ICE.Scheduler.Tasks.OldTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class TestButtons
    {
        public static unsafe void Draw()
        {
            ImGui.Text($"Current Mission: {CosmicHelper.CurrentLunarMission}");
            ImGui.Text($"Artisan Endurance: {P.Artisan.GetEnduranceStatus()}");

            //  4 - Col 2  - Unknown 7
            //  8 - Col 3  - Unknown 0
            // 10 - Col 4  - Unknown 1
            //  3 - Col 7  - Unknown 12
            //  7 - Col 8  - Unknown 2
            //  2 - Col 10 - Unknown 13
            //  5 - Col 11 - Unknown 3
            //  1 - Col 13 - Unknown 14
            //  5 - Col 14 - Unknown 4
            //  0          - Unknown 5
            //  0          - Unknown 6
            //  0          - Unknown 8
            //  1          - Unknown 9 
            //  1          - Unknown 10
            //  1          - Unknown 11

            ImGui.Text($"{WKSManager.Instance()->CurrentMissionUnitRowId}");

            if (ImGui.Button("Find Mission"))
            {
                // TaskMissionFind.Enqueue();
            }
            if (ImGui.Button("Clear Task"))
            {
                P.TaskManager.Abort();
            }
            if (ImGui.Button("Artisan Craft"))
            {
                P.Artisan.CraftItem(36176, 1);
            }
            if (ImGui.Button("RecipeNote"))
            {
                AddonHelper.OpenRecipeNote();
            }
            var gameObject = Utils.TryGetObjectNearestEventObject();
            float gameObjectDistance = 0;
            if (gameObject is not null)
                gameObjectDistance = PlayerHelper.GetDistanceToPlayer(gameObject);
            if (ImGui.Button("Click Nearest EventObject"))
            {
                Utils.TargetgameObjectTask(gameObject);
                Utils.InteractWithObject(gameObject);
            }
            ImGui.SameLine();
            ImGui.Text($"Distance to nearest: {gameObjectDistance}");

            var collectionPoint = Utils.TryGetObjectCollectionPoint();
            float collectionPointDistance = 0;
            if (collectionPoint is not null)
                collectionPointDistance = PlayerHelper.GetDistanceToPlayer(collectionPoint);
            if (ImGui.Button("Click Nearest Collection Point"))
            {
                Utils.TargetgameObjectTask(collectionPoint);
                Utils.InteractWithObject(collectionPoint);
            }
            ImGui.SameLine();
            ImGui.Text($"Distance to nearest: {collectionPointDistance}");

            if (ImGui.Button("Print GatheringPoint Info"))
            {
                var gatheringPoint = Svc.ClientState.LocalPlayer.TargetObject;
                if (gatheringPoint is not null)
                {
                    var nodeId = gatheringPoint.DataId;
                    var position = gatheringPoint.Position;
                    var landZone = gatheringPoint.Position;
                    var gatheringType = (Job)PlayerHelper.GetClassJobId() == Job.MIN ? 2 : 3;
                    var currentMission = CosmicHelper.CurrentMissionInfo;
                    var nodeSet = currentMission?.NodeSet ?? 0;

                    string info = $"new GathNodeInfo\n{{\n    ZoneId = 1237,\n    NodeId = {nodeId},\n    Position = new Vector3({position.X}f, {position.Y}f, {position.Z}f),\n    LandZone = new Vector3({landZone.X}f, {landZone.Y}f, {landZone.Z}f),\n    GatheringType = {gatheringType},\n    NodeSet = {nodeSet}\n}}";

                    ImGui.SetClipboardText(info);
                    Svc.Chat.Print(info);
                }
                else
                {
                    Svc.Chat.Print("No GatheringPoint targeted.");
                }
            }

            if (ImGui.Button("Switch class to CRP"))
            {
                GearsetHandler.TaskClassChange(Job.CRP);
            }
            if (ImGui.Button("Switch class to MIN"))
            {
                GearsetHandler.TaskClassChange(Job.MIN);
            }
        }
    }
}
