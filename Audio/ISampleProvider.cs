namespace SortingVisualizer.Audio
{
    public interface ISampleProvider
    {
        public int SampleRate { get; }

        public void Read(short[] buffer);
    }
}
