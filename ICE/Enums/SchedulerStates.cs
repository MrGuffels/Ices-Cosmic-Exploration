namespace ICE.Enums
{
    [Flags]
    internal enum IceState
    {
        Idle = 0,
        Start = 1,
        GrabMission = 2,
        ExecutingMission = 3,
        ForceTurnin = 4,
        ScoreCheck = 5,
        ManualMode = 6,
        Craft = 7,
        Gather = 8,
        Fish = 9,
        ScoringMission = 10,
        AnimationLock = 11,
        Gambling = 12,
        RelicTurnin = 13,
        Waiting = 14,
    }
}