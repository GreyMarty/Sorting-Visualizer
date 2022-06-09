using System;


namespace SortingVisualizer.InputTypes
{
    public class InputTypeShuffledTail : InputType
    {
        public override void Generate(int[] array)
        {
            for (int i = 0; i < array.Length; i++) 
            {
                array[i] = i + 1;
            }

            int mid = array.Length / 2;
            Random random = new Random();

            for (int i = mid; i < array.Length; i++) 
            {
                int swapIndex = random.Next(mid, array.Length);

                int temp = array[i];
                array[i] = array[swapIndex];
                array[swapIndex] = temp;
            }
        }
    }
}
