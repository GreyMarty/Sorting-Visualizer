namespace SortingVisualizer.Sorting
{
    public static class Sorters
    {
        public static ISorter BubbleSort => new BubbleSort();
        public static ISorter SelectionSort => new SelectionSort();
        public static ISorter InsertionSort => new InsertionSort();
        public static ISorter MergeSort => new MergeSort();
        public static ISorter RadixSort => new RadixSort();
    }
}
