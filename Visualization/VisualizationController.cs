using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using SortingVisualizer.Sorting;
using SortingVisualizer.InputTypes;

namespace SortingVisualizer.Visualization
{
    /// <summary>
    /// Provides methods for visualization control
    /// </summary>
    public class VisualizationController
    {
        public IVizualizer Visualizer { get; init; }
        public ISorter Sorter { get; private set; }

        public int Delay { get; set; }

        private int[] _array;
        private IEnumerator<SortStep>? _sortEnumerator;

        private bool _isRunning = false;
        private Thread _visualizationThread;
        private ManualResetEvent _visualizationStopEvent;

        public VisualizationController(IVizualizer vizualizer, ISorter sorter)
        {
            Visualizer = vizualizer;
            Sorter = sorter;

            _visualizationStopEvent = new ManualResetEvent(false);
        }

        /// <summary>
        /// Stops visualization and resets array
        /// </summary>
        public void Reset(int arraySize, InputType inputType) 
        {
            Pause();

            _array = new int[arraySize];
            inputType.Generate(_array);

            _sortEnumerator = null;

            Redraw();
        }

        /// <summary>
        /// Stops visualization, resets array and sorting algorithm
        /// </summary>
        public void Reset(int arraySize, InputType inputType, ISorter sorter)
        {
            Reset(arraySize, inputType);

            Sorter = sorter;
        }

        /// <summary>
        /// Starts visualization in different thread
        /// </summary>
        public void Run() 
        {
            _sortEnumerator = _sortEnumerator ?? Sorter.Sort(_array);

            if (_isRunning) 
            {
                return;
            }

            _visualizationThread = new Thread(ThreadRun);
            _visualizationStopEvent.Reset();
            _visualizationThread.Start();
        }

        /// <summary>
        /// Pauses visualization
        /// </summary>
        public void Pause() 
        {
            _visualizationStopEvent.Set();
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
                Visualizer.Visualize(_sortEnumerator.Current);
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

        private void ThreadRun() 
        {
            Stopwatch stopwatch = new Stopwatch();
            _isRunning = true;

            while (_isRunning && (_sortEnumerator?.MoveNext() ?? false)) 
            {
                if (_visualizationStopEvent.WaitOne(Math.Max(0, Delay - (int)stopwatch.ElapsedMilliseconds))) 
                {
                    break;
                }

                stopwatch.Reset();

                Visualizer.Visualize(_sortEnumerator.Current);

                stopwatch.Stop();
            }

            _isRunning = false;
        }
    }
}
