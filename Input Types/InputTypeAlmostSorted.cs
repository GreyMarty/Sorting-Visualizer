using System;


namespace SortingVisualizer.InputTypes
{
    internal class InputTypeAlmostSorted : ArrayInputType
    {
        public override void Generate(int[] array)
        {
            for (int i = 0; i < array.Length; i++) 
            {
                array[i] = i + 1;
            }

            Random random = new Random();

            int shuffleCount = array.Length / 5;

            for (int i = 0; i < shuffleCount; i++) 
            {
                int index1 = random.Next(array.Length);
                int index2 = random.Next(array.Length);

                int temp = array[index1];
                array[index1] = array[index2];
                array[index2] = temp;
            }
        }
    }
}
