using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public class PigeonHoleSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            int n = array.Length;

            SortStep step;

            int min = array[0];
            int max = array[0];

            for (int i = 0; i < n; i++)
            {
                step = new SortStep(array);
                step.AccessedIndices.Add(i);
                yield return step;

                if (array[i] > max)
                {
                    max = array[i];
                }

                if (array[i] < min)
                {
                    min = array[i];
                }
            }

            int range = max - min + 1;
            int[] phole = new int[range];

            for (int i = 0; i < n; i++)
            {
                step = new SortStep(array);
                step.AccessedIndices.Add(i);
                yield return step;

                phole[array[i] - min]++;
            }


            int index = 0;

            for (int j = 0; j < range; j++)
            {
                while (phole[j]-- > 0)
                {
                    step = new SortStep(array);
                    step.ChangedIndices.Add(index);
                    yield return step;

                    array[index++] = j + min;
                }
            }

            yield return new SortStep(array);
        }
    }
}
