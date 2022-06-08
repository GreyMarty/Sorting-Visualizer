using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    internal class RadixSort : ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array)
        {
            int max = array[0];

            for (int i = 0; i < array.Length; i++) 
            {
                SortStep step = new SortStep(array);
                step.AccessedIndices.Add(i);
                yield return step;

                if (array[i] > max) 
                {
                    max = array[i];
                }
            }

            for (int exp = 1; max / exp > 0; exp *= 10) 
            {
                IEnumerator<SortStep> sortEnumerator = CountingSort(array, exp);

                while (sortEnumerator.MoveNext()) 
                {
                    yield return sortEnumerator.Current;
                }
            }

            yield return new SortStep(array);
        }

        private IEnumerator<SortStep> CountingSort(int[] array, int exp) 
        {
            int n = array.Length;
            int[] digits = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] temp = new int[n];

            for (int i = 0; i < n; i++) 
            {
                SortStep step = new SortStep(array);
                step.AccessedIndices.Add(i);
                yield return step;

                digits[GetDigitAt(array[i], exp)]++;
            }

            for (int i = 1; i < digits.Length; i++) 
            {
                digits[i] += digits[i - 1];
            }

            for (int i = n - 1; i >= 0; i--) 
            {
                SortStep step = new SortStep(array);
                step.AccessedIndices.Add(i);
                yield return step;

                temp[digits[GetDigitAt(array[i], exp)] - 1] = array[i];
                digits[GetDigitAt(array[i], exp)]--;
            }

            for (int i = 0; i < n; i++) 
            {
                SortStep step = new SortStep(array);
                step.ChangedIndices.Add(i);
                yield return step;

                array[i] = temp[i];
            }
        }

        private int GetDigitAt(int number, int exp) 
        {
            return (number / exp) % 10;
        }
    }
}
