using System.Collections.Generic;
using Dalamud.Interface.Utility.Raii;
using ICE.Utilities.Cosmic_Helper;
using OtterGui.Table;
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

    internal class Mission_Table : Table<MissionInfo>, IDisposable
    {
        public readonly EnabledColumn _enabledColumn = new() { Label = "Enabled" };
        public readonly NameColumn _nameColumn = new() { Label = "Name" };
        public readonly IdColumn _idColumn = new() { Label = "ID" };

        public Mission_Table(List<MissionInfo> itemList) : base("Item_Table", itemList)
        {
            List<Column<MissionInfo>> headers = [_enabledColumn, _idColumn, _nameColumn];
            this.Headers = [.. headers];

            Sortable = true;
            Flags |= ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Resizable;
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
                ImGui.Selectable(mission.SheetInfo.Name);
            }
        }

        public sealed class IdColumn : VerticalCenterColumnString
        {
            public override string ToName(MissionInfo item) => item.Id.ToString();
            public override int Compare(MissionInfo lhs, MissionInfo rhs) => lhs.Id.CompareTo(rhs.Id);

            public override void DrawColumn(MissionInfo item, int _)
            {
                ImGui.Selectable($"{item.Id}");
            }
        }
    }
}
