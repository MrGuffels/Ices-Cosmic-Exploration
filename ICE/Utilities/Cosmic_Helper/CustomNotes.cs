using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Utilities;

public static partial class CosmicHelper
{
    public class CustomNotes
    {
        public uint Icon { get; set; }
        public string NoteInfo { get; set; }
        public float SPM { get; set; }
    };

    private static string BasicNotes = "Best Score Per Minute outside of weather missions";

    public static Dictionary<uint, CustomNotes> CustomMissionNotes = new()
    {
        [295] = new()
        {
            NoteInfo = "Best Score Per Minute outside of weather missions",
            SPM = 323.68f,
        },
        [115] = new()
        {
            NoteInfo = "Best Score Per Minute outside of weather missions",
            SPM = 323.68f,
        },
        [70] = new()
        {
            NoteInfo = "Best Score Per Minute outside of weather missions",
            SPM = 323.68f,
        },
        [205] = new()
        {
            NoteInfo = "Best Score Per Minute outside of weather missions",
            SPM = 323.68f,
        },
        [25] = new()
        {
            NoteInfo = "Best Score Per Minute outside of weather missions",
            SPM = 323.68f,
        },
        [340] = new()
        {
            NoteInfo = "Best Score Per Minute outside of weather missions",
            SPM = 323.68f,
        },
        [160] = new()
        {
            NoteInfo = "Best Score Per Minute outside of weather missions",
            SPM = 323.68f,
        },
        [250] = new()
        {
            NoteInfo = "Best Score Per Minute outside of weather missions",
            SPM = 323.68f,
        }
    };
}
