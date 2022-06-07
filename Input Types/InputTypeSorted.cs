using System;
using System.Data;

namespace SortingVisualizer.InputTypes
{
    public class InputTypeSorted : InputType
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
        }
    }
}
