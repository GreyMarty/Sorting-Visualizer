using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public class BubbleSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            for (int i = 0; i < array.Length; i++) 
            {
                for (int j = 0; j < array.Length - i - 1; j++) 
                {
                    SortStep step = new SortStep(array);
                    step.AccessedIndices.Add(j);
                    step.AccessedIndices.Add(j + 1);

                    step.Comparsions++;

                    if (array[j] > array[j + 1])
                    {
                        step.ChangedIndices.Add(j);
                        step.ChangedIndices.Add(j + 1);
                        yield return step;

                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                    else 
                    {
                        yield return step;
                    }
                }
            }

            yield return new SortStep(array);
        }
    }
}
