namespace SortingVisualizer.Sorting
{
    public static class Sorters
    {
        public static ISorter BubbleSort => new BubbleSort();
        public static ISorter SelectionSort => new SelectionSort();
        public static ISorter InsertionSort => new InsertionSort();
        public static ISorter MergeSort => new MergeSort();
        public static ISorter MergeSortIterative => new MergeSortIterative();
        public static ISorter RadixSort => new RadixSort();
        public static ISorter QuickSort => new QuickSort();
        public static ISorter HoareQuickSort => new HoareQuickSort();
    }
}
