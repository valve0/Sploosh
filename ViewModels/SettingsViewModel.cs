using HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sploosh.ViewModels
{
    class SettingsViewModel
    {
        // Help from https://stackoverflow.com/questions/44829097/what-is-the-mvvm-way-to-call-wpf-command-from-another-viewmodel
        private ISetupGame _mainViewModel;

        public ICommand QuitCommand { get; set; }
       public ICommand RestartCommand { get; set; }
       public ICommand HelpCommand { get; set; }
       public ICommand BackCommand { get; set; }
       public ICommand AboutCommand { get; set; }

       //Static image paths
       public static ImageSource BackgroundImagePath { get; private set; }

        public SettingsViewModel(ISetupGame mainViewModel)
       {
           _mainViewModel = mainViewModel;

           QuitCommand = new RelayCommand(QuitApplication, CanQuitApplication);
           RestartCommand = new RelayCommand(RestartApplication, CanRestartApplication);
           HelpCommand = new RelayCommand(OpenHelpWindow, CanOpenHelpWindow);
           BackCommand = new RelayCommand(CloseSettingsWindow, CanCloseSettingsWindow);
           AboutCommand = new RelayCommand(OpenAboutWindow, CanOpenAboutWindow);

           BackgroundImagePath = new BitmapImage(new Uri($@"{FileRepository.AssemblyDirectory}\Images\Pergament1.png", UriKind.Relative));

        }

        private bool CanOpenAboutWindow(object obj)
        {
            return true;
        }

        private void OpenAboutWindow(object obj)
        {
        }

        private bool CanCloseSettingsWindow(object obj)
        {
            return true;
        }

        private void CloseSettingsWindow(object obj)
        {
        }

        private bool CanOpenHelpWindow(object obj)
        {
            return true;
        }

        private void OpenHelpWindow(object obj)
        {
        }

        private bool CanRestartApplication(object obj)
        {
            return true;
        }

        private void RestartApplication(object obj)
        {
            _mainViewModel.SetupGame();
        }

        private bool CanQuitApplication(object obj)
        {
            return true;
        }

        private void QuitApplication(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
