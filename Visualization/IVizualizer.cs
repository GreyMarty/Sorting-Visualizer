using System;
using SortingVisualizer.Sorting;


namespace SortingVisualizer.Visualization
{
    /// <summary>
    /// Defines methods for sorting visualization
    /// </summary>
    public interface IVizualizer : IDisposable
    {
        public void Visualize(int[] array);

        public void Visualize(SortStep sortState);
    }
}
