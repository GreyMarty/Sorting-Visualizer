using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using SortingVisualizer.Sorting;
using SortingVisualizer.InputTypes;
using SortingVisualizer.Audio; 


namespace SortingVisualizer.Visualization
{
    /// <summary>
    /// Provides methods for visualization control
    /// </summary>
    public class VisualizationController : IDisposable
    {
        public IVizualizer Visualizer { get; init; }
        public ISorter Sorter { get; private set; }

        public bool SoundEnabled 
        {
            get 
            {
                return _soundEnabled;
            }
            set 
            {
                _soundEnabled = value;

                if (!value) 
                {
                    _soundPlayer?.Pause();
                }
            }
        }

        public int Delay { get; set; }

        public int ArrayAccesses { get; private set; } = 0;
        public int Comparsions { get; private set; } = 0;
        public int ArrayWrites { get; private set; } = 0;

        public event EventHandler CountersChanged;
        
        private SortStepPlayer _soundPlayer;
        private bool _soundEnabled = false;

        private int[] _array;
        private IEnumerator<SortStep> _sortEnumerator;

        private bool _isRunning = false;
        private Thread _visualizationThread;
        private ManualResetEvent _visualizationStopEvent;

        private const int TargetArraySize = 512;

        public VisualizationController(IVizualizer vizualizer, ISorter sorter)
        {
            Visualizer = vizualizer;
            Sorter = sorter;

            _array = new int[0];

            _visualizationStopEvent = new ManualResetEvent(false);

            _soundPlayer = new SortStepPlayer();
        }

        /// <summary>
        /// Stops visualization and resets array
        /// </summary>
        public void Reset(int arraySize, ArrayInputType inputType) 
        {
            Pause();

            _array = new int[arraySize];
            inputType.Generate(_array);
            _sortEnumerator = null;

            ArrayAccesses = 0;
            Comparsions = 0;
            ArrayWrites = 0;
            CountersChanged?.Invoke(this, EventArgs.Empty);

            _soundPlayer.Pause();

            Redraw();
        }

        /// <summary>
        /// Stops visualization, resets array and sorting algorithm
        /// </summary>
        public void Reset(int arraySize, ArrayInputType inputType, ISorter sorter)
        {
            Reset(arraySize, inputType);

            Sorter = sorter;
        }

        /// <summary>
        /// Starts visualization in different thread
        /// </summary>
        public void Play() 
        {
            _sortEnumerator = _sortEnumerator ?? Sorter.Sort(_array);

            if (_isRunning) 
            {
                return;
            }

            _visualizationThread = new Thread(ThreadPlay);
            _visualizationStopEvent.Reset();
            _visualizationThread.Start();
        }

        /// <summary>
        /// Pauses visualization
        /// </summary>
        public void Pause() 
        {
            _visualizationStopEvent.Set();
            _soundPlayer.Pause();
        }

        /// <summary>
        /// Pauses visualization and proceeds one step forward
        /// </summary>
        public void Step()
        {
            if (_isRunning)
            {
                Pause();
            }

            _sortEnumerator = _sortEnumerator ?? Sorter.Sort(_array);

            if (_sortEnumerator.MoveNext())
            {
                SortStep sortStep = _sortEnumerator.Current;

                VisualizeStep(sortStep);

                ArrayAccesses += sortStep.ArrayAccesses;
                Comparsions += sortStep.Comparsions;
                ArrayWrites += sortStep.ArrayWrites;
                CountersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Redraws current visualization state
        /// </summary>
        public void Redraw() 
        {
            if (_sortEnumerator?.Current is not null) 
            {
                Visualizer.Visualize(_sortEnumerator.Current);
                return;
            }

            if (_array is not null)
            {
                Visualizer.Visualize(_array);
            }
        }

        private void VisualizeStep(SortStep sortStep, bool sound=false) 
        {
            if (sound) 
            {
                _soundPlayer.Play(sortStep);
            }
            
            Visualizer.Visualize(sortStep);
        }

        private void ThreadPlay() 
        {
            if (_sortEnumerator is null) 
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            _isRunning = true;

            while (_isRunning && (_sortEnumerator?.MoveNext() ?? false)) 
            {
                if (_visualizationStopEvent.WaitOne(Math.Max(0, Delay - (int)stopwatch.ElapsedMilliseconds))) 
                {
                    _isRunning = false;
                    return;
                }

                stopwatch.Reset();

                int stepCount = _array.Length < TargetArraySize ? 1 : _array.Length / TargetArraySize;

                SortStep sortStep = new SortStep(_array);

                for (int i = 0; i < stepCount; i++) 
                {
                    foreach (int index in _sortEnumerator.Current.AccessedIndices) 
                    {
                        sortStep.AccessedIndices.Add(index);
                    }

                    foreach (int index in _sortEnumerator.Current.ChangedIndices)
                    {
                        sortStep.ChangedIndices.Add(index);
                    }

                    ArrayAccesses += _sortEnumerator.Current.ArrayAccesses;
                    Comparsions += _sortEnumerator.Current.Comparsions;
                    ArrayWrites += _sortEnumerator.Current.ArrayWrites;

                    _sortEnumerator.MoveNext();
                }

                VisualizeStep(sortStep, SoundEnabled);

                CountersChanged?.Invoke(this, EventArgs.Empty);

                stopwatch.Stop();
            }

            PlayFinishAnimation();

            _soundPlayer.Pause();
            _isRunning = false;
        }

        private void PlayFinishAnimation() 
        {
            Stopwatch stopwatch = new Stopwatch();
            _isRunning = true;

            SortStep sortStep = new SortStep(_array);
            int arrayIndex = 0;

            while (_isRunning && arrayIndex < _array.Length)
            {
                if (_visualizationStopEvent.WaitOne(Math.Max(0, Delay - (int)stopwatch.ElapsedMilliseconds)))
                {
                    break;
                }

                stopwatch.Reset();

                int stepCount = _array.Length < TargetArraySize ? 1 : _array.Length / TargetArraySize;

                foreach (int index in sortStep.ChangedIndices) 
                {
                    sortStep.AccessedIndices.Add(index);
                }

                sortStep.ChangedIndices.Clear();

                for (int i = 0; i < stepCount && arrayIndex + i < _array.Length; i++)
                {
                    sortStep.ChangedIndices.Add(arrayIndex + i);
                }

                arrayIndex += stepCount;

                VisualizeStep(sortStep, SoundEnabled);

                stopwatch.Stop();
            }

            _soundPlayer.Pause();
            _isRunning = false;
        }

        public void Dispose()
        {
            Pause();
            Visualizer.Dispose();
            _soundPlayer.Dispose();
        }
    }
}
