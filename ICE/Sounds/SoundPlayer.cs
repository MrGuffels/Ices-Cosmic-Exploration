using NAudio.Wave;
using NAudio.Wave.SampleProviders;
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
                    Console.WriteLine($"Could not find embedded resource: {sound}");
                    return;
                }

                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using var reader = new Mp3FileReader(memoryStream);

                var sampleProvider = reader.ToSampleProvider();
                var volumeProvider = new VolumeSampleProvider(sampleProvider)
                {
                    Volume = C.SoundVolume // Apply volume at sample level (0.0 to 1.0)
                };

                using var waveOut = new WaveOutEvent();
                // Keep waveOut.Volume at 1.0 (full) so it doesn't affect system volume
                waveOut.Volume = 1.0f;

                var tcs = new TaskCompletionSource<bool>();
                waveOut.PlaybackStopped += (sender, args) => tcs.SetResult(true);

                // Convert back to WaveProvider for WaveOut
                waveOut.Init(volumeProvider.ToWaveProvider16());
                waveOut.Play();

                await tcs.Task;
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }
    }
}