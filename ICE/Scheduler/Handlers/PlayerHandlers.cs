using Dalamud.Game.ClientState.Conditions;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System.Collections.Generic;
using Time = (int start, int end);

namespace ICE.Scheduler.Handlers;

internal static unsafe class PlayerHandlers
{
    public static readonly Dictionary<Time, string[]> stage9TimeMap = new()
    {
        { (0, 1), new[] { "CRP", "ALC", "GSM" } },
        { (2, 3), new[] { "MIN" } },
        { (4, 5), new[] { "BSM", "CUL", "LTW" } },
        { (6, 7), new[] { "FSH" } },
        { (8, 9), new[] { "ARM", "WVR",  } },
        { (10, 11), new[] { "BTN" } },
        { (12, 13), new[] { "GSM", "CRP", "ALC" } },
        { (14, 15), new[] { "MIN" } },
        { (16, 17), new[] { "LTW", "BSM", "CUL" } },
        { (18, 19), new[] { "FSH" } },
        { (20, 21), new[] { "WVR", "ARM" } },
        { (22, 23), new[] { "BTN" } }
    };

    public static readonly Dictionary<Time, string[]> PhaennaMap = new()
    {
        { (0, 2), new [] { "CRP", "LTW", "ALC", "BTN"} },
        { (2, 4), new [] { "MIN"} },
        { (0, 4), new [] { "ARM" } },
        { (4, 6), new [] { "BSM", "LTW", "WVR", "CUL" } },
        { (4, 8), new [] { "GSM", "ALC", "FSH" } },
        { (6, 8), new [] { "FSH"} },
        { (8, 10), new [] { "CRP", "ARM", "WVR", "ALC", "FSH" } },
        { (8, 12), new [] { "LTW", "CUL", "BTN"} },
        { (10, 12), new [] { "BTN"} },
        { (12, 14), new [] { "BSM", "GSM", "CUL" } },
        { (12, 16), new [] { "WVR" } },
        { (16, 18), new [] { "ARM", "MIN"} },
        { (16, 20), new [] { "CRP" } },
        { (20, 22), new [] { "GSM" } },
        { (20, 24), new [] { "BSM" } },
    };

    private static readonly uint stellarSprintID = 4398;

    public static float Distance(this Vector3 v, Vector3 v2)
    {
        return new Vector2(v.X - v2.X, v.Z - v2.Z).Length();
    }
    public static unsafe bool IsMoving()
    {
        return AgentMap.Instance()->IsPlayerMoving;
    }

    internal static void Tick()
    {
        P.overlayWindow.IsOpen = C.ShowOverlay && PlayerHelper.IsInCosmicZone() && PlayerHelper.UsingSupportedJob();

        if (C.MoonSprint && PlayerHelper.IsInCosmicZone() && !PlayerHelper.HasStatusId(stellarSprintID) && Svc.Condition[ConditionFlag.NormalConditions] && IsMoving()) UseSprint();

        if ((!PlayerHelper.IsInCosmicZone() || !PlayerHelper.UsingSupportedJob()) && SchedulerMain.State != IceState.Idle)
        {
            DisablePlugin();
        }
    }

    internal static void DisablePlugin()
    {
        if (SchedulerMain.State != IceState.Idle)
        {
            P.TaskManager.Abort();
            SchedulerMain.DisablePlugin();
        }
    }

    private static void UseSprint()
    {
        var am = ActionManager.Instance();
        var isSprintReady = am->GetActionStatus(ActionType.GeneralAction, 4) == 0;

        if (isSprintReady) am->UseAction(ActionType.GeneralAction, 4);
    }

    private static (long, long) GetEorzeaTime()
    {
        var eorzeaTime = Framework.Instance()->ClientTime.EorzeaTime;
        long hours = eorzeaTime / 3600 % 24;
        long minutes = eorzeaTime / 60 % 60;
        return (hours, minutes);
    }

    internal static (string[], KeyValuePair<(int start, int end), string[]>) GetTimedJob()
    {
        var currentTimeBonuses = new List<string>();
        KeyValuePair<(int start, int end), string[]> nextTimeBonus = default;
        Dictionary<Time, string[]> currentTimeMap = new();

        if (PlayerHelper.IsInSinusArdorum()) currentTimeMap = stage9TimeMap;
        if (PlayerHelper.IsInPhaenna()) currentTimeMap = PhaennaMap;

        (long hours, _) = GetEorzeaTime();

        // Find ALL current active bonuses and flatten them
        var currentTimes = currentTimeMap.Where(time => hours >= time.Key.start && hours <= time.Key.end);
        foreach (var timeBonus in currentTimes)
        {
            currentTimeBonuses.AddRange(timeBonus.Value);
        }

        // Remove duplicates if needed
        var uniqueCurrentBonuses = currentTimeBonuses.Distinct().ToArray();

        // Find next time bonus
        var nextTime = currentTimeMap
            .Where(time => hours < time.Key.start)
            .OrderBy(time => time.Key.start)
            .FirstOrDefault();

        if (!nextTime.Equals(default(KeyValuePair<(int, int), string[]>)))
            nextTimeBonus = nextTime;
        else
            nextTimeBonus = currentTimeMap.OrderBy(time => time.Key.start).First();

        return (uniqueCurrentBonuses, nextTimeBonus);
    }
}
