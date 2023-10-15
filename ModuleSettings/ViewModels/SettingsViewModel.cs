using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace Sploosh.Modules.Settings.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private IRegionManager _regionManager;

        public SettingsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateGameCommand = new DelegateCommand<string>(NavigateGame);
            OpenSettingsDetails = new DelegateCommand<string>(NavigateDetails);
        }

        public DelegateCommand<string> NavigateGameCommand { get; private set; }
        public DelegateCommand<String> OpenSettingsDetails { get; }

        private void NavigateDetails(string navigationPath)
        {
            _regionManager.RequestNavigate("SettingsDetailsRegion", navigationPath);
        }

        private void NavigateGame(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentNullException();

            _regionManager.RequestNavigate("ContentRegion", navigationPath);

        }

    }
}