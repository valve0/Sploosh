using Sploosh.Models;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sploosh.ViewModels
{
    class SettingsViewModel
    {
        // Help from https://stackoverflow.com/questions/44829097/what-is-the-mvvm-way-to-call-wpf-command-from-another-viewmodel
       private MainWindowViewModel _mainWindowViewModel;

        // Parameterless ctor to allow for testing of bindings in design mode
        public SettingsViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                ArrowImagePath = new BitmapImage(GameConstants.ArrowImage);
                BackgroundImagePath = new BitmapImage(GameConstants.BackgroundImage);
            }
        }

        public SettingsViewModel(MainWindowViewModel mainWindowViewModel)
       {
           _mainWindowViewModel = mainWindowViewModel;

           QuitCommand = new RelayCommand(QuitApplication, CanQuitApplication);
           RestartCommand = new RelayCommand(RestartApplication, CanRestartApplication);
           HelpCommand = new RelayCommand(OpenHelpWindow, CanOpenHelpWindow);
           AboutCommand = new RelayCommand(OpenAboutWindow, CanOpenAboutWindow);

           BackgroundImagePath = new BitmapImage(GameConstants.BackgroundImage);
           ArrowImagePath = new BitmapImage(GameConstants.ArrowImage);
        }

        public delegate void RestartEventAction();
        public event RestartEventAction? RestartEvent;

        public ICommand QuitCommand { get; set; }
        public ICommand RestartCommand { get; set; }
        public ICommand HelpCommand { get; set; }
        public ICommand AboutCommand { get; set; }

        public static ImageSource BackgroundImagePath { get; private set; }
        public static ImageSource ArrowImagePath { get; private set; }

        /// <summary>
        /// Resets the ViewModel Properties to intial values- resetting the game.
        /// </summary>
        private void RestartApplication(object obj)
        {
            _mainWindowViewModel.RestartGame(_mainWindowViewModel);
        }

        private bool CanOpenAboutWindow(object obj)
        {
            return true;
        }

        private void OpenAboutWindow(object obj)
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
