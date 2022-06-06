using System;
using System.Data;

namespace SortingVisualizer.InputTypes
{
    public class InputTypeRandomShuffle : InputType
    {
        public override void Generate<T>(T[] array)
        {
            Type type = typeof(T);

            if (type != typeof(int) && type != typeof(float) && type != typeof(double))
            {
                throw new InvalidConstraintException($"{type} is not supported!");
            }

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (T)Convert.ChangeType(i + 1, type);
            }

            Random rand = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                int swapIndex = rand.Next(array.Length);

                T temp = array[swapIndex];
                array[swapIndex] = array[i];
                array[i] = temp;
            }
        }
    }
}
