using System;
using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public class TimSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            const int Run = 32;
            int n = array.Length;

            IEnumerator<SortStep> enumerator;

            for (int i = 0; i < n; i += Run)
            {
                enumerator = InsertionSort(array, i, Math.Min((i + Run - 1), (n - 1)));

                while (enumerator.MoveNext()) 
                {
                    yield return enumerator.Current;
                }
            }

            for (int size = Run; size < n; size = 2 * size)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = Math.Min((left + 2 * size - 1), (n - 1));

                    if (mid < right)
                    {
                        enumerator = Merge(array, left, mid, right);

                        while (enumerator.MoveNext()) 
                        {
                            yield return enumerator.Current;
                        }
                    }
                }
            }

            yield return new SortStep(array);
        }

        private static IEnumerator<SortStep> InsertionSort(int[] array, int left, int right)
        {
            SortStep step;

            for (int i = left + 1; i <= right; i++)
            {
                step = new SortStep(array);
                step.AccessedIndices.Add(i);
                yield return step;

                int temp = array[i];
                int j = i - 1;

                while (j >= left && array[j] > temp)
                {
                    step.Comparsions++;
                    step.AccessedIndices.Add(j);
                    step.ChangedIndices.Add(j + 1);
                    yield return step;

                    array[j + 1] = array[j];
                    j--;
                }

                step = new SortStep(array);
                step.ChangedIndices.Add(j + 1);
                yield return step;

                array[j + 1] = temp;
            }
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
