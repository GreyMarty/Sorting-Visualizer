using System;
using System.Windows;
using System.Windows.Controls;
using SortingVisualizer.InputTypes;
using SortingVisualizer.Visualization;
using SortingVisualizer.Sorting;


namespace SortingVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MinSpeed = 0;
        private const int MaxSpeed = 1000;
        private const float MinDelay = 5f;
        private const float MaxDelay = 1000f;

        private const int MinArraySize = 10;
        private const int MaxArraySize = 1000;

        private const string delayFormat = "{0:0} ms";

        private InputType[] _inputTypes;
        private ISorter[] _sorters;

        private int _delay;
        private int _arraySize;
        private InputType _inputType;
        private ISorter _sorter;

        private VisualizationController _controller;

        public MainWindow()
        {
            InitializeComponent();
            
            InitializeSliders();

            InitializeAlgorithms();

            InitializeInputTypes();

            _controller = new VisualizationController(new SimpleCanvasVisualizer(canvas), _sorter);
            
            InitializeEvents();
        }

        private void InitializeEvents() 
        {
            Loaded += MainWindow_Load;
            SizeChanged += MainWindow_SizeChanged;

            buttonPlay.Click += buttonPlay_Click;
            buttonPause.Click += ButtonPause_Click;
            buttonStep.Click += buttonStep_Click;
            buttonReset.Click += buttonReset_Click;

            sliderSpeed.ValueChanged += sliderSpeed_ValueChanged;
            sliderArraySize.ValueChanged += sliderArraySize_ValueChanged;

            comboBoxInputType.SelectionChanged += comboBoxInputType_SelectionChanged;
            listBoxAlgorithms.SelectionChanged += listBoxAlgorithms_SelectionChanged;

            _controller.StateChanged += _controller_StateChanged;
        }

        private void InitializeAlgorithms()
        {
            _sorters = new ISorter[]
            {
                Sorters.BubbleSort,
                Sorters.SelectionSort,
                Sorters.InsertionSort,
                Sorters.MergeSort,
                Sorters.RadixSort
            };

            listBoxAlgorithms.Items.Add("Bubble Sort");
            listBoxAlgorithms.Items.Add("Selection Sort");
            listBoxAlgorithms.Items.Add("Insertion Sort");
            listBoxAlgorithms.Items.Add("Merge Sort");
            listBoxAlgorithms.Items.Add("Radix Sort");

            listBoxAlgorithms.SelectedIndex = 0;
            _sorter = _sorters[0];
        }

        private void InitializeInputTypes() 
        {
            _inputTypes = new InputType[]
            {
                InputType.Sorted,
                InputType.Reversed,
                InputType.RandomShuffle,
                InputType.SortedStart,
                InputType.SortedEnd
            };

            comboBoxInputType.Items.Add("Sorted");
            comboBoxInputType.Items.Add("Sorted (Reverse)");
            comboBoxInputType.Items.Add("Random Shuffle");
            comboBoxInputType.Items.Add("Sorted Start");
            comboBoxInputType.Items.Add("Sorted End");

            comboBoxInputType.SelectedIndex = 0;
            _inputType = _inputTypes[0];
        }

        private void InitializeSliders() 
        {
            sliderSpeed.Minimum = MinSpeed;
            sliderSpeed.Maximum = MaxSpeed;

            sliderArraySize.Minimum = MinArraySize;
            sliderArraySize.Maximum = MaxArraySize;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            sliderSpeed_ValueChanged(sliderSpeed, EventArgs.Empty);
            sliderArraySize_ValueChanged(sliderArraySize, EventArgs.Empty);
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            _controller.Redraw();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            _controller.Run();
        }
        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            _controller.Pause();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            _controller.Reset(_arraySize, _inputType, _sorter);
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            _controller.Step();
        }

        private void sliderSpeed_ValueChanged(object sender, EventArgs e)
        {
            Slider slider = (Slider)sender;

            if (slider is null) 
            {
                throw new ArgumentNullException(nameof(sender), $"{nameof(sender)} must not be null.");
            }

            _delay = (int)((MaxSpeed - slider.Value) * (MaxDelay - MinDelay) / (MaxSpeed - MinSpeed) + MinDelay);
            _controller.Delay = _delay;
            labelDelay.Content = string.Format(delayFormat, _delay);
        }

        private void sliderArraySize_ValueChanged(object sender, EventArgs e)
        {
            Slider slider = (Slider)sender;

            if (slider is null) 
            {
                throw new ArgumentNullException(nameof(sender), $"{nameof(sender)} must not be null.");
            }

            _arraySize = (int)slider.Value;
            labelArraySize.Content = _arraySize;
        }

        private void comboBoxInputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (sender is null) 
            {
                throw new ArgumentNullException(nameof(sender), $"{nameof(sender)} must not be null.");
            }

            _inputType = _inputTypes[comboBox.SelectedIndex];
        }

        private void listBoxAlgorithms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;

            if (sender is null) 
            {
                throw new ArgumentNullException(nameof(sender), $"{nameof(sender)} must not be null.");
            }

            _sorter = _sorters[listBox.SelectedIndex];
        }

        private void _controller_StateChanged(object sender, EventArgs e)
        {
            VisualizationController controller = (VisualizationController)sender;

            if (sender is null) 
            {
                throw new ArgumentNullException(nameof(sender), $"{nameof(sender)} must not be null.");
            }

            Dispatcher.Invoke(() =>
            {
                labelArrayAccesses.Content = controller.ArrayAccesses;
                labelComparsions.Content = controller.Comparsions;
                labelArrayWrites.Content = controller.ArrayWrites;
            });
        }
    }
}
