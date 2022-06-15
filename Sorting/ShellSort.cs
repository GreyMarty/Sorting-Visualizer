using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public class ShellSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            int n = array.Length;

            SortStep sortStep;

            for (int step = n / 2; step > 0; step /= 2) 
            {
                for (int i = step; i < n; i++) 
                {
                    sortStep = new SortStep(array);
                    sortStep.AccessedIndices.Add(i);
                    yield return sortStep;

                    int temp = array[i];

                    int j;
                    for (j = i; j >= step && array[j - step] > temp; j -= step) 
                    {
                        sortStep = new SortStep(array);
                        sortStep.AccessedIndices.Add(j - step);
                        sortStep.ChangedIndices.Add(j);
                        sortStep.Comparsions++;
                        yield return sortStep;

                        array[j] = array[j - step];
                    }

                    sortStep = new SortStep(array);
                    sortStep.ChangedIndices.Add(j);
                    yield return sortStep;

                    array[j] = temp;
                }
            }

            yield return new SortStep(array);
        }
    }
}
