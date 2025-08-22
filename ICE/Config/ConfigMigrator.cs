using ECommons;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Config
{
    internal class ConfigMigrator
    {
        public static void MigrateConfigv1()
        {
            if (C.ConfigVersion == 0)
            {
                Svc.Log.Information("You seem to be running the old config version, lets migrate you to the new one");

                C.StopOnAbort = OldConfig.StopOnAbort;
                C.RejectUnknownYesno = OldConfig.RejectUnknownYesno;
                C.DelayGrabMission = OldConfig.DelayGrabMission;
                C.DelayIncrease = OldConfig.DelayIncrease;
                C.DelayCraft = OldConfig.DelayCraft;
                C.DelayCraftIncrease = OldConfig.DelayCraftIncrease;
                C.AnimationLockAbandon = OldConfig.AnimationLockAbandon;

                Svc.Log.Information("Migration of the Safety Settings Completed");

                C.SelectedJob = OldConfig.SelectedJob;
                C.XPRelicGrind = OldConfig.XPRelicGrind;
                C.XPRelicIgnoreManual = OldConfig.XPRelicIgnoreManual;
                C.XPRelicOnlyEnabled = OldConfig.XPRelicOnlyEnabled;
                C.ShowCritical = OldConfig.showCritical;
                C.ShowSequential = OldConfig.showSequential;
                C.ShowWeather = OldConfig.showWeather;
                C.ShowTimeRestricted = OldConfig.showTimeRestricted;
                C.ShowClassA = OldConfig.showClassA;
                C.ShowClassB = OldConfig.showClassB;
                C.ShowClassC = OldConfig.showClassC;
                C.ShowClassD = OldConfig.showClassD;

                Svc.Log.Information("Migration of the Main Window Settings Complete");

                C.ShowOverlay = OldConfig.ShowOverlay;
                C.ShowSeconds = OldConfig.ShowSeconds;

                Svc.Log.Information("Migration of the overlay settings complete");

                C.OnlyGrabMission = OldConfig.OnlyGrabMission;
                C.TargetLevel = OldConfig.TargetLevel;
                C.StopWhenLevel = OldConfig.StopWhenLevel;
                C.StopOnceHitCosmoCredits = OldConfig.StopOnceHitCosmoCredits;
                C.CosmoCreditsCap = OldConfig.CosmoCreditsCap;
                C.StopOnceHitLunarCredits = OldConfig.StopOnceHitLunarCredits;
                C.LunarCreditsCap = OldConfig.LunarCreditsCap;
                C.StopOnceHitCosmicScore = OldConfig.StopOnceHitCosmicScore;
                C.CosmoCreditsCap = OldConfig.CosmicScoreCap;
                C.SequenceMissionPriority = OldConfig.SequenceMissionPriority;
                C.WeatherMissionPriority = OldConfig.WeatherMissionPriority;
                C.TimedMissionPriority = OldConfig.TimedMissionPriority;

                Svc.Log.Information("Migration of the mission settings complete");

                C.TableSortOption = OldConfig.TableSortOption;
                C.HideUnsupportedMissions = OldConfig.HideUnsupportedMissions;
                C.AutoPickCurrentJob = OldConfig.AutoPickCurrentJob;

                Svc.Log.Information("Migration of the table settings complete");

                C.SelfRepairGather = OldConfig.SelfRepairGather;
                C.RepairPercent = OldConfig.RepairPercent;
                C.SelfSpiritbondGather = OldConfig.SelfSpiritbondGather;
                C.SelectedGatherIndex = OldConfig.SelectedGatherIndex;

                Svc.Log.Information("Migration of the base gather settings complete");

                C.AutoCordial = OldConfig.AutoCordial;
                C.inverseCordialPrio = OldConfig.inverseCordialPrio;
                C.CordialMinGp = OldConfig.CordialMinGp;
                C.UseOnFisher = OldConfig.UseOnFisher;
                C.PreventOvercap = OldConfig.PreventOvercap;
                C.UseOnlyInMission = OldConfig.UseOnlyInMission;

                Svc.Log.Information("Migration of the cordial settings complete");

                foreach (var entry in OldConfig.GatherSettings)
                {
                    int Id = entry.Id;
                    string Name = entry.Name;
                    int MimimumGp = entry.MinimumGP;
                    int DualClassAmount = entry.InitialGatheringItemMultiplier;

                    var currentEntry = C.GatherSettings.Where(x => x.Id == Id).FirstOrDefault();
                    if (currentEntry != null)
                    {
                        currentEntry.Id = Id;
                        currentEntry.Name = Name;
                        currentEntry.MinimumGp = MimimumGp;
                        currentEntry.DualClassCraftAmount = DualClassAmount;

                        var buffs = currentEntry.GatherBuffs.Buffs;
                        var oldBuffs = entry.Buffs;

                        SetBuff(buffs["BoonIncrease2"], oldBuffs.BoonIncrease2, oldBuffs.BoonIncrease2Gp, oldBuffs.BoonIncrease2MaxUse);
                        SetBuff(buffs["BoonIncrease1"], oldBuffs.BoonIncrease1, oldBuffs.BoonIncrease1Gp, oldBuffs.BoonIncrease1MaxUse);
                        SetBuff(buffs["Tidings"], oldBuffs.TidingsBool, oldBuffs.TidingsGp, oldBuffs.TidingsMaxUse);
                        SetBuff(buffs["YieldII"], oldBuffs.YieldII, oldBuffs.YieldIIGp, oldBuffs.YieldIIMaxUse);
                        SetBuff(buffs["YieldI"], oldBuffs.YieldI, oldBuffs.YieldIGp, oldBuffs.YieldIMaxUse);
                        SetBuff(buffs["BountifulYieldII"], oldBuffs.BountifulYieldII, oldBuffs.BountifulYieldIIGp, oldBuffs.BountifulYieldIIMaxUse);
                        SetBuff(buffs["BonusIntegrity"], oldBuffs.BonusIntegrity, oldBuffs.BonusIntegrityGp, oldBuffs.BonusIntegrityMaxUse);

                        currentEntry.GatherBuffs.BountifulMinItem = entry.Buffs.BountifulMinItem;
                    }
                    else
                    {
                        var newEntry = new GatherProfile
                        {
                            Id = Id,
                            Name = Name,
                            MinimumGp = MimimumGp,
                            DualClassCraftAmount = DualClassAmount,
                            GatherBuffs = new GatherBuffs()
                        };

                        var oldBuffs = entry.Buffs;

                        SetBuff(newEntry.GatherBuffs.Buffs["BoonIncrease2"], oldBuffs.BoonIncrease2, oldBuffs.BoonIncrease2Gp, oldBuffs.BoonIncrease2MaxUse);
                        SetBuff(newEntry.GatherBuffs.Buffs["BoonIncrease1"], oldBuffs.BoonIncrease1, oldBuffs.BoonIncrease1Gp, oldBuffs.BoonIncrease1MaxUse);
                        SetBuff(newEntry.GatherBuffs.Buffs["Tidings"], oldBuffs.TidingsBool, oldBuffs.TidingsGp, oldBuffs.TidingsMaxUse);
                        SetBuff(newEntry.GatherBuffs.Buffs["YieldII"], oldBuffs.YieldII, oldBuffs.YieldIIGp, oldBuffs.YieldIIMaxUse);
                        SetBuff(newEntry.GatherBuffs.Buffs["YieldI"], oldBuffs.YieldI, oldBuffs.YieldIGp, oldBuffs.YieldIMaxUse);
                        SetBuff(newEntry.GatherBuffs.Buffs["BountifulYieldII"], oldBuffs.BountifulYieldII, oldBuffs.BountifulYieldIIGp, oldBuffs.BountifulYieldIIMaxUse);
                        SetBuff(newEntry.GatherBuffs.Buffs["BonusIntegrity"], oldBuffs.BonusIntegrity, oldBuffs.BonusIntegrityGp, oldBuffs.BonusIntegrityMaxUse);

                        newEntry.GatherBuffs.BountifulMinItem = entry.Buffs.BountifulMinItem;

                        C.GatherSettings.Add(newEntry);
                    }
                }

                Svc.Log.Information("Migration of the Gather Profiles Completed");

                foreach (var gambaItem in OldConfig.GambaItemWeights)
                {
                    var existingItem = C.GambaItemWeights.FirstOrDefault(x => x.ItemId == gambaItem.ItemId);
                    if (existingItem != null)
                    {
                        // Update existing item
                        existingItem.Weight = gambaItem.Weight;
                        existingItem.Type = gambaItem.Type;
                    }
                    else
                    {
                        // Create new instance of the target type
                        C.GambaItemWeights.Add(new Gamba
                        {
                            ItemId = gambaItem.ItemId,
                            Weight = gambaItem.Weight,
                            Type = gambaItem.Type
                        });
                    }
                }

                Svc.Log.Information("Migration of the Gamba Weights are complete");

                foreach (var missionEntry in OldConfig.Missions)
                {
                    var key = missionEntry.Id;

                    var enabled = missionEntry.Enabled;
                    var manualMode = missionEntry.ManualMode;
                    var gatherProfileId = missionEntry.GatherSettingId;
                    bool useAny = (missionEntry.TurnInGold && missionEntry.TurnInSilver && missionEntry.TurnInASAP);
                    bool turninGold = missionEntry.TurnInGold;
                    bool turninSilver = missionEntry.TurnInSilver;
                    bool turninBronze = missionEntry.TurnInASAP;
                    if (useAny)
                    {
                        turninGold = false;
                        turninSilver = false;
                        turninBronze = false;
                    }

                    C.MissionConfig.Add(key, new MissionSettings
                    {
                        Enabled = enabled,
                        ManualMode = manualMode,
                        GatherProfileId = gatherProfileId,
                        AutoTurnin = useAny,
                        TurninGold = turninGold,
                        TurninSilver = turninSilver,
                        TurninBronze = turninBronze,
                    });
                }

                Svc.Log.Information("Migration of the mission configs are now complete");
                Svc.Log.Information("Migration complete, Saving the config and updating config version");
                C.ConfigVersion = 1;
                C.Save();
            }
        }

        private static void SetBuff(GatherBuff buff, bool enabled, int minGp, int maxUse)
        {
            buff.Enabled = enabled;
            buff.MinGp = minGp;
            buff.MaxUse = maxUse;
        }
    }
}
