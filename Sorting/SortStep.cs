using System.Collections.Generic;

namespace SortingVisualizer.Sorting
{
    /// <summary>
    /// Describes one step (iteration) of a sorting algorithm
    /// </summary>
    public class SortStep
    {
        public int[] Array { get; private set; }

        public HashSet<int> AccessedIndices { get; private set; }
        public HashSet<int> ChangedIndices { get; private set; }

        public int ArrayAccesses => AccessedIndices.Count + ChangedIndices.Count;
        public int ArrayWrites => ChangedIndices.Count;
        public int Comparsions { get; set; }

        public SortStep(int[] array)
        {
            Array = array;

            AccessedIndices = new HashSet<int>();
            ChangedIndices = new HashSet<int>();
        }
    }
}
