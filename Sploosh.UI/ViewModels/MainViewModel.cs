using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Sploosh.Services;
using Sploosh.UI.Events;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Sploosh.UI.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private IRegionManager _regionManager;

        public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {

            BackgroundImagePath = new BitmapImage(EnvironmentVariables.BackgroundImage);

            NavigateSettingsCommand = new DelegateCommand<string>(NavigateSettings);

            NavigateGameCommand = new DelegateCommand<string>(NavigateGame);
            _regionManager = regionManager;

            eventAggregator.GetEvent<RestartGameEvent>().Subscribe(RestartGame);
            
        }

        // When the restart button is clcicked in the settings the Main window changes the content presented back to the game.
        private void RestartGame()
        {
            _regionManager.RequestNavigate("ContentRegion", "GameUI");
        }

        public ImageSource BackgroundImagePath { get; private set; }

        public DelegateCommand<string> NavigateSettingsCommand { get; private set; }
        public DelegateCommand<string> NavigateGameCommand { get; private set; }

        private void NavigateSettings(string navigationPath)
        {

            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentNullException();

            _regionManager.RequestNavigate("ContentRegion", navigationPath);

        }

        private void NavigateGame(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentNullException();

            _regionManager.RequestNavigate("ContentRegion", navigationPath);
        }


        private void WaitOnScreen(int time)
        {
            //Timer to control how long splash screen active
            DispatcherTimer _timer = new();
            _timer.Interval = TimeSpan.FromSeconds(time); // Wait 6 seconds
            _timer.Start();
        }



    }
}
