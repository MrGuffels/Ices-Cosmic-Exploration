namespace ICE.Enums
{
    [Flags]
    internal enum IceState
    {
        Idle = 0,
        Start = 1,
        GrabMission = 2,
        ExecutingMission = 3,
        AbandonMission = 4,
        ForceTurnin = 5,
        ScoreCheck = 6,
        ManualMode = 7,
        Craft = 8,
        Gather = 9,
        Fish = 10,
        ScoringMission = 11,
        AnimationLock = 12,
        Gambling = 13,
        RelicTurnin = 14,
        Waiting = 15,
    }
}