using Sploosh.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sploosh
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(ISetupGame _mainViewModel)
        {


            InitializeComponent();
            SettingsViewModel settingsViewModel = new SettingsViewModel(_mainViewModel);
            this.DataContext = settingsViewModel;
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            
        }


        private void RestartButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutButton_OnClick(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        private void HelpButton_OnClick(object sender, RoutedEventArgs e)
        {
            var helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
