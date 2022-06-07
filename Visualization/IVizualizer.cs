using SortingVisualizer.Sorting;

namespace SortingVisualizer.Visualization
{
    /// <summary>
    /// Defines methods for sorting visualization
    /// </summary>
    public interface IVizualizer
    {
        public void Visualize(int[] array, bool forceRedraw = false);

        public void Visualize(SortStep sortState, bool forceRedraw = false);
    }
}
