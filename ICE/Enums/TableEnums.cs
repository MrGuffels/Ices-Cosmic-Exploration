using System;
using System.Collections.Generic;
using System.Text;

namespace ICE.Enums
{
    [Flags]
    public enum ItemFilter
    {
        NoItems = 0,

        // Config States
        Enabled = 1 << 0,
        Disabled = 1 << 1,

        // Completion States
        NotCompleted = 1 << 2,
        Completed = 1 << 3,
        Gold = 1 << 4,

        // Tokens
        NoTokens = 1 << 5,
        HasTokens = 1 << 6,

        // Exp Kinds
        HasI = 1 << 7,
        HasII = 1 << 8,
        HasIII = 1 << 9,
        HasIV = 1 << 10,
        HasV = 1 << 11,
        HasVI = 1 << 12,
        // HasVII = 1 << 13

        // Planets
        Sinus = 1 << 14,
        Phaenna = 1 << 15,
        Oizys = 1 << 16,
        // Last Planet = 1 << 17

        // Turnin Level
        TurninGold = 1 << 18,
        TurninSilver = 1 << 19,
        TurninBronze = 1 << 20,


        All = Enabled + Disabled 
            + NotCompleted + Completed + Gold
            + NoTokens + HasTokens
            + HasI + HasII + HasIII + HasIV + HasVI // + HasVIi
            + Sinus + Phaenna + Oizys // + Planet#4
            + TurninGold + TurninSilver + TurninBronze
            
    }

    [Flags]
    public enum MissionFilter
    {
        None = 0,
        RedAlert = 1 << 0,
        Sequence = 1 << 1,
        Weather = 1 << 2,
        Timed = 1 << 3,
        ARank = 1 << 4,
        BRank = 1 << 5,
        CRank = 1 << 6,
        DRank = 1 << 7,

        All = RedAlert
            + Sequence + Weather + Timed
            + ARank + BRank + CRank + DRank
    }

    public enum JobFilter
    {
        None = 0,
        CRP = 1 << 0,
        BSM = 1 << 1,
        ARM = 1 << 2,
        GSM = 1 << 3,
        LTW = 1 << 4,
        WVR = 1 << 5,
        ALC = 1 << 6,
        CUL = 1 << 7,
        MIN = 1 << 8,
        BTN = 1 << 9,
        FSH = 1 << 10,

        All = CRP + BSM + ARM + GSM
            + LTW + WVR + ALC + CUL 
            + MIN + BTN + FSH
    }
}
