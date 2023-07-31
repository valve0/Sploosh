using System.Windows;
using Sploosh.ViewModel;

namespace Sploosh.View
{
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        public MainWindow(MainViewModel viewModel)
        {
            _mainViewModel = viewModel;
            DataContext = _mainViewModel;
            InitializeComponent();
        }      
    }
}
