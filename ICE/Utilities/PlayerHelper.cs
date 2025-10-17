using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.ExcelServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.Sheets;

namespace ICE.Utilities;

public class PlayerHelper
{
    // A lot of these functions are dupes to what is in Ecommons: GameHelper.Player
    // Which means that a lot of these can get depreciated becuase they are either:
    // -> Safer in how they are grabbed
    // -> Less Reduntant in code
    // -> Just genereally better 

    public static bool UsingSupportedJob()
    {
        var jobId = Player.JobId;
        return jobId >= 8 || jobId <= 18;
    }

    public static unsafe int GetLevel(int expArrayIndex = -1)
    {
        if (expArrayIndex == -1) expArrayIndex = Svc.ClientState.LocalPlayer?.ClassJob.Value.ExpArrayIndex ?? 0;
        return UIState.Instance()->PlayerState.ClassJobLevels[expArrayIndex];
    }

    public static bool IsInCosmicZone() => IsInSinusArdorum() || IsInPhaenna();
    public static bool IsInSinusArdorum() => IsInZone(1237);
    public static bool IsInPhaenna() => IsInZone(1291);
    public static bool IsInZone(uint zoneID) => Svc.ClientState.TerritoryType == zoneID;
    private static IPlayerCharacter Object => Svc.ClientState.LocalPlayer;
    private static unsafe float AnimationLock => *(float*)((nint)ActionManager.Instance() + 8);
    public static bool IsAnimationLocked => AnimationLock > 0;
    public static bool CustomIsBusy => GenericHelpers.IsOccupied() || Object.IsCasting || IsAnimationLocked;

    public static unsafe bool HasStatusId(params uint[] statusIDs)
    {
        if (Svc.ClientState.LocalPlayer == null)
            return false;

        var statusID = Svc.ClientState.LocalPlayer.StatusList
            .Select(se => se.StatusId)
            .ToList().Intersect(statusIDs)
            .FirstOrDefault();

        return statusID != default;
    }

    public static int GetGp()
    {
        var gp = Svc.ClientState.LocalPlayer?.CurrentGp ?? 0;
        return (int)gp;
    }

    public static int MaxGp()
    {
        var maxGp = Svc.ClientState.LocalPlayer?.MaxGp ?? 0;
        return (int)maxGp;
    }

    internal static unsafe float GetDistanceToPlayer(Vector3 v3) => Vector3.Distance(v3, Player.GameObject->Position);
    internal static unsafe float GetDistanceToPlayer(IGameObject gameObject) => GetDistanceToPlayer(gameObject.Position);
    public static unsafe bool GetItemCount(uint itemID, out int count, bool includeHq = true, bool includeNq = true)
    {
        try
        {
            itemID = itemID >= 1_000_000 ? itemID - 1_000_000 : itemID;
            count = 0;
            if (includeHq)
                count += InventoryManager.Instance()->GetInventoryItemCount(itemID, true);
            if (includeNq)
                count += InventoryManager.Instance()->GetInventoryItemCount(itemID, false);
            count += InventoryManager.Instance()->GetInventoryItemCount(itemID + 500_000);
            return true;
        }
        catch
        {
            count = 0;
            return false;
        }
    }
    public static unsafe bool NeedsRepair(float below = 0)
    {
        var im = InventoryManager.Instance();
        if (im == null)
        {
            Svc.Log.Error("InventoryManager was null");
            return false;
        }

        var equipped = im->GetInventoryContainer(InventoryType.EquippedItems);
        if (equipped == null)
        {
            Svc.Log.Error("InventoryContainer was null");
            return false;
        }

        if (!equipped->IsLoaded)
        {
            Svc.Log.Error($"InventoryContainer is not loaded");
            return false;
        }

        for (var i = 0; i < equipped->Size; i++)
        {
            var item = equipped->GetInventorySlot(i);
            if (item == null)
                continue;

            var itemCondition = Convert.ToInt32(Convert.ToDouble(item->Condition) / 30000.0 * 100.0);

            if (itemCondition <= below)
            {
                IceLogging.Debug($"Found an item that needed repair. Condition: {itemCondition}");
                return true;
            }
        }

        return false;
    }
}
