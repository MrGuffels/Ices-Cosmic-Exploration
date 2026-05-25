using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.STD;
using System.Runtime.InteropServices;

namespace ICE.Utilities.Cosmic_Helper;

/// <summary>
/// Shim for AgentWKSMission members not yet in the shipped FFXIVClientStructs DLL.
/// Remove once upstream CS ships GetCriticalMissions.
/// </summary>
public static unsafe class AgentWKSMissionEx
{
    private delegate bool GetCriticalMissionsDelegate(
        AgentWKSMission* agent,
        StdVector<AgentWKSMission.MissionEntry>* list);

    private static readonly GetCriticalMissionsDelegate? _getCriticalMissions;

    static AgentWKSMissionEx()
    {
        try
        {
            var ptr = Svc.SigScanner.ScanText( "48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 41 54 41 55 41 56 41 57 48 83 EC ?? 48 8B EA E8");
            _getCriticalMissions = Marshal.GetDelegateForFunctionPointer<GetCriticalMissionsDelegate>(ptr);
        }
        catch (Exception ex)
        {
            IceLogging.Error($"{ex.Message} | [AgentWKSMissionEx] Failed to scan GetCriticalMissions sig");
        }
    }

    /// <summary>
    /// Populates <paramref name="list"/> with the current critical missions, same as
    /// GetBasicMissions / GetProvisionalMissions. Returns false if the sig wasn't found.
    /// </summary>
    public static bool GetCriticalMissions( AgentWKSMission* agent, StdVector<AgentWKSMission.MissionEntry>* list)
    {
        if (_getCriticalMissions == null || agent == null) return false;
        return _getCriticalMissions(agent, list);
    }
}
