using System.Collections.Generic;


namespace SortingVisualizer.Sorting
{
    public class MergeSort : ISorter
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

        private IEnumerator<SortStep> Sort(int[] array, int begin, int end) 
        {
            if (begin >= end)
                yield break;

            int mid = begin + (end - begin) / 2;

            IEnumerator<SortStep> enumerator;

            enumerator = Sort(array, begin, mid);

            while (enumerator.MoveNext()) 
            {
                yield return enumerator.Current;
            }

            enumerator = Sort(array, mid + 1, end);

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            enumerator = Merge(array, begin, mid, end);

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        private IEnumerator<SortStep> Merge(int[] array, int left, int mid, int right)
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
