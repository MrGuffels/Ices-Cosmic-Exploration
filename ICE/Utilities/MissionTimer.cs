using System.Diagnostics;

public class MissionTimer
{
    private Stopwatch stopwatch;
    private uint currentMission;
    private bool isRunning;

    public int TimeHistoryLimit => C.TimeHistoryLimit;

    public MissionTimer()
    {
        stopwatch = new Stopwatch();
    }

    public void StartMission(uint missionId)
    {
        currentMission = missionId;
        stopwatch.Restart();
        isRunning = true;
    }

    public TimeSpan CompleteMission()
    {
        if (!isRunning)
        {
            return TimeSpan.Zero;
        }

        stopwatch.Stop();
        var duration = stopwatch.Elapsed;
        isRunning = false;

        UpdateMissionStats(currentMission, duration);

        return duration;
    }

    private void UpdateMissionStats(uint missionId, TimeSpan duration)
    {
        if (!C.MissionConfig.ContainsKey(missionId))
        {
            C.MissionConfig[missionId] = new();
        }

        var stats = C.MissionConfig[missionId];

        // Add the new time
        stats.Times.Add(duration.TotalSeconds);

        // Increment total completions (always tracks full history)
        stats.TotalCompletions++;

        // Apply time history limit if set
        if (TimeHistoryLimit > 0 && stats.Times.Count > TimeHistoryLimit)
        {
            // Remove oldest times to maintain the limit
            stats.Times.RemoveRange(0, stats.Times.Count - TimeHistoryLimit);
        }

        // Calculate stats based on the (possibly limited) time history
        stats.BestTime = stats.Times.Min();
        stats.AverageTime = stats.Times.Average();

        C.Save();
    }

    public void ResetTimers(uint missionId)
    {
        if (!C.MissionConfig.ContainsKey(missionId))
        {
            C.MissionConfig[missionId] = new();
        }

        var stats = C.MissionConfig[missionId];
        stats.Times.Clear();
        stats.BestTime = double.MaxValue;
        stats.AverageTime = 0;
        stats.TotalCompletions = 0;

        C.Save();
    }

    public static class MissionStatsCalculator
    {
        public static double CalculateScorePerHour(double averageTimeSeconds, uint baseScore, double multiplier)
        {
            if (averageTimeSeconds <= 0) return 0;

            double hoursPerCompletion = averageTimeSeconds / 3600.0;
            double completionsPerHour = 1.0 / hoursPerCompletion;
            double scorePerCompletion = baseScore * multiplier;

            return completionsPerHour * scorePerCompletion;
        }
    }

    public void AbandonMission()
    {
        stopwatch.Stop();
        stopwatch.Reset();
        isRunning = false;
        currentMission = 0;
    }
}