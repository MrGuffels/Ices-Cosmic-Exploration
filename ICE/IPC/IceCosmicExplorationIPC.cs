using ECommons.EzIpcManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.IPC;

public class IceCosmicExplorationIPC
{
    public IceCosmicExplorationIPC() => EzIPC.Init(this);


    [EzIPC] public bool IsRunning() => SchedulerMain.State != IceState.Idle;
    [EzIPC] public void Enable() => SchedulerMain.EnablePlugin();
    [EzIPC] public void Disable() => SchedulerMain.DisablePlugin();
    /// <summary>
    /// Adds the following missions to your mission list
    /// </summary>
    /// <param name="missionId"></param>
    [EzIPC] public void AddMissions(HashSet<uint> missionId)
    {
        foreach (var id in missionId)
        {
            C.MissionConfig[id].Enabled = true;
        }
        C.Save();
    }

    /// <summary>
    /// Disables/Removes the missions from being selected
    /// </summary>
    /// <param name="missionIds"></param>
    [EzIPC] public void RemoveMissions(HashSet<uint> missionIds)
    {
        foreach (var id in missionIds)
        {
            C.MissionConfig[id].Enabled = false;
        }
        C.Save();
    }

    /// <summary>
    /// Toggle the following missions states
    /// </summary>
    /// <param name="missionIds"></param>
    [EzIPC] public void ToggleMissions(HashSet<uint> missionIds)
    {
        foreach (var id in missionIds)
        {
            C.MissionConfig[id].Enabled = !C.MissionConfig[id].Enabled;
        }
        C.Save();
    }
    /// <summary>
    /// Sets only the missions in the hashset to enabled, everything else will be false
    /// </summary>
    /// <param name="missionIds"></param>
    [EzIPC] public void OnlyMissions(HashSet<uint> missionIds)
    {
        foreach (var mission in C.MissionConfig.Where(x => x.Value.Enabled))
        {
            mission.Value.Enabled = false;
        }

        foreach (var id in missionIds)
        {
            if (C.MissionConfig.TryGetValue(id, out var mission))
            {
                mission.Enabled = true;
            }
        }
        C.Save();
    }

    /// <summary>
    /// Clears all missions that you have enabled
    /// </summary>
    [EzIPC] public void ClearAllMissions()
    {
        foreach (var mission in C.MissionConfig)
        {
            mission.Value.Enabled = false;
        }
        C.Save();
    }

    /// <summary>
    /// Opens the map and flags/shows a radius of where the mission's gathering point is
    /// </summary>
    /// <param name="id"></param>
    [EzIPC] public void FlagMissionArea(uint id)
    {
        var info = CosmicHelper.SheetMissionDict.FirstOrDefault(x => x.Key == id);
        if (info.Value == default) return;
        if (info.Value.MarkerId == 0) return;

        Utils.SetGatheringRing(info.Value.TerritoryId, (int)info.Value.MapPosition.X, (int)info.Value.MapPosition.Y, info.Value.Radius, info.Value.Name);
    }

    /// <summary>
    /// Sets "Grab Mission Only" to the state of your choice
    /// </summary>
    [EzIPC] public void GrabMissionOnly(bool state)
    {
        Mission_Settings.StopBeforeGrab = state;
    }

    /// <summary>
    /// Returns the current state(s) that ICE is currently in a string format. 
    /// </summary>
    /// <returns></returns>
    [EzIPC] public string CurrentState()
    {
        return SchedulerMain.State.ToString();
    }
}
