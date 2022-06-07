﻿namespace SortingVisualizer.InputTypes
{
    public abstract class InputType
    {
        public static InputType Sorted => new InputTypeSorted();
        public static InputType Reversed => new InputTypeReversed();
        public static InputType RandomShuffle => new InputTypeRandomShuffle();

        public abstract void Generate(int[] array);
    }
}
