namespace SortingVisualizer.InputTypes
{
    public abstract class InputType
    {
        public static InputType Sorted => new InputTypeSorted();
        public static InputType Reversed => new InputTypeReversed();
        public static InputType RandomShuffle => new InputTypeRandomShuffle();
        public static InputType ShuffledTail => new InputTypeShuffledTail();
        public static InputType ShuffledHead => new InputTypeShuffledHead();
        public static InputType AlmostSorted => new InputTypeAlmostSorted();

        /// <summary>
        /// Fills array in a certain way
        /// </summary>
        public abstract void Generate(int[] array);
    }
}
