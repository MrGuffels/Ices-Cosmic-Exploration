using Dalamud.Game.ClientState.Conditions;
using ECommons.Automation.NeoTaskManager;
using ECommons.GameHelpers;
using ECommons.UIHelpers.AddonMasterImplementations;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using ICE.Config;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;
using static ICE.Utilities.CosmicHelper;

namespace ICE.Scheduler.Tasks
{
    internal static class Task_FindMission
    {
        /// <summary>
        /// List of all available critical missions
        /// </summary>
        private static HashSet<uint> CriticalMissions = new HashSet<uint>();
        /// <summary>
        /// List of all the available<br></br>
        /// -> Timed <br></br>
        /// -> Weather <br></br>
        /// -> Sequence <br></br>
        /// </summary>
        private static HashSet<uint> WeatherMissions = new HashSet<uint>();
        private static HashSet<uint> TimedMissions = new HashSet<uint>();
        private static HashSet<uint> SequenceMissions = new HashSet<uint>();
        private static HashSet<uint> ExARankMissions = new HashSet<uint>();
        private static HashSet<uint> ARankMissions = new HashSet<uint>();
        private static HashSet<uint> BRankMissions = new HashSet<uint>();
        private static HashSet<uint> CRankMissions = new HashSet<uint>();
        private static HashSet<uint> DRankMissions = new HashSet<uint>();

        private static int SpecialMissionCount = 0;
        private static int BasicMissionCount = 0;

        private static List<Vector3> fishingPath = new List<Vector3>();
        private static GatheringUtil.FisherSpotInfo fishingEntry = new();

        private static readonly Random _random = new Random();
        private static uint missionToAbandon = 0;

        public static void Enqueue()
        {

            P.TaskManager.Enqueue(RefreshMissionUi, "Refreshing Mission UI");
            P.TaskManager.Enqueue(OpenMissionUi, "Opening it on proper class");
            P.TaskManager.Enqueue(RefreshSelectedMissions, "Refreshing the list of viable missions");
            if (C.XPRelicGrind)
            {
                P.TaskManager.Enqueue(() => OpenTab("ExpCheck"), "Opening Standard tab for relic grind");
            }
            else
            {
                P.TaskManager.Enqueue(TabTasksCheck, "Checking which tabs to check for missions");
            }
        }
        public static bool? RefreshMissionUi()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var hud) && !hud.IsAddonReady)
            {
                IceLogging.Debug("Mission Selection Hud is no longer visible and been refreshed. Continuing on");
                return true;
            }
            else
            {
                if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                {
                    if (EzThrottler.Throttle("Closing the hud to make sure it's on the right class"))
                    {
                        IceLogging.Info("Closing out the mission selection hud");
                        moonHud.Mission();
                    }
                }

            }

            return false;
        }
        public static bool? OpenMissionUi()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var hud) && hud.IsAddonReady)
            {
                return true;
            }
            else
            {
                if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                {
                    if (EzThrottler.Throttle("Opening the mission ui"))
                    {
                        IceLogging.Info("Opening the moon mission selection hud");
                        moonHud.Mission();
                    }
                }
            }

            return false;
        }
        public static bool? RefreshSelectedMissions()
        {
            CriticalMissions.Clear();
            WeatherMissions.Clear();
            TimedMissions.Clear();
            SequenceMissions.Clear();
            ExARankMissions.Clear();
            ARankMissions.Clear();
            BRankMissions.Clear();
            CRankMissions.Clear();
            DRankMissions.Clear();
            SpecialMissionCount = 0;
            BasicMissionCount = 0;

            uint? currentJobId = PlayerHelper.GetClassJobId();

            foreach (var mission in C.MissionConfig)
            {
                var enabled = mission.Value.Enabled;

                if (C.XPRelicGrind)
                {
                    if (!enabled && C.XPRelicOnlyEnabled)
                        continue;
                }
                else if (!enabled)
                    continue;

                var missionId = mission.Key;
                var MissionDictionary = CosmicHelper.SheetMissionDict.TryGetValue(missionId, out var missionInfo);
                HashSet<uint> missionJobs = new HashSet<uint>();
                missionJobs = missionInfo.Jobs;
                if (!missionJobs.Contains(currentJobId.Value))
                    continue;

                // Alright, mission was double checked to make sure it was enabled
                // And also checked to make sure that the current job is on the mission, time to actually add it to the mission info

                if (missionInfo.Attributes.HasFlag(MissionAttributes.Critical))
                    CriticalMissions.Add(missionId);
                else if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalSequential))
                {
                    SequenceMissions.Add(missionId);
                    SpecialMissionCount += 1;
                }
                else if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalTimed))
                {
                    TimedMissions.Add(missionId);
                    SpecialMissionCount += 1;
                }
                else if (missionInfo.Attributes.HasFlag(MissionAttributes.ProvisionalWeather))
                {
                    WeatherMissions.Add(missionId);
                    SpecialMissionCount += 1;
                }
                else if (missionInfo.Rank == 5)
                {
                    ExARankMissions.Add(missionId);
                    BasicMissionCount += 1;
                }
                else if (missionInfo.Rank == 4)
                {
                    ARankMissions.Add(missionId);
                    BasicMissionCount += 1;
                }
                else if (missionInfo.Rank == 3)
                {
                    BRankMissions.Add(missionId);
                    BasicMissionCount += 1;
                }
                else if (missionInfo.Rank == 2)
                {
                    CRankMissions.Add(missionId);
                    BasicMissionCount += 1;
                }
                else if (missionInfo.Rank == 1)
                {
                    DRankMissions.Add(missionId);
                    BasicMissionCount += 1;
                }
            }

            IceLogging.Info($"Mission count has been updated to the following for JobId {currentJobId.Value}: \n" +
                $"Critical Count: {CriticalMissions.Count}\n" +
                $"Sequence Count: {SequenceMissions.Count}\n" +
                $"Timed Count: {TimedMissions.Count}\n" +
                $"Weather Count: {WeatherMissions.Count}\n" +
                $"A-EX Rank: {ExARankMissions.Count} \n" +
                $"A Rank: {ARankMissions.Count} \n" +
                $"B Rank: {BRankMissions.Count} \n" +
                $"C Rank: {CRankMissions.Count} \n" +
                $"D Rank: {DRankMissions.Count} \n" +
                $"Total Critical Missions: {CriticalMissions.Count} \n" +
                $"Total Special Missions: {SpecialMissionCount} \n" +
                $"Total Basic Missions: {BasicMissionCount} \n");

            return true;
        }
        public static bool? TabTasksCheck()
        {
            bool hasCritical = CriticalMissions.Count > 0;
            bool hasSpecial = SpecialMissionCount > 0;
            bool hasBasic = BasicMissionCount > 0;

            if (!(hasCritical || hasSpecial || hasBasic))
            {
                IceLogging.Debug("You have... no active missions and you're not on relic grinding mode. Disabling this for now");
                SchedulerMain.State = IceState.Idle;
                SchedulerMain.DisablePlugin();
            }

            if (hasCritical)
                P.TaskManager.Enqueue(() => OpenTab("Critical"), "Opening the critical tab for missions");
            if (hasSpecial)
                P.TaskManager.Enqueue(() => OpenTab("Provisional"), "Opening the provisional tab for missions");
            if (hasBasic)
            {
                P.TaskManager.Enqueue(() => OpenTab("Standard"), "Opening the standard mission tab");
                P.TaskManager.Enqueue(() => FrameDelay(8), "Delaying 8 frames for tab");
                P.TaskManager.Enqueue(CheckStandard, "Checking the standard missions for any potentional missions");
            }

            return true;
        }
        public static bool? OpenTab(string type)
        {
            if (CosmicHelper.CurrentLunarMission != 0)
            {
                IceLogging.Info($"Mission has been accepted. No reason to open tab: {type}");
                return true;
            }

            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                switch (type)
                {
                    case "Critical":
                        {
                            x.CriticalMissions();
                            P.TaskManager.InsertMulti
                            (
                                new(() => FrameDelay(8), "Delaying 8 frames for tab"),
                                new(CheckCritical, "Checking to see if current missions match up with the critical")
                            );
                            break;
                        }
                    case "Provisional":
                        {
                            x.ProvisionalMissions();
                            P.TaskManager.InsertMulti
                            (
                                new(() => FrameDelay(8), "Delaying 8 frames for tab"),
                                new(CheckProvisional, "Checking to see if any provisional missions exist")
                            );
                            break;
                        }
                    case "ExpCheck":
                        {
                            x.BasicMissions();
                            P.TaskManager.InsertMulti
                            (
                                new(() => FrameDelay(8), "Delaying 8 frames for the tab"),
                                new(() => CheckExp(), "Checking Exp Missions")
                            );
                            break;
                        }
                    case "Reset":
                        {
                            x.BasicMissions();
                            P.TaskManager.InsertMulti
                            (
                                new(() => FrameDelay(8), "Throwing a delay in to make sure you're on the right tab"),
                                new(() => FindReroll(), "Finding->Accepting next reroll")
                            );
                            break;
                        }
                    default:
                        {
                            x.BasicMissions();
                            P.TaskManager.InsertMulti
                            (
                                new(() => FrameDelay(8), "Delaying 8 frames for tab"),
                                new(CheckStandard, "Checking the standard missions for any potentional missions")
                            );
                            break;
                        }
                }

                return true;
            }
            else
            {
                if (EzThrottler.Throttle("Opening the mission ui"))
                {
                    if (GenericHelpers.TryGetAddonMaster<WKSHud>("WKSHud", out var moonHud) && moonHud.IsAddonReady)
                        moonHud.Mission();
                }
            }

            return false;
        }
        public static bool? CheckCritical()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                foreach (var mission in x.StellerMissions)
                {
                    if (CriticalMissions.Contains(mission.MissionId))
                    {
                        mission.Select();
                        InsertGrabMission(mission.MissionId);
                        return true;
                    }
                }

                IceLogging.Debug("No mission was found under the critical tab, continuing onto the next", "[Critical Mission Check]");
                return true;
            }

            return false;
        }
        private static bool? CheckProvisional()
        {
            if (CosmicHelper.CurrentLunarMission != 0)
            {
                IceLogging.Debug("Mission has already been accepted, skipping Provisional Missions");
                return true;
            }
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                foreach (var missionType in C.MissionPrio)
                {
                    HashSet<uint> missionIds = missionType switch
                    {
                        ProvisionalTypes.ProvisionalWeather => WeatherMissions,
                        ProvisionalTypes.ProvisionalSequential => SequenceMissions,
                        ProvisionalTypes.ProvisionalTimed => TimedMissions,
                        _ => new HashSet<uint>()
                    };

                    if (missionIds.Count == 0)
                        continue;

                    foreach (var mission in x.StellerMissions)
                    {
                        if (missionIds.Contains(mission.MissionId))
                        {
                            mission.Select();
                            // Insert the task for the following:
                            // -> Check if mission is a gathering or critical
                            //   -> If yes, insert a task to check if need to pathfind to area
                            // -> Insert delay here (small one, like 500 ms)
                            // -> Insert grab mission and switch states to whichever is necessary
                            InsertGrabMission(mission.MissionId);

                            return true;
                        }
                    }
                }

                IceLogging.Debug("No mission was found under the critical tab, continuing onto the next", "[Provisional Mission Check]");
                return true;
            }

            return false;
        }
        public static bool? CheckStandard()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                List<string> RankPriority = new() { "ExA", "A", "B", "C", "D" };

                foreach (var rankType in RankPriority)
                {
                    // Get the appropriate HashSet for this rank
                    HashSet<uint> missionHashSet = rankType switch
                    {
                        "ExA" => ExARankMissions,
                        "A" => ARankMissions,
                        "B" => BRankMissions,
                        "C" => CRankMissions,
                        "D" => DRankMissions,
                        _ => new HashSet<uint>()
                    };

                    // Skip if no missions configured for this rank
                    if (missionHashSet.Count == 0)
                        continue;

                    // Look for missions of this rank type
                    foreach (var mission in x.StellerMissions.Where(m => missionHashSet.Contains(m.MissionId)))
                    {
                        mission.Select();
                        InsertGrabMission(mission.MissionId);
                        IceLogging.Debug($"Mission was found!: {mission.MissionId}. Activating it");
                        return true; // Found and processed a mission
                    }
                }

                // No mission was found, time to kick in Mr. Resetti
                IceLogging.Debug("No mission was found in standard", "[FindMission: CheckStandard]");
                P.TaskManager.Insert(() => CheckReroll(), "Checking Re-Roll Status");
                IceLogging.Debug("Initating the check reroll", "[FindMission: CheckStandard]");
                return true;
            }

            return false;
        }
        private static unsafe bool? CheckExp()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                var bestIndex = FindBestRelicMission();

                if (bestIndex > 0)
                {
                    IceLogging.Debug($"A mission was found for the xp grind: {bestIndex}.");
                    var selectedMission = x.StellerMissions.Where(x => x.MissionId == bestIndex).FirstOrDefault();

                    selectedMission.Select();
                    InsertGrabMission(selectedMission.MissionId);
                    return true;
                }
                else
                {
                    IceLogging.Debug("\n" +
                                     "Somehow, you manage to find absolutely no missions. That's actually impressive.\n" +
                                     "You might have one of the following issues:\n" +
                                     "1: Ignore Manual Mode is enabled, and you have all missions set to manual mode.\n" +
                                     "2: Only Enabled Missions is on, and you have a very limited pool of missions that somehow missed the mark\n" +
                                     "If it's 2, then this should go on to re-roll for you (assuming that you have ATLEAST 1 mission enabled somewhere...\n" +
                                     "Checking this now", "[Xp Grind]");

                    if (C.XPRelicOnlyEnabled && BasicMissionCount != 0)
                    {
                        IceLogging.Debug($"Only relic grind was enabled. Continuing to re-roll mission now");
                        P.TaskManager.Insert(() => FindReroll(), "Finding Reroll mission for Relic Grind");
                        return true;
                    }
                    else
                    {
                        IceLogging.Error($"Okay, something is wrong. Stopping the process", "[Relic Grind]");
                        SchedulerMain.DisablePlugin();
                    }
                }
            }

            return false;
        }

        public static unsafe uint? FindBestRelicMission()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                var wksManager = WKSManager.Instance();
                if (wksManager == null || wksManager->ResearchModule == null || !wksManager->ResearchModule->IsLoaded)
                    return null;

                var job = PlayerHelper.GetClassJobId().Value;
                var toolClassId = (byte)(job - 7);
                var stage = wksManager->ResearchModule->CurrentStages[toolClassId - 1];
                var nextstate = wksManager->ResearchModule->UnlockedStages[toolClassId - 1];

                if (Svc.Data.GetExcelSheet<WKSCosmoToolClass>().TryGetRow(toolClassId, out var row)) { }

                Dictionary<int, CosmicHelper.XPType> XPTable = new Dictionary<int, CosmicHelper.XPType>();

                if (C.UseDummyXp)
                {
                    XPTable = C.DummyXP;
                }
                else
                {
                    for (byte type = 1; type < 6; type++)
                    {
                        if (!wksManager->ResearchModule->IsTypeAvailable(toolClassId, type))
                            break;

                        var neededXP = wksManager->ResearchModule->GetNeededAnalysis(toolClassId, type);

                        var currentXp = wksManager->ResearchModule->GetCurrentAnalysis(toolClassId, type);
                        var requiredXp = neededXP - currentXp;
                        if (!XPTable.ContainsKey(type))
                        {
                            XPTable[type] = new XPType()
                            {
                                CurrentXP = currentXp,
                                NeededXP = neededXP,
                            };
                        }
                    }
                }

                var urgencies = new Dictionary<int, float>();
                for (int i = 0; i < XPTable.Count; i++)
                {
                    var bar = XPTable[i + 1];
                    urgencies[i + 1] = bar.NeededXP > 0 ? 1f - (float)bar.CurrentXP / bar.NeededXP : 0f;
                    IceLogging.Debug($"XP Type: {i+1} | Urgency: {urgencies[i + 1]}");
                }

                Dictionary<uint, Dictionary<int, float>> rewardMissions = new();
                foreach (var availMission in x.StellerMissions)
                {
                    var id = availMission.MissionId;
                    if (CosmicHelper.SheetMissionDict.TryGetValue(id, out var mission))
                    {
                        var missionConfig = C.MissionConfig[id];

                        int minLevel = 10;
                        var rank = mission.Rank;
                        switch (rank)
                        {
                            case 1:
                                minLevel = 10;
                                break;
                            case 2:
                                minLevel = 50;
                                break;
                            case 3:
                                minLevel = 90;
                                break;
                            case 4:
                            case 5:
                            case 6:
                                minLevel = 100;
                                break;
                            default:
                                minLevel = 10;
                                break;
                        }

                        bool properLevel = minLevel <= PlayerHelper.GetLevel();
                        bool IgnoreManual = C.XPRelicIgnoreManual && missionConfig.ManualMode;
                        bool IgnoreNotEnabled = C.XPRelicOnlyEnabled && !missionConfig.Enabled;

                        if (!properLevel) continue;
                        if (IgnoreManual) continue;
                        if (IgnoreNotEnabled) continue;

                        Dictionary<int, float> rewardDict = new();
                        foreach (var reward in mission.RelicXpInfo.OrderBy(x => x.Key))
                        {
                            rewardDict[reward.Key] = reward.Value;
                        }
                        rewardMissions[id] = rewardDict;
                    }
                }

                uint? bestMissionId = null;
                float bestScore = float.NegativeInfinity;

                foreach (var kvp in rewardMissions)
                {
                    uint missionId = kvp.Key;
                    var reward = kvp.Value;
                    float score = 0f;
                    IceLogging.Info($"Currently checking mission: {missionId}");

                    foreach (var rewardEntry in reward)
                    {
                        IceLogging.Debug($"Checking for value: {rewardEntry.Key}");
                        if (urgencies.TryGetValue(rewardEntry.Key, out var urgency))
                        {
                            IceLogging.Info($"Checking urgency for: {rewardEntry.Key}");
                            float contribution = urgency * rewardEntry.Value;

                            // Only add positive contributions (high urgency rewards)
                            if (contribution > 0)
                            {
                                score += contribution;
                                IceLogging.Info($"Adding positive score: {contribution}");
                            }
                            else
                            {
                                IceLogging.Debug($"Skipping negative score: {contribution}");
                            }
                        }
                    }

                    // Compare this mission's score with the current best
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMissionId = missionId;
                        IceLogging.Info($"New best mission: {missionId} with score: {score}");
                    }
                }

                return bestMissionId;
            }
            else
            {
                return null;
            }
        }

        public static bool? FindReroll()
        {
            if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var x) && x.IsAddonReady)
            {
                List<uint> RankId = new() { 5, 4, 3, 2, 1 };
                Dictionary<uint, HashSet<uint>> MissionKeeper = new()
                {
                    [5] = new HashSet<uint>(),
                    [4] = new HashSet<uint>(),
                    [3] = new HashSet<uint>(),
                    [2] = new HashSet<uint>(),
                    [1] = new HashSet<uint>()
                };
                HashSet<uint> AExRank = new HashSet<uint>();
                HashSet<uint> ARank = new HashSet<uint>();
                HashSet<uint> BRank = new HashSet<uint>();
                HashSet<uint> CRank = new HashSet<uint>();
                HashSet<uint> DRank = new HashSet<uint>();

                foreach (var mission in x.StellerMissions)
                {
                    var rank = CosmicHelper.SheetMissionDict[mission.MissionId].Rank;
                    if (RankId.Contains(rank))
                        MissionKeeper[rank].Add(mission.MissionId);
                }

                foreach (var rank in RankId)
                {
                    var missionSet = MissionKeeper[rank];
                    if (missionSet.Count > 0)
                    {
                        bool checkARank = (rank == 5 || rank == 4)
                                       && (MissionKeeper[5].Count > 0 && MissionKeeper[4].Count > 0)
                                       && (ExARankMissions.Count != 0 || ARankMissions.Count != 0);

                        if (checkARank)
                        {

                            if (Mission_Settings.previouslyAbandoned == 5)
                            {
                                Mission_Settings.previouslyAbandoned = 4;
                                missionToAbandon = MissionKeeper[4].First();
                            }
                            else if (Mission_Settings.previouslyAbandoned == 4)
                            {
                                Mission_Settings.previouslyAbandoned = 5;
                                missionToAbandon = MissionKeeper[5].First();
                            }
                            else
                            {
                                missionToAbandon = missionSet.First();
                            }
                        }
                        else if (rank == 3 && BRankMissions.Count > 0)
                        {
                            missionToAbandon = missionSet.First();
                        }
                        else if (rank == 2 && CRankMissions.Count > 0)
                        {
                            missionToAbandon = missionSet.First();
                        }
                        else if (rank == 1 && DRankMissions.Count > 0)
                        {
                            missionToAbandon = missionSet.First();
                        }

                        break;
                    }
                }

                if ((ExARankMissions.Count > 0 || ARankMissions.Count > 0) &&
                    (MissionKeeper[4].Count > 0 || MissionKeeper[5].Count > 0))
                {
                    // If both A and A-EX ranks have missions, alternate between them
                    if (MissionKeeper[4].Count > 0 || MissionKeeper[5].Count > 0)
                    {
                        if (Mission_Settings.previouslyAbandoned == 5)
                        {
                            Mission_Settings.previouslyAbandoned = 4;
                            missionToAbandon = MissionKeeper[4].First();
                            IceLogging.Info("Setting previous abandon to 4");
                            IceLogging.Info($"Abandoning: {missionToAbandon}");
                        }
                        else if (Mission_Settings.previouslyAbandoned == 4)
                        {
                            Mission_Settings.previouslyAbandoned = 5;
                            missionToAbandon = MissionKeeper[5].First();
                            IceLogging.Info("Setting previous abandon to 5", "[Abandoning Mission]");
                            IceLogging.Info($"Abandoning: {missionToAbandon}", "[Abandoning Mission]");
                        }
                        else
                        {
                            // Default to A-EX rank first time
                            Mission_Settings.previouslyAbandoned = 5;
                            missionToAbandon = MissionKeeper[5].First();
                            IceLogging.Info("No previous abandon was found, setting it to 5", "[Abandoning Mission]");
                            IceLogging.Info($"Abandoning: {missionToAbandon}", "[Abandoning Mission]");
                        }
                    }
                    // If only A rank has missions
                    else if (MissionKeeper[4].Count > 0)
                    {
                        Mission_Settings.previouslyAbandoned = 4;
                        missionToAbandon = MissionKeeper[4].First();
                    }
                    // If only A-EX rank has missions
                    else if (MissionKeeper[5].Count > 0)
                    {
                        Mission_Settings.previouslyAbandoned = 5;
                        missionToAbandon = MissionKeeper[5].First();
                    }
                }
                else if (BRankMissions.Count > 0 && MissionKeeper[3].Count > 0)
                {
                    missionToAbandon = MissionKeeper[3].First();
                }
                else if (CRankMissions.Count > 0 && MissionKeeper[2].Count > 0)
                {
                    missionToAbandon = MissionKeeper[2].First();
                }
                else if (DRankMissions.Count > 0 && MissionKeeper[1].Count > 0)
                {
                    missionToAbandon = MissionKeeper[1].First();
                }

                // If we found a mission to abandon
                if (missionToAbandon != 0)
                {
                    var abandonMission = x.StellerMissions.Where(m => m.MissionId == missionToAbandon).First();
                    abandonMission.Select();
                    P.TaskManager.Insert(() => GrabMission(missionToAbandon, true), "Going to abandon mission now");
                    IceLogging.Debug($"Attempting to abandon mission ID: {missionToAbandon} (Rank: {CosmicHelper.SheetMissionDict[missionToAbandon].Rank})");

                    return true;
                }
                else
                {
                    IceLogging.Debug("No valid missions found to abandon");
                    return false;
                }
            }

            return false;
        }
        public static void InsertGrabMission(uint missionId)
        {
            P.TaskManager.InsertMulti(
                new(() => ClearNavFishing(), "Clearing the previous navmesh pathing"),
                new(() => Navmesh_MoveToMission(missionId), "Checking if movement is necessary", Utils.TaskConfig),
                new(() => FrameDelay(8), "Waiting 8 frames before next action"),
                new(() => GrabMission(missionId), "Selecting mission for grabbing")
            );
        }
        private static bool? GrabMission(uint missionId, bool reroll = false)
        {
            if (CosmicHelper.CurrentLunarMission != 0)
            {
                // shouldn't really end up in this state, but as a failsafe of "we're in a mission", returning true to get out of here.
                SchedulerMain.State = IceState.ExecutingMission;
                return true;
            }
            else if (GenericHelpers.TryGetAddonMaster<SelectYesno>("SelectYesno", out var select) && select.IsAddonReady)
            {
                if (EzThrottler.Throttle("Selecting Yesno window"))
                {
                    if (CosmicHandler.commenceStrings.Any(s => select.Text.Contains(s)) || !C.RejectUnknownYesno)
                    {
                        select.Yes();
                        if (reroll)
                            SchedulerMain.State = IceState.AbandonMission;
                        else
                            SchedulerMain.State = IceState.ExecutingMission;
                        IceLogging.Debug($"Current State upon  grabbing mission: {SchedulerMain.State}");
                        P.TaskManager.Tasks.Clear();
                        P.TaskManager.Insert(() => CosmicHelper.CurrentLunarMission != 0);
                        IceLogging.Debug($"Are we expected to reroll? {reroll}", "[Grab Mission]");
                        return true;
                    }
                    else
                    {
                        IceLogging.Error($"Unexpected Yesno window: {select.Text}", "[Grab Mission Task]");
                        select.No();
                        return false;
                    }
                }
            }
            else if (GenericHelpers.TryGetAddonMaster<WKSMission>("WKSMission", out var mission) && mission.IsAddonReady)
            {
                if (EzThrottler.Throttle("Initiating the quest"))
                {
                    var selectedMission = mission.StellerMissions.Where(x => x.MissionId == missionId).FirstOrDefault();
                    selectedMission.Initiate();
                }
            }

            return false;
        }
        public static bool? ClearNavFishing()
        {
            fishingPath.Clear();
            fishingEntry = null;

            return true;
        }
        public static unsafe bool? Navmesh_MoveToMission(uint missionId)
        {
            var missionEntry = CosmicHelper.SheetMissionDict[missionId];
            if (missionEntry.Attributes.HasFlag(MissionAttributes.Gather) || missionEntry.Attributes.HasFlag(MissionAttributes.Critical))
            {
                // Mission was found to be a gathering or critical mission, seeing if you're within range of it
                Vector2 PlayerPos = new Vector2(Player.Position.X, Player.Position.Z);
                Vector2 MapCenter = missionEntry.MapPosition;

                float distance = missionEntry.MarkerId != 0 ? Vector2.Distance(PlayerPos, MapCenter) + 5 : 0;
                if (distance > missionEntry.Radius)
                {
                    if (PlayerHelper.IsPlayerNotBusy() && !Svc.Condition[ConditionFlag.Mounted] && C.UseMountOutsideMission && distance > C.MountRadius)
                    {
                        if (EzThrottler.Throttle("Attempting to mount up"))
                        {
                            Utils.MountAction();
                        }
                    }

                    if (!Svc.Condition[ConditionFlag.Unknown101])
                    {
                        // Condition unknown 101 pops up while you're on the flying... mount thingies. Need to make sure to try and not pathfind while on these/stop current pathfind if you manage to land on one.

                        if (!P.Navmesh.IsRunning())
                        {
                            var nodeLoc = GatheringUtil.GatheringLocation[MapCenter][0].LandZone;
                            if (EzThrottler.Throttle("Initiating navmesh to move to the first landing zone"))
                                P.Navmesh.PathfindAndMoveTo(nodeLoc, false);
                        }
                    }
                    else
                    {
                        if (P.Navmesh.IsRunning())
                            if (EzThrottler.Throttle("Special Movement + Navmesh found, temp stopping"))
                                P.Navmesh.Stop();
                    }

                    if (!P.Navmesh.IsRunning())
                    {
                        // Condition unknown 101 pops up while you're on the flying... mount thingies. Need to make sure to try and not pathfind while on these/stop current pathfind if you manage to land on one.
                        var nodeLoc = GatheringUtil.GatheringLocation[MapCenter][0].LandZone;
                        if (EzThrottler.Throttle("Initiating navmesh to move to the first landing zone"))
                            P.Navmesh.PathfindAndMoveTo(nodeLoc, false);
                    }
                    else if (P.Navmesh.IsRunning())
                    {
                        if (Svc.Condition[ConditionFlag.Unknown101])
                        {
                            P.Navmesh.Stop();
                        }
                    }
                }
                else
                {
                    // Distance has been met, moving onto next condition:
                    IceLogging.Debug("Distance to the node has been closed. Moving onto the next step:", "[Task_FindMission: Navmesh]");
                    return true;
                }
            }
            else if (missionEntry.Attributes.HasFlag(MissionAttributes.Fish) || missionEntry.Attributes.HasFlag(MissionAttributes.LargeFish))
            {
                // Just a way to handle fishing missions specifically (that isn't under the critical unbrella).
                // Need to generate a path to the pre-set spot per fishing hole
                // Then need to add the last path to it once the base has been generated, to make sure that you're facing to the fishing hole properly.
                var location = missionEntry.MapPosition;
                var distance = Player.DistanceTo(location);
                if (GatheringUtil.FishingLocation.TryGetValue(location, out var fisherSpotInfos))
                {
                    if (!P.Navmesh.IsRunning())
                    {
                        foreach (var spot in fisherSpotInfos)
                        {
                            if (Player.DistanceTo(spot.FishingSpot) < 2)
                            {
                                return true;
                            }
                        }

                        // Not in any of the pre-made fishing spots. So time to calculate a path and move to it.
                        if (fishingPath.Count == 0 && !P.Navmesh.PathfindInProgress())
                        {
                            // The current pathfinding does not have a route already made up. Time to create one. 
                            // Make sure to store the route that is selected somewhere, so it knows which one to pathfind to next
                            if (EzThrottler.Throttle("Starting to find path"))
                            {
                                int randomIndex = _random.Next(fisherSpotInfos.Count);
                                fishingEntry = fisherSpotInfos[randomIndex];

                                UpdateFishingPath(fishingEntry.NavtoSpot);
                                // Fire and forget - this will update pathTo when complete
                            }
                        }
                        else if (fishingPath.Count > 0 && !P.Navmesh.IsRunning())
                        {
                            if (EzThrottler.Throttle("Telling navmesh to move to the fishing spot"))
                            {
                                // A path has been made up! Time to
                                // -> Randomly select a spot to move to
                                // -> Add that spot to the end of the route
                                // -> Proceed to move to that route
                                fishingPath.Add(fishingEntry.FishingSpot);
                                P.Navmesh.MoveTo(new List<Vector3>(fishingPath), false);
                                fishingPath.Clear(); // this just exist to reset the current path in case you stop navmesh somehow
                            }
                        }
                    }
                    else
                    {
                        if (PlayerHelper.IsPlayerNotBusy() && !Svc.Condition[ConditionFlag.Mounted] && C.UseMountOutsideMission && distance > C.MountRadius)
                        {
                            if (EzThrottler.Throttle("Attempting to mount up"))
                            {
                                Utils.MountAction();
                            }
                        }
                        return false;
                    }
                }
            }
            else
            {
                IceLogging.Debug("Mission was not a gathering or critical mission. Navmesh moving was not necessary. Moving onto next step", "[Task_FindMission: Navmesh]");
                return true;
            }

            return false;
        }
        private static void UpdateFishingPath(Vector3 pathToArea)
        {
            Vector3 currentPos = Player.Position;

            // Fire and forget - this will update pathTo when complete
            _ = Task.Run(async () =>
            {
                fishingPath = await FindTask(currentPos, pathToArea);
            });
        }

        private static async Task<List<Vector3>> FindTask(Vector3 currentPos, Vector3 pathToArea)
        {
            return await P.Navmesh.Pathfind(currentPos, pathToArea, false);
        }
        private static bool? CheckReroll()
        {
            if (SchedulerMain.State == IceState.ExecutingMission)
            {
                IceLogging.Debug("No reason to reroll, you found a proper mission");
            }
            else
            {
                IceLogging.Debug("No mission was found, time for rerolling!", "[Check Reroll]");

                P.TaskManager.Insert(() => OpenTab("Reset"), "Opening tab to the reset mission");
                IceLogging.Debug("Task for re-roll thrown in", "[Check Reroll]");
            }
            return true;
        }
        public static bool? FrameDelay(int amount)
        {
            P.TaskManager.InsertDelay(amount, true);
            return true;

        }
        public static bool? TimeDelay(int amount)
        {
            P.TaskManager.InsertDelay(amount);
            return true;
        }
    }
}
