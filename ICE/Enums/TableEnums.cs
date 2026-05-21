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
        // NotCompleted = 1 << 2,
        // Completed = 1 << 3,
        // Gold = 1 << 4,

        All = Enabled + Disabled 
         // + NotCompleted + Completed + Gold
            
    }
}
