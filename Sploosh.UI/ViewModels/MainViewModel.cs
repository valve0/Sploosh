using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Sploosh.Services;
using Sploosh.UI.Events;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sploosh.UI.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private IRegionManager _regionManager;

        public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {

            _regionManager = regionManager;

            BackgroundImagePath = new BitmapImage(EnvironmentVariables.BackgroundImage);

            eventAggregator.GetEvent<RestartGameEvent>().Subscribe(RestartGame);

            //ShowSplashScreen
            _regionManager.RequestNavigate("ContentRegion", "SplashView");

        }

        public ImageSource BackgroundImagePath { get; private set; }


        // When the restart button is clcicked in the settings the Main window changes the content presented back to the game.
        private void RestartGame()
        {
            _regionManager.RequestNavigate("ContentRegion", "GameUI");
        }

    }
}
