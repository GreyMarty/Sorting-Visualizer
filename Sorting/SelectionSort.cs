using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    internal class SelectionSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            int n = array.Length;

            for (int i = 0; i < n - 1; i++) 
            {
                SortStep step;

                int minIndex = i;

                for (int j = i + 1; j < n; j++) 
                {
                    step = new SortStep(array);
                    step.AccessedIndices.Add(minIndex);
                    step.AccessedIndices.Add(j);
                    step.Comparsions++;
                    yield return step;

                    if (array[j] < array[minIndex]) 
                    {
                        minIndex = j;
                    }
                }

                step = new SortStep(array);
                step.ChangedIndices.Add(minIndex);
                step.ChangedIndices.Add(i);
                yield return step;

                int temp = array[minIndex];
                array[minIndex] = array[i];
                array[i] = temp;
            }

            yield return new SortStep(array);
        }
    }
}
