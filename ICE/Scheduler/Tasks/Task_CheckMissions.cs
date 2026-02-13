using ECommons.Automation.NeoTaskManager.Tasks;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using ICE.Utilities.Cosmic_Helper;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Core.Tokens;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_CheckMissions
    {
        private static Dictionary<string, List<uint>> MissionLibrary = new()
        {
            ["Critical"] = new(),
            ["Weather"] = new(),
            ["Timed"] = new(),
            ["Sequence"] = new(),
            ["Ex"] = new(),
            ["A"] = new(),
            ["B"] = new(),
            ["C"] = new(),
            ["D"] = new(),
            ["???"] = new(),
        };

        private static readonly Random _random = new Random();

        private static string LibraryInfo(KeyValuePair<uint, CosmicHelper.CosmicInfo> mission)
        {
            string entry = string.Empty;
            var attribute = mission.Value.Attributes;
            var rank = mission.Value.Rank;

            if (attribute.HasFlag(MissionAttributes.ProvisionalWeather))
                entry = "Weather";
            else if (attribute.HasFlag(MissionAttributes.ProvisionalTimed))
                entry = "Timed";
            else if (attribute.HasFlag(MissionAttributes.ProvisionalSequential))
                entry = "Sequence";
            else if (attribute.HasFlag(MissionAttributes.Critical))
                entry = "Critical";
            else if (rank != 0)
            {
                entry = rank switch
                {
                    5 => "Ex",
                    4 => "A",
                    3 => "B",
                    2 => "C",
                    1 => "D",
                    _ => "???",
                };
            }

            return entry;
        }
        public static bool? RefreshMissionLibrary()
        {
            string tag = "Task Check Mission: Refresh Mission Library";

            foreach (var entry in MissionLibrary)
            {
                entry.Value.Clear();
            }

            var modeSelected = Mission_Settings.Mode;
            foreach (var mission in CosmicHelper.SheetMissionDict)
            {
                if (mission.Value.TerritoryId != Player.Territory.RowId)
                    continue;

                bool provisional = mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalSequential)
                                || mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalTimed)
                                || mission.Value.Attributes.HasFlag(MissionAttributes.ProvisionalWeather);

                var missionId = mission.Key;

                if (C.MissionConfig.TryGetValue(missionId, out var config))
                {
                    if (modeSelected == ModeSelect.LevelMode && CosmicHelper.QuickLevelList.Contains(mission.Key))
                    {
                        var job = Mission_Settings.SelectedJob;
                        var jobLevel = Player.GetLevel((Job)job);
                        var missionLevel = mission.Value.Level;

                        // Short end of it all, making sure to see what tier the player should be doing
                        // Taking the players level and making sure it matches to the tier
                        // 90+ = 90
                        // 50-89 = 50
                        // 10-49 = 10
                        int playerTier = jobLevel >= 90 ? 90 : jobLevel >= 50 ? 50 : 10;

                        if (missionLevel != playerTier)
                            continue;
                        else
                        {
                            MissionLibrary[LibraryInfo(mission)].Add(missionId);
                        }
                    }
                    else if (modeSelected == ModeSelect.RelicMode)
                    {
                        if (!provisional)
                        {
                            if (C.XPRelicOnlyEnabled && config.Enabled)
                            {
                                MissionLibrary[LibraryInfo(mission)].Add(missionId);
                            }
                            else
                            {
                                if (mission.Value.Jobs.Contains(Mission_Settings.SelectedJob))
                                    MissionLibrary[LibraryInfo(mission)].Add(missionId);
                            }
                        }
                    }
                    else if (modeSelected == ModeSelect.Standard)
                    {
                        if (!config.Enabled)
                            continue;

                        if (provisional)
                        {
                            if (C.GrindAllProvisionals)
                            {
                                MissionLibrary[LibraryInfo(mission)].Add(missionId);
                            }
                            else if (mission.Value.Jobs.Contains(Mission_Settings.SelectedJob))
                            {
                                MissionLibrary[LibraryInfo(mission)].Add(missionId);
                            }
                        }
                        else if (mission.Value.Attributes.HasFlag(MissionAttributes.Critical))
                        {
                            if (C.GrindOffClassRedAlert)
                                MissionLibrary[LibraryInfo(mission)].Add(missionId);
                            else if (mission.Value.Jobs.Contains(Mission_Settings.SelectedJob))
                                MissionLibrary[LibraryInfo(mission)].Add(missionId);
                        }
                        else
                        {
                            if (mission.Value.Jobs.Contains(Mission_Settings.SelectedJob))
                                MissionLibrary[LibraryInfo(mission)].Add(missionId);
                        }
                    }
                }
                else
                {
                    IceLogging.Error("We're missing a mission from the config, please report back so I can fix this.\n" +
                        $"MissionID: {mission.Key} | Job (First) {mission.Value.Jobs.First()} | Rank: {mission.Value.Rank}");
                }
            }

            IceLogging.Debug("All viable missions have been loaded, reporting back all counts");
            foreach (var entry in MissionLibrary)
            {
                IceLogging.Debug($"[{entry.Key}] = {entry.Value.Count}", tag);
            }

            if (MissionLibrary.All(x => x.Value.Count == 0))
            {
                IceLogging.Verbose("We currently have no viable missions... which is odd. Please make sure you have some enabled, or report back if this is incorrect\n" +
                    $"Config Mode: {C.SelectedMode} | Mode going into this: {Mission_Settings.Mode}", tag);

                SchedulerMain.State = IceState.Idle;
                P.TaskManager.Tasks.Clear();
                return true;
            }
            else
            {
                IceLogging.Verbose($"Mission finder says we have a valid mission list. So we gonna go find one", tag);
                P.TaskManager.Enqueue(() => OpenMissionUi(), "Opening the mission Ui");
                return true;
            }
        }
        public static bool? OpenMissionUi()
        {
            string tag = "[Task Check Mission: Open Mission UI]";

            if (GenericHelpers.TryGetAddonMaster<Talk>("Talk", out var talkUi) && talkUi.IsAddonReady)
            {
                if (EzThrottler.Throttle("Closing the talk"))
                {
                    IceLogging.Info("Talk ui was visible, clicking through", tag);
                    talkUi.Click();
                }

                return false;
            }

            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var hud) && hud.IsAddonReady)
            {
                IceLogging.Info("The Mission Selection Ui is visible! Continuing on", tag);
                return true;
            }
            else
            {
                if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                {
                    if (EzThrottler.Throttle("Opening the mission ui"))
                    {
                        IceLogging.Info("Opening the moon mission selection hud", tag);
                        moonHud.Mission();
                    }
                }
            }

            return false;
        }
        public static bool? CheckTabs()
        {
            static bool? OpenSpecificTab(MissionTypes type)
            {
                string tag = "Opening Tab";

                if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var hud) && hud.IsAddonReady)
                {
                    switch (type)
                    {
                        case MissionTypes.RedAlert:
                            hud.CriticalMissions(); break;
                        case MissionTypes.Provisional:
                            hud.ProvisionalMissions(); break;
                        case MissionTypes.Standard:
                            hud.BasicMissions(); break;
                    }
                    return true;
                }
                else
                {
                    if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                    {
                        if (EzThrottler.Throttle("Opening the mission ui"))
                        {
                            IceLogging.Info("Opening the moon mission selection hud", tag);
                            moonHud.Mission();
                        }
                    }
                }

                return false;
            }

            string tag = "Check Missions: Check Tabs";
            var priority = C.MissionTypePrio;

            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                foreach (var type in C.MissionTypePrio)
                {
                    switch (type)
                    {
                        case MissionTypes.RedAlert:
                            {
                                if (MissionLibrary["Critical"].Count > 0)
                                {
                                    P.TaskManager.InsertMulti
                                    (
                                        new(() => OpenSpecificTab(type), $"Opening {type}"),
                                        new(() => FrameDelay(8), "Delaying 8 frames for tab"),
                                        new(() => CheckMissions(MissionLibrary["Critical"], type), "Checking Critical tab for missions")
                                    );
                                }
                                break;
                            }
                        case MissionTypes.Provisional:
                            {
                                List<uint> provisionals = new();
                                foreach (var ProvisionalPrio in C.MissionPrio)
                                {
                                    string key = ProvisionalPrio switch
                                    {
                                        ProvisionalTypes.ProvisionalWeather => "Weather",
                                        ProvisionalTypes.ProvisionalSequential => "Sequence",
                                        ProvisionalTypes.ProvisionalTimed => "Timed",
                                        _ => ""
                                    };

                                    if (MissionLibrary.TryGetValue(key, out var missionList))
                                    {
                                        foreach (var jobId in C.JobPrio)
                                        {
                                            // Add missions that match this provisional type AND this job
                                            foreach (var missionId in missionList)
                                            {
                                                if (CosmicHelper.SheetMissionDict.TryGetValue(missionId, out var missionInfo) && missionInfo.Jobs.Contains(jobId) && !provisionals.Contains(missionId))
                                                {
                                                    provisionals.Add(missionId);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (provisionals.Count > 0)
                                {
                                    P.TaskManager.InsertMulti
                                    (
                                        new(() => OpenSpecificTab(type), $"Opening {type}"),
                                        new(() => FrameDelay(8), "Delaying 8 frames for tab"),
                                        new(() => CheckMissions(provisionals, type), "Checking Critical tab for missions")
                                    );
                                }
                                break;
                            }
                        case MissionTypes.Standard:
                            {
                                List<uint> basicMissions = new();
                                List<string> MissionRanks = new() { "Ex", "A", "B", "C", "D" };
                                foreach (var rank in MissionRanks)
                                {
                                    if (MissionLibrary.TryGetValue(rank, out var missionList))
                                    {
                                        foreach (var mission in missionList)
                                        {
                                            if (!basicMissions.Contains(mission))
                                                basicMissions.Add(mission);
                                        }
                                    }
                                }
                                P.TaskManager.InsertMulti
                                (
                                    new(() => OpenSpecificTab(type), $"Opening {type}"),
                                    new(() => FrameDelay(8), "Delaying 8 frames for tab"),
                                    new(() => CheckMissions(basicMissions, type), "Checking Critical tab for missions")
                                );
                                break;
                            }
                        case MissionTypes.DroneSearch:
                            {
                                break;
                            }
                    }
                }
            }
            return true;
        }
        private static bool? CheckMissions(List<uint> missionList, MissionTypes type)
        {
            string tag = "[Check Missions: Queue]";

            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var missionInfo) && missionInfo.IsAddonReady)
            {
                if (type == MissionTypes.Standard)
                {
                    bool OnCorrectTab()
                    {
                        foreach (var mission in missionInfo.StellerMissions)
                        {
                            var sheetInfo = CosmicHelper.SheetMissionDict[mission.MissionId];
                            bool isCritical = sheetInfo.Attributes.HasFlag(MissionAttributes.Critical);
                            bool isProvisional = sheetInfo.Attributes.HasFlag(MissionAttributes.ProvisionalSequential)
                                || sheetInfo.Attributes.HasFlag(MissionAttributes.ProvisionalTimed)
                                || sheetInfo.Attributes.HasFlag(MissionAttributes.ProvisionalWeather);

                            if (isCritical || isProvisional)
                            {
                                if (EzThrottler.Throttle("We're on the wrong tab... changing"))
                                {
                                    missionInfo.BasicMissions();
                                }

                                return false;
                            }
                            if (!sheetInfo.Jobs.Contains(Mission_Settings.SelectedJob))
                            {
                                var jobTab = (int)Mission_Settings.SelectedJob - 8;

                                if (EzThrottler.Throttle("Selecting job tab"))
                                {
                                    missionInfo.SelectClass[jobTab].Select();
                                }
                                return false;
                            }
                        }

                        return true;
                    }

                    if (OnCorrectTab())
                    {
                        var mode = Mission_Settings.Mode;
                        if (mode == ModeSelect.Standard)
                        {
                            foreach (var missionId in missionList)
                            {
                                var mission = missionInfo.StellerMissions.Where(x => x.MissionId == missionId).FirstOrDefault();
                                if (mission != null)
                                {
                                    //TODO: Very direct of "Find the first mission in this list -> grab it. Put in after
                                }
                            }
                        }
                        else if (mode == ModeSelect.LevelMode)
                        {
                            foreach (var missionId in missionList)
                            {
                                // Ideally... there's only 1 mission here if it filters out properly. 
                                var mission = missionInfo.StellerMissions.Where(x => x.MissionId == missionId).FirstOrDefault();
                                if (mission != null)
                                {
                                    //TODO: Very direct of "Find the first mission in this list -> grab it. Put in after
                                }
                            }
                            IceLogging.Verbose($"We seem to have not found the mission. Going to double check to make sure we have the tab unlocked");
                            var firstMission = missionInfo.StellerMissions.FirstOrDefault();
                            if (firstMission != null)
                            {
                                IceLogging.Verbose($"The very first available mission is visible. Checking the level");
                                var TestMissionLevel = CosmicHelper.SheetMissionDict[firstMission.MissionId].Level;
                                var ListMissionLevel = CosmicHelper.SheetMissionDict[missionList.First()].Level;

                                if (TestMissionLevel < ListMissionLevel)
                                {
                                    IceLogging.Verbose("We need to actually grab a mission that hasn't been completed for the purpose of getting the next rank unlocked");
                                    if (ListMissionLevel == 90)
                                    {
                                        if (TestMissionLevel == 50)
                                        {
                                            var mission = missionInfo.StellerMissions
                                                .Where(m => CosmicHelper.Unlock_MissionList.Contains(m.MissionId))
                                                .Where(m => CosmicHelper.SheetMissionDict[m.MissionId].Level == 50)
                                                .Where(m => !MissionCompleted(m.MissionId))
                                                .FirstOrDefault();

                                            if (mission != null)
                                            {
                                                mission.Select();
                                                // TODO: Insert Grab mission logic here;
                                                IceLogging.Verbose($"Mission was found to unlock lv. 90 tab {mission.MissionId}");
                                            }
                                        }
                                        else if (TestMissionLevel == 10)
                                        {
                                            var mission = missionInfo.StellerMissions
                                                .Where(m => CosmicHelper.Unlock_MissionList.Contains(m.MissionId))
                                                .Where(m => CosmicHelper.SheetMissionDict[m.MissionId].Level == 10)
                                                .Where(m => !MissionCompleted(m.MissionId))
                                                .FirstOrDefault();

                                            if (mission != null)
                                            {
                                                mission.Select();
                                                // TODO: Insert Grab mission logic here;
                                                IceLogging.Verbose($"Mission was found to unlock lv. 90 tab {mission.MissionId}");
                                            }
                                        }
                                    }
                                    else if (ListMissionLevel == 50)
                                    {
                                        if (TestMissionLevel == 10)
                                        {
                                            var mission = missionInfo.StellerMissions
                                                .Where(m => CosmicHelper.Unlock_MissionList.Contains(m.MissionId))
                                                .Where(m => CosmicHelper.SheetMissionDict[m.MissionId].Level == 10)
                                                .Where(m => !MissionCompleted(m.MissionId))
                                                .FirstOrDefault();

                                            if (mission != null)
                                            {
                                                mission.Select();
                                                // TODO: Insert Grab mission logic here;
                                                IceLogging.Verbose($"Mission was found to unlock lv. 90 tab {mission.MissionId}");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (mode == ModeSelect.RelicMode)
                        {
                            var job = Mission_Settings.SelectedJob;
                            var relicInfo = CosmicHelper.Cosmic_ClassInfo();
                            var classInfo = relicInfo[job];

                            var urgency = new Dictionary<int, float>();
                            foreach (var exp in classInfo.CurrentExp)
                            {
                                if (classInfo.Stage_Current != classInfo.Stage_Next)
                                    urgency[exp.Key] = exp.Value.Needed > 0 ? 1f - (float)exp.Value.Current / exp.Value.Needed : 0f;
                                else
                                    urgency[exp.Key] = 1f - (float)exp.Value.Current / exp.Value.Max;
                            }
                            IceLogging.Verbose($"Urgency Exp Values");
                            foreach (var exp in urgency)
                            {
                                IceLogging.Verbose($"{exp.Key} : Value: {exp.Value:N2}");
                            }

                            uint? bestMissionId = null;
                            float bestScore = float.NegativeInfinity;
                            foreach (var missionId in missionList)
                            {
                                var mission = missionInfo.StellerMissions.Where(x => x.MissionId == missionId).FirstOrDefault();
                                if (mission != null)
                                {
                                    if (CosmicHelper.SheetMissionDict.TryGetValue(mission.MissionId, out var sheetInfo))
                                    {
                                        float score = 0;
                                        foreach (var reward in sheetInfo.RelicXpInfo)
                                        {
                                            if (urgency.TryGetValue(reward.Key, out var info))
                                            {
                                                float contribution = info * reward.Value;
                                                if (contribution > 0)
                                                {
                                                    score += contribution;
                                                }
                                            }
                                        }
                                        if (score > bestScore)
                                        {
                                            bestScore = score;
                                            bestMissionId = missionId;
                                        }
                                    }
                                }
                            }

                            if (bestMissionId != null)
                            {
                                // Need to insert the logic to grab the mission here if it's not null, for now break
                            }
                        }
                    }
                }
                else if (type == MissionTypes.Provisional)
                {
                    if (missionInfo.StellerMissions.Count() > 0)
                    {
                        foreach (var missionId in missionList)
                        {
                            var mission = missionInfo.StellerMissions.Where(x => x.MissionId == missionId).FirstOrDefault();
                            if (mission != null)
                            {
                                //TODO: Very direct of "Find the first mission in this list -> grab it. Put in after
                                //ALSO TODO: need to mark these as abandoned/not completed if it was abandoned...
                            }
                        }
                    }
                }
                else if (type == MissionTypes.RedAlert)
                {
                    if (missionInfo.StellerMissions.Count() > 0)
                    {
                        foreach (var missionId in missionList)
                        {
                            var mission = missionInfo.StellerMissions.Where(x => x.MissionId == missionId).FirstOrDefault();
                            if (mission != null)
                            {
                                //TODO: Very direct of "Find the first mission in this list -> grab it. Put in after
                            }
                        }
                    }
                }
            }

            return false;
        }
        private static void Insert_GrabMissionTask()
        {

        }
        private static bool? Mission_ChangeJob(uint missionId)
        {
            IceLogging.Verbose("Starting to change job");

            var mission = CosmicHelper.SheetMissionDict[missionId];
            var jobId = mission.Jobs.First();

            if ((uint)Player.Job == jobId)
                return true;
            else
            {
                if (EzThrottler.Throttle("Swapping to job for mission"))
                {
                    GearsetHandler.TaskClassChange((Job)jobId);
                    IceLogging.Debug($"Swapping to job: {jobId}");
                }
                return false;
            }
        }


        public static bool? FrameDelay(int amount)
        {
            P.TaskManager.InsertDelay(amount, true);
            return true;

        }
        private static unsafe bool MissionCompleted(uint id)
        {
            var managerPtr = WKSManager.Instance();
            if (managerPtr == null) return true;

            var manager = (WKSManagerCustom*)managerPtr;
            var isCompleted = manager->IsMissionCompleted(id);

            return isCompleted;
        }

        private static void Notes()
        {
            /*
             * This is kind of my place to just... figure out how tf the logic is going to work. 
             * Right now, the logic is 
             * 1: Store all the missions in the dictionary.
             *   - This doesn't matter if what kind of mode, we're just storing it. It should... allow for re-rolling of missions even when in relic mode on weird edge cases (aka, only selected missions for some reason)
             * 2: Added in logic for checking each tab, and adding drone checking somewhere in there. 
             *   - The way this works should be: Check each tab for a mission. If one exist in that place, we're just going to clear the queue -> just proceed to the grab mission task 
             *   - If not, then it continues onto the next kind
             *   - Drone mode is in there as a general "Hey, we gonna check to see if we can open a drone/have a drone running -> find it between missions (this is nice cause it allows users to dictate when they're going to go looking for a box in case of weather. red alert...)
             * 3: If we get to this point in the queue and we STILL haven't grabbed a mission, it means that we need to reroll for one. 
             *   - Logic will be the same here as before. Check to see what ones need to be rerolled if possible
             *
            */
        }
    }
}
