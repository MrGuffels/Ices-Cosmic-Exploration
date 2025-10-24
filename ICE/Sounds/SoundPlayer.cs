using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ICE.Sounds
{
    public static class SoundPlayer
    {
        public static async Task PlaySoundAsync()
        {
            try
            {
                string sound = "ICE.Sounds.Task_Completed.mp3";
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sound);

                if (stream == null)
                {
                    PluginLog.Warning($"Could not find embedded resource: {sound}");
                    return;
                }

                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
                memoryStream.Position = 0;

                using var reader = new Mp3FileReader(memoryStream);

                var sampleProvider = reader.ToSampleProvider();
                var volumeProvider = new VolumeSampleProvider(sampleProvider)
                {
                    Volume = C.SoundVolume // Apply volume at sample level (0.0 to 1.0)
                };

                var waveOut = new WaveOutEvent();
                waveOut.Volume = 1.0f;

                var tcs = new TaskCompletionSource<bool>();

                waveOut.PlaybackStopped += (sender, args) =>
                {
                    tcs.TrySetResult(true);
                    waveOut.Dispose(); // Dispose after playback stops
                };

                waveOut.Init(volumeProvider.ToWaveProvider16());
                waveOut.Play();

                await tcs.Task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                PluginLog.Error($"Failed to play sound: {ex}");
            }
        }
    }
}