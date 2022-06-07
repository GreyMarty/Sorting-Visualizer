using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SortingVisualizer.InputTypes
{
    public abstract class InputType
    {
        public static InputType Sorted => new InputTypeSorted();
        public static InputType Reversed => new InputTypeReversed();
        public static InputType RandomShuffle => new InputTypeRandomShuffle();

        public abstract void Generate(int[] array);
    }
}
