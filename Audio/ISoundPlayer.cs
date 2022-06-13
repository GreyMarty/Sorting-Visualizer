using System;


namespace SortingVisualizer.Audio
{
    public interface ISoundPlayer : IDisposable
    {
        public SoundPlayerState State { get; }

        public void Play();
        public void Pause();
    }
}
