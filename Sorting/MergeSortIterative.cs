using System;
using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public class MergeSortIterative : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            int n = array.Length;

            for (int currentSize = 1; currentSize <= n - 1; currentSize = 2 * currentSize)
            {
                for (int leftStart = 0; leftStart < n - 1; leftStart += 2 * currentSize)
                {
                    int mid = Math.Min(leftStart + currentSize - 1, n - 1);

                    int rightEnd = Math.Min(leftStart + 2 * currentSize - 1, n - 1);

                    IEnumerator<SortStep> enumerator = Merge(array, leftStart, mid, rightEnd);

                    while (enumerator.MoveNext()) 
                    {
                        yield return enumerator.Current;
                    }
                }
            }

            yield return new SortStep(array);
        }

        private static IEnumerator<SortStep> Merge(int[] array, int left, int mid, int right)
        {
            int lengthLeft = mid - left + 1;
            int lengthRight = right - mid;

            int[] leftArray = new int[lengthLeft];
            int[] rightArray = new int[lengthRight];

            SortStep step;

            for (int i = 0; i < lengthLeft; i++)
            {
                step = new SortStep(array);
                step.AccessedIndices.Add(left + i);
                yield return step;

                leftArray[i] = array[left + i];
            }

            for (int i = 0; i < lengthRight; i++)
            {
                step = new SortStep(array);
                step.AccessedIndices.Add(mid + 1 + i);
                yield return step;

                rightArray[i] = array[mid + 1 + i];
            }

            int indexLeft = 0;
            int indexRight = 0;
            int indexMerged = left;

            while (indexLeft < lengthLeft && indexRight < lengthRight)
            {
                step = new SortStep(array);
                step.ChangedIndices.Add(indexMerged);
                step.Comparsions++;
                yield return step;

                if (leftArray[indexLeft] <= rightArray[indexRight])
                {
                    array[indexMerged] = leftArray[indexLeft];
                    indexLeft++;
                }
                else
                {
                    array[indexMerged] = rightArray[indexRight];
                    indexRight++;
                }

                indexMerged++;
            }

            while (indexLeft < lengthLeft)
            {
                step = new SortStep(array);
                step.ChangedIndices.Add(indexMerged);
                yield return step;

                array[indexMerged] = leftArray[indexLeft];
                indexLeft++;
                indexMerged++;
            }

            while (indexRight < lengthRight)
            {
                step = new SortStep(array);
                step.ChangedIndices.Add(indexMerged);
                yield return step;

                array[indexMerged] = rightArray[indexRight];
                indexRight++;
                indexMerged++;
            }
        }
    }
}
