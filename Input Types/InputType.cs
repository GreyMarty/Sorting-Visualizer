namespace SortingVisualizer.InputTypes
{
    public abstract class InputType
    {
        public static InputType Sorted => new InputTypeSorted();
        public static InputType Reversed => new InputTypeReversed();
        public static InputType RandomShuffle => new InputTypeRandomShuffle();
        public static InputType SortedStart => new InputTypeSortedStart();
        public static InputType SortedEnd => new InputTypeSortedEnd();

        /// <summary>
        /// Fills array in a certain way
        /// </summary>
        public abstract void Generate(int[] array);
    }
}
