using System;
using System.Collections.Generic;
using System.Linq;
using SortingVisualizer.Sorting;


namespace SortingVisualizer.Audio
{
    public class SortStepPlayer : IDisposable
    {
        public SoundPlayerState State => _streamSoundPlayer.State;

        private const int MinFrequency = 120;
        private const int MaxFrequency = 1200;

        private StreamSoundPlayer _streamSoundPlayer;
        private SineWaveProvider _sampleProvider;


        public SortStepPlayer() 
        {
            _sampleProvider = new SineWaveProvider();
            _streamSoundPlayer = new StreamSoundPlayer(_sampleProvider);
        }

        public void Play(SortStep step)
        {
            if (TryMapToFrequency(step, out int frequency)) 
            {
                _sampleProvider.Frequency = frequency;
                _streamSoundPlayer.Play();
            }
        }

        public void Pause()
        {
            _streamSoundPlayer.Pause();
        }

        public void Dispose()
        {
            _streamSoundPlayer?.Dispose();
        }

        private static bool TryMapToFrequency(SortStep step, out int frequency) 
        {
            if (step is null) 
            {
                frequency = 0;
                return false;
            }

            IEnumerable<int> indices = step.AccessedIndices.Union(step.ChangedIndices);

            if (!indices.Any())
            {
                frequency = 0;
                return false;
            }

            int lastIndex = indices.Last();
            int value = step.Array[lastIndex];

            frequency = (int)(MinFrequency + (float)value / step.Array.Length * (MaxFrequency - MinFrequency));
            return true;
        }
    }
}
