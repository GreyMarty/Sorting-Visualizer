using System;
using System.Collections.Generic;
using OpenTK.Wpf;
using System.Windows;
using SortingVisualizer.Sorting;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SortingVisualizer.Visualization
{
    public class OpenGLVisualizer : IVizualizer
    {
        public GLWpfControl GLControl { get; init; }

        private SortStep _currentState;

        private int _vertexBufferObject;
        private int _vertexArrayObject;

        private bool _glLoaded = false;

        private Shader _shader;

        private readonly Vector3 _defaultColor = new Vector3(1f, 1f, 1f);
        private readonly Vector3 _accessedColor = new Vector3(0f, 1f, 0f);
        private readonly Vector3 _changedColor = new Vector3(1f, 0f, 0f);

        public OpenGLVisualizer(GLWpfControl glControl) 
        {
            GLControl = glControl;

            glControl.Loaded += GLControl_Loaded;
            glControl.Render += GLControl_Render;

            if (glControl.IsLoaded)
            {
                GLControl_Loaded(GLControl, (RoutedEventArgs)EventArgs.Empty);
            }
        }

        ~OpenGLVisualizer() 
        {
            GLControl.Render -= GLControl_Render;
        }

        public void Visualize(int[] array)
        {
            _currentState = new SortStep(array);

            GLControl.Dispatcher.Invoke(GLControl.InvalidateVisual);
        }

        public void Visualize(SortStep sortState)
        {
            _currentState = sortState;

            GLControl.Dispatcher.Invoke(GLControl.InvalidateVisual);
        }

        private void GenerateBuffer() 
        {
            _vertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            // Buffer format
            // 
            // | position | size | color |
            // |    2     |   2  |   3   |

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            int positionIndex = 0;
            GL.VertexAttribPointer(positionIndex, 2, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionIndex);

            int sizeIndex = 1;
            GL.VertexAttribPointer(sizeIndex, 2, VertexAttribPointerType.Float, false, 7 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(sizeIndex);

            int colorIndex = 2;
            GL.VertexAttribPointer(colorIndex, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 4 * sizeof(float));
            GL.EnableVertexAttribArray(colorIndex);
        }

        private float[] GenerateVertices(SortStep step)
        {
            List<float> vertices = new List<float>();

            float width = 2f / step.Array.Length;
            float heightCoef = 2 * 0.8f / step.Array.Length;

            for (int i = 0; i < step.Array.Length; i++)
            {
                // Position
                vertices.Add(width * i - 1);
                vertices.Add(-1f);

                // Size
                vertices.Add(width);
                vertices.Add(heightCoef * step.Array[i]);

                // Color
                if (step.ChangedIndices.Contains(i))
                {
                    vertices.Add(_changedColor.X);
                    vertices.Add(_changedColor.Y);
                    vertices.Add(_changedColor.Z);
                }
                else if (step.AccessedIndices.Contains(i))
                {
                    vertices.Add(_accessedColor.X);
                    vertices.Add(_accessedColor.Y);
                    vertices.Add(_accessedColor.Z);
                }
                else 
                {
                    vertices.Add(_defaultColor.X);
                    vertices.Add(_defaultColor.Y);
                    vertices.Add(_defaultColor.Z);
                }
            }

            return vertices.ToArray();
        }

        private void GLControl_Loaded(object sender, RoutedEventArgs e)
        {
            GL.ClearColor(0f, 0f, 0f, 1f);

            _shader = new Shader("Shaders/default.vert", "Shaders/default.geom", "Shaders/default.frag");

            GenerateBuffer();

            _glLoaded = true;
        }

        private void GLControl_Render(TimeSpan timeDelta)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (_currentState is null || !_glLoaded) 
            {
                return;
            }

            _shader.Use();

            float[] vertices = GenerateVertices(_currentState);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawArrays(PrimitiveType.Points, 0, vertices.Length / 7);
        }
    }
}
