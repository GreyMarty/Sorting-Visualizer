using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingVisualizer.Sorting
{
    public interface ISorter
    {
        public IEnumerator<SortStep> Sort(int[] array);
    }
}
