﻿using System;


namespace SortingVisualizer.InputTypes
{
    public class InputTypeSortedEnd : InputType
    {
        public override void Generate(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i + 1;
            }

            int mid = array.Length / 2;
            Random random = new Random();

            for (int i = 0; i < mid; i++)
            {
                int swapIndex = random.Next(0, mid);

                int temp = array[i];
                array[i] = array[swapIndex];
                array[swapIndex] = temp;
            }
        }
    }
}
