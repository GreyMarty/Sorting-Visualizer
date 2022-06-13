using System;
using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    internal class QuickSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            IEnumerator<SortStep> enumerator = Sort(array, 0, array.Length - 1);

            while (enumerator.MoveNext()) 
            {
                yield return enumerator.Current;
            }

            yield return new SortStep(array);
        }

        private IEnumerator<SortStep> Sort(int[] array, int start, int end) 
        {
            if (start > end) 
            {
                yield break;
            }

            SortStep step = new SortStep(array);
            step.AccessedIndices.Add(end);
            yield return step;

            int pivot = array[end];

            int i = (start - 1);

            for (int j = start; j <= end - 1; j++)
            {
                step = new SortStep(array);
                step.AccessedIndices.Add(j);
                step.Comparsions++;

                if (array[j] < pivot)
                {
                    i++;

                    step.ChangedIndices.Add(i);
                    step.ChangedIndices.Add(j);
                    yield return step;

                    Swap(ref array[i], ref array[j]);
                }
                else 
                {
                    yield return step;
                }
            }

            step = new SortStep(array);
            step.ChangedIndices.Add(i + 1);
            step.ChangedIndices.Add(end);
            yield return step;

            Swap(ref array[i + 1], ref array[end]);

            int pi = i + 1;

            IEnumerator<SortStep> enumerator = Sort(array, start, pi - 1);

            while (enumerator.MoveNext()) 
            {
                yield return enumerator.Current;
            }

            enumerator = Sort(array, pi + 1, end);

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        private static void Swap(ref int a, ref int b) 
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}
