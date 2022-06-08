using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public class InsertionSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            int n = array.Length;

            for (int i = 1; i < n; i++)
            {
                SortStep step = new SortStep(array);
                step.AccessedIndices.Add(i);
                step.Comparsions++;
                yield return step;

                int key = array[i];
                int j = i - 1;

                while (j >= 0 && array[j] > key)
                {
                    step = new SortStep(array);
                    step.ChangedIndices.Add(j);
                    step.ChangedIndices.Add(j + 1);
                    step.Comparsions++;
                    yield return step;

                    array[j + 1] = array[j];
                    j--;
                }

                step = new SortStep(array);
                step.ChangedIndices.Add(j + 1);
                yield return step;

                array[j + 1] = key;
            }

            yield return new SortStep(array);
        }
    }
}
