using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public interface ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array);
    }
}
