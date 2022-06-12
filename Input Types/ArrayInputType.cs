namespace SortingVisualizer.InputTypes
{
    public abstract class ArrayInputType
    {
        public static ArrayInputType Sorted => new InputTypeSorted();
        public static ArrayInputType Reversed => new InputTypeReversed();
        public static ArrayInputType RandomShuffle => new InputTypeRandomShuffle();
        public static ArrayInputType ShuffledTail => new InputTypeShuffledTail();
        public static ArrayInputType ShuffledHead => new InputTypeShuffledHead();
        public static ArrayInputType AlmostSorted => new InputTypeAlmostSorted();

        /// <summary>
        /// Fills array in a certain way
        /// </summary>
        public abstract void Generate(int[] array);
    }
}
