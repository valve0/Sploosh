using Sploosh.ViewModels;
using System.Windows;

namespace Sploosh
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(MainWindowViewModel mainWindowViewModel)
        {
            SettingsViewModel settingsViewModel = new SettingsViewModel(mainWindowViewModel);
            this.DataContext = settingsViewModel;
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            InitializeComponent();
        }


    }
}
