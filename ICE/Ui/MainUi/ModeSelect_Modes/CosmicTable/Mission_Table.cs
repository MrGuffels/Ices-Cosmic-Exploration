using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ICE.Utilities.Cosmic_Helper;
using OtterGui;
using OtterGui.Table;
using System.Collections.Generic;
using System.Reflection;
using static ICE.Utilities.Cosmic_Helper.CosmicHelper;

namespace ICE.Ui.MainUi.ModeSelect_Modes.CosmicTable
{
    internal class VerticalCenterColumnString : ColumnString<MissionInfo>
    {
        public override void PreDraw()
        {
            var pos = ImGui.GetCursorPosY();
            var offset = (ImageSize - ImGui.GetTextLineHeight()) / 2;
            ImGui.SetCursorPosY(pos + offset);
        }
    }

    internal class ItemFilterColumn : ColumnFlags<ItemFilter, MissionInfo>
    {
        private ItemFilter[] FlagValues = Array.Empty<ItemFilter>();
        private string[] FlagNames = Array.Empty<string>();

        private string ReturnFlagNames(ItemFilter item)
        {
            return item switch
            {
                ItemFilter.NoItems => "No Items",
                ItemFilter.Enabled => "Enabled",
                ItemFilter.Disabled => "Disabled",
                // ItemFilter.NotCompleted => "Not Completed",
                // ItemFilter.Completed => "Completed",
                // ItemFilter.Gold => "Gold",
                _ => "Unknown",
            };
        }

        protected void SetFlags(params ItemFilter[] flags)
        {
            FlagValues = flags;
            AllFlags = FlagValues.Aggregate((f, g) => f | g);
        }

        protected void SetFlagsAndNames(params ItemFilter[] flags)
        {
            SetFlags(flags);
            SetNames(flags.Select(f => ReturnFlagNames(f)).ToArray());
        }

        protected void SetNames(params string[] names) => FlagNames = names;

        protected sealed override IReadOnlyList<ItemFilter> Values => FlagValues;

        protected sealed override string[] Names => FlagNames;

        public sealed override ItemFilter FilterValue => C.ItemFilter;

        protected sealed override void SetValue(ItemFilter f, bool v)
        {
            var tmp = v ? FilterValue | f : FilterValue & ~f;
            if (tmp == FilterValue)
                return;

            C.ItemFilter = tmp;
            C.SaveDebounced();
        }
    }
    internal class MissionFilterColumn : ColumnFlags<MissionFilter, MissionInfo>
    {
        private MissionFilter[] FlagValues = Array.Empty<MissionFilter>();
        private string[] FlagNames = Array.Empty<string>();

        protected void SetFlags(params MissionFilter[] flags)
        {
            FlagValues = flags;
            AllFlags = FlagValues.Aggregate((f, g) => f | g);
        }

        protected void SetFlagsAndNames(params MissionFilter[] flags)
        {
            SetFlags(flags);
            SetNames(flags.Select(f => f.ToString()).ToArray());
        }

        protected void SetNames(params string[] names) => FlagNames = names;

        protected sealed override IReadOnlyList<MissionFilter> Values => FlagValues;

        protected sealed override string[] Names => FlagNames;

        public sealed override MissionFilter FilterValue => C.MissionFilter;

        protected sealed override void SetValue(MissionFilter f, bool v)
        {
            var tmp = v ? FilterValue | f : FilterValue & ~f;
            if (tmp == FilterValue)
                return;

            C.MissionFilter = tmp;
            C.SaveDebounced();
        }
    }
    internal class Mission_Table : Table<MissionInfo>, IDisposable
    {
        public readonly EnabledColumn _enabledColumn = new() { Label = "Enabled" };
        public readonly NameColumn _nameColumn = new() { Label = "Name" };
        public readonly IdColumn _idColumn = new() { Label = "ID" };
        public readonly MissionColumn _missionColumn = new() { Label = "Rank" };
        public readonly CompletionColumn _completionColumn = new() { Label = "Completed" };
        public readonly ClassScoreColumn _classScoreColumn = new() { Label = "Class Score" };
        public readonly CosmocreditColumn _cosmoColumn = new() { Label = "Cosmo" };
        public readonly LunarCreditColumn _lunarColumn = new() { Label = "Lunar" };
        public readonly DroneCreditColumn _droneColumn = new() { Label = "Dronebits" };
        public readonly PlanetTokensColumn _planetTokenColumn = new() { Label = "Planet Tokens" };
        public readonly SPMColumn _spmColumn = new() { Label = "SPM" };
        public readonly TurninColumn _turninColumn = new() { Label = "Turnin Goal" };
        public readonly PlanetColumn _planetColumn = new() { Label = "Moons" };

        public Mission_Table(List<MissionInfo> itemList) : base("Item_Table", itemList)
        {
            List<Column<MissionInfo>> headers = [_enabledColumn, _completionColumn, _idColumn, _planetColumn, _missionColumn, _nameColumn, _classScoreColumn, _cosmoColumn, _lunarColumn, _droneColumn, _planetTokenColumn, _spmColumn, _turninColumn];

            var tierFlags = new (int tier, ItemFilter flag)[]
            {
                (1, ItemFilter.HasI),   (2, ItemFilter.HasII),  (3, ItemFilter.HasIII),
                (4, ItemFilter.HasIV),  (5, ItemFilter.HasV),   (6, ItemFilter.HasVI),
            };

            foreach (var (tier, flag) in tierFlags)
            {
                string tierName = tier switch { 1 => "I", 2 => "II", 3 => "III", 4 => "IV", 5 => "V", 6 => "VI", _ => "?" };
                headers.Add(new RelicExpColumn(tier, flag) { Label = $"Exp {tierName}" });
            }
            this.Headers = [.. headers];

            Sortable = true;
            Flags |= ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
        }

        public void Dispose()
        {

        }

        public sealed class EnabledColumn : ItemFilterColumn
        {
            public EnabledColumn()
            {
                Flags = ImGuiTableColumnFlags.NoHide;
                SetFlags(ItemFilter.Enabled, ItemFilter.Disabled);
                SetNames("Enabled", "Disabled");
            }

            public override int Compare(MissionInfo lhs, MissionInfo rhs)
                => lhs.Enabled.CompareTo(rhs.Enabled);

            public override void DrawColumn(MissionInfo item, int _)
            {
                bool disabled = C.SelectedMode == ModeSelect.MissionGoldMode
                             || C.SelectedMode == ModeSelect.LevelMode
                             || (C.SelectedMode == ModeSelect.RelicMode && !C.XPRelicOnlyEnabled);

                if (!disabled)
                {
                    var enabled = item.Enabled;
                    PreDraw();
                    if (ImGui.Checkbox("###Enabled", ref enabled))
                    {
                        C.MissionConfig[item.Id].Enabled = enabled;
                        C.SaveDebounced();
                    }
                }
            }

            public override bool FilterFunc(MissionInfo item)
            {
                return item.Enabled ? FilterValue.HasFlag(ItemFilter.Enabled) : FilterValue.HasFlag(ItemFilter.Disabled);
            }
        }
        public sealed class NameColumn : VerticalCenterColumnString
        {
            public NameColumn() => Flags |= ImGuiTableColumnFlags.NoHide;
            public override string ToName(MissionInfo mission) => mission.SheetInfo.Name;
            public override void DrawColumn(MissionInfo mission, int _)
            {
                foreach (var job in mission.SheetInfo.Jobs)
                {
                    var icon = CosmicHelper.JobIconDict[job];
                    ImGui.Image(icon.GetWrapOrEmpty().Handle, new(24, 24));
                    ImGui.SameLine();
                }
                PreDraw();
                if (ImGui.Button(mission.SheetInfo.Name))
                {

                }
            }
        }
        public sealed class IdColumn : VerticalCenterColumnString
        {
            public override string ToName(MissionInfo item) => item.Id.ToString();
            public override int Compare(MissionInfo lhs, MissionInfo rhs) => lhs.Id.CompareTo(rhs.Id);

            public override void DrawColumn(MissionInfo item, int _)
            {
                ImGuiUtil.Center($"{item.Id}");
            }
        }
        public sealed class CompletionColumn : ItemFilterColumn
        {
            public CompletionColumn()
            {
                Flags = ImGuiTableColumnFlags.None;
                SetFlags(ItemFilter.NotCompleted, ItemFilter.Completed, ItemFilter.Gold);
                SetNames("Not Completed", "Completed", "Gold");
            }
            public override int Compare(MissionInfo lhs, MissionInfo rhs) => lhs.SheetInfo.CompletionStatus.CompareTo(rhs.SheetInfo.CompletionStatus);
            public override void DrawColumn(MissionInfo item, int idx)
            {
                var status = item.SheetInfo.CompletionStatus;
                var frameHeight = ImGui.GetFrameHeight();
                var size = new Vector2(frameHeight);

                var columnWidth = ImGui.GetColumnWidth();
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + (columnWidth - frameHeight) / 2);

                if (status is Status.Gold)
                {
                    if (Svc.Texture.GetFromGame("ui/uld/WKSMission_hr1.tex") is { } tex && tex.TryGetWrap(out var wrap, out _))
                    {
                        ImGui.Image(wrap.Handle, size, new Vector2(0.2347f, 0.3500f), new Vector2(0.2959f, 0.6500f));
                    }
                }
                else
                {
                    var icon = status is Status.None ? FontAwesomeIcon.Times : FontAwesomeIcon.Check;
                    var color = status is Status.None ? EColor.Red : EColor.Green;

                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + ImGui.GetStyle().FramePadding.X);

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    using (ImRaii.PushColor(ImGuiCol.Text, color))
                    {
                        ImGuiEx.Icon(icon);
                    }
                }
            }
            public override bool FilterFunc(MissionInfo item)
            {
                var status = item.SheetInfo.CompletionStatus;

                if (FilterValue.HasFlag(ItemFilter.NotCompleted) && status == Status.None) return true;
                if (FilterValue.HasFlag(ItemFilter.Completed) && status == Status.Completed) return true;
                if (FilterValue.HasFlag(ItemFilter.Gold) && status == Status.Gold) return true;

                return false;
            }
        }
        public sealed class ClassScoreColumn : VerticalCenterColumnString
        {
            public override string ToName(MissionInfo mission) => mission.SheetInfo.ClassScore.ToString();
            public override int Compare(MissionInfo lhs, MissionInfo rhs) => lhs.SheetInfo.ClassScore.CompareTo(rhs.SheetInfo.ClassScore);
            public override void DrawColumn(MissionInfo mission, int _)
            {
                ImGuiUtil.Center($"{mission.SheetInfo.ClassScore}");
            }
        }
        public sealed class CosmocreditColumn : VerticalCenterColumnString
        {
            public override string ToName(MissionInfo mission) => mission.SheetInfo.CosmoCredit.ToString();
            public override int Compare(MissionInfo lhs, MissionInfo rhs) => lhs.SheetInfo.CosmoCredit.CompareTo(rhs.SheetInfo.CosmoCredit);
            public override void DrawColumn(MissionInfo mission, int _)
            {
                ImGuiUtil.Center($"{mission.SheetInfo.CosmoCredit}");
            }
        }
        public sealed class LunarCreditColumn : VerticalCenterColumnString
        {
            public override string ToName(MissionInfo mission) => mission.SheetInfo.LunarCredit.ToString();
            public override int Compare(MissionInfo lhs, MissionInfo rhs) => lhs.SheetInfo.LunarCredit.CompareTo(rhs.SheetInfo.LunarCredit);
            public override void DrawColumn(MissionInfo mission, int _)
            {
                ImGuiUtil.Center($"{mission.SheetInfo.LunarCredit}");
            }
        }
        public sealed class DroneCreditColumn : VerticalCenterColumnString
        {
            public override string ToName(MissionInfo mission) => mission.SheetInfo.DronebitReward.ToString();
            public override int Compare(MissionInfo lhs, MissionInfo rhs) => lhs.SheetInfo.DronebitReward.CompareTo(rhs.SheetInfo.DronebitReward);
            public override void DrawColumn(MissionInfo mission, int _)
            {
                ImGuiUtil.Center($"{mission.SheetInfo.DronebitReward}");
            }
        }
        public sealed class PlanetTokensColumn : ItemFilterColumn
        {
            public PlanetTokensColumn()
            {
                Flags = ImGuiTableColumnFlags.None;
                SetFlags(ItemFilter.HasTokens, ItemFilter.NoTokens);
                SetNames("Has Tokens", "No Tokens");
            }
            public override int Compare(MissionInfo lhs, MissionInfo rhs) => lhs.SheetInfo.TokenItemAmount.CompareTo(rhs.SheetInfo.TokenItemAmount);
            public override void DrawColumn(MissionInfo mission, int _)
            {
                ImGuiUtil.Center($"{mission.SheetInfo.TokenItemAmount}");
            }

            public override bool FilterFunc(MissionInfo mission)
            {
                return mission.SheetInfo.TokenItemAmount > 0 ? FilterValue.HasFlag(ItemFilter.HasTokens) : FilterValue.HasFlag(ItemFilter.NoTokens);
            }
        }
        public sealed class RelicExpColumn : ItemFilterColumn
        {
            private readonly int _tier;
            private readonly ItemFilter _flag;

            public RelicExpColumn(int tier, ItemFilter flag)
            {
                Flags = ImGuiTableColumnFlags.None;
                _tier = tier;
                _flag = flag;
                SetFlags(ItemFilter.HasI, ItemFilter.HasII, ItemFilter.HasIII, ItemFilter.HasIV, ItemFilter.HasV, ItemFilter.HasVI);
                SetNames("I", "II", "III", "IV", "V", "VI");
            }

            public override int Compare(MissionInfo lhs, MissionInfo rhs)
                => lhs.SheetInfo.RelicXpInfo.GetValueOrDefault(_tier)
                   .CompareTo(rhs.SheetInfo.RelicXpInfo.GetValueOrDefault(_tier));

            public override void DrawColumn(MissionInfo mission, int _)
            {
                var value = mission.SheetInfo.RelicXpInfo.GetValueOrDefault(_tier);

                if (value == 0)
                {
                    ImGuiUtil.Center("-");
                    return;
                }

                Vector4 pillColor = _tier switch
                {
                    1 => new Vector4(0.3f, 0.5f, 0.8f, 0.8f),
                    2 => new Vector4(0.3f, 0.7f, 0.5f, 0.8f),
                    3 => new Vector4(0.7f, 0.6f, 0.2f, 0.8f),
                    4 => new Vector4(0.7f, 0.3f, 0.6f, 0.8f),
                    5 => new Vector4(0.8f, 0.4f, 0.2f, 0.8f),
                    6 => new Vector4(0.8f, 0.2f, 0.2f, 0.8f),
                    7 => new Vector4(0.6f, 0.8f, 0.9f, 0.8f),
                    _ => new Vector4(0.5f, 0.5f, 0.5f, 0.8f),
                };

                var buttonWidth = ImGui.CalcTextSize($"{value}").X + ImGui.GetStyle().FramePadding.X * 2;
                var columnWidth = ImGui.GetColumnWidth();
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + (columnWidth - buttonWidth) / 2);

                using (ImRaii.PushColor(ImGuiCol.Button, pillColor)
                             .Push(ImGuiCol.ButtonHovered, pillColor with { W = 1.0f })
                             .Push(ImGuiCol.ButtonActive, pillColor))
                {
                    ImGui.SmallButton($"{value}##exp{_tier}");
                }
            }

            public override bool FilterFunc(MissionInfo mission)
            {
                var hasExp = mission.SheetInfo.RelicXpInfo.GetValueOrDefault(_tier) > 0;
                return hasExp ? FilterValue.HasFlag(_flag) : true;
            }
        }
        public sealed class MissionColumn : MissionFilterColumn
        {
            public MissionColumn()
            {
                Flags = ImGuiTableColumnFlags.None;
                SetFlags(MissionFilter.RedAlert, MissionFilter.Sequence, MissionFilter.Weather, MissionFilter.Timed, MissionFilter.ARank, MissionFilter.BRank, MissionFilter.CRank, MissionFilter.DRank);
                SetNames("Red Alert", "Sequence", "Weather", "Timed", "A Rank", "B Rank", "C Rank", "D Rank");
            }

            private static int GetMissionPriority(CosmicInfo info)
            {
                if (info.IsCritical) return 10;
                if (info.IsSequence) return 9;
                if (info.IsWeather) return 8;
                if (info.IsTimed) return 7;
                // Rank 6 = Provisional (handled above), 5 = Ex, 4 = A, 3 = B, 2 = C, 1 = D
                return (int)info.Rank;
            }

            public override int Compare(MissionInfo lhs, MissionInfo rhs) =>
                GetMissionPriority(lhs.SheetInfo).CompareTo(GetMissionPriority(rhs.SheetInfo));
            public override void DrawColumn(MissionInfo item, int idx)
            {
                var status = item.SheetInfo.CompletionStatus;
                var frameHeight = ImGui.GetFrameHeight();
                var size = new Vector2(frameHeight);

                var columnWidth = ImGui.GetColumnWidth();
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + (columnWidth - frameHeight) / 2);

                if (item.SheetInfo.IsCritical || item.SheetInfo.IsWeather)
                {
                    var texture = item.SheetInfo.IsCritical ?
                        Svc.Texture.GetFromManifestResource(Assembly.GetExecutingAssembly(), "ICE.Resources.Red_Alert.png").GetWrapOrEmpty() 
                      : CosmicHelper.WeatherIconDict[item.SheetInfo.Weather].GetWrapOrEmpty();

                    ImGui.Image(texture.Handle, size);
                }
                else
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + ImGui.GetStyle().FramePadding.X);
                    if (item.SheetInfo.IsProvisional)
                    {
                        var icon = item.SheetInfo.IsTimed ? FontAwesomeIcon.Clock : FontAwesomeIcon.ListOl;
                        ImGuiEx.Icon(icon);
                    }
                    else
                    {
                        string rank = item.SheetInfo.Rank switch
                        {
                            5 or 4 => "A",
                            3 => "B",
                            2 => "C",
                            1 => "D",
                            _ => "???"
                        };
                        ImGui.Text(rank);
                    }
                }
            }
            public override bool FilterFunc(MissionInfo item)
            {
                var sheetInfo = item.SheetInfo;

                if (FilterValue.HasFlag(MissionFilter.RedAlert) && sheetInfo.IsCritical) return true;
                if (FilterValue.HasFlag(MissionFilter.Sequence) && sheetInfo.IsSequence) return true;
                if (FilterValue.HasFlag(MissionFilter.Timed) && sheetInfo.IsTimed) return true;
                if (FilterValue.HasFlag(MissionFilter.Weather) && sheetInfo.IsWeather) return true;
                if (FilterValue.HasFlag(MissionFilter.ARank) && sheetInfo.ARank) return true;
                if (FilterValue.HasFlag(MissionFilter.BRank) && sheetInfo.BRank) return true;
                if (FilterValue.HasFlag(MissionFilter.CRank) && sheetInfo.CRank) return true;
                if (FilterValue.HasFlag(MissionFilter.DRank) && sheetInfo.Drank) return true;

                return false;
            }
        }
        public sealed class SPMColumn : VerticalCenterColumnString
        {
            private double GetScore(MissionInfo item)
            {
                var scoreInfo = item.SheetInfo.ScoreInfo();
                return item.SheetInfo.IsCritical ? scoreInfo[TurninState.Critical].Score : scoreInfo.Values.MaxBy(r => r.Score)?.Score ?? 0;
            }

            public override string ToName(MissionInfo item) => $"{GetScore(item):N2}";
            public override int Compare(MissionInfo x, MissionInfo y) => GetScore(x).CompareTo(GetScore(y));
            public override void DrawColumn(MissionInfo item, int _)
            {
                var scoreInfo = item.SheetInfo.ScoreInfo();
                var bestScore = scoreInfo.MaxBy(r => r.Value.Score);

                if (bestScore.Value.Score != 0)
                {
                    string scoreText = $"{bestScore.Value.Score:N2}";

                    var buttonWidth = ImGui.CalcTextSize(scoreText).X + ImGui.GetStyle().FramePadding.X * 2;
                    var columnWidth = ImGui.GetColumnWidth();
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + (columnWidth - buttonWidth) / 2);

                    Vector4 pillColor = bestScore.Key switch
                    {
                        TurninState.Bronze => new(0.6f, 0.35f, 0.15f, 1.0f), // darker bronze
                        TurninState.Silver => new(0.6f, 0.6f, 0.6f, 1.0f),
                        TurninState.Gold => new(0.85f, 0.70f, 0.0f, 1.0f), // slightly muted gold
                        TurninState.Critical => new(0.7f, 0.1f, 0.9f, 1.0f), // purple feels "special"
                        _ => new(0.5f, 0.5f, 0.5f, 0.8f)
                    };

                    bool isBright = bestScore.Key is TurninState.Gold or TurninState.Silver;
                    Vector4 textColor = isBright ? new(0.1f, 0.1f, 0.1f, 1.0f) : new(1.0f, 1.0f, 1.0f, 1.0f);

                    using (ImRaii.PushColor(ImGuiCol.Button, pillColor)
                        .Push(ImGuiCol.ButtonHovered, pillColor with { W = 1.0f })
                        .Push(ImGuiCol.ButtonActive, pillColor)
                        .Push(ImGuiCol.Text, textColor))
                    {
                        ImGui.SmallButton(scoreText);
                    }
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text($"[Average] Rewards per minute");
                        if (ImGui.BeginTable($"Score Info Table_{item.SheetInfo.MissionId}", 5, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders))
                        {
                            ImGui.TableSetupColumn("Kind");
                            ImGui.TableSetupColumn("Score");
                            ImGui.TableSetupColumn("Credits");
                            ImGui.TableSetupColumn("Planetary");
                            ImGui.TableSetupColumn("Tokens");

                            ImGui.TableHeadersRow();

                            foreach (var entry in scoreInfo.Where(x => x.Value.Score != 0))
                            {
                                ImGui.TableNextRow();
                                ImGui.TableSetColumnIndex(0);
                                ImGui.Text($"{entry.Key.ToString()}");

                                ImGui.TableNextColumn();
                                ImGui.Text($"{entry.Value.Score:N2}");

                                ImGui.TableNextColumn();
                                ImGui.Text($"{entry.Value.Cosmocredit:N2}");

                                ImGui.TableNextColumn();
                                ImGui.Text($"{entry.Value.PlanetCredits:N2}");

                                ImGui.TableNextColumn();
                                string tokens = entry.Value.Tokens > 0 ? $"{entry.Value.Tokens:N2}" : "-";
                                ImGui.Text(tokens);
                            }

                            ImGui.EndTable();
                        }
                        ImGui.EndTooltip();
                    }
                }
                else
                {
                    ImGuiUtil.Center("-");
                }

            }
        }
        public sealed class TurninColumn : ItemFilterColumn
        {
            public TurninColumn()
            {
                Flags = ImGuiTableColumnFlags.None;
                SetFlags(ItemFilter.TurninGold, ItemFilter.TurninSilver, ItemFilter.TurninBronze);
                SetNames("Gold", "Silver", "Bronze");
            }
            public override int Compare(MissionInfo lhs, MissionInfo rhs)
            {
                if (C.MissionConfig.TryGetValue(lhs.Id, out var lhsConfig) && C.MissionConfig.TryGetValue(rhs.Id, out var rhsConfig))
                {
                    return lhsConfig.TurninGoal.CompareTo(rhsConfig.TurninGoal);
                }
                else
                {
                    return 0;
                }
            }
            public override void DrawColumn(MissionInfo item, int idx)
            {
                if (item.SheetInfo.Attributes.HasFlag(MissionAttributes.ScoreTimeRemaining) || item.SheetInfo.IsCritical)
                    ImGuiUtil.Center("Auto");
                else
                {
                    Vector4 BronzeColor = new Vector4(0.804f, 0.498f, 0.196f, 1.0f);
                    Vector4 SilverColor = new Vector4(0.753f, 0.753f, 0.753f, 1.0f);
                    Vector4 GoldColor = new Vector4(1.0f, 0.843f, 0.0f, 1.0f);
                    Vector4 DisabledColor = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);

                    ImGui.PushID($"Mission_{item.Id}");

                    if (C.MissionConfig.TryGetValue(item.Id, out var configInfo))
                    {
                        var highestTurnin = configInfo.TurninGoal;
                        var goldEnabled = highestTurnin >= TurninState.Gold;
                        var silverEnabled = highestTurnin >= TurninState.Silver;
                        var bronzeEnabled = highestTurnin >= TurninState.Bronze;

                        using (ImRaii.PushColor(ImGuiCol.Text, goldEnabled ? GoldColor : DisabledColor))
                        {
                            if (ImGuiEx.IconButton(FontAwesomeIcon.Trophy, "##Gold"))
                            {
                                configInfo.TurninGoal = TurninState.Gold;
                                C.SaveDebounced();
                            }
                        }
                        ImGui.SameLine();
                        using (ImRaii.PushColor(ImGuiCol.Text, silverEnabled ? SilverColor : DisabledColor))
                        {
                            if (ImGuiEx.IconButton(FontAwesomeIcon.Trophy, "##Silver"))
                            {
                                configInfo.TurninGoal = TurninState.Silver;
                                C.SaveDebounced();
                            }

                        }
                        ImGui.SameLine();
                        using (ImRaii.PushColor(ImGuiCol.Text, bronzeEnabled ? BronzeColor : DisabledColor))
                        {
                            if (ImGuiEx.IconButton(FontAwesomeIcon.Trophy, "##Bronze"))
                            {
                                configInfo.TurninGoal = TurninState.Bronze;
                                C.SaveDebounced();
                            }
                        }

                    }

                    ImGui.PopID();
                }
            }
        }
        public sealed class PlanetColumn : ItemFilterColumn
        {
            public PlanetColumn()
            {
                Flags = ImGuiTableColumnFlags.None;
                SetFlags(ItemFilter.Sinus, ItemFilter.Phaenna, ItemFilter.Oizys);
                SetNames("Sinus", "Phaenna", "Oizys");
            }

            public override int Compare(MissionInfo lhs, MissionInfo rhs) => lhs.SheetInfo.TerritoryId.CompareTo(rhs.SheetInfo.TerritoryId);
            public override void DrawColumn(MissionInfo item, int idx)
            {
                var status = item.SheetInfo.CompletionStatus;
                var frameHeight = ImGui.GetFrameHeight();
                var size = new Vector2(frameHeight);

                var columnWidth = ImGui.GetColumnWidth();
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + (columnWidth - frameHeight) / 2);

                string planetIcon = item.SheetInfo.TerritoryId switch
                {
                    1237 => "ICE.Resources.Sinus_Ardorum.png",
                    1291 => "ICE.Resources.Phaenna.png",
                    1310 => "ICE.Resources.Oizys.png",
                    _ => "ICE.Resources.Sinus_Ardorum.png",
                };

                var texture = Svc.Texture.GetFromManifestResource(Assembly.GetExecutingAssembly(), planetIcon).GetWrapOrEmpty();
                ImGui.Image(texture.Handle, size);
            }
            public override bool FilterFunc(MissionInfo item)
            {
                return item.SheetInfo.TerritoryId switch
                {
                    1237 => FilterValue.HasFlag(ItemFilter.Sinus),
                    1291 => FilterValue.HasFlag(ItemFilter.Phaenna),
                    1310 => FilterValue.HasFlag(ItemFilter.Oizys),
                    _ => false
                };
            }
        }
    }
}
