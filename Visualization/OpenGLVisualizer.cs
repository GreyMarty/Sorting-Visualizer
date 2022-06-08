using SortingVisualizer.Sorting;
using System;
using OpenTK.Wpf;

namespace SortingVisualizer.Visualization
{
    public class OpenGLVisualizer : IVizualizer
    {
        public GLWpfControl GLControl { get; init; }

        private SortStep _currentState;

        public OpenGLVisualizer(GLWpfControl glControl) 
        {
            GLControl = glControl;
            glControl.Render += GlControl_Render;
        }

        ~OpenGLVisualizer() 
        {
            GLControl.Render -= GlControl_Render;
        }

        public void Visualize(int[] array)
        {
            _currentState = new SortStep(array);
            
            GLControl.InvalidateVisual();
        }

        public void Visualize(SortStep sortState)
        {
            _currentState = sortState;
            
            GLControl.InvalidateVisual();
        }

        private void GlControl_Render(TimeSpan timeDelta)
        {
            // TODO: Render current state
        }
    }
}
