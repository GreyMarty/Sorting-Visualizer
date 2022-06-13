using System;
using System.Threading;
using OpenTK.Audio.OpenAL;


namespace SortingVisualizer.Audio
{
    public class StreamSoundPlayer : ISoundPlayer
    {
        public ISampleProvider SampleProvider { get; init; }
        
        public int UpdateInterval { get; set; }

        public SoundPlayerState State { get; private set; }

        private ALDevice _device;
        private ALContext _context;

        private int _source;

        private ManualResetEvent _pauseEvent;


        public StreamSoundPlayer(ISampleProvider sampleProvider)
        {
            SampleProvider = sampleProvider;
            UpdateInterval = 20;

            State = SoundPlayerState.Initial;

            _pauseEvent = new ManualResetEvent(false);
        }

        public unsafe void Play()
        {
            switch (State) 
            {
                case SoundPlayerState.Initial:
                    _device = ALC.OpenDevice(null);
                    _context = ALC.CreateContext(_device, (int*)null);
                    ALC.MakeContextCurrent(_context);

                    _source = AL.GenSource();
                    break;
                case SoundPlayerState.Playing:
                    return;
                case SoundPlayerState.Disposed:
                    throw new ObjectDisposedException(GetType().FullName);
            }

            new Thread(ThreadPlay).Start();
            State = SoundPlayerState.Playing;
        }

        public void Pause()
        {
            switch (State)
            {
                case SoundPlayerState.Playing:
                    _pauseEvent.Set();
                    State = SoundPlayerState.Paused;
                    break;
                case SoundPlayerState.Disposed:
                    throw new ObjectDisposedException(GetType().FullName);
                default:
                    return;
            }
        }

        public void Dispose()
        {
            if (State == SoundPlayerState.Initial) 
            {
                State = SoundPlayerState.Disposed;
                return;
            }

            _pauseEvent.Set();

            AL.SourceStop(_source);
            
            AL.GetSource(_source, ALGetSourcei.BuffersQueued, out int buffersCount);

            int[] buffers = AL.SourceUnqueueBuffers(_source, buffersCount);

            AL.DeleteSource(_source);
            AL.DeleteBuffers(buffers);

            ALC.MakeContextCurrent(ALContext.Null);
            ALC.DestroyContext(_context);

            ALC.CloseDevice(_device);

            State = SoundPlayerState.Disposed;
        }

        private void ThreadPlay() 
        {
            while (true) 
            {
                int buffer;

                AL.GetSource(_source, ALGetSourcei.BuffersQueued, out int buffersQueued);
                AL.GetSource(_source, ALGetSourcei.BuffersProcessed, out int buffersProcessed);

                int buffersRemaining = buffersQueued - buffersProcessed;

                if (buffersRemaining < 2)
                {
                    if (buffersProcessed > 0)
                    {
                        int[] buffers = new int[buffersProcessed];
                        AL.SourceUnqueueBuffers(_source, buffersProcessed, buffers);

                        AL.DeleteBuffers(buffersProcessed - 1, buffers);

                        buffer = buffers[buffersProcessed - 1];
                    }
                    else 
                    {
                        buffer = AL.GenBuffer();
                    }

                
                    int dataSize = UpdateInterval * SampleProvider.SampleRate / 1000;
                    short[] data = new short[dataSize];
                    SampleProvider.Read(data);

                    AL.BufferData(buffer, ALFormat.Mono16, data, SampleProvider.SampleRate);
                    AL.SourceQueueBuffer(_source, buffer);
                }

                ALSourceState sourceState = AL.GetSourceState(_source);
                if (sourceState != ALSourceState.Playing) 
                {
                    AL.SourcePlay(_source);
                }

                if (_pauseEvent.WaitOne(UpdateInterval / 2)) 
                {
                    AL.SourceStop(_source);

                    _pauseEvent.Reset();
                    break;
                }
            }
        }
    }
}
