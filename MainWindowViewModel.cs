using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Wpf;
using SortingVisualizer.InputTypes;
using SortingVisualizer.Sorting;
using SortingVisualizer.Visualization;


namespace SortingVisualizer
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int MinSpeed { get; } = 0;
        public int MaxSpeed { get; } = 1000;

        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                _delay = (int)((MaxSpeed - _speed) * (float)(MaxDelay - MinDelay) / (MaxSpeed - MinSpeed) + MinDelay);

                if (_visualizationController is not null) 
                {
                    _visualizationController.Delay = _delay;
                }

                OnPropertyChanged(nameof(Speed));
                OnPropertyChanged(nameof(DelayLabel));
            }
        }
        public string DelayLabel => $"{_delay} ms";

        public string ArrayAccessesLabel => _visualizationController?.ArrayAccesses.ToString();
        public string ComparsionsLabel => _visualizationController?.Comparsions.ToString();
        public string ArrayWritesLabel => _visualizationController?.ArrayWrites.ToString();

        public int MinArraySize { get; } = 16;
        public int MaxArraySize { get; } = 2048;

        public int ArraySize
        {
            get
            {
                return _arraySize;
            }
            set
            {
                _arraySize = value;

                OnPropertyChanged(nameof(ArraySize));
                OnPropertyChanged(nameof(ArraySizeLabel));
            }
        }
        public string ArraySizeLabel => ArraySize.ToString();

        public IReadOnlyDictionary<string, ArrayInputType> InputTypes { get; } = new Dictionary<string, ArrayInputType> 
        {
            { "Sorted", ArrayInputType.Sorted },
            { "Sorted (Reverse)", ArrayInputType.Reversed },
            { "Almost Sorted", ArrayInputType.AlmostSorted },
            { "Shuffled Head", ArrayInputType.ShuffledHead },
            { "Shuffled Tail", ArrayInputType.ShuffledTail },
            { "Random Shuffle", ArrayInputType.RandomShuffle }
        };

        public KeyValuePair<string, ArrayInputType> SelectedInputType 
        {
            get 
            {
                return _selectedInputType;
            }
            set 
            {
                _selectedInputType = value;
                _inputType = value.Value;

                OnPropertyChanged(nameof(SelectedInputType));
            }
        }

        public IReadOnlyDictionary<string, ISorter> SortingAlgorithms{ get; } = new Dictionary<string, ISorter>()
        {
            { "Bubble Sort", Sorters.BubbleSort },
            { "Selection Sort", Sorters.SelectionSort },
            { "Insertion Sort", Sorters.InsertionSort },
            { "Merge Sort", Sorters.MergeSort },
            { "Merge Sort (Iterative)", Sorters.MergeSortIterative },
            { "Radix Sort", Sorters.RadixSort },
            { "Quick Sort", Sorters.QuickSort },
            { "Hoare's Quick Sort", Sorters.HoareQuickSort },
        };

        public KeyValuePair<string, ISorter> SelectedSorter 
        {
            get 
            {
                return _selectedSorter;
            }
            set 
            {
                _selectedSorter = value;
                _sorter = value.Value;

                OnPropertyChanged(nameof(SelectedSorter));
            }
        }

        public ICommand InitCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand StepCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }

        public GLWpfControl GLWpfControl { get; set; }

        private const int MinDelay = 1;
        private const int MaxDelay = 500;

        private int _speed;
        private int _delay;

        private int _arraySize;

        private KeyValuePair<string, ArrayInputType> _selectedInputType;
        private ArrayInputType _inputType;

        private KeyValuePair<string, ISorter> _selectedSorter;
        private ISorter _sorter;

        private VisualizationController _visualizationController;


        public MainWindowViewModel() 
        {
            SelectedInputType = InputTypes.First();
            SelectedSorter = SortingAlgorithms.First();

            Speed = MinSpeed;
            ArraySize = MinArraySize;

            InitCommand = new RelyCommand(Init);
            PlayCommand = new RelyCommand(Play);
            PauseCommand = new RelyCommand(Pause);
            StepCommand = new RelyCommand(Step);
            ResetCommand = new RelyCommand(Reset);
            CloseCommand = new RelyCommand(Close);
        }

        private void Init() 
        {
            GLWpfControlSettings setting = new GLWpfControlSettings() 
            {
                MajorVersion = 4,
                MinorVersion = 3,
                RenderContinuously = false
            };

            GLWpfControl.Start(setting);

            _visualizationController = new VisualizationController(new OpenGLVisualizer(GLWpfControl), _sorter);
            _visualizationController.CountersChanged += _visualizationController_CountersChanged;
            Reset();
        }

        private void Play() 
        {
            _visualizationController?.Play();
        }

        private void Pause() 
        {
            _visualizationController?.Pause();
        }

        private void Step() 
        {
            _visualizationController?.Step();
        }

        private void Reset() 
        {
            _visualizationController?.Reset(ArraySize, _inputType, _sorter);
        }

        private void Close() 
        {
            _visualizationController?.Dispose();
        }

        private void OnPropertyChanged(string? propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void _visualizationController_CountersChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ArrayAccessesLabel));
            OnPropertyChanged(nameof(ComparsionsLabel));
            OnPropertyChanged(nameof(ArrayWritesLabel));
        }
    }
}
