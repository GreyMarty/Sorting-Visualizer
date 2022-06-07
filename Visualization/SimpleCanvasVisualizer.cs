using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Shapes;
using SortingVisualizer.Sorting;

namespace SortingVisualizer.Visualization
{
    /// <summary>
    /// Provides visualization mechanism of sorting on canvas
    /// </summary>
    public class SimpleCanvasVisualizer : IVizualizer
    {
        public Canvas Canvas { get; init; }

        private Dispatcher _dispatcher => Canvas.Dispatcher;

        private bool _needsInit = true;

        private bool _rememberLast;
        private int[] _lastVisualized;
        private List<int> _colorResetIndices;

        private readonly SolidColorBrush _defaultColorBrush = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush _accessedColorBrush = new SolidColorBrush(Colors.Lime);
        private readonly SolidColorBrush _changedColorBrush = new SolidColorBrush(Colors.Red);


        public SimpleCanvasVisualizer(Canvas canvas, bool rememberLast = true) 
        {
            Canvas = canvas;
            _rememberLast = rememberLast;
        }

        public void Visualize(int[] array, bool forceRedraw = false)
        {
            _dispatcher.Invoke(() => { UpdatePool(array.Length); });

            float width = (float)Canvas.ActualWidth / array.Length;
            float heightCoefficient = (float)Canvas.ActualHeight / array.Length * 0.8f;

            if (forceRedraw) 
            {
                _lastVisualized = null;
            }

            int[] changedIndices = FindChangedIndices(array);

            foreach (int i in changedIndices) 
            {
                _dispatcher.Invoke(() => 
                {
                    Rectangle rectangle = (Rectangle)Canvas.Children[i];
                    
                    rectangle.Width = width;
                    rectangle.Height = heightCoefficient * array[i];

                    Canvas.SetLeft(rectangle, width * i);
                    Canvas.SetTop(rectangle, Canvas.ActualHeight - rectangle.Height);
                });
            }

            if (_colorResetIndices is not null) 
            {
                foreach (int i in _colorResetIndices) 
                {
                    _dispatcher.Invoke(() =>
                    {
                        Rectangle rectangle = (Rectangle)Canvas.Children[i];

                        rectangle.Fill = _defaultColorBrush;
                    });
                }
            }

            if (_rememberLast) 
            {
                _lastVisualized = new int[array.Length];
                array.CopyTo(_lastVisualized, 0);
            }
        }

        public void Visualize(SortStep sortState, bool forceRedraw = false)
        {
            Visualize(sortState.Array, forceRedraw);

            _colorResetIndices = _colorResetIndices ?? new List<int>();
            _colorResetIndices.Clear();

            foreach (int i in sortState.AccessedIndices) 
            {
                if (sortState.ChangedIndices.Contains(i)) 
                {
                    continue;
                }

                _dispatcher.Invoke(() =>
                {
                    Rectangle rectangle = (Rectangle)Canvas.Children[i];

                    rectangle.Fill = _accessedColorBrush;
                });

                _colorResetIndices.Add(i);
            }

            foreach (int i in sortState.ChangedIndices)
            {
                _dispatcher.Invoke(() =>
                {
                    Rectangle rectangle = (Rectangle)Canvas.Children[i];

                    rectangle.Fill = _changedColorBrush;
                });

                _colorResetIndices.Add(i);
            }
        }

        private void UpdatePool(int targetCount) 
        {
            if (_needsInit) 
            {
                Canvas.Children.Clear();
                _needsInit = false;
            }

            int childrenCount = Canvas.Children.Count;
            int delta = targetCount - childrenCount;

            if (delta > 0) 
            {
                for (int i = 0; i < delta; i++) 
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                    rectangle.Fill = _defaultColorBrush;

                    Canvas.Children.Add(rectangle);
                }

                return;
            }

            if (delta < 0) 
            {
                Canvas.Children.RemoveRange(childrenCount + delta - 1, -delta);
            }
        }

        private int[] FindChangedIndices(int[] array) 
        {
            if (array is null) 
            {
                throw new ArgumentNullException(nameof(array), $"{nameof(array)} must not be null.");
            }

            List<int> changedIndices = new List<int>();

            if (_lastVisualized is null || array.Length != _lastVisualized.Length) 
            {
                for (int i = 0; i < array.Length; i++) 
                {
                    changedIndices.Add(i);
                }

                return changedIndices.ToArray();
            }

            for (int i = 0; i < array.Length; i++) 
            {
                if (_lastVisualized[i] != array[i]) 
                {
                    changedIndices.Add(i);
                }
            }

            return changedIndices.ToArray();
        }
    }
}
