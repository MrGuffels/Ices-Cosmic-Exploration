using ECommons;
using FFXIVClientStructs.FFXIV.Client.Game.WKS;
using ICE.Utilities.Cosmic_Helper;
using ICE.Utilities.GatheringHelper;
using Lumina.Excel.Sheets;
using System.Collections.Generic;
using static ICE.Enums.MissionAttributes;
using static ICE.Utilities.CosmicHelper;
using static ICE.Utilities.ExcelHelper;

namespace ICE;

public sealed partial class ICE
{
    public static unsafe void DictionaryCreation()
    {
        var wk = WKSManager.Instance();

        foreach (var entry in MoonMissionSheet)
        {
            Dictionary<ushort, CosmicHelper.CraftingInfo> crafts_Main = new();
            Dictionary<ushort, CosmicHelper.CraftingInfo> crafts_Pre = new();
            Dictionary<uint, int> gathering_Min = new();
            HashSet<uint> jobs = new();
            Dictionary<int, int> relicXp = new();

            uint keyId = entry.RowId;
            string missionName = entry.Name.ToString();
            missionName = missionName.Replace("<nbsp>", " ");
            missionName = missionName.Replace("<->", "");

            if (missionName == "")
                continue;

            jobs.Add(entry.ClassJobCategory[0].RowId - 1);
            var Job2 = entry.ClassJobCategory[1].RowId;
            if (Job2 != 0)
            {
                jobs.Add(Job2 - 1);
            }
            uint timeLimit = entry.MissionTime;
            uint silver = entry.SilverStarRequirement;
            uint gold = entry.GoldStarRequirement;
            HashSet<uint> previousMissionId = new() { entry.LockedBehind.RowId };

            uint timeAndWeather = entry.WKSMissionLotterySpecialCond.RowId;
            uint startTime = 0;
            uint endTime = 0;
            CosmicWeather weather = CosmicWeather.FairSkies;
            if (!CosmicHelper.WeatherSelection.Contains(timeAndWeather))
            {
                var timeSheet = Svc.Data.GetExcelSheet<WKSMissionLotterySpecialCond>().GetRow(timeAndWeather);
                startTime = timeSheet.Unknown1; // Start Time
                endTime = timeSheet.Unknown2; // End Time
            }
            else
            {
                weather = (CosmicWeather)(timeAndWeather - 12);
                // TODO: Go back and assign enums based on the value instead... or just directly give it a flag. Unsure. Feels dirty
            }

            uint rank = entry.LevelGroup;
            bool isCritical = entry.IsSpecialQuest;

            uint RecipeId = entry.WKSMissionRecipe.RowId;

            uint toDoValue = entry.MissionToDo[0].RowId;

            var wksToDo = ToDoSheet.GetRow(toDoValue);
            uint missionText = wksToDo.WKSMissionText.Value.RowId;
            var marker = MarkerSheet.GetRow(wksToDo.Unknown13);
            uint territoryId = 1237; 
            if (keyId < 545)
            {
                territoryId = 1237;
            }
            else
            {
                territoryId = 1291;
            }
            // TODO: Make this set the correct territoryId once new planets are added and we figure out where it is.

            int _x = marker.Unknown1 - 1024;
            int _y = marker.Unknown2 - 1024;
            int radius = marker.Unknown3;

            MissionAttributes attributes = None;

            if (CosmicHelper.CrafterJobList.Overlaps(jobs) && CosmicHelper.GatheringJobList.Overlaps(jobs))
            {
                if (jobs.Contains(18))
                    attributes = Craft | Fish;
                else
                    attributes = Craft | Gather;
            }
            else if (CosmicHelper.CrafterJobList.Overlaps(jobs))
            {
                // Just making this nice and simple. Making all crafter missions just check for crafting.
                // Then going to add critical after
                attributes = Craft;
            }
            else if (CosmicHelper.GatheringJobList.Overlaps(jobs))
            {
                if (jobs.Contains(18))
                    attributes |= Fish;
                else
                    attributes |= Gather;

                attributes = missionText switch
                {
                    103 => Gather | Limited,
                    104 => Gather | ScoreTimeRemaining,
                    105 => Gather,
                    106 => Gather | ScoreChains,
                    107 => Gather | ScoreGatherersBoon,
                    108 => Gather | ScoreChains | ScoreGatherersBoon,
                    109 or 111 => Gather | Collectables,
                    110 => Gather | ReducedItems | ScoreTimeRemaining,
                    112 => Gather | ReducedItems,
                    113 => Fish | ScoreVariety | ScoreTimeRemaining,
                    114 or 115 => Fish | ScoreTimeRemaining,
                    116 => Fish | Limited | ScoreVariety,
                    117 => Fish | Limited | ScoreLargestSize,
                    118 => Fish | Limited | Collectables,
                    119 or 121 => Fish,
                    120 => Fish | ScoreLargestSize,
                    122 => Fish | Collectables,
                    >= 123 and <= 134 => Craft | Gather, // Dual class
                    >= 135 and <= 138 => Craft | Fish,  // Dual class
                    139 => jobs.Contains(18) ? Fish : Gather, // Critical
                    141 => Fish,
                    _ => None
                };
            }

            attributes |= isCritical ? Critical : None;
            attributes |= weather != CosmicWeather.FairSkies ? ProvisionalWeather : None;
            attributes |= (startTime != 0 || endTime != 0) ? ProvisionalTimed : None;
            attributes |= !previousMissionId.Contains(0) ? ProvisionalSequential : None;

            // - - - HEY. BRONZE SCORE IS KEPT HERE - - - //
            uint bronze = wksToDo.Unknown2; // Bronze score for Score missions

            if (CrafterJobList.Overlaps(jobs))
            {
                var wksRecipeRow = wksMissionRecipe.GetRow(RecipeId);

                if (isCritical) // Criticals are sus
                {
                    var itemAmount = 3; // It's a pass/fail progress, you need to go till you are full on score
                    if (keyId < 535)
                        itemAmount = 3;
                    else if (keyId < 1039)
                        itemAmount = 2;

                    var missionRecipeRow = RecipeSheet?.Where(e => e.RowId == wksRecipeRow.Recipe[0].RowId).FirstOrDefault();
                    var itemId = missionRecipeRow.Value.ItemResult. RowId;
                    var itemName = ItemSheet.GetRow(itemId).Name.ToString();
                    var craftingType = missionRecipeRow.Value.CraftType.Value.RowId;
                    IceLogging.Verbose($"Recipe Row ID: {missionRecipeRow.Value.RowId} | for item: {itemId} | {itemName}", debugOnly: true);
                    var item1RecipeId = missionRecipeRow.Value.RowId;

                    crafts_Main[(ushort)item1RecipeId] = new CraftingInfo()
                    {
                        ItemId = itemId,
                        RequiredAmount = itemAmount,
                    };
                }
                else
                {
                    // Reason for the following code is this:
                    // If it's a pre-craft, it should be further down the list, which means adding it first to the pre-crafts
                    // If it's required, then all of them SHOULD... be required. *-shrugs-*
                    List<ushort> recipeIds = new();
                    for (int x = 2; x >= 0; x--)
                    {
                        var recipeId = (ushort)wksRecipeRow.Recipe[x].Value.RowId;
                        if (recipeId != 0 && !recipeIds.Contains(recipeId))
                            recipeIds.Add(recipeId);
                    }

                    if (recipeIds.Count == 1)
                    {
                        // Only a single item exist in this table. So into the maincrafts it goes
                        IceLogging.Verbose($"Mission: {keyId} had 1 recipie", debugOnly:true);
                        var recipeId = recipeIds[0];
                        var recipeRow = RecipeSheet.GetRow(recipeId);
                        var itemId = recipeRow.ItemResult.RowId;
                        var amountNeeded = wksToDo.RequiredItemQuantity[0];
                        if (amountNeeded == 0)
                        {
                            // this should never happen. But on the off chance that square decides to be a dick and change it's place
                            amountNeeded = 1;
                        }
                        var requiredItem = recipeRow.Ingredient[0].RowId;
                        var requiredAmount = recipeRow.AmountIngredient[0];
                        var requiredItem2 = recipeRow.Ingredient[1].RowId;
                        var requiredAmount2 = recipeRow.AmountIngredient[1];

                        if (requiredItem2 != 0)
                        {
                            crafts_Main[recipeId] = new()
                            {
                                ItemId = itemId,
                                RequiredAmount = amountNeeded,
                                RequiredItems = new()
                                {
                                    [requiredItem] = requiredAmount,
                                    [requiredItem2] = requiredAmount2
                                }
                            };
                        }
                        else
                        {
                            crafts_Main[recipeId] = new()
                            {
                                ItemId = itemId,
                                RequiredAmount = amountNeeded,
                                RequiredItems = new()
                                {
                                    [requiredItem] = requiredAmount
                                }
                            };
                        }
                    }
                    else if (recipeIds.Count == 2)
                    {
                        IceLogging.Verbose($"Mission: {keyId} had 2 recipies", debugOnly: true);
                        // First one is going to be the main item that you need.

                        var recipeId = recipeIds[0];
                        var recipeRow = RecipeSheet.GetRow(recipeId);
                        var itemId = recipeRow.ItemResult.RowId;
                        var amountNeeded = wksToDo.RequiredItemQuantity[0];
                        if (amountNeeded == 0)
                        {
                            // this should never happen. But on the off chance that square decides to be a dick and change it's place
                            amountNeeded = 1;
                        }
                        var requiredItem = recipeRow.Ingredient[0].RowId;
                        var requiredAmount = recipeRow.AmountIngredient[0];
                        crafts_Main[recipeId] = new()
                        {
                            ItemId = itemId,
                            RequiredAmount = amountNeeded,
                            RequiredItems = new()
                            {
                                [requiredItem] = requiredAmount
                            }
                        };

                        // Second one is going to be the pre-crafting mat that you need
                        var preRecipeId = recipeIds[1];
                        var preRecipeRow = RecipeSheet.GetRow(preRecipeId);
                        var preItemId = preRecipeRow.ItemResult.RowId;
                        var preAmountNeeded = requiredAmount;

                        var crateId = preRecipeRow.Ingredient[0].RowId;

                        crafts_Pre[preRecipeId] = new()
                        {
                            ItemId = preItemId,
                            RequiredAmount = preAmountNeeded,
                            RequiredItems = new()
                            {
                                [crateId] = preAmountNeeded
                            }
                        };

                    }
                    else if (recipeIds.Count == 3)
                    {
                        IceLogging.Verbose($"Mission: {keyId} had 3 recipies", debugOnly: true);
                        // all of these should be valid. 
                        for (int i = 0; i < recipeIds.Count; i++)
                        {
                            // Only a single item exist in this table. So into the maincrafts it goes
                            var recipeId = recipeIds[i];
                            var recipeRow = RecipeSheet.GetRow(recipeId);
                            var itemId = recipeRow.ItemResult.RowId;
                            var amountNeeded = wksToDo.RequiredItemQuantity[i];
                            if (amountNeeded == 0)
                            {
                                // this should never happen. But on the off chance that square decides to be a dick and change it's place
                                amountNeeded = 1;
                            }
                            var requiredItem = recipeRow.Ingredient[0].RowId;
                            var requiredAmount = recipeRow.AmountIngredient[0];
                            crafts_Main[recipeId] = new()
                            {
                                ItemId = itemId,
                                RequiredAmount = amountNeeded,
                                RequiredItems = new()
                                {
                                    [requiredItem] = requiredAmount
                                }
                            };
                        }
                    }

                    // This is just a general sanity check in itself for mission where there isn't a required item count, but moreso just needs score. 
                    if (crafts_Main.Count == 0)
                    {
                        // These are missions that don't require an item, but for the sanity check of it all, going to just have it be 1. 
                        // Still need to hardcode the bronze scores in though

                        foreach (var item in crafts_Pre)
                        {
                            item.Value.RequiredAmount = 1;
                            crafts_Main.Add(item);
                            crafts_Pre.Remove(item);
                        }
                    }
                }
            }

            if (GatheringJobList.Overlaps(jobs))
            {
                var todoRow = ToDoSheet.GetRow(toDoValue);

                if (todoRow.RequiredItem[0].RowId != 0) // First item in the gathering list. Shouldn't be 0...
                {
                    var minAmount = todoRow.RequiredItemQuantity[0].ToInt();
                    var itemInfoId = MoonItemInfoSheet.GetRow(todoRow.RequiredItem[0].RowId).Item.RowId;
                    if (!gathering_Min.ContainsKey(itemInfoId))
                    {
                        gathering_Min.Add(itemInfoId, minAmount);
                    }
                }
                if (todoRow.RequiredItem[1].RowId != 0) // First item in the gathering list. Shouldn't be 0...
                {
                    var minAmount = todoRow.RequiredItemQuantity[1].ToInt();
                    var itemInfoId = MoonItemInfoSheet.GetRow(todoRow.RequiredItem[1].RowId).Item.RowId;
                    if (!gathering_Min.ContainsKey(itemInfoId))
                    {
                        gathering_Min.Add(itemInfoId, minAmount);
                    }
                }
                if (todoRow.RequiredItem[2].RowId != 0) // First item in the gathering list. Shouldn't be 0...
                {
                    var minAmount = todoRow.RequiredItemQuantity[2].ToInt();
                    var itemInfoId = MoonItemInfoSheet.GetRow(todoRow.RequiredItem[2].RowId).Item.RowId;
                    if (!gathering_Min.ContainsKey(itemInfoId))
                    {
                        gathering_Min.Add(itemInfoId, minAmount);
                    }
                }
            }

            // Col 3 -> Cosmocredits - Unknown 0
            // Col 4 -> Lunar Credits - Unknown 1
            // Col 7 ->  Lv. 1 Type - Unknown 12
            // Col 8 ->  Lv. 1 Exp - Unknown 2
            // Col 10 -> Lv. 2 Type - Unknown 13
            // Col 11 -> Lv. 2 Exp - Unknown 3
            // Col 13 -> Lv. 3 Type - Unknown 14
            // Col 14 -> Lv. 3 Exp - Unknown 4

            // Something to note here, a mission can only have a max of 3 types of XP at a time.
            // Which is why there's only 3 entries.

            uint Cosmo = ExpSheet.GetRow(keyId).Unknown0;
            uint Lunar = ExpSheet.GetRow(keyId).Unknown1;
            uint rewardItemId = 0;
            uint rewardItemAmount = 0;

            if (ExpSheet.GetRow(keyId).Unknown2 != 0)
            {
                var xp1Kind = ExpSheet.GetRow(keyId).Unknown12;
                var xp1Amount = ExpSheet.GetRow(keyId).Unknown2;
                relicXp[xp1Kind] = xp1Amount;
            }
            if (ExpSheet.GetRow(keyId).Unknown3 != 0)
            {
                var xp2Kind = ExpSheet.GetRow(keyId).Unknown13;
                var xp2Amount = ExpSheet.GetRow(keyId).Unknown3;
                relicXp[xp2Kind] = xp2Amount;
            }
            if (ExpSheet.GetRow(keyId).Unknown4 != 0)
            {
                var xp3Kind = ExpSheet.GetRow(keyId).Unknown14;
                var xp3Amount = ExpSheet.GetRow(keyId).Unknown4;
                relicXp[xp3Kind] = xp3Amount;
            }
            if (ExpSheet.GetRow(keyId).Unknown15 != 0)
            {
                rewardItemId = ExpSheet.GetRow(keyId).Unknown15; // Column 15 | Item
                rewardItemAmount = ExpSheet.GetRow(keyId).Unknown8;
            }

            if (!SheetMissionDict.ContainsKey(keyId))
            {
                SheetMissionDict[keyId] = new CosmicInfo()
                {
                    Name = missionName,
                    Jobs = jobs,
                    ToDoId = toDoValue,
                    Rank = rank,
                    Attributes = attributes,
                    Weather = weather,
                    StartTime = startTime,
                    EndTime = endTime,
                    CosmoCredit = Cosmo,
                    LunarCredit = Lunar,
                    PreviousMissions = previousMissionId,
                    RelicXpInfo = relicXp,
                    BronzeScore = bronze,
                    SilverScore = silver,
                    GoldScore = gold,

                    RewardItem = rewardItemId,
                    RewardItemAmount = rewardItemAmount,

                    MapPosition = new Vector2(_x, _y),
                    Radius = radius,
                    TerritoryId = territoryId,
                    MarkerId = marker.RowId,

                    Gathering_Min = gathering_Min,

                    Crafts_Main = crafts_Main,
                    Crafts_Pre = crafts_Pre,
                };
            }
        }

        foreach (var Icon in LeveAssignmentSheet)
        {
            var iconId = Icon.RowId;

            if (iconId is 2 or 3 or 4)
            {
                iconId += 14;
            }
            else if (iconId > 4 && iconId < 13)
            {
                iconId += 3;
            }
            else
                continue;

            if (Icon.Name != "" && Icon.Icon is { } jobicon)
            {
                if (Svc.Texture.TryGetFromGameIcon(jobicon, out var texture))
                {
                    JobIconDict.TryAdd(iconId, texture);
                }
            }
        }

        for (int i = 0; i < GreyIconList.Count; i++)
        {
            var slot = i + 8;
            var iconId = GreyIconList[i];

            if (Svc.Texture.TryGetFromGameIcon(iconId, out var texture))
            {
                GreyTexture.TryAdd((uint)slot, texture);
            }
        }

        CosmicHelper.LoadMissionScores();

        foreach (var entry in SheetMissionDict)
        {
            var missionId = entry.Key;

            if (MissionScoreDict.TryGetValue(missionId, out var score))
            {
                entry.Value.ClassScore = score;
            }
            else
            {
                entry.Value.ClassScore = 0;
            }
        }

        foreach (var item in MoonItemInfoSheet)
        {
            var itemId = item.Item.RowId;
            if (itemId == 0) continue;
            string itemName = ItemSheet.GetRow(itemId).Name.ToString();
            var type = item.WKSItemSubCategory.RowId;
            IceLogging.Debug($"RowID: {item.RowId} | ID: {itemId} | Name: {itemName}", debugOnly: true);

            if (CosmicHelper.GatheringItems.TryGetValue(itemName, out var itemEntry))
            {
                itemEntry.itemIds.Add(itemId);
            }
            else
            {
                IceLogging.Debug($"Adding a new entry: {itemName}", debugOnly: true);

                CosmicHelper.GatheringItems[itemName] = new()
                {
                    Type = item.WKSItemSubCategory.RowId,
                    itemIds = new HashSet<uint> { itemId },
                };
            }
        }

        foreach (var weather in WeatherIds)
        {
            if (Svc.Texture.TryGetFromGameIcon(weather.Value, out var texture))
            {
                WeatherIconDict[weather.Key] = texture;
            }
        }

        foreach (var supply in Svc.Data.GetExcelSheet<WKSItemInfo>())
        {
            if (supply.Item.RowId == 0) continue;
            if (supply.WKSItemSubCategory.RowId != 2 && supply.WKSItemSubCategory.RowId != 5) continue;

            var itemId = supply.Item.RowId;
            var kind = supply.WKSItemSubCategory.RowId;
            var name = Svc.Data.GetExcelSheet<Item>().GetRow(itemId).Name.ToString();
            IceLogging.Info($"Name: {name} | ItemID: {itemId} | kind: {kind}");

            // Seafood/fish
            if (kind == 2)
            {
                if (GatheringUtil.MoonFish.TryGetValue(name, out var fishList))
                {
                    if (!fishList.Contains(itemId))
                    {
                        fishList.Add(itemId);
                    }
                }
                else
                {
                    GatheringUtil.MoonFish[name] = new() { itemId };
                }
            }
            // Baits
            else if (kind == 5)
            {
                if (GatheringUtil.MoonBaits.TryGetValue(name, out var fishBait))
                {
                    if (!fishBait.Contains(itemId))
                    {
                        fishBait.Add(itemId);
                    }
                }
                else
                {
                    GatheringUtil.MoonBaits[name] = new() { itemId };
                }
            }
        }
    }
    private static string GetClassAcronym(uint jobId)
    {
        // Map your job IDs to the acronyms used in the CSV
        // You'll need to determine what these mappings are based on your game data
        return jobId switch
        {
            8 => "CRP",  // Carpenter
            9 => "BSM",  // Blacksmith
            10 => "ARM", // Armorer
            11 => "GSM", // Goldsmith
            12 => "LTW", // Leatherworker
            13 => "WVR", // Weaver
            14 => "ALC", // Alchemist
            15 => "CUL", // Culinarian
            16 => "MIN", // Miner
            17 => "BTN", // Botanist
            18 => "FSH", // Fisher
            _ => ""
        };
    }
}
