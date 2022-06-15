using System;
using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public class CombSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            int n = array.Length;
            int gap = n;

            bool swapped = true;

            SortStep step;

            while (gap != 1 || swapped)
            {
                gap = NextGap(gap);

                swapped = false;

                for (int i = 0; i < n - gap; i++)
                {
                    step = new SortStep(array);
                    step.AccessedIndices.Add(i);
                    step.AccessedIndices.Add(i + gap);
                    step.Comparsions++;
                    yield return step;

                    if (array[i] > array[i + gap])
                    {
                        step = new SortStep(array);
                        step.ChangedIndices.Add(i);
                        step.ChangedIndices.Add(i + gap);
                        yield return step;

                        int temp = array[i];
                        array[i] = array[i + gap];
                        array[i + gap] = temp;

                        swapped = true;
                    }
                }
            }

            yield return new SortStep(array);
        }

        private static int NextGap(int gap) 
        {
            gap = (gap * 10) / 13;
            return gap < 1 ? 1 : gap;
        }
    }
}
