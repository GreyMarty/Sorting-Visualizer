using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public class HeapSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            int n = array.Length;

            IEnumerator<SortStep> enumerator;

            for (int i = n / 2 - 1; i >= 0; i--) 
            {
                enumerator = Heapify(array, n, i);

                while (enumerator.MoveNext()) 
                {
                    yield return enumerator.Current;
                }
            }

            for (int i = n - 1; i > 0; i--) 
            {
                int temp = array[0];
                array[0] = array[i];
                array[i] = temp;

                enumerator = Heapify(array, i, 0);

                while (enumerator.MoveNext()) 
                {
                    yield return enumerator.Current;
                }
            }

            yield return new SortStep(array);
        }

        private static IEnumerator<SortStep> Heapify(int[] array, int n, int rootIndex) 
        {
            int largestIndex = rootIndex;
            int leftIndex = 2 * rootIndex + 1;
            int rightIndex = 2 * rootIndex + 2;

            SortStep step;

            if (leftIndex < n)
            {
                step = new SortStep(array);
                step.AccessedIndices.Add(leftIndex);
                step.AccessedIndices.Add(largestIndex);
                step.Comparsions++;
                yield return step;
            }

            if (leftIndex < n && array[leftIndex] > array[largestIndex]) 
            {
                largestIndex = leftIndex;
            }

            if (rightIndex < n)
            {
                step = new SortStep(array);
                step.AccessedIndices.Add(rightIndex);
                step.AccessedIndices.Add(largestIndex);
                step.Comparsions++;
                yield return step;
            }

            if (rightIndex < n && array[rightIndex] > array[largestIndex]) 
            {
                largestIndex = rightIndex;
            }

            if (largestIndex == rootIndex) 
            {
                yield break;
            }

            step = new SortStep(array);
            step.ChangedIndices.Add(largestIndex);
            step.ChangedIndices.Add(rootIndex);
            yield return step;

            int temp = array[largestIndex];
            array[largestIndex] = array[rootIndex];
            array[rootIndex] = temp;

            IEnumerator<SortStep> enumerator = Heapify(array, n, largestIndex);

            while (enumerator.MoveNext()) 
            {
                yield return enumerator.Current;
            }
        }
    }
}
