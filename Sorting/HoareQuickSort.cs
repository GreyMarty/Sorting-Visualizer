using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    internal class HoareQuickSort : ISorter
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
            if (start >= end)
            {
                yield break;
            }

            SortStep step = new SortStep(array);
            step.AccessedIndices.Add(start);
            yield return step;

            int pivot = array[start];
            int i = (start - 1);
            int j = (end + 1);

            while (true) 
            {
                do
                {
                    i++;

                    step = new SortStep(array);
                    step.AccessedIndices.Add(i);
                    step.Comparsions++;
                    yield return step;
                } while (array[i] < pivot);

                do
                {
                    j--;

                    step = new SortStep(array);
                    step.AccessedIndices.Add(j);
                    step.Comparsions++;
                    yield return step;
                } while (array[j] > pivot);

                if (i >= j) 
                {
                    break;
                }

                step = new SortStep(array);
                step.ChangedIndices.Add(i);
                step.ChangedIndices.Add(j);
                yield return step;

                Swap(ref array[i], ref array[j]);
            }

            int pi = j;

            IEnumerator<SortStep> enumerator = Sort(array, start, pi);

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
