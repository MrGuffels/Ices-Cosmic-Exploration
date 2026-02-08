using System;
using System.Collections.Generic;
using System.Text;

namespace ICE.Enums
{
    [Flags]
    public enum ModeSelect
    {
        Standard = 0,
        RelicMode = 1,
        LevelMode = 2,
        ScoreMode = 3,

        AgendaMode = 10,
    }
}
