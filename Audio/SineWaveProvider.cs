using System;


namespace SortingVisualizer.Audio
{
    public class SineWaveProvider : ISampleProvider
    {
        public int Frequency 
        {
            get 
            {
                return _frequency;
            }
            set 
            {
                _frequency = value;
                _seekPhase = true;
            }
        }
        public float Volume { get; set; }

        public int SampleRate { get; } = 44100;

        private int _frequency;
        private float _phase;

        private float _previousFrequency;
        private float _previousPhase;

        private bool _seekPhase;

        private uint _offset = 0;


        public SineWaveProvider() 
        {
            Frequency = 1000;
            Volume = 0.1f;
        }

        public void Read(short[] buffer)
        {
            float dt = (float)(2 * Math.PI / SampleRate);

            float volume = Volume;
            int frequency = Frequency;

            if (_seekPhase)
            {
                // Calculate new phase so that sine wave doesn't interrupt
                _phase = dt * _previousFrequency * _offset + _previousPhase - dt * frequency * _offset;
                
                _previousFrequency = _frequency;
                _previousPhase = _phase;
                _seekPhase= false;
            }

            for (int i = 0; i < buffer.Length; i++) 
            {
                buffer[i] = (short)(volume * short.MaxValue * Math.Sin(dt * frequency * _offset + _phase));

                _offset++;
            }
        }
    }
}
