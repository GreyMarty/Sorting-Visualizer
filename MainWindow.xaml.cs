using System;
using System.Windows;
using System.Windows.Controls;

using SortingVisualizer.InputTypes;


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

        private int _delay;
        private int _arraySize;
        private InputType _inputType;

        public MainWindow()
        {
            InitializeComponent();
            
            InitializeSliders();

            InitializeAlgorithms();

            InitializeInputTypes();
            
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

            comboBoxInputType.SelectionChanged += ComboBoxInputType_SelectionChanged;
        }

        private void InitializeAlgorithms()
        {
            // TODO: Inialize list box
        }

        private void InitializeInputTypes() 
        {
            _inputTypes = new InputType[]
            {
                InputType.Sorted,
                InputType.Reversed,
                InputType.RandomShuffle
            };

            comboBoxInputType.Items.Add("Sorted");
            comboBoxInputType.Items.Add("Sorted (Reverse)");
            comboBoxInputType.Items.Add("Random Shuffle");

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
            // TODO: Redraw array
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            // TODO: Start animation
        }
        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Pause animation
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            // TODO: Stop animation and reset array
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            // TODO: Proceed animation one step forward
        }

        private void sliderSpeed_ValueChanged(object sender, EventArgs e)
        {
            Slider slider = (Slider)sender;

            if (slider is null) 
            {
                throw new ArgumentNullException(nameof(sender), $"{nameof(sender)} must not be null.");
            }

            _delay = (int)((MaxSpeed - slider.Value) * (MaxDelay - MinDelay) / (MaxSpeed - MinSpeed) + MinDelay);
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

        private void ComboBoxInputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (sender is null) 
            {
                throw new ArgumentNullException(nameof(sender), $"{nameof(sender)} must not be null.");
            }

            _inputType = _inputTypes[comboBox.SelectedIndex];
        }
    }
}
