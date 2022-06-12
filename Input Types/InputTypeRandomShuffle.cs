using System;


namespace SortingVisualizer.InputTypes
{
    public class InputTypeRandomShuffle : ArrayInputType
    {
        public override void Generate(int[] array)
        {
            if (array is null) 
            {
                throw new ArgumentNullException(nameof(array), $"{nameof(array)} must not be null.");
            }

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i + 1;
            }

            Random rand = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                int swapIndex = rand.Next(array.Length);

                int temp = array[swapIndex];
                array[swapIndex] = array[i];
                array[i] = temp;
            }
        }
    }
}
