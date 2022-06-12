using System.Windows;


namespace SortingVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel viewModel = new MainWindowViewModel();
            viewModel.GLWpfControl = glControl;
            DataContext = viewModel;

            Loaded += (o, e) => viewModel.InitCommand.Execute(null);
            Closing += (o, e) => viewModel.CloseCommand.Execute(null);
        }
    }
}
